using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructures;
using BrainStructures;

namespace DataAccess
{
    public class CharMap
    {

                                                            // Creates a new neuron to add to a
                                                            // binary search tree;
        public static Neuron Neuron(int i)
        {
            return new Neuron(0);
        }

                                                            // Creates a new binary search tree of neuron/character
                                                            // pairs based on the contents of a text file.
        public static void CreateMap(Collective toCreate, String filePath)
        {
            toCreate.SensoryMap = new BinarySearchTree<char, Neuron>((i,j)=>(int)i-(int)j);
            try
            {
               updateMap(toCreate, filePath);
            }
            catch
            {
                throw;
            }
            
        }

                                                            // Updates a given neural net to ensure it can
                                                            // respond to every character in the given file
        public static void updateMap(Collective toUpdate,
            String filePath) {
            var mapToUpdate = toUpdate.SensoryMap;
            try
            {
                using (StreamReader getChars = new StreamReader(filePath))
                {
                    while (!getChars.EndOfStream)
                    {
                        char charToAdd = Char.ToLower((char)getChars.Read());
                        if (!mapToUpdate.has(charToAdd))
                        {
                            mapToUpdate.add(charToAdd, new Neuron(0));
                            foreach(NetStructure component in toUpdate.Components)
                            {
                                component.SensoryList.AddAfter(new Neuron(1));
                                mapToUpdate.search(charToAdd).TempSynapseList = new DataStructures.LinkedList<Synapse>()
                                {
                                    [0] = new Synapse
                                    {
                                        Target = component.SensoryList[component.SensoryList.Length-1],
                                        Weight = 1,
                                        TargetSensoryIndex = component.SensoryList.Length - 1,
                                        MotorIndex = 0,
                                        ComponentIndex = 0
                                    }
                                };
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw;
            }
            toUpdate.SensoryMap = new BinarySearchTree<char, BrainStructures.Neuron>(mapToUpdate);
        }
    }
}
