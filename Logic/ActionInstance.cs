using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrainStructures;
using DataStructures;


namespace Logic
{
    // Runs the logic cycle of a
    // neural net in training.
    class ActionInstance
    {
        // Indicates if the neural net
        // produced a correct response.
        bool guessedCorrectly;

        // Used to halt the logic cycle
        // if the neural net has already
        // proposed a guess.
        bool continueTest;

        // Indicates if the instance is
        // still performing tests.
        bool runLatch;

        // Holds a list of characters
        // used to test a neural net.
        TestAgainst testAccomplice;

        // A list of neural nets to be
        // tested.
        EvaluatedNetwork[] trainingList;
        int[,,] SensoryBrick;
        int[,,] IntermediateBrick;
        int[,] tallyGrid;
        int connectedCount;
        int maxIntermediateLength;

        // Sets the testAccomplice
        public void getTestAccomplice(TestAgainst toCopy)
        {
            this.testAccomplice = new TestAgainst(toCopy);
        }

        // Creates an ActionInstance, along
        // with a list of neural nets to
        // train.
        public ActionInstance(LinkedList<EvaluatedNetwork> toClone, Mutator trainingMutator, int j)
        {
            runLatch = false;
            trainingList = new EvaluatedNetwork[10];
            //const double SELECTPROB = 0.8;
            //Breeder breeder = new Breeder();
            maxIntermediateLength = 0;
            var cloningVat = new FlowerPot() { NeuronActions = new Neuron.actionType[] { this.SendFalse, this.SendTrue }};
            for (int k = 0; k < 10; k++)
            {          
                trainingList[k] = 
                    new EvaluatedNetwork { Trainee = cloningVat.CloneNet(toClone[k].Trainee, j) };
                trainingMutator.MutateNetwork(trainingList[k].Trainee.Components[0]);
                FlowerPot.HardenNet(trainingList[k].Trainee);

                if (trainingList[k].Trainee.Components[0].Intermediates.Length > maxIntermediateLength)
                {
                    maxIntermediateLength = trainingList[k].Trainee.Components[0].Intermediates.Length;
                }
            }
            var sensoryCount = trainingList[0].Trainee.SensoryMap.getSize();
            var motorCount = 2;
            connectedCount = maxIntermediateLength + motorCount;
            SensoryBrick = new int[connectedCount, sensoryCount, trainingList.Length];
            IntermediateBrick = new int[connectedCount, maxIntermediateLength, trainingList.Length];
            tallyGrid = new int[connectedCount, trainingList.Length];
            for (var netIterator = 0; netIterator < trainingList.Length; netIterator++)
            {
                var sensoryList = trainingList[netIterator].Trainee.Components[0].SensoryGrid;
                for (var targetIterator = 0; targetIterator < sensoryList.GetLength(0); targetIterator++)
                {
                    for (var firingIterator = 0; firingIterator < sensoryList.GetLength(1); firingIterator++)
                    {
                        SensoryBrick[targetIterator, firingIterator, netIterator] = sensoryList[targetIterator, firingIterator];
                    }
                }
            }

            var flatIntermedidateStart = trainingList.Length * sensoryCount * connectedCount;
            for (var netIterator = 0; netIterator < trainingList.Length; netIterator++)
            {
                var intermediateList = trainingList[netIterator].Trainee.Components[0].IntermediateGrid;

                for (var targetIterator = 0; targetIterator < intermediateList.GetLength(0); targetIterator++)
                {
                    for (var firingIterator = 0; firingIterator < intermediateList.GetLength(1); firingIterator++)
                    {
                        IntermediateBrick[targetIterator, firingIterator, netIterator] = intermediateList[targetIterator, firingIterator];
                    }
                }
            }
        }

        // Allows a neural net to propose that a
        // given string resembles what it has been
        // trained to recognize.
        public void SendTrue()
        {
            guessedCorrectly = testAccomplice.isReal;
            continueTest = false;
        }

