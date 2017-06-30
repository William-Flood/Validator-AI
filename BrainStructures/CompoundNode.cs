using System;
//using System.Collections.Generic;
using DataStructures;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainStructures
{
    public class CompoundNode:INode
    {
        public int SensoryCount
        {
            get
            {
                return Sensory.Length;
            }
            set
            {
                Sensory = new Neuron[value];
            }
        }

        public int MotorCount
        {
            get
            {
                return Motor.Length;
            }
            set
            {
                Motor = new Neuron[value];
            }
        }

        public Neuron[] Sensory{ get; private set; }
        public Neuron[] Motor{ get; private set; }
        public LinkedList<INode> Intermediates { get; set; }

        public bool MarkedForDeletion { get; private set; }
        public int? MadeFrom { get; set; }
        public LinkedList<Synapse> TempSynapseList { get; set; }
        public Synapse[] SynapseList { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public CompoundNode()
        {
            this.MarkedForDeletion = false;
            TempSynapseList = new LinkedList<Synapse>();
            Intermediates = new LinkedList<INode>();
        }

        public void FillClone(int cloneID)
        {
            throw new NotImplementedException();
        }

        public void Fire()
        {
            throw new NotImplementedException();
        }

        public INode GetClone(int cloneID)
        {
            throw new NotImplementedException();
        }

        public void MarkForDeletion()
        {
            throw new NotImplementedException();
        }

        public Neuron SensoryAt(int index)
        {
            throw new NotImplementedException();
        }

        public void Trigger(int sensoryIndex, int weight)
        {
            throw new NotImplementedException();
        }

        public void PurgeRun(double checkAgainst, double oddsOfPruning)
        {
            throw new NotImplementedException();
        }

        public void NewSynapse(int motorIndex, INode toAdd, int sensoryIndex, int weight)
        {
            throw new NotImplementedException();
        }

        public void HardenNeuron()
        {
            throw new NotImplementedException();
        }

        public void Dealloc()
        {
            throw new NotImplementedException();
        }
    }
}
