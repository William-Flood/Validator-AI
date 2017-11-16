using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Logic;
using System.Threading;

namespace NeuralNetWPF
{
    /// <summary>
    /// Interaction logic for TrainNet.xaml
    /// </summary>
    public partial class TrainNet : Window
    {
        int cyclesComplete;
        Controller mainController;
        TrainingReporter getReportsFrom;
        doneHandler trainingDone;
        doneHandler iterationDone;
        event doneHandler stopTraining;
        Thread trainingThread;

        public TrainNet()
        {
            InitializeComponent();
        }

        public TrainNet(Controller mainController, TrainingReporter getReportsFrom)
        {
            this.mainController = mainController;
            this.getReportsFrom = getReportsFrom;
            mainController.setFileNotFoundErrorDisplay(this.DisplayFileError);
            InitializeComponent();
        }

        private void btnStartClick(object sender, RoutedEventArgs e)
        {
            cyclesComplete = 0;
            this.btnStart.IsEnabled = false;
            this.btnStop.IsEnabled = true;
            stopTraining += mainController.getQuit();
            trainingDone = (doneSender, doneE) => Application.Current.Dispatcher.Invoke(new Action(() => this.HandleTrainingDone()));
            iterationDone = (doneSender, doneE) => Application.Current.Dispatcher.Invoke(new Action(() => this.HandleIterationDone()));
            mainController.addIterationDoneAlert(iterationDone);
            mainController.addTrainingDoneAlert(trainingDone);
            mainController.setTrainingCycles((int)this.intCycles.Value);
            trainingThread = new Thread(new ThreadStart(mainController.TrainNet));
            trainingThread.Start();
        }

        private void HandleIterationDone()
        {
            this.lblGuess.Content = (int)(10000 * getReportsFrom.percentRun) / 100.0
                + "%";
            this.lblAccuracy.Content = (int)(10000 * getReportsFrom.percentCorrect) / 100.0
                + "%";
            cyclesComplete++;
            this.lblCycle.Content = cyclesComplete.ToString();
            if (getReportsFrom.percentRun > (double)dcmGuessRate.Value)
            {
                stopTraining(this, EventArgs.Empty);
            }
            if (getReportsFrom.percentCorrect > (double)dcmAccuracy.Value)
            {
                stopTraining(this, EventArgs.Empty);
            }
        }

        /** Allows the form to handle the
         completion of a training session.
        **/
        private void HandleTrainingDone()
        {
            this.btnStop.IsEnabled = false;
            this.btnStart.IsEnabled = true;
            MessageBox.Show("Training Done");
            stopTraining -= mainController.getQuit();
            mainController.removeIterationDoneAlert(iterationDone);
            mainController.removeTrainingDoneAlert(trainingDone);
            trainingThread.Abort();

        }

        private void ClickStop(object sender, EventArgs e)
        {
            stopTraining(this, e);
        }

        public void DisplayFileError()
        {
            MessageBox.Show("File not found!  Select another.");
        }

        private void CloseForm(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (null != stopTraining)
            {
                stopTraining(this, EventArgs.Empty);
            }
        }
        
    }
}
