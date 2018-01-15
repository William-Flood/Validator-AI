using System;
//using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using BrainStructures;
using DataAccess;
using DataStructures;
using System.IO;
using System.Runtime.InteropServices;

namespace Logic
{
    public delegate void doneHandler(object sender, EventArgs e);
    public delegate void nameSetter(String name);
    public delegate void displayError();
    public class Controller
    {

        // The currently loaded neural net.
        Collective loadedNet;

        // A file to be used to train a neural net.
        String trainingDocumentName = "DogTraining.txt";

        // Indicates if the logic cycle of a neural
        // net should be continued.
        bool continueTest;

        // Used to record the results of training.
        TrainingReporter sendReportTo;

        // Used to mutate neural nets.
        Mutator trainingMutator;

        // Signals a display that an iteration
        // of training has been completed.
        public event doneHandler iterationDone;

        // Signals a display that training
        // has been completed.
        public event doneHandler trainingDone;

        // The number of training cycles to
        // be completed during a session.
        int trainingCycles;

        // Indicates if training should
        // be continued.
        bool continueTraining;

        // Created neurons will have a threshhold
        // equal to the value set in NetStructure.
        const int NEURON_THRESHHOLD = Neuron.DEFAULT_THRESHHOLD;

        // Holds a description of saved neural nets.
        LinkedList<NetListElement> netList;

        // Stores the performance of the currently
        // loaded net so it may be saved.
        double trackedPercentRan;

        // Stores the performance of the currently
        // loaded net so it may be saved.
        double trackedPercentCorrect;

        // Allows the main display to display
        // the name a neural net was saved to.
        nameSetter nameDisplayer;

        // Allows an error to be displayed if
        // a file failed to be loaded.
        displayError displayFileNotFoundError;

        // Stores the guess made by a neural net.
        bool guessSent;

        const int JUDGED_NETS_QUANTITY = 50;


        public Controller(TrainingReporter reporter, nameSetter nameDisplayer)
        {
            FileManager.CreateFolders();
            netList = new LinkedList<NetListElement>();
            this.nameDisplayer = nameDisplayer;
            RefreshList();
            sendReportTo = reporter;
            if(FileManager.FileExists("DogTraining.txt"))
            {
                trainingDocumentName = "DogTraining.txt";
            }
        }

        // Creates a new neural net.
        public void CreateNet()
        {
            var creationPot = new FlowerPot() { NeuronActions = new Neuron.actionType[] { this.SendFalse, this.SendTrue }, TrainingDocumentName = this.trainingDocumentName, StartingIntermediates = 20 };
            loadedNet = creationPot.CreateNet();
            trainingMutator = new Mutator(.05, .05, .3, .05, .9);
            trackedPercentCorrect = 0;
            trackedPercentRan = 0;
            if (0 == netList.Length ||
                !netList[0].name.Equals("default"))
            {
                netList.AddAtStart(new NetListElement("default", 0, 0));
            }
            netList[0].percentCorrect = 0;
            netList[0].percentRan = 0;
            this.nameDisplayer("default");
            NetXMLAccess.SaveNet("default.xml", loadedNet);
            //NetListManager.saveNetList(netList);

        }

        // Allows a neural net to guess
        // a given string matched its
        // training.
        public void SendTrue()
        {
            guessSent = true;
            continueTest = false;
        }

        // Allows a neural net to guess
        // a given stringfailed to match
        // its training.
        public void SendFalse()
        {
            guessSent = false;
            continueTest = false;
        }

        // Reports the list of saved neural nets.
        public LinkedList<NetListElement> GetNetList()
        {
            RefreshList();

            return netList;
        }

