using System;
//using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructures;
using System.IO;
using BrainStructures;

namespace DataAccess
{
                                                            // Manages file i/o for a list of
                                                            // neural nets
    public class NetListManager
    {

                                                            // Returns a list describing saved neural nets
        public static LinkedList<NetListElement> getNestList()
        {
            var netList = new LinkedList<NetListElement>();

            char[] separator = { ',' };

            try
            {
                StreamReader fileReader = new StreamReader(AppData.SavedNeuronsPath + 
                    AppData.NetListFileName);
                while (fileReader.EndOfStream == false)
                {
                    string line = fileReader.ReadLine();
                    string[] parts;
                    parts = line.Split(separator);
                    if (parts.Count() == 3)
                    {
                                                            // NetListElement objects store 
                                                            // information about a neural net
                        NetListElement newNet = new NetListElement(
                            parts[0],
                             Double.Parse(parts[1]),
                             Double.Parse(parts[2]));
                        newNet.name = parts[0];
                        newNet.percentRan = Double.Parse(parts[1]);
                        newNet.percentCorrect = Double.Parse(parts[2]);
                        netList.AddAfter(newNet);           // Appends the NetListElement to netList

                        netList.MoveUp();                   // Adjusts netList so that the insertion point is
                                                            // after the neural net just added
                    }

                }
                fileReader.Close();
            }
            catch (FileNotFoundException)
            {
                                                            // Simply returns the blank netList - the controller
                                                            // will take further action from here
                return netList;
            }
            catch (DirectoryNotFoundException)
            {
                return netList;
            }
            catch
            {
                throw;
            }

            return netList;
        }

                                                            // Records information on the neural nets
                                                            // passed through the list parameter
        public static void saveNetList(LinkedList<NetListElement> netList)
        {
            try {
                using (StreamWriter csvWriter = new StreamWriter(AppData.SavedNeuronsPath + 
                    AppData.NetListFileName))
                {
                    foreach (NetListElement element in netList)
                    {
                                                            // The stream writer writes the name of the neural net,
                                                            // the percentage the neural nets take any action
                                                            // and the percentage the neural nets guess correctly
                        csvWriter.WriteLine(element.name + "," + 
                            String.Format("{0:F2}",element.percentRan) + "," +
                            String.Format("{0:F2}",element.percentCorrect));
                    }
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