        // Allows a neural net to propose that a
        // given string does not resemble what it has
        // been trained to recognize.
        public void SendFalse()
        {
            guessedCorrectly = (false == testAccomplice.isReal);
            continueTest = false;
        }

        /*
        // Produces a clone of a neural net.
        public NetStructure CloneNet(NetStructure cloningNet, int i)
        {
            NetStructure returnNet = new NetStructure();
            returnNet = new NetStructure();
            NetStructure cloning = returnNet;
            cloning.SensoryList = new LinkedList<Neuron>();
            for (int j = 0; j < cloningNet.SensoryList.Length; j++)
            {
                cloning.SensoryList.AddAfter((Neuron)cloningNet.SensoryList[j].GetClone(i));
                cloningNet.SensoryList[j].FillClone(i);
                cloning.SensoryList.MoveUp();
            }
            cloning.Intermediates = CloneNetPiece(i, cloningNet.Intermediates);
            cloning.MotorList = CloneNetPiece(i, cloningNet.MotorList);
            cloning.MotorList[0].SetAction(this.sendFalse);
            cloning.MotorList[1].SetAction(this.sendTrue);
            return returnNet;
        }*/

        // Produces a clone of a linked list of neurons.
        static LinkedList<INode> CloneNetPiece(int i, LinkedList<INode> toClone)
        {
            LinkedList<INode> clone = new LinkedList<INode>();
            int j = 0;
            foreach (INode cloningNeuron in toClone)
            {
                if (!cloningNeuron.MarkedForDeletion)
                {
                    clone[j] = cloningNeuron.GetClone(i);
                    cloningNeuron.FillClone(i);
                    j++;
                }
            }
            return clone;
        }

        // Produces a clone of a linked list of neurons.
        static LinkedList<Neuron> CloneNetPiece(int i, LinkedList<Neuron> toClone)
        {
            var clone = new LinkedList<Neuron>();
            int j = 0;
            foreach (INode cloningNeuron in toClone)
            {
                if (!cloningNeuron.MarkedForDeletion)
                {
                    clone[j] = (Neuron)cloningNeuron.GetClone(i);
                    cloningNeuron.FillClone(i);
                    j++;
                }
            }
            return clone;
        }

        // Performs the logic of a neural net for
        // a list of neural nets.
        public void runTrainingCycle()
        {
            runLatch = false;

            var testData = new int[testAccomplice.TestArray.Length];
            var testIndex = 0;
            foreach (var character in testAccomplice.TestArray)
            {
                testData[testIndex] = trainingList[0].Trainee.SensoryMap.IndexOf(character);
                testIndex++;
            }
            foreach (var inputValue in testData) //Execution split up to judge execution times.
            {
                LinearNetRunner.RunNet(SensoryBrick, IntermediateBrick, tallyGrid, inputValue, 20);
            }
            var motorIndex = maxIntermediateLength;
            var netIndex = 0;
            foreach (EvaluatedNetwork candidateNet in trainingList)
            {
                var trueCount = tallyGrid[motorIndex, netIndex];
                var falseCount = tallyGrid[motorIndex + 1, netIndex];
                var guessedTrue = trueCount >= falseCount;
                var guessedCorrectly = guessedTrue;
                if (!testAccomplice.isReal)
                {
                    guessedCorrectly = !guessedCorrectly;
                }
                if (guessedCorrectly)
                {
                    candidateNet.TimesCorrect += 1;
                }
                netIndex++;
            }
            runLatch = true;
        }
        

        // Indicates that the instance has
        // completed all needed tests
        public bool TrainingDone()
        {
            return runLatch;
        }

        // Adds the neural nets from this instance
        // to a linked list.
        public void AddNets(LinkedList<EvaluatedNetwork> toAddTo)
        {
            foreach (EvaluatedNetwork netToAdd in trainingList)
            {
                toAddTo.AddAtStart(netToAdd);
            }
        }
    }
}