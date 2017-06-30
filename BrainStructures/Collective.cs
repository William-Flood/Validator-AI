using System;
//using System.Collections.Generic;
using DataStructures;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainStructures
{
    public class Collective
    {
        public NetStructure[] Components { get; set; }
        public BinarySearchTree<char, Neuron> SensoryMap { get; set; }
        public Neuron[] MotorList { get; set; }
        public void Dealloc()
        {
            for(int i = 0; i<Components.Length; i++)
            {
                Components[i].Dealloc();
                Components[i] = null;
            }
        }
    }
}
