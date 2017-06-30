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

namespace NeuralNetForms
{
                                                            // Allows the user to submit text
                                                            // to be evaluated by neural net.
    public partial class UserInput : Form
    {
        Controller mainController;

        public UserInput(Controller mainController)
        {
            this.mainController = mainController;
            InitializeComponent();
        }

                                                            // Sends text from the user to the 
                                                            // controller for evaluation.
        private void clickSubmit(object sender, EventArgs e)
        {
            if("" == txtInput.Text)
            {
                MessageBox.Show("Input data first");
            }
            else if(mainController.HasNet())
            {
                if(mainController.validateText(txtInput.Text))
                {
                    MessageBox.Show("Input recognized as valid");
                }
                else
                {
                    MessageBox.Show("Input recognized as invalid");
                }
            }
        }
    }
}
