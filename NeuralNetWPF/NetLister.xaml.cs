using BrainStructures;
using Logic;
using System;
//using System.Collections.Generic;
using DataStructures;
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
using Exceptions;

namespace NeuralNetWPF
{
    /// <summary>
    /// Interaction logic for NetLister.xaml
    /// </summary>
    public partial class NetLister : Window
    {
        Controller mindController;
        int currentIndex;
        public NetLister(Controller mindController)
        {
            this.mindController = mindController;
            InitializeComponent();
        }

        private void LoadLister(object sender, EventArgs e)
        {
            PopulateList();
        }


        public void PopulateList()
        {

            netList.Items.Clear();
            LinkedList<NetListElement> netListGotten = mindController.GetNetList();
            netList.ItemsSource = netListGotten;
        }

        private void SelectLoad(object sender, EventArgs e)
        {
            if (CheckSelection())
            {
                try
                {
                    mindController.loadNet(this.currentIndex);
                    MessageBox.Show("Load Complete.");
                    this.Close();
                }
                catch (FileCorruptionException ex)
                {
                    MessageBox.Show("A problem was found on line " +
                        ex.badLine + " of the selected document");
                }
                catch (NotSupportedException ex)
                {
                    MessageBox.Show("Load failed.  File may have been deleted " +
                        "externally, " + "or the list was corrupted");
                }
            }
        }

        private bool CheckSelection()
        {
            int itemsCount = this.netList.SelectedItems.Count;
            if (itemsCount == 0)
            {
                MessageBox.Show("You haven't selected anything!");
                return false;
            }
            currentIndex = this.netList.SelectedIndex;
            return true;
        }

        private void ClickDelete(object sender, EventArgs e)
        {
            if (this.CheckSelection())
            {
                if (this.currentIndex != 0)
                {
                    mindController.deleteNet(currentIndex);
                }
                else
                {
                    MessageBox.Show("Cannot delete default net");
                }
                PopulateList();
            }
        }

        private void ClickCancel(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
