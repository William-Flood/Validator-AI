using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrainStructures;
using DataStructures;
using Logic;
using Exceptions;

namespace NeuralNetForms
{
                                                            // Lists the saved neural nets
    public partial class NetLister : Form
    {
        Controller mainController;
        int currentIndex;
        
        public NetLister(Controller mainController)
        {
            this.mainController = mainController;
            InitializeComponent();
        }

        private void loadLister(object sender, EventArgs e)
        {
            populateList();
        }

                                                            // Populates the list of neural nets
        public void populateList()
        {

            netList.Items.Clear();
            LinkedList<NetListElement> netListGotten = mainController.GetNetList();
            int i = 0;
            foreach (NetListElement element in netListGotten)
            {
                this.netList.Items.Add(element.name);
                this.netList.Items[i].SubItems.Add(element.percentRan.ToString());
                this.netList.Items[i].SubItems.Add(element.percentCorrect.ToString());
                i++;
            }
        }

                                                            // Indicates to the controller that a
                                                            // given neural net should be loaded.
        private void selectLoad(object sender, EventArgs e)
        {
            if(checkSelection())
            {
                try
                {
                    mainController.loadNet(this.currentIndex);
                    MessageBox.Show("Load Complete.");
                    this.Close();
                }
                catch (FileCorruptionException ex)
                {
                    MessageBox.Show("A problem was found on line " +
                        ex.badLine + " of the selected document");
                }
                catch
                {
                    MessageBox.Show("Load failed.  File may have been deleted" +
                        "externally, " + "or the list was corrupted");
                }
            }
        }


                                                            // Ensures that at least one neural net
                                                            // has been selected.
        private bool checkSelection()
        {
            int itemsCount = this.netList.SelectedItems.Count;
            if (itemsCount == 0)
            {
                MessageBox.Show("You haven't selected anything!");
                return false;
            }
            currentIndex = this.netList.SelectedIndices[0];
            return true;
        }

                                                            // Allows the user to delete a neural net.
        private void clickDelete(object sender, EventArgs e)
        {
            if (this.checkSelection())
            {
                if (this.currentIndex != 0)
                {
                    mainController.deleteNet(currentIndex);
                }
                else {
                    MessageBox.Show("Cannot delete default net");
                }
                populateList();
            }
        }

                                                            // Allows the user to dismiss the form.
        private void clickCancel(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
