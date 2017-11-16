using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic;
using BrainStructures;
using DataStructures;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        //[TestMethod]
        //public void TestMethod1()
        //{
        //}

        [TestMethod]
        public void CheckNeuralNetCalculation()
        {
            AppData.TrainingDocumentsPath = System.AppDomain.CurrentDomain.BaseDirectory + @"\trainingfiles\";
            var creationPot = new FlowerPot() { NeuronActions = new Neuron.actionType[] { ()=>{ }, () => { } }, TrainingDocumentName = "test.txt" };
            var netsToTest = new NetStructure[1];
            Mutator generatorMutator = new Mutator(.1, 0, .9, .01, .8);
            int maxIntermediateLength = 0;
            for(int i = 0; i<netsToTest.Length; i++)
            {
                netsToTest[i] = creationPot.CreateNet().Components[0];
                LinkedList<INode> intermediatesClone = new LinkedList<INode>();
                int j = 0;
                foreach (INode cloningNeuron in netsToTest[i].Intermediates)
                {
                    if (!cloningNeuron.MarkedForDeletion)
                    {
                        intermediatesClone[j] = cloningNeuron.GetClone(i);
                        cloningNeuron.FillClone(i);
                        j++;
                    }
                }
                netsToTest[i].TempIntermediates = intermediatesClone;

                for (int k = 0; k < 10; k++)
                {
                    generatorMutator.MutateNetwork(netsToTest[i]);
                }
                if(netsToTest[i].Intermediates.Length > maxIntermediateLength)
                {
                    maxIntermediateLength = netsToTest[i].Intermediates.Length;
                }
            }
            var sensoryCount = netsToTest[0].SensoryList.Length;
            var motorCount = 2;
            //var blockCount = CUDAWrapper.CalculateBlockCount(motorCount + maxIntermediateLength);
            var blockCount = 4;
            var sensoryGridCount = sensoryCount * (maxIntermediateLength + motorCount) * netsToTest.Length;
            var intermediateGridCount = maxIntermediateLength * (maxIntermediateLength + motorCount) * netsToTest.Length;
            var tallyGridCount = (maxIntermediateLength + motorCount) * netsToTest.Length;
            var flatNetGrid = new int[sensoryGridCount + intermediateGridCount + tallyGridCount + tallyGridCount*blockCount];
        }
    }
}
