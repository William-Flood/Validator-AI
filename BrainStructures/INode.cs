using System;
//using System.Collections.Generic;
using DataStructures;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainStructures
{
    public interface INode
    {
        void Trigger(int sensoryIndex, int weight);
        void Fire();
        bool MarkedForDeletion { get;}
        void MarkForDeletion();
        INode GetClone(int cloneID);
        void FillClone(int cloneID);
        int SensoryCount { get; set; }
        Neuron SensoryAt(int index);
        int MotorCount { get; set; }
        int? MadeFrom { get; set; }
        LinkedList<Synapse> TempSynapseList { get; set; }
        Synapse[] SynapseList { get; set; }
        void NewSynapse(int motorIndex, INode toAdd, int sensoryIndex, int weight);
        void HardenNeuron();
        void Dealloc();
    }
}
