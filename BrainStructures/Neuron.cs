using System;
//using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructures;

namespace BrainStructures
{
                                                            // The computational unit of
                                                            // the neural net; behavior emerges
                                                            // as neurons excite and inhibit
                                                            // one another
    public class Neuron : INode
    {
        public LinkedList<Synapse> TempSynapseList { get; set; }
        public Synapse[] SynapseList { get; set; }

                                                            // This neuron will fire
                                                            // once count is greater or equal
                                                            // to threshhold
        int threshhold;

        public const int DEFAULT_THRESHHOLD = 20;

                                                            // Count is increased and decreased
                                                            // as other neurons signal it.
        int count;

        public int SensoryCount
        {
            get
            {
                return 1;
            }
            set
            {
                if(value != 1)
                {
                    throw new ArgumentException("Attempting to set sensory count of a neuron");
                }
            }
        }

        public int MotorCount
        {
            get
            {
                return 1;
            }
            set
            {
                if (value != 1)
                {
                    throw new ArgumentException("Attempting to set motor count of a neuron");
                }
            }
        }

        public Neuron[] Sensory
        {
            get
            {
                return new Neuron[] { this };
            }
        }

        public Neuron[] Motor
        {
            get
            {
                return new Neuron[] { this };
            }
        }

        // Set to true if this neuron is to
        // be removed from the neural net,
        // allowing other neurons to remove
        // it from their synapse lists.
        public bool MarkedForDeletion { get; private set; }

        
        public int? MadeFrom {
            get
            {
                return null;
            }
            set => throw new NotSupportedException();
        }


        // Indicates if this neuron takes
        // some action beyond signalling other
        // neurons.
        bool isMotor;

                                                            // A delegate type used to pass methods
                                                            // to motor neurons.
        public delegate void actionType();

                                                            // The action this neuron should take,
                                                            // if it is a motor neuron
        actionType action;

                                                            // Used in an algorithm which decides if
                                                            // a new clone should be generated -
                                                            // see the getClone method
        int cloneNumber;

                                                            // A clone of this neuron
        Neuron clone;

        public Neuron()
        {
            // -1 will never be passed to getClone,
            // ensuring the method generates a new clone
            // when it is first called.
            this.cloneNumber = -1;
            this.threshhold = DEFAULT_THRESHHOLD;
            this.TempSynapseList = new LinkedList<Synapse>();
            this.count = 0;
            this.clone = this;
            this.isMotor = false;
        }

        public Neuron (int threshhold)
        {
                                                            // -1 will never be passed to getClone,
                                                            // ensuring the method generates a new clone
                                                            // when it is first called.
            this.cloneNumber = -1;
            this.threshhold = threshhold;
            this.TempSynapseList = new LinkedList<Synapse>();
            this.count = 0;
            this.clone = this;
            this.isMotor = false;
        }

                                                            // Initializes a motor neuron
        public Neuron(int threshhold, actionType action) : this(threshhold)
        {
            this.action = action;
            this.isMotor = true;
        }


        /** Allows this neuron's action to be
        * set to a new method.  The neuron will
        * also be set to a motor neuron, if this
        * was not already the case.
        **/
        public void SetAction(actionType action)
        {
            isMotor = true;
            this.action = action;
        }

        // Sends a signal to this neuron
        public void Trigger(int sensoryIndex, int weight)
        {
            if(weight > 0 || count > -1*weight)
            {
                count += weight;
            }
        }

                                                            // Sets MarkedForDeletion to true, so
                                                            // that the neuron can be effectively removed
                                                            // from the neural net
        public void MarkForDeletion()
        {
            MarkedForDeletion = true;
        }

                                                            // Reports if this neuron should remain
                                                            // in the neural net.
        public bool ToRemain()
        {
            return !MarkedForDeletion;
        }

                                                            // Sends signals to other neurons, or takes
                                                            // some action, if appropriate
        public void Fire()
        {
            if(count >= threshhold && false == MarkedForDeletion)
            {
                if (isMotor)
                {
                    action();
                }
                else
                {
                    foreach(Synapse synapse in SynapseList)
                    {
                        if (!synapse.Target.MarkedForDeletion)
                        {
                            synapse.Target.Trigger(synapse.TargetSensoryIndex, synapse.Weight);
                        }
                    }
                }
                count = 0;
            }
        }

