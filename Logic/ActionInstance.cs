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
            Random rng = new Random();
            //const double SELECTPROB = 0.8;
            //Breeder breeder = new Breeder();
            var cloningVat = new FlowerPot() { NeuronActions = new Neuron.actionType[] { this.SendFalse, this.SendTrue }};
            for (int k = 0; k < 10; k++)
            {          
                trainingList[k] = 
                    new EvaluatedNetwork { Trainee = cloningVat.CloneNet(toClone[k].Trainee, j) };
                trainingMutator.MutateNetwork(trainingList[k].Trainee.Components[0]);
                FlowerPot.HardenNet(trainingList[k].Trainee);
                
                //bool motherNeeded = true;
                //Collective mother = null;
                //bool fatherNeeded = true;
                //Collective father = null;
                //int mateIndex = 0;
                //while (fatherNeeded)
                //{
                //    if(rng.NextDouble() < SELECTPROB)
                //    {
                //        if (motherNeeded)
                //        {
                //            motherNeeded = false;
                //            mother = toClone[mateIndex];
                //        }
                //        else
                //        {
                //            fatherNeeded = false;
                //            father = toClone[mateIndex];
                //        }
                //    }
                //    mateIndex = (mateIndex + 1) % toClone.Length;
                //}
                //breeder.sequenceNets(father, mother);
                //trainingList.AddAtStart(new EvaluatedNetwork {Trainee=breeder.breed()});
                //trainingMutator.mutate(trainingList[0].Trainee);
                //trainingList[0].Trainee.MotorList[0].SetAction(sendFalse);
                //trainingList[0].Trainee.MotorList[1].SetAction(sendTrue);
                //k++;
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
            foreach (EvaluatedNetwork candidateNet in trainingList)
            {
                continueTest = true;
                int trialsTaken = LogicCycle(candidateNet.Trainee);
                if (false == continueTest)
                {
                    candidateNet.TimesGuessed++;
                    if (guessedCorrectly)
                    {
                        candidateNet.TimesCorrect++;
                    }
                    candidateNet.TotalCyclesTaken += trialsTaken;
                }
                candidateNet.Trainee.Reset();
            }
            runLatch = true;
        }

        //Performs the logic of the neural net.  
        //Returns the number of cycles taken
        int LogicCycle(Collective activeNet)
        {
            testAccomplice.resetPointer();
            const int OUTER_ITERATIONS_ALLOWED = 100;
            const int INNER_ITERATIONS_USED = 10;
            int outerIterations = 0;
            while (outerIterations < OUTER_ITERATIONS_ALLOWED && continueTest)
            {
                if (testAccomplice.charsRemain())
                {
                    char toSend = testAccomplice.getNext();
                    if (activeNet.SensoryMap.has(toSend))
                    {
                        activeNet.SensoryMap.search(
                            toSend).
                            Fire();
                    }
                }
                int innerIterations = 0;
                while (innerIterations < INNER_ITERATIONS_USED && continueTest)
                {
                    foreach (NetStructure activeComponent in activeNet.Components)
                    {
                        foreach(Neuron sensory in activeComponent.SensoryList)
                        {
                            sensory.Fire();
                        }
                        foreach (INode intermediate in activeComponent.Intermediates)
                        {
                            intermediate.Fire();
                        }
                        foreach (Neuron motor in activeComponent.MotorList)
                        {
                            motor.Fire();
                        }
                    }
                    //foreach(Neuron motor in activeNet.MotorList)
                    //{
                    //    motor.Fire();
                    //}
                    innerIterations++;
                }
                outerIterations++;

            }
            return outerIterations;
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