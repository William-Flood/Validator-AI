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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using Logic;
using BrainStructures;

namespace NeuralNetWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Controller mindController;
        TrainingReporter getReportsFrom;
        public MainWindow()
        {

            AppData.SavedNeuronsPath = System.AppDomain.CurrentDomain.BaseDirectory + @"\" + "savednets" + @"\";
            AppData.TrainingDocumentsPath = System.AppDomain.CurrentDomain.BaseDirectory + @"\" + "trainingfiles" + @"\";
            getReportsFrom = new TrainingReporter();
            InitializeComponent();
            this.mindController = new Controller(getReportsFrom, SetNetName);
            if(mindController.HasFile())
            {
                TrainingFileLabel.Content = "DogTraining";
            }
        }

        public void SetNetName(String name)
        {
            this.NetLabel.Content = name;
        }

        private void mnStartTraining(object sender, RoutedEventArgs e)
        {
            if (!mindController.HasNet())
            {
                MessageBox.Show("A neural net is needed.  Create or load one.");
            }
            else if (!mindController.HasFile())
            {
                MessageBox.Show("A training file is needed.");
            }
            else
            {
                var trainDialog = new TrainNet(mindController, getReportsFrom);
                trainDialog.Show();
            }
        }

        private void CreateNewNet(object sender, EventArgs e)
        {
            //try
            //{
                mindController.CreateNet();
            //MessageBox.Show("New net created!");
            NetLabel.Content = "New";
            /*}
            catch
            {
                MessageBox.Show("No training file!");
            }*/
        }

        private void ClickLoadTrainingFile(object sender, EventArgs e)
        {
            //clear dialog of residual data
            var fileDialog = new OpenFileDialog() {
                DefaultExt=".txt",
                //Filter= "Text files(*.txt) | *.txt | All files(*.*) | *.*",
                Multiselect=false,
                InitialDirectory = AppData.TrainingDocumentsPath
            };
            fileDialog.ShowDialog();
            if (fileDialog.FileName != null &&
                false == fileDialog.FileName.Equals(""))
            {
                try
                {
                    this.mindController.getTrainingFile(fileDialog.SafeFileName);
                    this.TrainingFileLabel.Content = fileDialog.SafeFileName;
                }
                catch
                {
                    MessageBox.Show("File selection failed!");
                }
            }
        }

        private void SaveNet(object sender, RoutedEventArgs e)
        {
            var saver = new SaveNet(mindController);
            saver.ShowDialog();
        }

        private void clickLoadNet(object sender, RoutedEventArgs e)
        {
            var netLister = new NetLister(mindController);
            netLister.ShowDialog();
        }
    }
}
