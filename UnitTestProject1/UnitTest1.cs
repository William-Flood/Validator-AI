using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic;
using BrainStructures;
using DataStructures;
using System.Runtime.InteropServices;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void CheckCUDANeuralNetCalculation()
        {
            AppData.TrainingDocumentsPath = System.AppDomain.CurrentDomain.BaseDirectory + @"\trainingfiles\";
            var creationPot = new FlowerPot()
            {
                NeuronActions = new Neuron.actionType[] { () => { }, () => { } },
                TrainingDocumentName = "test.txt",
                StartingIntermediates = 50
            };
            var netsToTest = new NetStructure[80];
            Mutator generatorMutator = new Mutator(0, 0, .9, .01, .8);
            int maxIntermediateLength = 0;
            for (int i = 0; i < netsToTest.Length; i++)
            {
                var collectiveWrapper = creationPot.CreateNet();
                netsToTest[i] = collectiveWrapper.Components[0];
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

                for (int k = 0; k < 30; k++)
                {
                    netsToTest[i].TempIntermediates.ResetPointer();
                    generatorMutator.MutateNetwork(netsToTest[i]);
                }
                FlowerPot.HardenNet(collectiveWrapper);
                if (netsToTest[i].Intermediates.Length > maxIntermediateLength)
                {
                    maxIntermediateLength = netsToTest[i].Intermediates.Length;
                }
            }
            var sensoryCount = netsToTest[0].SensoryList.Length;
            var motorCount = 2;
            var blockCount = CUDAWrapper.CalculateBlockCount(motorCount + maxIntermediateLength);
            var connectedCount = maxIntermediateLength + motorCount;
            var sensoryGridCount = sensoryCount * (connectedCount) * netsToTest.Length;
            var intermediateGridCount = maxIntermediateLength * (connectedCount) * netsToTest.Length;
            var tallyGridCount = (maxIntermediateLength + motorCount) * netsToTest.Length;
            var flatNetGrid = new int[sensoryGridCount + intermediateGridCount + tallyGridCount + tallyGridCount * blockCount];

            var SensoryBrick = new int[connectedCount, sensoryCount, netsToTest.Length];
            var IntermediateBrick = new int[connectedCount, maxIntermediateLength, netsToTest.Length];
            var tempTallyGrid = new int[connectedCount, netsToTest.Length];
            var tallyGrid = new int[connectedCount, netsToTest.Length];
            for (var netIterator = 0; netIterator < netsToTest.Length; netIterator++)
            {
                var sensoryList = netsToTest[netIterator].SensoryGrid;
                for (int i = 0; i < sensoryList.GetLength(1); i++)
                {
                    var synapseRow = LinearNetRunner.WriteRow(sensoryList, i);
                }
                for (var targetIterator = 0; targetIterator < sensoryList.GetLength(0); targetIterator++)
                {
                    for (var firingIterator = 0; firingIterator < sensoryList.GetLength(1); firingIterator++)
                    {
                        SensoryBrick[targetIterator, firingIterator, netIterator] = sensoryList[targetIterator, firingIterator];
                        flatNetGrid[targetIterator + firingIterator * connectedCount + netIterator * sensoryCount * connectedCount] = sensoryList[targetIterator, firingIterator];
                        //SensoryBrick[targetIterator, firingIterator, netIterator] = 0;
                        //flatNetGrid[targetIterator + firingIterator * connectedCount + netIterator * sensoryCount * connectedCount] = 0;
                    }
                }
            }

            var flatIntermedidateStart = netsToTest.Length * sensoryCount * connectedCount;
            for (var netIterator = 0; netIterator < netsToTest.Length; netIterator++)
            {
                var intermediateList = netsToTest[netIterator].IntermediateGrid;

                for (int i = 0; i < intermediateList.GetLength(1); i++)
                {
                    var synapseRow = LinearNetRunner.WriteRow(intermediateList, i);
                }
                for (var targetIterator = 0; targetIterator < intermediateList.GetLength(0); targetIterator++)
                {
                    for (var firingIterator = 0; firingIterator < intermediateList.GetLength(1); firingIterator++)
                    {
                        IntermediateBrick[targetIterator, firingIterator, netIterator] = intermediateList[targetIterator, firingIterator];
                        flatNetGrid[flatIntermedidateStart + targetIterator + firingIterator * connectedCount + netIterator * maxIntermediateLength * connectedCount] = intermediateList[targetIterator, firingIterator];
                    }
                }
            }
            var flatTallyStart = flatIntermedidateStart + netsToTest.Length * connectedCount * maxIntermediateLength;


            //var rng = new Random(59810687);
            var rngSeed = (new Random()).Next();
            var rng = new Random(rngSeed);
            //int neuronToTally;
            //var netToTally = rng.Next(netsToTest.Length);
            //neuronToTally = rng.Next(connectedCount);
            //tallyGrid[neuronToTally, netToTally] = 25;
            //flatNetGrid[flatTallyStart + neuronToTally + netToTally * connectedCount] = 25;
            //neuronToTally = rng.Next(connectedCount);
            //tallyGrid[neuronToTally, netToTally] = 25;
            //flatNetGrid[flatTallyStart + neuronToTally + netToTally * connectedCount] = 25;

            //int underTallied;
            //for (int i = 0; i < 1; i++)
            //{
            //    neuronToTally = rng.Next(connectedCount);

            //    tallyGrid[neuronToTally, netToTally] = 25;
            //    underTallied = neuronToTally;
            //    flatNetGrid[flatTallyStart + neuronToTally + netToTally * connectedCount] = 25;
            //}
            //neuronToTally = rng.Next(connectedCount);
            //tallyGrid[neuronToTally, netToTally] = 25;
            //flatNetGrid[flatTallyStart + neuronToTally + netToTally * connectedCount] = 25;


            IntPtr netTransferBlock = CUDAWrapper.declare_transfer_block(flatNetGrid.Length);
            Marshal.Copy(flatNetGrid, 0, netTransferBlock, flatNetGrid.Length);
            IntPtr cudaSensoryBlock = CUDAWrapper.establish_net_block(netTransferBlock, flatNetGrid.Length);
            IntPtr cudaIntermediateBlock = CUDAWrapper.findIntermediateBlock(cudaSensoryBlock, sensoryCount, maxIntermediateLength, motorCount, netsToTest.Length);
            var cudaTallyBlock = CUDAWrapper.findTallyBlock(cudaIntermediateBlock, maxIntermediateLength, motorCount, netsToTest.Length);
            var cudaTempTallyBlock = CUDAWrapper.findTempTallyBlock(cudaTallyBlock, maxIntermediateLength, motorCount, netsToTest.Length);


            for (int sensoryInputIndex = 0; sensoryInputIndex < 1; sensoryInputIndex++)
            {

                //Net index: 5, Neuron index: 0
                var sensoryInput = new int[100];
                for(int i = 0; i< sensoryInput.Length; i++)
                {
                    sensoryInput[i] = rng.Next(sensoryCount);
                }
                foreach (var inputValue in sensoryInput) //Execution split up to judge execution times.
                {
                    LinearNetRunner.RunNet(SensoryBrick, IntermediateBrick, tallyGrid, inputValue, 20);
                }
                IntPtr sensoryTransferBlock = CUDAWrapper.declare_transfer_block(sensoryInput.Length);
                Marshal.Copy(sensoryInput, 0, sensoryTransferBlock, sensoryInput.Length);
                var sensoryAddress = CUDAWrapper.establish_sensory_input(sensoryTransferBlock, sensoryInput.Length);


                CUDAWrapper.runCycle(sensoryCount, maxIntermediateLength, motorCount, netsToTest.Length, cudaSensoryBlock, cudaIntermediateBlock, cudaTallyBlock, cudaTempTallyBlock, sensoryAddress, sensoryInput.Length);


                var returnedTally = CUDAWrapper.getNet(cudaTallyBlock, connectedCount * netsToTest.Length);
                var tallyResults = new int[connectedCount * netsToTest.Length];
                var returnedTempTally = CUDAWrapper.getNet(cudaTempTallyBlock, connectedCount * netsToTest.Length * blockCount);
                var tempTallyResults = new int[connectedCount * netsToTest.Length * blockCount];
                var returnedNet = CUDAWrapper.getNet(cudaSensoryBlock, flatNetGrid.Length);
                var netResults = new int[flatNetGrid.Length];
                Marshal.Copy(returnedTally, tallyResults, 0, connectedCount * netsToTest.Length);
                Marshal.Copy(returnedTempTally, tempTallyResults, 0, connectedCount * netsToTest.Length * blockCount);
                Marshal.Copy(returnedNet, netResults, 0, flatNetGrid.Length);


                CUDAWrapper.release(returnedTally);
                CUDAWrapper.release(returnedNet);
                CUDAWrapper.release(returnedTempTally);
                CUDAWrapper.release(sensoryTransferBlock);
                //CUDAWrapper.cuda_release(cudaSensoryBlock);
                CUDAWrapper.cuda_release(sensoryAddress);

                for (int i = 0; i < flatTallyStart; i++)
                {
                    try
                    {
                        Assert.AreEqual(flatNetGrid[i], netResults[i]);
                    }
                    catch
                    {
                        throw;
                    }
                }

                var offcount = 0;
                var offBy = 0;

                for (int netIndex = 0; netIndex < netsToTest.Length; netIndex++)
                {
                    for (var talliedNeuron = 0; talliedNeuron < connectedCount; talliedNeuron++)
                    {
                        if (tallyGrid[talliedNeuron, netIndex] != tallyResults[talliedNeuron + netIndex * connectedCount])
                        {
                            offcount++;
                            offBy = tallyResults[talliedNeuron + netIndex * connectedCount] - tallyGrid[talliedNeuron, netIndex];
                            tallyGrid[talliedNeuron, netIndex] = tallyResults[talliedNeuron + netIndex * connectedCount];
                        }
                        //try
                        //{
                        //    Assert.AreEqual(tallyGrid[talliedNeuron, netIndex], tallyResults[talliedNeuron + netIndex * connectedCount]);
                        //} catch
                        //{
                        //    CUDAWrapper.release(netTransferBlock);
                        //    CUDAWrapper.cuda_release(cudaSensoryBlock);
                        //    throw new Exception("At net " + netIndex + ", neuron " + talliedNeuron);
                        //}

                    }
                }
                if (0 < offcount)
                {
                    CUDAWrapper.release(netTransferBlock);
                    CUDAWrapper.cuda_release(cudaSensoryBlock);
                    throw new Exception(offcount + " neurons off by " + offBy + " at sensory input index" + sensoryInputIndex);
                }
            }
            CUDAWrapper.release(netTransferBlock);
            CUDAWrapper.cuda_release(cudaSensoryBlock);
        }


        //[TestMethod]
        //public void CheckLinearNetComputation()
        //{
        //
        //    AppData.TrainingDocumentsPath = System.AppDomain.CurrentDomain.BaseDirectory + @"\trainingfiles\";
        //    var creationPot = new FlowerPot()
        //    {
        //        NeuronActions = new Neuron.actionType[] { () => { }, () => { } },
        //        TrainingDocumentName = "test.txt",
        //        StartingIntermediates = 25
        //    };
        //    var netsToTest = new NetStructure[10];
        //    Mutator generatorMutator = new Mutator(0, 0, .9, .01, .8);
        //    int maxIntermediateLength = 0;
        //    for (int i = 0; i < netsToTest.Length; i++)
        //    {
        //        var collectiveWrapper = creationPot.CreateNet();
        //        netsToTest[i] = collectiveWrapper.Components[0];
        //        LinkedList<INode> intermediatesClone = new LinkedList<INode>();
        //        int j = 0;
        //        foreach (INode cloningNeuron in netsToTest[i].Intermediates)
        //        {
        //            if (!cloningNeuron.MarkedForDeletion)
        //            {
        //                intermediatesClone[j] = cloningNeuron.GetClone(i);
        //                cloningNeuron.FillClone(i);
        //                j++;
        //            }
        //        }
        //        netsToTest[i].TempIntermediates = intermediatesClone;
        //
        //        for (int k = 0; k < 30; k++)
        //        {
        //            netsToTest[i].TempIntermediates.ResetPointer();
        //            generatorMutator.MutateNetwork(netsToTest[i]);
        //        }
        //        FlowerPot.HardenNet(collectiveWrapper);
        //        if (netsToTest[i].Intermediates.Length > maxIntermediateLength)
        //        {
        //            maxIntermediateLength = netsToTest[i].Intermediates.Length;
        //        }
        //    }
        //    var sensoryCount = netsToTest[0].SensoryList.Length;
        //    var motorCount = 2;
        //    //var blockCount = CUDAWrapper.CalculateBlockCount(motorCount + maxIntermediateLength);
        //    var blockCount = 4;
        //    var connectedCount = maxIntermediateLength + motorCount;
        //    var sensoryGridCount = sensoryCount * (connectedCount) * netsToTest.Length;
        //    var intermediateGridCount = maxIntermediateLength * (connectedCount) * netsToTest.Length;
        //    var tallyGridCount = (maxIntermediateLength + motorCount) * netsToTest.Length;
        //    var flatNetGrid = new int[sensoryGridCount + intermediateGridCount + tallyGridCount + tallyGridCount * blockCount];
        //
        //    var SensoryBrick = new int[connectedCount, sensoryCount, netsToTest.Length];
        //    var IntermediateBrick = new int[connectedCount, maxIntermediateLength, netsToTest.Length];
        //    var tempTallyGrid = new int[connectedCount, netsToTest.Length];
        //    var tallyGrid = new int[connectedCount, netsToTest.Length];
        //    for (var netIterator = 0; netIterator < netsToTest.Length; netIterator++)
        //    {
        //        var sensoryList = netsToTest[netIterator].SensoryGrid;
        //        for(int i = 0; i<sensoryList.GetLength(1); i++)
        //        {
        //            var synapseRow = LinearNetRunner.WriteRow(sensoryList, i);
        //        }
        //        for (var targetIterator = 0; targetIterator < sensoryList.GetLength(0); targetIterator++)
        //        {
        //            for (var firingIterator = 0; firingIterator < sensoryList.GetLength(1); firingIterator++)
        //            {
        //                SensoryBrick[targetIterator, firingIterator, netIterator] = sensoryList[targetIterator, firingIterator];
        //                flatNetGrid[targetIterator + firingIterator * connectedCount + netIterator * sensoryCount * connectedCount] = sensoryList[targetIterator, firingIterator];
        //            }
        //        }
        //    }
        //
        //    var flatIntermedidateStart = netsToTest.Length * sensoryCount * connectedCount;
        //    for (var netIterator = 0; netIterator < netsToTest.Length; netIterator++)
        //    {
        //        var intermediateList = netsToTest[netIterator].IntermediateGrid;
        //
        //        for (int i = 0; i < intermediateList.GetLength(1); i++)
        //        {
        //            var synapseRow = LinearNetRunner.WriteRow(intermediateList, i);
        //        }
        //        for (var targetIterator = 0; targetIterator < intermediateList.GetLength(0); targetIterator++)
        //        {
        //            for (var firingIterator = 0; firingIterator < intermediateList.GetLength(1); firingIterator++)
        //            {
        //                IntermediateBrick[targetIterator, firingIterator, netIterator] = intermediateList[targetIterator, firingIterator];
        //                flatNetGrid[flatIntermedidateStart + targetIterator + firingIterator * connectedCount + netIterator * sensoryCount * connectedCount] = intermediateList[targetIterator, firingIterator];
        //            }
        //        }
        //    }
        //
        //    var rng = new Random();
        //    var sensoryInput = new int[100000];
        //    for (int i = 0; i < sensoryInput.Length; i++)
        //    {
        //        sensoryInput[i] = rng.Next(sensoryCount);
        //    }
        //
        //    var appendMode = false;
        //    foreach (var inputValue in sensoryInput) //Execution split up to judge execution times.
        //    {
        //
        //        LinearNetRunner.RunNet(SensoryBrick, IntermediateBrick, tallyGrid, inputValue, 20);
        //        appendMode = true;
        //    }
        //}
    }
}
