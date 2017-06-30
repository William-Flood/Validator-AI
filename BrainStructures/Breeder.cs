using System;
using DataStructures;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainStructures
{
                                                            // Creates a neural net given the
                                                            // structure of two others.
    /*public class Breeder
    {
        NetSequence sequence;
        int intermediateSize, motorSize;

                                                            // Wraps the constructor for a linked list in
                                                            // a method matching the delegate needed
                                                            // to construct a binary searchtree.
        LinkedList<SynapseSequence> sendList(int i)
        {
            return new LinkedList<SynapseSequence>();
        }

                                                            // Takes two neural nets and compares them.
        public void sequenceNets(Collective father, Collective mother)
        {
            sequence = new NetSequence();
            if (father.intermediates.Length() > mother.intermediates.Length())
            {
                intermediateSize = father.intermediates.Length();
            }
            else
            {
                intermediateSize = mother.intermediates.Length();
            }

            motorSize = mother.motorNeurons.Length();

            sequence.sensoryMap = new BinarySearchTree<char, LinkedList<SynapseSequence>>(father.sensoryMap.getSize(),
                (i) => father.sensoryMap[i], (i) => sendList(i), NetStructure.charCompare);

            sequence.intermediates = new LinkedList<LinkedList<SynapseSequence>>();
            for (int i = 0; i < intermediateSize; i++)
            {
                sequence.intermediates.AddAtStart(new LinkedList<SynapseSequence>());
            }

            for (int i = 0; i < father.sensoryMap.getSize(); i++)
            {
                for (int j = 0; j < intermediateSize; j++)
                {
                    int motherAction, fatherAction;
                    if (j < mother.intermediates.Length())
                    {
                        motherAction = mother.sensoryMap.valueAt(i).getSynapsesFor(mother.intermediates[j]);
                    }
                    else
                    {
                        motherAction = 0;
                    }

                    if (j < father.intermediates.Length())
                    {
                        fatherAction = father.sensoryMap.valueAt(i).getSynapsesFor(father.intermediates[j]);
                    }
                    else
                    {
                        fatherAction = 0;
                    }

                    if (false == (fatherAction == 0 &&
                        0 == motherAction))
                    {
                        sequence.sensoryMap.valueAt(i).AddAtStart(new SynapseSequence(
                        Function.Intermediate,
                        j,
                        motherAction,
                        fatherAction
                        ));
                    }
                }
                for (int j = 0; j < motorSize; j++)
                {
                    int motherAction, fatherAction;
                    if (j < mother.intermediates.Length())
                    {
                        motherAction = mother.sensoryMap.valueAt(i).getSynapsesFor(mother.motorNeurons[j]);
                    }
                    else
                    {
                        motherAction = 0;
                    }

                    if (j < father.intermediates.Length())
                    {
                        fatherAction = father.sensoryMap.valueAt(i).getSynapsesFor(father.motorNeurons[j]);
                    }
                    else
                    {
                        fatherAction = 0;
                    }

                    if (false == (fatherAction == 0 &&
                        0 == motherAction))
                    {
                        sequence.sensoryMap.valueAt(i).AddAtStart(new SynapseSequence(
                            Function.Motor,
                            j,
                            motherAction,
                            fatherAction
                            ));
                    }
                }
            }

            var motherBuilding = new LinkedList<Neuron>(mother.intermediates);
            var fatherBuilding = new LinkedList<Neuron>(father.intermediates);
            for(int i = 0; i < intermediateSize; i++)
            {

                for (int j = 0; j < intermediateSize; j++)
                {
                    int motherAction, fatherAction;
                    if (j < mother.intermediates.Length() && i < mother.intermediates.Length())
                    {
                        motherAction = motherBuilding[i].getSynapsesFor(mother.intermediates[j]);
                    }
                    else
                    {
                        motherAction = 0;
                    }

                    if (j < father.intermediates.Length() && i < father.intermediates.Length())
                    {
                        fatherAction = fatherBuilding[i].getSynapsesFor(father.intermediates[j]);
                    }
                    else
                    {
                        fatherAction = 0;
                    }

                    if (false == (fatherAction == 0 &&
                        0 == motherAction))
                    {
                        sequence.intermediates[i].AddAtStart(new SynapseSequence(
                        Function.Intermediate,
                        j,
                        motherAction,
                        fatherAction
                        ));
                    }
                }
                for (int j = 0; j < motorSize; j++)
                {
                    int motherAction, fatherAction;
                    if (j < mother.motorNeurons.Length() && i < mother.intermediates.Length())
                    {
                        motherAction = motherBuilding[i].getSynapsesFor(mother.motorNeurons[j]);
                    }
                    else
                    {
                        motherAction = 0;
                    }

                    if (j < father.motorNeurons.Length() && i < father.intermediates.Length())
                    {
                        fatherAction = fatherBuilding[i].getSynapsesFor(father.motorNeurons[j]);
                    }
                    else
                    {
                        fatherAction = 0;
                    }

                    if (false == (fatherAction == 0 &&
                        0 == motherAction))
                    {
                        sequence.intermediates[i].AddAtStart(new SynapseSequence(
                            Function.Motor,
                            j,
                            motherAction,
                            fatherAction
                            ));
                    }
                }
            }
        }

                                                            // Creates a neural net based off of
                                                            // two others.
        public NetStructure breed()
        {
            Random rng = new Random();
            NetStructure returnNet = new NetStructure();
            var rawMap = new BinarySearchTree<char, Neuron>(NetStructure.charCompare);

            returnNet.intermediates = new LinkedList<Neuron>();
            returnNet.motorNeurons = new LinkedList<Neuron>();
            for(int i = 0; i < intermediateSize; i++)
            {
                returnNet.intermediates.AddAtStart(new Neuron(NetStructure.SUGGESTED_NEURON_THRESHHHOLD));
            }

            for (int i = 0; i < motorSize; i++)
            {
                returnNet.motorNeurons.AddAtStart(new Neuron(NetStructure.SUGGESTED_NEURON_THRESHHHOLD));
            }

            for (int i = 0; i < sequence.sensoryMap.getSize(); i++)
            {
                Neuron appending = new Neuron(0);
                foreach(SynapseSequence neuronSequence in sequence.sensoryMap.valueAt(i))
                {
                    int min, max;
                    if(neuronSequence.mother > neuronSequence.father)
                    {
                        max = neuronSequence.mother;
                        min = neuronSequence.father;
                    }
                    else
                    {
                        max = neuronSequence.father;
                        min = neuronSequence.mother;
                    }
                    int synapseCount = rng.Next(min, max + 1);
                    if (synapseCount > 0)
                    {
                        for (int j = 0; j < synapseCount; j++)
                        {
                            if (Function.Intermediate == neuronSequence.function)
                            {
                                appending.NewSynapse(returnNet.intermediates[neuronSequence.index],
                                    true);
                            }
                            else
                            {
                                appending.NewSynapse(returnNet.motorNeurons[neuronSequence.index],
                                    true);
                            }
                        }
                    }
                    else if (synapseCount < 0)
                    {
                        for (int j = 0; j < -synapseCount; j++)
                        {
                            if (Function.Intermediate == neuronSequence.function)
                            {
                                appending.NewSynapse(returnNet.intermediates[neuronSequence.index],
                                    false);
                            }
                            else
                            {
                                appending.NewSynapse(returnNet.motorNeurons[neuronSequence.index],
                                    false);
                            }
                        }
                    }
                }
                rawMap.add(sequence.sensoryMap[i], appending);
            }

            returnNet.sensoryMap = new BinarySearchTree<char, Neuron>(rawMap);

            var building = new LinkedList<Neuron>(returnNet.intermediates);

            for(int i = 0; i < intermediateSize; i++) 
            {
                foreach (SynapseSequence neuronSequence in sequence.intermediates[i])
                {
                    int min, max;
                    if (neuronSequence.mother > neuronSequence.father)
                    {
                        max = neuronSequence.mother;
                        min = neuronSequence.father;
                    }
                    else
                    {
                        max = neuronSequence.father;
                        min = neuronSequence.mother;
                    }
                    int synapseCount = rng.Next(min, max + 1);
                    if (synapseCount > 0)
                    {
                        for (int j = 0; j < synapseCount; j++)
                        {
                            if (Function.Intermediate == neuronSequence.function)
                            {
                                returnNet.intermediates[i].NewSynapse(
                                    returnNet.intermediates[neuronSequence.index],
                                    true);
                            }
                            else
                            {
                                returnNet.intermediates[i].NewSynapse(
                                    returnNet.motorNeurons[neuronSequence.index],
                                    true);
                            }
                        }
                    }
                    else if (synapseCount < 0)
                    {
                        for (int j = 0; j < -synapseCount; j++)
                        {
                            if (Function.Intermediate == neuronSequence.function)
                            {
                                returnNet.intermediates[i].NewSynapse(
                                    returnNet.intermediates[neuronSequence.index],
                                    false);
                            }
                            else
                            {
                                returnNet.intermediates[i].NewSynapse(
                                    returnNet.motorNeurons[neuronSequence.index],
                                    false);
                            }
                        }
                    }
                }
            }

            return returnNet;
        }


        

    }*/
}