                                                            // Provides a copy of this neuron; 
        public INode GetClone(int cloneID)
        {
                                                            // If the neuron's own clone number
                                                            // doesn't match what was passed
                                                            // in as an argument, a new clone
                                                            // should be generated
            if(this.cloneNumber != cloneID)
            {
                this.cloneNumber = cloneID;
                if(isMotor)
                {
                    clone = new Neuron(this.threshhold, action);
                }
                else
                {
                    clone = new Neuron(this.threshhold);
                }
                
            }
            return clone;
        }

                                                            // Causes the neuron's copy to fill 
                                                            // its lists of neurons to trigger
        public void FillClone(int cloneID)
        {
            GetClone(cloneNumber);
            
            foreach (Synapse synapse in SynapseList)
            {

                if(!synapse.Target.MarkedForDeletion)
                {
                    clone.TempSynapseList.AddAtStart(new Synapse() {
                        Target = synapse.Target.GetClone(cloneNumber),
                        Weight = synapse.Weight,
                        TargetSensoryIndex = synapse.TargetSensoryIndex,
                        ComponentIndex = synapse.ComponentIndex
                    });
                }
            }
        }

                                                            // Allows a mutator to path through
                                                            // this neuron's synapes, optionally
                                                            // deleting them.
        public void PurgeRun(double checkAgainst, double oddsOfPruning)
        {
            if (TempSynapseList.ElementsRemain)
            {
                if(checkAgainst < oddsOfPruning/SynapseList.Length)
                {
                    TempSynapseList.Remove();
                }
                else
                {
                    TempSynapseList.MoveUp();
                }
            }
        }
        

                                                            // Adds a new synapse to this neuron
        public void NewSynapse(int motorIndex, INode toAdd, int sensoryIndex, int weight)
        {
            if(motorIndex != 0)
            {
                throw new ArgumentException("motorIndex out of range");
            }
            var target = toAdd.SensoryAt(sensoryIndex);
            if(!toAdd.MarkedForDeletion)
            {
                var newNeuron = true;
                TempSynapseList.ResetPointer();
                bool scanning = true;
                while (TempSynapseList.ElementsRemain && scanning)
                {
                    TempSynapseList.MoveUp();
                    if (TempSynapseList.Value.Target == target)
                    {
                        newNeuron = false;
                        if(-1*weight == TempSynapseList.Value.Weight)
                        {
                            TempSynapseList.Remove();
                        }
                        else
                        {
                            TempSynapseList.Value.Weight += weight;
                        }
                    }
                }
                if(newNeuron)
                {
                    TempSynapseList.AddAfter(new Synapse()
                    {
                        Target = target,
                        Weight = weight,
                        TargetSensoryIndex = sensoryIndex,
                        MotorIndex = 0
                    });
                }
            }

        }


        // Adds a new synapse to this neuron
        public void NewSynapse(int motorIndex, INode toAdd, int sensoryIndex, int weight, int componentIndex)
        {
            if (motorIndex != 0)
            {
                throw new ArgumentException("motorIndex out of range");
            }
            var target = toAdd.SensoryAt(sensoryIndex);
            if (!toAdd.MarkedForDeletion)
            {
                var newNeuron = true;
                TempSynapseList.ResetPointer();
                bool scanning = true;
                while (TempSynapseList.ElementsRemain && scanning)
                {
                    TempSynapseList.MoveUp();
                    if (TempSynapseList.Value.Target == target)
                    {
                        newNeuron = false;
                        if (-1 * weight == TempSynapseList.Value.Weight)
                        {
                            TempSynapseList.Remove();
                        }
                        else
                        {
                            TempSynapseList.Value.Weight += weight;
                        }
                    }
                }
                if (newNeuron)
                {
                    TempSynapseList.AddAfter(new Synapse()
                    {
                        Target = target,
                        Weight = weight,
                        TargetSensoryIndex = sensoryIndex,
                        MotorIndex = 0,
                        ComponentIndex = componentIndex
                    });
                }
            }

        }

        public Neuron SensoryAt(int index)
        {
            if(0 != index)
            {
                throw new ArgumentException("Index out of range");
            }
            else
            {
                return this;
            }
        }

