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
using System.Runtime.InteropServices;

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

            AppData.SavedNeuronsPath = System.AppDomain.CurrentDomain.BaseDirectory + @"savednets\";
            AppData.TrainingDocumentsPath = System.AppDomain.CurrentDomain.BaseDirectory + @"trainingfiles\";
            getReportsFrom = new TrainingReporter();
            InitializeComponent();
            try
            {
                this.mindController = new Controller(getReportsFrom, SetNetName);
            } catch(System.IO.DirectoryNotFoundException ex)
            {
                MessageBox.Show("Add trainingFiles folder inside Validator-AI\\NeuralNetWPF\\bin\\Debug");
                this.Close();
                return;
            }
            if(mindController.HasFile())
            {
                TrainingFileLabel.Content = "DogTraining";
            }
            //testGPU();
        }

        //void testGPU()
        //{
        //    //int sensoryCount = 1;
        //    //int intermediateCount = 4;
        //    //int motorCount = 1;
        //    //int netCount = 2;
        //    int sensoryCount = 1;
        //    int intermediateCount = 4;
        //    int motorCount = 1;
        //    int netCount = 2;
        //    //int sensoryCount = 1;
        //    //int intermediateCount = 4;
        //    //int motorCount = 1;
        //    //int netCount = 1;
        //    var sensoryBlock = new int[] {
        //        1, 0, 3, 0, 0, //0, 0, 0, 0, 0,

        //        0, 0, 0, 0, 0//, 0, 0, 0, 0, 0
        //    };
        //    var intermediateBlock = new int[] {
        //          0, 1, 0, 0, 0//, 0, 0, 0, 0, 0
        //        , 0, 0, 0, 0, 0//, 0, 0, 0, 0, 0
        //        , 0, 0, 0, 0, 4//, 0, 0, 0, 0, 0
        //        , 0, 0, 0, 0, 0//, 0, 0, 0, 0, 0,
        //                       //
        //        , 0, 1, 0, 0, 0//, 0, 0, 0, 0, 0
        //        , 0, 0, 0, 0, 0//, 0, 0, 0, 0, 0
        //        , 0, 0, 0, 0, 4//, 0, 0, 0, 0, 0
        //        , 0, 0, 0, 0, 0//, 0, 0, 0, 0, 0
        //    };
        //    var tallyCount = (motorCount + intermediateCount) * netCount;
        //    var tallyBlock = new int[tallyCount];
        //    var tempTallyCount = tallyBlock.Length * CUDAWrapper.CalculateBlockCount(motorCount + intermediateCount);
        //    var tempTallyBlock = new int[tempTallyCount];
        //    var encodedNet = new int[sensoryBlock.Length + intermediateBlock.Length + tallyBlock.Length + tempTallyBlock.Length];
        //    var encodingIndex = 0;
        //    foreach(var sensorySynapse in sensoryBlock)
        //    {
        //        encodedNet[encodingIndex] = sensorySynapse;
        //        //encodedNet[encodingIndex] = 1;
        //        encodingIndex++;
        //    }
        //    foreach (var intermediateSynapse in intermediateBlock)
        //    {
        //        encodedNet[encodingIndex] = intermediateSynapse;
        //        //encodedNet[encodingIndex] = 1;
        //        encodingIndex++;
        //    }
        //    for (int i = 0; i < tallyCount; i++)
        //    {
        //        encodedNet[encodingIndex] = 0;
        //        encodingIndex++;
        //    }
        //    encodedNet[sensoryBlock.Length + intermediateBlock.Length + 2] = 19;
        //    for (int i = 0; i< tempTallyCount; i++)
        //    {
        //        encodedNet[encodingIndex] = -1;
        //        encodingIndex++;
        //    }
        //    IntPtr netTransferBlock = CUDAWrapper.declare_transfer_block(encodedNet.Length);
        //    Marshal.Copy(encodedNet, 0, netTransferBlock, encodedNet.Length);
        //    IntPtr cudaSensoryBlock = CUDAWrapper.establish_net_block(netTransferBlock, encodedNet.Length);
        //    IntPtr cudaIntermediateBlock = CUDAWrapper.findIntermediateBlock(cudaSensoryBlock, sensoryCount, intermediateCount, motorCount, netCount);
        //    var cudaTallyBlock = CUDAWrapper.findTallyBlock(cudaIntermediateBlock, intermediateCount, motorCount, netCount);
        //    var cudaTempTallyBlock = CUDAWrapper.findTempTallyBlock(cudaTallyBlock, intermediateCount, motorCount, netCount);
        //    CUDAWrapper.runCycle(sensoryCount, intermediateCount, motorCount, netCount, cudaSensoryBlock, cudaIntermediateBlock, cudaTallyBlock, cudaTempTallyBlock, 0);
        //    //runCycle(sensoryCount, intermediateCount, motorCount, netCount, cudaSensoryBlock, cudaIntermediateBlock, cudaTallyBlock, cudaTempTallyBlock, 0);
        //    var returnedTally = CUDAWrapper.getNet(cudaTallyBlock, tallyBlock.Length);
        //    var tallyResults = new int[tallyBlock.Length];
        //    var returnedTempTally = CUDAWrapper.getNet(cudaTempTallyBlock, tempTallyBlock.Length);
        //    var tempTallyResults = new int[tempTallyBlock.Length];
        //    var returnedNet = CUDAWrapper.getNet(cudaSensoryBlock, encodedNet.Length);
        //    var netResults = new int[encodedNet.Length];
        //    Marshal.Copy(returnedTally, tallyResults, 0, tallyBlock.Length);
        //    Marshal.Copy(returnedTempTally, tempTallyResults, 0, tempTallyBlock.Length);
        //    Marshal.Copy(returnedNet, netResults, 0, encodedNet.Length);
        //    CUDAWrapper.release(returnedTally);
        //    CUDAWrapper.release(returnedNet);
        //    CUDAWrapper.release(netTransferBlock);
        //    CUDAWrapper.release(returnedTempTally);
        //    CUDAWrapper.cuda_release(cudaSensoryBlock);
        //}

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
