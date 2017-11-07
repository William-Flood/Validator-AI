using System;
//using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructures;

namespace BrainStructures
{
    public class NetStructure
    {
        
        public LinkedList<Neuron> SensoryList { get; set; }

                                                            // A linked list of neurons
        public LinkedList<INode> TempIntermediates { get; set; }
        public INode[] Intermediates { get; set; }

                                                            // A linked list of neurons
                                                            // which trigger some action
        public Neuron[] MotorList { get; set; }

        public int NetworkSeq { get; set; }
        
        public int RoleID { get; set; }

        public int RoleIndex { get; set; }

        public int NetworkLevel { get; set; }

        int compoundNodeID = 0;

        int[,] grid;
        

        public CompoundNode CreateCompoundNode()
        {
            var clone = new CompoundNode()
            {
                SensoryCount = SensoryList.Length,
                MotorCount = MotorList.Length,
                MadeFrom = this.NetworkSeq
            };
            int sensoryIndex = 0;
            foreach (Neuron sensory in SensoryList)
            {
                clone.Sensory[sensoryIndex] = (Neuron)sensory.GetClone(compoundNodeID);
                sensory.FillClone(compoundNodeID);
                sensoryIndex++;
            }
            foreach (INode intermediate in Intermediates)
            {
                clone.Intermediates.AddAfter(intermediate.GetClone(compoundNodeID));
                intermediate.GetClone(compoundNodeID);
                clone.Intermediates.MoveUp();
            }
            foreach (Neuron motor in MotorList)
            {
                clone.Sensory[sensoryIndex] = (Neuron)motor.GetClone(compoundNodeID);
                motor.FillClone(compoundNodeID);
                sensoryIndex++;
            }
            compoundNodeID++;
            return clone;
        }

        public void Dealloc()
        {
            this.SensoryList.ResetPointer();
            this.SensoryList.MoveUp();
            while(SensoryList.ElementsRemain)
            {
                this.SensoryList.Value.Dealloc();
                this.SensoryList.Remove();
            }
            for(int i = 0; i<Intermediates.Length; i++)
            {
                Intermediates[i].Dealloc();
                Intermediates[i] = null;
            }

        }

        public void Reset()
        {
            foreach(INode intermediate in Intermediates)
            {
                intermediate.Reset();
            }

            foreach(INode motor in MotorList)
            {
                motor.Reset();
            }
        }

        public void Griddify()
        {
            grid = new int[SensoryList.Length + Intermediates.Length, Intermediates.Length + MotorList.Length + 1];
            int nodeIndex = 0;
            foreach(Neuron sensory in SensoryList)
            {
                sensory.FillGraphArray(nodeIndex, grid);
                nodeIndex++;
            }
            foreach(INode intermediate in Intermediates)
            {
                intermediate.FillGraphArray(nodeIndex, grid);
                nodeIndex++;
            }
        }
        

        
    }
}
