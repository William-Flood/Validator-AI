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
using BrainStructures;

namespace NeuralNetForms
{
    public partial class MainMenu : Form
    {
                                                            // A controller to handle the main logic
                                                            // of the application.
        Controller mainController;

                                                            // A report of the performance of a neural net.
        TrainingReporter getReportsFrom;


        public MainMenu()
        {
            getReportsFrom = new TrainingReporter();
            InitializeComponent();
        }

                                                            // Allows the user to create a new neural net.
        private void createNewNet(object sender, EventArgs e)
        {
            try
            {
                mainController.CreateNet();
                MessageBox.Show("New net created!");
            }
            catch
            {
                MessageBox.Show("No training file!");
            }
        }

                                                            // Allows the user to save the neural net
                                                            // currently loaded by the controller
        private void saveNet(object sender, EventArgs e)
        {
            var saver = new SaveView(mainController);
            saver.ShowDialog();
            //mainController.saveNet("net.txt");
            //MessageBox.Show("Neural net saved!");
        }

                                                            // Allows the user to load a new neural net.
        private void selectLoadNet(object sender, EventArgs e)
        {
            var netLister = new NetLister(mainController);
            netLister.ShowDialog();
        }

                                                            // Allows the user to train the currently
                                                            // loaded neural net.
        private void clickTrain(object sender, EventArgs e)
        {
            if (!mainController.HasNet())
            {
                MessageBox.Show("A neural net is needed.  Create or load one.");
            }
            else if (!mainController.HasFile())
            {
                MessageBox.Show("A training file is needed.");
            }
            else
            {
                var trainDialog = new TrainForm(mainController, getReportsFrom);
                trainDialog.Show();
            }
        }

                                                            // Allows the user to load a new training file.
        private void clickLoadTrainingFile(object sender, EventArgs e)
        {
            //clear dialog of residual data
            this.trainingFileBrowser.FileName = "";
            this.trainingFileBrowser.Multiselect = false; //Multiselect allows multiple
            //files to be selected
            this.trainingFileBrowser.InitialDirectory = AppData.TrainingDocumentsPath;
            this.trainingFileBrowser.ShowDialog(this);
            if (trainingFileBrowser.FileName != null &&
                false == trainingFileBrowser.FileName.Equals("")) 
            {
                try
                {
                    this.mainController.getTrainingFile(trainingFileBrowser.SafeFileName);
                    this.txtTrainingFileName.Text = trainingFileBrowser.SafeFileName;
                }
                catch
                {
                    MessageBox.Show("File selection failed!");
                }
            }
        }

                                                            // Run upon loading the app; creates
                                                            // a new controller.
        private void loadMain(object sender, EventArgs e)
        {
            this.mainController = new Controller(getReportsFrom, setNetName);
        }


                                                            // Used by the controller to display the
                                                            // name of the currently loaded neural net.
        public void setNetName(String name)
        {
            this.txtNetName.Text = name;
        }

                                                            // Allows the user to update the characters
                                                            // supported by the neural net.
        private void clickUpdate(object sender, EventArgs e)
        {
            if(!mainController.HasNet())
            {
                MessageBox.Show("A neural net is needed");
            }
            else if (!mainController.HasFile())
            {
                MessageBox.Show("A training file is needed");
            }
            if(mainController.updateLoaded())
            {
                MessageBox.Show("Map updated");
            }
        }

                                                            // Allows the user to provide one's own 
                                                            // text to be evaluated by the neural net.
        private void clickSendText(object sender, EventArgs e)
        {
            if(mainController.HasNet())
            {
                var inputter = new UserInput(mainController);
                inputter.ShowDialog();
            }
            else
            {
                MessageBox.Show("A neural net is needed.  Create or load one.");
            }
        }

                                                            // Allows the user to import a new
                                                            // training file.
        private void clickImport(object sender, EventArgs e)
        {
            this.trainingFileBrowser.FileName = "";
            this.trainingFileBrowser.Multiselect = false; 
            this.trainingFileBrowser.InitialDirectory = "";
            this.trainingFileBrowser.ShowDialog(this);
            if (trainingFileBrowser.FileName != null)
            {
                try
                {
                    this.mainController.import(trainingFileBrowser.FileName,
                        trainingFileBrowser.SafeFileName);
                }
                catch
                {
                    MessageBox.Show("File selection failed!");
                }
            }
        }

                                                            // Replaces the main controller with a fresh instance.
                                                            // Part of troubleshooting a memory leak.
        private void clickRefreshController(object sender, EventArgs e)
        {
            this.mainController = new Controller(getReportsFrom, setNetName);
            this.txtNetName.Text = "<none>";
            this.txtTrainingFileName.Text = "<none>";
        }
    }
}
