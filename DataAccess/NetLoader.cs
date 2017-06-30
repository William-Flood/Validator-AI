using System;
using DataStructures;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrainStructures;
using System.IO;
using Exceptions;

namespace DataAccess
{
    public class NetLoader
    {

                                                                 // loadNet generates a neural net from a text file
        /*public static NetStructure loadNet(String fileDirectory, // The directory the file is located in
            String fileName,                                     // The name of the file
            Neuron.actionType[] actionArray                      // The actions the neuron can take
            ) {
            NetStructure returnNet = new NetStructure();
                                                                 // NEURON_THRESSHOLD represents the threshhold 
                                                                 // for neuron firing
            const int NEURON_THRESHHOLD = NetStructure.SUGGESTED_NEURON_THRESHHHOLD;
            try
            {
                using (StreamReader netReader = new StreamReader(fileDirectory + fileName))
                {
                                                                // lineOn is used for error reporting - if 
                                                                // a specific line causesparsing to fail,
                                                                // it can be reported to the user.
                    int lineOn = 1;
                    String intermediatesPresentDescription = netReader.ReadLine();
                    String intermediatesPresentWritten = intermediatesPresentDescription.Split(' ')[2];
                    int intermediatesPresent;
                    bool parseSucceeded = int.TryParse(intermediatesPresentWritten, out intermediatesPresent);
                    if (false == parseSucceeded)
                    {
                        throw new FileCorruptionException(lineOn);
                    }
                    returnNet.intermediates = new LinkedList<Neuron>();
                    for (int i = 0; i < intermediatesPresent; i++)
                    {
                        returnNet.intermediates.addAtStart(new Neuron(NEURON_THRESHHOLD));
                    }
                    returnNet.motorNeurons = new LinkedList<Neuron>();
                    for (int i = 0; i < actionArray.Count(); i++)
                    {
                        returnNet.motorNeurons.addAfter(new Neuron(NEURON_THRESHHOLD, actionArray[i]));
                        returnNet.motorNeurons.moveUp();
                    }
                    BinarySearchTree<char, Neuron> rawSensoryMap = new BinarySearchTree<char,Neuron>(
                        NetStructure.charCompare);
                    String insertionLine = netReader.ReadLine();
                    lineOn++;
                    char? sensoryNeuronValue = null;
                    while (false == (insertionLine[0].Equals('I') || insertionLine[0].Equals('M')))
                    {
                        if (insertionLine[0].Equals('S'))
                        {
                            if (insertionLine.Count() > 1)
                            {
                                sensoryNeuronValue = insertionLine[1];
                                rawSensoryMap.add((char)sensoryNeuronValue, new Neuron(0));
                            }
                            else
                            {
                                sensoryNeuronValue = (char)netReader.Read();
                                rawSensoryMap.add((char)sensoryNeuronValue, new Neuron(0));
                                insertionLine = netReader.ReadLine();
                            }
                        }
                        else
                        {
                            Neuron neuronToAdd = getNeuronFromSynapse(insertionLine, returnNet, lineOn, fileName);
                            bool toExcite = getSynapseDirection(insertionLine, returnNet, lineOn, fileName);
                            rawSensoryMap.search((char)sensoryNeuronValue).newSynapse(neuronToAdd, toExcite);
                        }
                            insertionLine = netReader.ReadLine();
                            lineOn++;

                    }
                    returnNet.sensoryMap = new BinarySearchTree<char, Neuron>(rawSensoryMap);
                    LinkedList<Neuron> intermediatesFilling = new LinkedList<Neuron>(returnNet.intermediates);
                    while (false == insertionLine[0].Equals('M'))
                    {
                        if (insertionLine[0].Equals('I'))
                        {
                            intermediatesFilling.moveUp();
                        }
                        else
                        {
                            Neuron neuronToAdd = getNeuronFromSynapse(insertionLine, returnNet, lineOn, fileName);
                            bool toExcite = getSynapseDirection(insertionLine, returnNet, lineOn, fileName);
                            intermediatesFilling.value().newSynapse(neuronToAdd, toExcite);
                        }
                        insertionLine = netReader.ReadLine();
                        lineOn++;
                    }
                }
            }
            catch
            {
                throw;
            }
            return returnNet;
        }

                                                            // Returns a neuron based on the a written
                                                            // description of its location
        static Neuron getNeuronFromSynapse(string insertionLine,
            NetStructure netOfNeuron,
            int lineOn,
            string fileName)
        {
            String stringOfIndex = insertionLine.Substring(3);
            int indexOfNeuron;
            bool parseSucceeded = int.TryParse(stringOfIndex, out indexOfNeuron);
            if (false == parseSucceeded)
            {
                throw new FileCorruptionException(lineOn);
            }
            if (insertionLine[2].Equals('I'))
            {
                return netOfNeuron.intermediates[indexOfNeuron];
            }
            else if (insertionLine[2].Equals('M'))
            {
                return netOfNeuron.motorNeurons[indexOfNeuron];
            }
            else
            {
                throw new FileCorruptionException(lineOn);
            }
        }

                                                            //Determines if a given synapse should be
                                                            //excitatory or inhibitory
        static bool getSynapseDirection(string insertionLine, NetStructure netOfNeuron, int lineOn, string fileName)
        {
            if (insertionLine[1].Equals('+'))
            {
                return true;
            }
            else if (insertionLine[1].Equals('-'))
            {
                return false;
            }
            else
            {
                throw new FileCorruptionException(lineOn);
            }
        }

        public static BinarySearchTree<String, NetStructure> loadCommitee(String fileDirectory, // The directory the file is located in
            String CommiteeDirectoryName,
            Neuron.actionType[] actionArray                      // The actions the neuron can take
            )
        {
            var members = Directory.GetFiles(fileDirectory + "/" + CommiteeDirectoryName);
            BinarySearchTree<String, NetStructure> commitee = new BinarySearchTree<string, NetStructure>(String.Compare);
            foreach (var member in members)
            {
                commitee.add(member, loadNet(fileDirectory, member, actionArray));
            }
            return commitee;
        }*/
    }
}
