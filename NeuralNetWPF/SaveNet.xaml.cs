using Logic;
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

namespace NeuralNetWPF
{
    /// <summary>
    /// Interaction logic for SaveNet.xaml
    /// </summary>
    public partial class SaveNet : Window
    {
        Controller mindController;

        public SaveNet(Controller mindController)
        {
            this.mindController = mindController;
            InitializeComponent();
        }

        private void ClickSave(object sender, EventArgs e)
        {
            if (NetName.Text.Equals(""))
            {
                MessageBox.Show("The name cannot be blank.");
            }
            else if (NetName.Text.Equals("default"))
            {
                MessageBox.Show("The name 'default' is reserved.");
            }
            else
            {
                var nameToSave = "";
                nameToSave += NetName.Text;
                mindController.SaveNet(nameToSave);
                MessageBox.Show("File saved.");
            }
            this.Close();
        }

        private void ClickCancel(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