        // Refreshes the list of saved neural nets.
        public void RefreshList()
        {
            netList = NetListManager.getNestList();
            if (0 == netList.Length && null != trainingDocumentName)
            {
                try
                {
                if (loadedNet == null)
                {
                    CreateNet();
                }
                    netList.AddAtStart(new NetListElement("default", 0, 0));
                    NetListManager.saveNetList(netList);
                    //NetXMLAccess.SaveNet("default.xml", loadedNet);
                }
                catch { }

            }
            else if (loadedNet == null && 0 < netList.Length)
            {
                try
                {
                    loadNet(0);
                }
                catch
                {
                    netList.ResetPointer();
                    netList.MoveUp();
                    netList.Remove();
                    NetListManager.saveNetList(netList);
                    nameDisplayer("none");
                }
            }
            else if (null == loadedNet)
            {
                nameDisplayer("none");
            }
        }

        // Imports a file into the
        // directory of training files.
        public void import(String oldFilePath, String newName)
        {
            FileManager.Import(oldFilePath, AppData.TrainingDocumentsPath + newName);
        }


        // Saves a neural net to file.
        public void SaveNet(String name)
        {
            netList.ResetPointer();
            bool latch = true;
            while (netList.ElementsRemain && latch)
            {
                netList.MoveUp();
                if (netList.Value.name.Equals(name))
                {
                    latch = false;
                    netList.Value.percentRan = trackedPercentRan;
                    netList.Value.percentCorrect = trackedPercentCorrect;
                }
            }
            if (latch)
            {
                netList.AddAfter(new NetListElement(name, trackedPercentRan, trackedPercentCorrect));
            }
            if (!netList[0].name.Equals("default"))
            {
                netList.AddAtStart(new NetListElement("default", trackedPercentRan, trackedPercentCorrect));
            }
            netList[0].percentCorrect = trackedPercentCorrect;
            netList[0].percentRan = trackedPercentRan;
            try
            {
                NetListManager.saveNetList(netList);
                NetXMLAccess.SaveNet(name + ".xml",loadedNet);
                NetXMLAccess.SaveNet("default.xml", loadedNet);
            }
            catch
            {
                throw;
            }
            nameDisplayer(name);
        }

        // Sets the file to be used
        // as a training file.
        public void getTrainingFile(String fileName)
        {
            trainingDocumentName = fileName;
        }

        // Reports if a neural net
        // has been loaded.
        public bool HasNet()
        {
            return !(loadedNet == null);
        }

        // Indicates if a training file
        // has been assigned.
        public bool HasFile()
        {
            return !(trainingDocumentName == null);
        }

        // Produces a clone of a
        // linked list of neurons.
        static LinkedList<INode> cloneNetPiece(int i, LinkedList<INode> toClone)
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

        public void setTrainingCycles(int cycles)
        {
            this.trainingCycles = cycles;
        }

