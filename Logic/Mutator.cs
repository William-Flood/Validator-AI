using System;
//using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrainStructures;
using DataStructures;

namespace Logic
{
                                                            // Mutates a neural net.
    public class Mutator
    {

                                                            // The odds that a new neuron will
                                                            // be added to the neural net.
        double oddsOfNeurogenesis;
                    
                                                            // The odds that a neural net will
                                                            // be removed from the neural net.
        double oddsOfLysing;

                                                            // The odds that a new synapse will
                                                            // be added to a given neuron.
        double oddsOfNewSynapses;

                                                            // The odds that a synapse will be
                                                            // removed from a given neuron.
        double oddsOfPruningSynapses;

                                                            // The odds that a synapse added
                                                            // to a neuron will be exitatory.
        double oddsOfExitation;
        const int NEURON_THRESHHOLD = Neuron.DEFAULT_THRESHHOLD;
        public static int RngSeed { get; private set; }
        static Random _rng;
        Random rng { get {
                if(null == _rng)
                {
                    //_rng = new Random();
                    RngSeed = (new Random()).Next();
                    _rng = new Random(RngSeed);
                    //_rng = new Random(425682);
                }
                return _rng;
            } }


        public Mutator(
            double oddsOfNeurogenesis,
            double oddsOfLysing,
            double oddsOfNewSynapses,
            double oddsOfPruningSynapses,
            double oddsOfExitation)
        {
            this.oddsOfNeurogenesis = oddsOfNeurogenesis;
            this.oddsOfLysing = oddsOfLysing;
            this.oddsOfNewSynapses = oddsOfNewSynapses;
            this.oddsOfPruningSynapses = oddsOfPruningSynapses;
            this.oddsOfExitation = oddsOfExitation;
            //rng = new Random();
        }

                                                            // Mutates a neural net.
        public void MutateNetwork(NetStructure toMutate)
        {
            int startingLengthOfIntermediates = toMutate.TempIntermediates.Length;
            while(toMutate.TempIntermediates.ElementsRemain)
            {
                toMutate.TempIntermediates.MoveUp();
                if(rng.NextDouble() < oddsOfLysing/startingLengthOfIntermediates)
                {
                    toMutate.TempIntermediates.Value.MarkForDeletion();
                    toMutate.TempIntermediates.Remove();
                }
            }
            
            if(rng.NextDouble() < oddsOfNeurogenesis)
            {
                toMutate.TempIntermediates.AddAtStart(new Neuron(NEURON_THRESHHOLD));
            }


            
            var temMotorList = new LinkedList<INode>();
            foreach(INode motor in toMutate.MotorList)
            {
                temMotorList.AddAtStart(motor);
            }
            var canConnectTo = (toMutate.TempIntermediates + temMotorList).ToArray();
            foreach(Neuron sensory in toMutate.SensoryList)
            {
                MutateSynapses(sensory, canConnectTo);
            }
            toMutate.TempIntermediates.ResetPointer();
            for (int i = 0; i <toMutate.TempIntermediates.Length; i++)
            {
                toMutate.TempIntermediates.MoveUp();
                INode mightMutate = toMutate.TempIntermediates.Value;
                MutateSynapses(mightMutate, canConnectTo);
            }
        }

        /*
        public void MutateCollectiveSensory(Collective toMutate)
        {
            var compositeSensory = new LinkedList<INode>();
            foreach(NetStructure subNet in toMutate.Components)
            {
                foreach(INode sensory in subNet.SensoryList)
                {
                    compositeSensory.AddAtStart(sensory);
                }
            }
            for(int i = 0; i< toMutate.SensoryMap.getSize(); i++)
            {

                MutateSynapses(toMutate.SensoryMap.valueAt(i), compositeSensory);

            }
        }*/

                                                            // Mutates an individual neuron
        void MutateSynapses(INode mightMutate, INode[] canConnectTo)
        {
            var nodeSynapseList = mightMutate.TempSynapseList;
            nodeSynapseList.ResetPointer();
            while (nodeSynapseList.ElementsRemain)
            {
                if (rng.NextDouble() < oddsOfPruningSynapses / nodeSynapseList.Length)
                {
                    nodeSynapseList.Remove();
                }
                else
                {
                    nodeSynapseList.MoveUp();
                }
            }

            if (rng.NextDouble() < oddsOfNewSynapses)
            {
                int j = canConnectTo.Length;
                int componentIndex = rng.Next(canConnectTo.Length);
                var targetNode = canConnectTo[componentIndex];
                var newMotorIndex = rng.Next(mightMutate.MotorCount);
                var sensosryIndex = rng.Next(targetNode.SensoryCount);
                mightMutate.NewSynapse(newMotorIndex, targetNode, sensosryIndex, rng.NextDouble() < oddsOfExitation ? 1 : -1,componentIndex);
            }
        }


                                                            // Adjusts the odds of neurogenesis.
        public void adjustNeurogenesisOdds(double moveTo)
        {
            oddsOfNeurogenesis = adjustOdds(oddsOfNeurogenesis, moveTo);
        }

                                                            // Adjusts the odds of lysing a neuron.
        public void adjustLysingOdds(double moveTo)
        {
            oddsOfLysing = adjustOdds(oddsOfLysing, moveTo);
        }

                                                            // Adjusts the odds of adding a synapse.
        public void adjustNewSynapsesOdds(double moveTo)
        {
            oddsOfNewSynapses = adjustOdds(oddsOfNewSynapses, moveTo);
        }

                                                            // Adjusts the odds of pruning a synapse.
        public void adjustPruneSynapseOdds(double moveTo)
        {
            oddsOfPruningSynapses = adjustOdds(oddsOfPruningSynapses, moveTo);
        }

                                                            // Adjusts the odds of making a
                                                            // synapse exitatory.
        public void adjustExitationOdds(double moveTo)
        {
            oddsOfExitation = adjustOdds(oddsOfExitation, moveTo);
        }

                                                            // Adjusts a given probability.
        static double adjustOdds(double currentValue, double moveTo)
        {
            return currentValue * 0.75 + moveTo * 0.25;
        }

                                                            //Has a one in fifty chance of returning true; used to
                                                            //determine if this mutator should be replaced by a
                                                            //stronger-acting one.
        public bool createBumper()
        {
            return rng.NextDouble() < .02;
        }

    }
}
