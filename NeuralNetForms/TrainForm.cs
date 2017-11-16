using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Logic;
using System.Threading;

namespace NeuralNetForms
{
                                                            // Allows the user to train a neural net.
    public partial class TrainForm : Form
    {
        Controller mainController;
        TrainingReporter getReportsFrom;

                                                            // A delegate for functions to be passed
                                                            // into Invoke, allowing the display to
                                                            // be adjusted across threads.
        delegate void trainingControlSetter(object sender, EventArgs e);

                                                            // Records the number of training cycles
                                                            // completed during a session.
        int cyclesComplete;
        event doneHandler stopTraining;
        Thread trainingThread;
        public TrainForm(Controller mainController, TrainingReporter getReportsFrom)
        {
            this.mainController = mainController;
            this.getReportsFrom = getReportsFrom;
            mainController.setFileNotFoundErrorDisplay(this.displayFileError);
            InitializeComponent();
        }

                                                            // Allows the user to start training
        private void ClickStartTraining(object sender, EventArgs e)
        {
            cyclesComplete = 0;
            this.btnStart.Enabled = false;
            this.btnStop.Enabled = true;
            stopTraining += mainController.getQuit();
            mainController.addIterationDoneAlert(this.handleIterationDone);
            mainController.addTrainingDoneAlert(this.HandleTrainingDone);
            mainController.setTrainingCycles((int)this.nudCycles.Value);
            trainingThread = new Thread(new ThreadStart(mainController.TrainNet));
            trainingThread.Start();
        }

                                                            // Updates the display when a training
                                                            // cycle has been completed.
        private void handleIterationDone(object sender, EventArgs e)
        {
            if (this.txtGuessRate.InvokeRequired)
            {
                trainingControlSetter incrementSetter = new trainingControlSetter(this.handleIterationDone);
                this.Invoke(incrementSetter, new object[] { sender, e });
            }
            else
            {
                this.txtGuessRate.Text = (int)(10000 * getReportsFrom.percentRun) / 100.0
                    + "%";
                this.txtAccuracyRate.Text = (int)(10000 * getReportsFrom.percentCorrect) / 100.0
                    + "%";
                cyclesComplete++;
                this.txtCycleCount.Text = cyclesComplete.ToString();
                if (getReportsFrom.percentRun > (double)nudGuessRate.Value)
                {
                    stopTraining(this, EventArgs.Empty);
                }
                if (getReportsFrom.percentCorrect > (double) nudAccuracy.Value)
                {
                    stopTraining(this, EventArgs.Empty);
                }
            }
        }

                                                            // Allows the form to handle the
                                                            // completion of a training session.
        private void HandleTrainingDone(object sender, EventArgs e)
        {
            if (this.btnStop.InvokeRequired)
            {
                trainingControlSetter doneSetter = new trainingControlSetter(this.HandleTrainingDone);
                this.Invoke(doneSetter, new Object[] { sender, e });
            }
            else
            {
                this.btnStop.Enabled = false;
                this.btnStart.Enabled = true;
                MessageBox.Show("Training Done");
                stopTraining -= mainController.getQuit();
                mainController.removeIterationDoneAlert(this.handleIterationDone);
                mainController.removeTrainingDoneAlert(this.HandleTrainingDone);
                trainingThread.Abort();
            }
        }

                                                            // Allows the user to abort training.
        private void clickStop(object sender, EventArgs e)
        {
            stopTraining(this, e);
        }

                                                            // Handles the case of a training
                                                            // file not being found.
        public void displayFileError()
        {
            MessageBox.Show("File not found!  Select another.");
        }

        private void closeWindow(object sender, EventArgs e)
        {

        }

        private void closeForm(object sender, FormClosingEventArgs e)
        {
            if (null != stopTraining)
            {
                stopTraining(this, EventArgs.Empty);
            }
        }
    }
}