        // Trains the neural net.
        public void TrainNet()
        {
            int totalSentences = 0;
            int totalTimesRan = 0;
            int totalTimesCorrect = 0;
            continueTraining = true;
            LinkedList<EvaluatedNetwork> trainingList;
            const int NUMBER_OF_CLONES = 8;
            GetTrainingString trainerProvider = new GetTrainingString(AppData.TrainingDocumentsPath +
                trainingDocumentName);
            int i = 0;
            var judgedNets = new LinkedList<EvaluatedNetwork>();
            var cloneVat = new FlowerPot() { NeuronActions = new Neuron.actionType[] { this.SendFalse, this.SendTrue }, TrainingDocumentName = this.trainingDocumentName };
            for (int j = 0; j < 10; j++)
            {
                judgedNets[j] = new EvaluatedNetwork()
                {
                    Trainee = cloneVat.CloneNet(loadedNet, j)
                };
                trainingMutator.MutateNetwork(judgedNets[j].Trainee.Components[0]);
                FlowerPot.HardenNet(judgedNets[j].Trainee);
            }
            LinkedList<ActionInstance> trainingInstances;
            LinkedList<Thread> trainingThreads;
            while (continueTraining && i < trainingCycles)
            {
                i++;
                String testString;
                try
                {
                    testString = trainerProvider.getSentence();
                }
                catch (FileNotFoundException)
                {
                    testString = "END OF FILE";
                    continueTraining = false;
                    displayFileNotFoundError();
                }
                catch (DirectoryNotFoundException)
                {
                    testString = "END OF FILE";
                    continueTraining = false;
                    displayFileNotFoundError();
                }
                totalSentences = 0;
                totalTimesRan = 0;
                totalTimesCorrect = 0;
                int numberOfSentences = 0;
                const int PARAGRAPHS_BETWEEN_JUDEGEMENT = 10;
                while (false == testString.Equals("END OF FILE") && continueTraining)
                {
                    numberOfSentences = 0;
                    trainingInstances = new LinkedList<ActionInstance>();
                    for (int j = 0; j < NUMBER_OF_CLONES; j++)
                    {
                        trainingInstances.AddAtStart(
                            new ActionInstance(judgedNets, trainingMutator, j)
                            );
                    }
                    judgedNets.ResetPointer();
                    judgedNets.MoveUp();
                    while (judgedNets.ElementsRemain)
                    {
                        judgedNets.Value.Dealloc();
                        judgedNets.Remove();
                    }
                    for (int paragraph = 0;
                        paragraph < PARAGRAPHS_BETWEEN_JUDEGEMENT;
                        paragraph++)
                    {
                        var test = "test";
                        while ((false == testString.Equals("END OF PARAGRAPH")) &&
                            (false == testString.Equals("END OF FILE")) &&
                            continueTraining)
                        {
                            numberOfSentences++;
                            totalSentences++;
                            TestAgainst testAccomplice = new TestAgainst(testString);
                            trainingThreads = new LinkedList<Thread>();
                            foreach (ActionInstance trainingInstance in trainingInstances)
                            {
                                trainingInstance.getTestAccomplice(testAccomplice);
                                //trainingInstance.runTrainingCycle();

                                trainingThreads.AddAtStart(new Thread(
                                    new ThreadStart(trainingInstance.runTrainingCycle)));
                                trainingThreads[0].Start();
                            }
                            bool cycleLatch = true;
                            while (cycleLatch)
                            {
                                Thread.Sleep(5);
                                cycleLatch = false;
                                foreach (ActionInstance trainingInstance in trainingInstances)
                                {
                                    cycleLatch = cycleLatch || false == trainingInstance.TrainingDone();
                                }
                            }
                            foreach (Thread trainingThread in trainingThreads)
                            {
                                trainingThread.Abort();
                            }

                            testString = trainerProvider.getSentence();
                        }
                        if (testString.Equals("END OF PARAGRAPH"))
                        {
                            testString = trainerProvider.getSentence();
                        }
                    }
                    trainingList = new LinkedList<EvaluatedNetwork>();
                    foreach (ActionInstance trainingInstance in trainingInstances)
                    {
                        trainingInstance.AddNets(trainingList);
                    }
                    judgedNets = new LinkedList<EvaluatedNetwork>();
                    LinkedList<EvaluatedNetwork> alreadyJudged = new LinkedList<EvaluatedNetwork>(judgedNets);
                    foreach (EvaluatedNetwork candidateNet in trainingList)
                    {
                        bool latch = true;
                        judgedNets.ResetPointer();
                        alreadyJudged.ResetPointer();
                        while (latch && alreadyJudged.ElementsRemain)
                        {
                            alreadyJudged.MoveUp();
                            if (candidateNet.OutPerformed(alreadyJudged.Value, numberOfSentences))
                            {
                                judgedNets.AddAfter(candidateNet);
                                latch = false;
                            }
                            judgedNets.MoveUp();
                        }
                        if (latch)
                        {
                            judgedNets[judgedNets.Length] = candidateNet;
                        }
                    }
                    judgedNets.ResetPointer();
                    int[] judgedCurrectGuesses = new int[judgedNets.Length];
                    while (judgedNets.ElementsRemain)
                    {
                        judgedNets.MoveUp();
                        judgedCurrectGuesses[judgedNets.CurrentIndex] = judgedNets.Value.TimesCorrect;
                    }
                    totalTimesRan += judgedNets[0].TimesGuessed;
                    totalTimesCorrect += judgedNets[0].TimesCorrect;
                    loadedNet = judgedNets[0].Trainee;
                    const double GUESSTOLERANCE = .9;
                    const double SEARCHBACK = .3;
                    const double SEARCHBACK_IMPROVEMENT_THRESHHOLD = 1;
                    /*if (trainingMutator.createBumper())
                    {
                        trainingMutator = new Mutator(.9, .9, .9, .9, .5);
                    }
                    //else if (judgedNets[0].intermediates.length() > 25 || cullLatch)
                    //{
                    //    if (judgedNets[0].intermediates.length() > 25 && !cullLatch)
                    //    {
                    //        cullLatch = true;
                    //    }
                    //    trainingMutator.adjustNeurogenesisOdds(0);
                    //    trainingMutator.adjustLysingOdds(.5);
                    //    trainingMutator.adjustNewSynapsesOdds(0);
                    //    trainingMutator.adjustPruneSynapseOdds(.1);
                    //    trainingMutator.adjustExitationOdds(.5);
                    //    if (judgedNets[0].intermediates.length() < 10)
                    //    {
                    //        cullLatch = false;
                    //    }
                    //
                    //}
                    else */
                    if (judgedNets[0].TimesGuessed <
                 GUESSTOLERANCE * numberOfSentences)
                    {
                        // The parameters indicate the
                        // value the given odd should approach.
                        trainingMutator.adjustNeurogenesisOdds(0);
                        trainingMutator.adjustLysingOdds(.01);
                        trainingMutator.adjustNewSynapsesOdds(.3);
                        trainingMutator.adjustPruneSynapseOdds(.01);
                        trainingMutator.adjustExitationOdds(.9);
                    }
                    else if (judgedNets[0].TimesCorrect ==
                        judgedNets[(int)(judgedNets.Length * SEARCHBACK)].TimesCorrect * SEARCHBACK_IMPROVEMENT_THRESHHOLD)
                    {
                        trainingMutator.adjustNeurogenesisOdds(.01);
                        trainingMutator.adjustLysingOdds(.1);
                        trainingMutator.adjustNewSynapsesOdds(0);
                        trainingMutator.adjustPruneSynapseOdds(.5);
                        trainingMutator.adjustExitationOdds(.2);
                    }
                    else if (judgedNets[0].TimesCorrect < numberOfSentences)
                    {
                        trainingMutator.adjustNeurogenesisOdds(.05);
                        trainingMutator.adjustLysingOdds(.01);
                        trainingMutator.adjustNewSynapsesOdds(.05);
                        trainingMutator.adjustPruneSynapseOdds(.05);
                        trainingMutator.adjustExitationOdds(.2);
                    }
                    else
                    {
                        trainingMutator.adjustNeurogenesisOdds(.1);
                        trainingMutator.adjustLysingOdds(.3);
                        trainingMutator.adjustNewSynapsesOdds(.01);
                        trainingMutator.adjustPruneSynapseOdds(.05);
                        trainingMutator.adjustExitationOdds(.5);
                    }
                    foreach (EvaluatedNetwork judgedNet in judgedNets)
                    {
                        judgedNet.TimesCorrect = 0;
                        judgedNet.TimesGuessed = 0;
                        judgedNet.TotalCyclesTaken = 0;
                    }
                    numberOfSentences = 0;

                }
                if (totalSentences > 0)
                {
                    sendReportTo.percentRun = (double)totalTimesRan / totalSentences;
                    sendReportTo.percentCorrect = (double)totalTimesCorrect / totalSentences;
                }
                iterationDone(this, EventArgs.Empty);
            }
            trainingInstances = new LinkedList<ActionInstance>();
            if (totalSentences > 0)
            {
                netList[0].percentCorrect = (double)totalTimesRan / totalSentences;
                netList[0].percentRan = (double)totalTimesCorrect / totalSentences;
                trackedPercentCorrect = (double)totalTimesCorrect / totalSentences;
                trackedPercentRan = (double)totalTimesRan / totalSentences;
                NetListManager.saveNetList(netList);
                NetXMLAccess.SaveNet("default.xml", loadedNet);
            }

            trainingDone(this, EventArgs.Empty);
        }