        public void HardenNeuron()
        {
            this.SynapseList = new Synapse[this.TempSynapseList.Length];
            this.TempSynapseList.ResetPointer();
            if(0<TempSynapseList.Length)
            {
                this.TempSynapseList.MoveUp();
                for(int i = 0; i<this.SynapseList.Length; i++)
                {
                    this.SynapseList[i] = this.TempSynapseList.Value;
                    this.TempSynapseList.Remove();
                }
            }
        }

        public void Dealloc()
        {
            for(int i = 0; i<this.SynapseList.Length; i++)
            {
                SynapseList[i] = null;
            }
        }

        /*                                                    // Provides a description of the synapses
                                                            // this neuron has
        public LinkedList<Synapse> giveSynapses(LinkedList<Neuron> intermediates, LinkedList<Neuron> motor)
        {
            LinkedList<Synapse> upSynapseList = describeSynapseList(intermediates, motor, triggerUp);
            LinkedList<Synapse> downSynapseList = describeSynapseList(intermediates, motor, triggerDown);
            foreach(Synapse synapse in upSynapseList)
            {
                synapse.action = Action.Exitatory;
            }
            foreach (Synapse synapse in downSynapseList)
            {
                synapse.action = Action.Inhibitory;
            }
            return upSynapseList + downSynapseList;
        }

                                                            // Generates a description of a given
                                                            // linked list of synapses based on
                                                            // their index in either a list of
                                                            // intermediate or motor neurons      
        private static LinkedList<Synapse> describeSynapseList (
            LinkedList<Neuron> intermediates, 
            LinkedList<Neuron> motor, 
            LinkedList<Neuron> synapseList)
        {
            LinkedList<Synapse> returnSynapseList = new LinkedList<Synapse>();
            synapseList.ResetPointer();
            bool latch;
            if (synapseList.ElementsRemain())
            {
                synapseList.MoveUp();
                latch = true;
            }
            else
            {
                latch = false;
            }
            while (latch)
            {
                if (synapseList.Value().MarkedForDeletion)
                {
                    if (!synapseList.ElementsRemain())
                    {
                        latch = false;
                    }
                    synapseList.Remove();
                }
                else
                {
                    returnSynapseList.AddAtStart(synapseList.Value().
                        giveLocation(intermediates, motor));
                    if(synapseList.ElementsRemain()){
                        synapseList.MoveUp();
                    }
                    else{
                        latch = false;
                    }
                }
            }
            return returnSynapseList;
        }

                                                            // Allows this neuron to describe its
                                                            // position in either a list of
                                                            // intermediate or motor neurons
        Synapse giveLocation(LinkedList<Neuron> intermediates, LinkedList<Neuron> motor)
        {
            int location = givePossibleLocation(intermediates);
            if (-1 == location)
            {
                location = givePossibleLocation(motor);
                if(-1 == location)
                {
                    return new Synapse(Function.Unknown, location);
                }
                else
                {
                    return new Synapse(Function.Motor, location);
                }
            }
            else
            {
                return new Synapse(Function.Intermediate, location);
            }
            //String returnString = "I" + namePossibleLocation(intermediates);
            //if(returnString.Equals("I-"))
            //{
            //    returnString = "M" + namePossibleLocation(motor);
            //}
        }

                                                            // Allows this neuron to describe its
                                                            // position in a list of neurons, or
                                                            // report that it is not present in the
                                                            // given list
        int givePossibleLocation(LinkedList<Neuron> possibleLocation)
        {
            int i = 0;
            possibleLocation.ResetPointer();
            while (possibleLocation.ElementsRemain())
            {
                possibleLocation.MoveUp();
                if (this == possibleLocation.Value())
                {
                    return i;
                }
                i++;
            }
                                                            // -1 cannot be the index of a neuron in
                                                            // a linked list, making it useful as a flag.
            return -1;
        }


                                                            // Determines how many up or down synapses
                                                            // this neuron has pointing to searchingFor.
        public int getSynapsesFor(Neuron searchingFor)
        {
            int synapseCount = 0;
            triggerUp.resetPointer();
            foreach(Neuron synapse in triggerUp)
            {
                if(synapse == searchingFor)
                {
                    synapseCount++;
                }
            }
            triggerDown.resetPointer();
            foreach (Neuron synapse in triggerDown)
            {
                if (synapse == searchingFor)
                {
                    synapseCount--;
                }
            }
            return synapseCount;
        }*/
    }
}
