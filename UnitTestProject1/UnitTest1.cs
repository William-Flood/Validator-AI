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
            var connectedCount = maxIntermediateLength + motorCount;
            var sensoryGridCount = sensoryCount * (connectedCount) * netsToTest.Length;
            var intermediateGridCount = maxIntermediateLength * (connectedCount) * netsToTest.Length;
            var tallyGridCount = (maxIntermediateLength + motorCount) * netsToTest.Length;
            var flatNetGrid = new int[sensoryGridCount + intermediateGridCount + tallyGridCount + tallyGridCount*blockCount];

            var SensoryBrick = new int[sensoryCount, connectedCount, netsToTest.Length];
            var IntermediateBrick = new int[maxIntermediateLength, connectedCount, netsToTest.Length];
            var tempTallyGrid = new int[connectedCount, netsToTest.Length];
            var tallyGrid = new int[connectedCount, netsToTest.Length];
            for (var netIterator =  0; netIterator<netsToTest.Length; netIterator++ )
            {
                var sensoryList = netsToTest[netIterator].SensoryGrid;

                for (var firingIterator = 0; firingIterator < sensoryList.GetLength(0); firingIterator++)
                {
                    for(var targetIterator = 0; targetIterator< sensoryList.GetLength(1); targetIterator++)
                    {
                        SensoryBrick[targetIterator, firingIterator, netIterator] = sensoryList[firingIterator, targetIterator];
                        flatNetGrid[targetIterator + firingIterator * connectedCount + netIterator * sensoryCount * connectedCount] = sensoryList[firingIterator, targetIterator];
                    }
                }
            }

            var flatIntermedidateStart = netsToTest.Length * sensoryCount * connectedCount;
            for (var netIterator = 0; netIterator < netsToTest.Length; netIterator++)
            {
                var intermediateList = netsToTest[netIterator].IntermediateGrid;

                for (var firingIterator = 0; firingIterator < intermediateList.GetLength(0); firingIterator++)
                {
                    for (var targetIterator = 0; targetIterator < intermediateList.GetLength(1); targetIterator++)
                    {
                        IntermediateBrick[targetIterator, firingIterator, netIterator] = intermediateList[firingIterator, targetIterator];
                        flatNetGrid[flatIntermedidateStart + targetIterator + firingIterator * connectedCount + netIterator * sensoryCount * connectedCount] = intermediateList[firingIterator, targetIterator];
                    }
                }
            }

            var rng = new Random();
            var sensoryInput = new int[50];
            for(int i = 0; i< sensoryInput.Length; i++)
            {
                sensoryInput[i] = rng.Next(sensoryCount);
            }

            foreach(var inputValue in sensoryInput)
            {
                LinearNetRunner.RunNet(SensoryBrick, IntermediateBrick, tallyGrid, inputValue, 20);
            }
        }
    }
}