        // Adds an event handler to interationDone.
        public void addIterationDoneAlert(doneHandler toHandle)
        {
            iterationDone += toHandle;
        }

        // Removes an event handler from iterationDone.
        public void removeIterationDoneAlert(doneHandler toRemove)
        {
            iterationDone -= toRemove;
        }

        // Adds an event handler to trainingDone.
        public void addTrainingDoneAlert(doneHandler toHandle)
        {
            trainingDone += toHandle;
        }

        // Removes an event handler from trainingDone.
        public void removeTrainingDoneAlert(doneHandler toRemove)
        {
            trainingDone -= toRemove;
        }

        // Allows the neural net to judge a
        // string sent from the user.
        public bool validateText(String toValidate)
        {
            continueTest = true;
            loadedNet.MotorList[0].SetAction(SendFalse);
            loadedNet.MotorList[1].SetAction(SendTrue);
            LinkedList<char> validationArray = new LinkedList<char>();
            foreach (char character in toValidate)
            {
                validationArray.AddAfter(character);
                validationArray.MoveUp();
            }
            validationArray.ResetPointer();
            const int OUTER_ITERATIONS_ALLOWED = 100;
            const int INNER_ITERATIONS_USED = 10;
            int outerIterations = 0;
            while (outerIterations < OUTER_ITERATIONS_ALLOWED && continueTest)
            {
                if (validationArray.ElementsRemain)
                {
                    validationArray.MoveUp();
                    char toSend = validationArray.Value;
                    if (loadedNet.SensoryMap.has(toSend))
                    {
                        loadedNet.SensoryMap.search(
                            toSend).
                            Fire();
                    }
                }
                int innerIterations = 0;
                while (innerIterations < INNER_ITERATIONS_USED && continueTest)
                {
                    foreach (NetStructure activeComponent in loadedNet.Components)
                    {
                        foreach (Neuron sensory in activeComponent.SensoryList)
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
                    foreach (Neuron motor in loadedNet.MotorList)
                    {
                        motor.Fire();
                    }
                    innerIterations++;
                }
                outerIterations++;

            }
            return guessSent;
        }

        // Loads a neural net from file.
        public void loadNet(int index)
        {
            String netName = netList[index].name;
            try
            {
                loadedNet = NetXMLAccess.LoadNet(
                   netName);
                trackedPercentCorrect = netList[index].percentCorrect;
                trackedPercentRan = netList[index].percentRan;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            trainingMutator = new Mutator(.7, .1, .2, .1, .5);
            nameDisplayer(netName);
            if (false == netList[0].name.Equals("default"))
            {
                netList.AddAtStart(new NetListElement("default",
                    trackedPercentRan,
                    trackedPercentCorrect)
                    );
            }
        }

        // Handles the user electing to halt
        // the training of a neural net.
        public void quitTrain(Object sender, EventArgs e)
        {
            continueTraining = false;
        }

        // Allows the UI to add quitTrain as
        // an event handler.
        public doneHandler getQuit()
        {
            return quitTrain;
        }

        // Removes a neural net from the file.
        public void deleteNet(int index)
        {
            netList.ResetPointer();
            for (int i = 0; i <= index; i++)
            {
                netList.MoveUp();
            }
            String nameToRemove = netList.Value.name;
            netList.Remove();
            FileManager.DeleteNet(AppData.SavedNeuronsPath + nameToRemove + ".txt");
            NetListManager.saveNetList(netList);
        }

        // Sets what should be displayed if 
        // a requested file cannot be found.
        public void setFileNotFoundErrorDisplay(displayError display)
        {
            this.displayFileNotFoundError = display;
        }

        // Updates the loaded neural net.
        public bool updateLoaded()
        {
            try
            {
                CharMap.updateMap(loadedNet,
                AppData.TrainingDocumentsPath
                    + trainingDocumentName);
                return true;
            }
            catch (NullReferenceException)
            {
                return false;
            }
            catch (DirectoryNotFoundException)
            {
                String foo = AppData.TrainingDocumentsPath
                    + trainingDocumentName;
                return false;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
        }

    }
}