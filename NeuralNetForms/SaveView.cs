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
using BrainStructures;

namespace NeuralNetForms
{
                                                            // Allows the user to save a neural net
    public partial class SaveView : Form
    {
        Controller mainController;

        bool toCommitee;

        public SaveView(Controller mainController)
        {
            this.mainController = mainController;
            InitializeComponent();
            toCommitee = false;
        }

                                                            // Tells the controller to save the neural
                                                            // net with a given name.
        private void clickSave(object sender, EventArgs e)
        {
            if (nameBox.Text.Equals(""))
            {
                MessageBox.Show("The name cannot be blank.");
            }
            else if (nameBox.Text.Equals("default"))
            {
                MessageBox.Show("The name 'default' is reserved.");
            }
            else
            {
                var nameToSave = "";
                if (commiteeCheckBox.Checked) {
                    nameToSave = AppData.CommiteeFolderName;
                }
                nameToSave += nameBox.Text;
                mainController.SaveNet(nameToSave);
                MessageBox.Show("File saved.");
            }
            this.Close();
        }

                                                            // Allows the user to dismiss this form.
        private void clickCancel(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            toCommitee = commiteeCheckBox.Checked;
        }
    }
}
