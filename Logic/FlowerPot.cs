using BrainStructures;
using DataAccess;
using DataStructures;
using System;
using System.IO;
//using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class FlowerPot
    {
        public Neuron.actionType[] NeuronActions { get; set; }
        public string TrainingDocumentName { get; set; }
        // Creates a new neural net.
        public Collective CreateNet()
        {
            var createdNet = new Collective() {
                Components = new NetStructure[] { new NetStructure() { SensoryList = new LinkedList<Neuron>() } },
                MotorList = new Neuron[0]
            };
            var firstNet = createdNet.Components[0];
            var nodeMotorList = new LinkedList<INode>();
            try
            {
                CharMap.CreateMap(createdNet, AppData.TrainingDocumentsPath + TrainingDocumentName);

            }
            catch (DirectoryNotFoundException)
            {
                throw;
            }
            firstNet.TempIntermediates = new LinkedList<INode>();
            for (int i = 0; i < 5; i++)
            {
                firstNet.TempIntermediates.AddAtStart(new Neuron());
            }
            firstNet.MotorList = new Neuron[]
            {
                new Neuron(Neuron.DEFAULT_THRESHHOLD, NeuronActions[0]),
                new Neuron(Neuron.DEFAULT_THRESHHOLD, NeuronActions[1])
            };
            Mutator generatorMutator = new Mutator(0, 0, .9, .01, .8);
            for (int i = 0; i < 5; i++)
            {
                generatorMutator.MutateNetwork(firstNet);
            }
            HardenNet(createdNet);
            return createdNet;
        }


        public Collective CloneNet(Collective cloningNet, int i)
        {
            Collective returnNet = new Collective() {
                Components = new NetStructure[cloningNet.Components.Length],
                SensoryMap = new BinarySearchTree<char, Neuron>((chari,charj)=>(int)chari - (int)charj),
                //MotorList = new Neuron[cloningNet.MotorList.Length]
            };
            //Cloning the collective's motor neurons
            //for(int motorIndex = 0; motorIndex < cloningNet.MotorList.Length; motorIndex++)
            //{
            //    returnNet.MotorList[motorIndex] = (Neuron)cloningNet.MotorList[motorIndex].GetClone(i);
            //}
            int componentIndex = 0;
            foreach(NetStructure component in cloningNet.Components)
            {
                var componentClone = new NetStructure{
                    RoleID = component.RoleID,
                    RoleIndex = component.RoleIndex,
                    NetworkLevel = component.NetworkLevel
                };
                returnNet.Components[componentIndex] = componentClone;
                componentClone.MotorList = new Neuron[component.MotorList.Length];
                for (int motorIndex = 0; motorIndex < component.MotorList.Length; motorIndex++) {
                    componentClone.MotorList[motorIndex] = (Neuron)component.MotorList[motorIndex].GetClone(i);
                    componentClone.MotorList[motorIndex].SetAction(NeuronActions[motorIndex]);
                }
                LinkedList<INode> intermediatesClone = new LinkedList<INode>();
                int j = 0;
                foreach (INode cloningNeuron in component.Intermediates)
                {
                    if (!cloningNeuron.MarkedForDeletion)
                    {
                        intermediatesClone[j] = cloningNeuron.GetClone(i);
                        cloningNeuron.FillClone(i);
                        j++;
                    }
                }
                componentClone.TempIntermediates = intermediatesClone;


                LinkedList<Neuron> sensoryClone = new LinkedList<Neuron>();
                j = 0;
                foreach (Neuron cloningNeuron in component.SensoryList)
                {
                    if (!cloningNeuron.MarkedForDeletion)
                    {
                        sensoryClone[j] = (Neuron)cloningNeuron.GetClone(i);
                        cloningNeuron.FillClone(i);
                        j++;
                    }
                }
                componentClone.SensoryList = sensoryClone;
                
                componentIndex++;
            }
            //Cloning the collective's sensory neurons
            var charArray = new char[cloningNet.SensoryMap.getSize()];
            var sensoryArray = new Neuron[cloningNet.SensoryMap.getSize()];
            for (int sensoryIndex = 0; sensoryIndex < cloningNet.SensoryMap.getSize(); sensoryIndex++)
            {
                charArray[sensoryIndex] = cloningNet.SensoryMap[sensoryIndex];
                sensoryArray[sensoryIndex] = (Neuron)cloningNet.SensoryMap.search(cloningNet.SensoryMap[sensoryIndex]).GetClone(i);
                cloningNet.SensoryMap.search(cloningNet.SensoryMap[sensoryIndex]).FillClone(i);
            }
            returnNet.SensoryMap = new BinarySearchTree<char, Neuron>(
                charArray.Length, 
                (charIndex) => charArray[charIndex],
                (neuronIndex) => sensoryArray[neuronIndex], 
                (charA, charB) => (int)charA - (int)charB
            );
            return returnNet;
        }

        // Produces a clone of a
        // linked list of neurons.
        static LinkedList<INode> CloneNetPiece(int i, LinkedList<INode> toClone)
        {
            LinkedList<INode> clone = new LinkedList<INode>();
            int j = 0;
            foreach (INode cloningNeuron in toClone)
            {
                if (!cloningNeuron.MarkedForDeletion)
                {
                    clone[j] = cloningNeuron.GetClone(i);
                    cloningNeuron.FillClone(i);
                    j++;
                }
            }
            return clone;
        }

        public static void HardenNet(Collective toHarden)
        {
            for(int i = 0; i<toHarden.SensoryMap.getSize(); i++)
            {
                toHarden.SensoryMap.valueAt(i).HardenNeuron();
            }
            foreach (NetStructure component in toHarden.Components)
            {
                component.Intermediates = new Neuron[component.TempIntermediates.Length];
                component.TempIntermediates.ResetPointer();
                component.TempIntermediates.MoveUp();
                for(int intermediateIndex = 0; intermediateIndex<component.Intermediates.Length; intermediateIndex++)
                {
                    component.Intermediates[intermediateIndex] = component.TempIntermediates.Value;
                    component.TempIntermediates.Remove();
                }
                component.TempIntermediates = null;
                foreach(Neuron sensory in component.SensoryList)
                {
                    sensory.HardenNeuron();
                }
                foreach(INode intermediate in component.Intermediates)
                {
                    intermediate.HardenNeuron();
                }
            }
        }
    }
}
