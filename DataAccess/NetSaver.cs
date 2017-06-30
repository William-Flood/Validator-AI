using System;
using DataStructures;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrainStructures;

namespace DataAccess
{
                                                            // Allows neural nets to be
                                                            // saved to a file
    public class NetSaver
    {

                                                            // Produces a saved file
                                                            // of a neuron
        /*public static void saveNet(NetStructure toSave, String filePath)
        {
            try {
                  using (StreamWriter netWriter = new StreamWriter(filePath))
                  {
                      netWriter.WriteLine("Intermediate neurons: " +
                          toSave.intermediates.length());
                      for (int i = 0; i<toSave.sensoryMap.getSize(); i++)
                      {
                          netWriter.WriteLine("S" + toSave.sensoryMap[i] +
                              " Links to: ");
                          LinkedList<Synapse> synapsesToDescribe = 
                              toSave.sensoryMap.valueAt(i).giveSynapses(
                                  toSave.intermediates,
                                  toSave.motorNeurons);
                          writeSynapses(netWriter,
                              synapsesToDescribe);
                      }
                      int neuronIndex = 0;
                      LinkedList<Neuron> intermediateParser = new 
                          LinkedList<Neuron>(toSave.intermediates);
                      foreach(Neuron intermediate in intermediateParser)
                      {
                          netWriter.WriteLine("I" + neuronIndex.ToString() +
                              " Links to: ");
                          LinkedList<Synapse> synapsesToDescribe =
                              intermediate.giveSynapses(
                                  toSave.intermediates,
                                  toSave.motorNeurons);
                          writeSynapses(netWriter,
                              synapsesToDescribe);
                          neuronIndex++;
                      }
                      neuronIndex = 0;
                      LinkedList<Neuron> motorNeuronParser = new
                          LinkedList<Neuron>(toSave.motorNeurons);
                      foreach (Neuron motor in motorNeuronParser)
                      {
                          netWriter.WriteLine("M" + neuronIndex.ToString() +
                              " Links to: ");
                          LinkedList<Synapse> synapsesToDescribe =
                              motor.giveSynapses(
                                  toSave.intermediates,
                                  toSave.motorNeurons);
                          writeSynapses(netWriter,
                              synapsesToDescribe);
                          neuronIndex++;
                      }
                  }
            }
            catch
            {
                throw;
            }
        }

                                                            // Writes a description of a
                                                            // neuron's synapses to a file
        static void writeSynapses(StreamWriter netWriter, 
            LinkedList<Synapse> synapsesToDescribe)
        {
            foreach (Synapse synapse in synapsesToDescribe)
            {
                netWriter.WriteLine(" " + synapse.toString());
            }
        }*/
    }
}
