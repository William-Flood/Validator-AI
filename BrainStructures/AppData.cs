using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainStructures
{
    public class AppData
    {
                                                            // The name of the file used to list neural nets
        public const String NetListFileName = "netlist.csv";

        public const String CommiteeFolderName = "commitee\\";
                                                            // Holds the path neural nets are saved to
        public static string SavedNeuronsPath { get; set; }
                                                            // Holds the path containing 
        public static string TrainingDocumentsPath { get; set; }
    }
}
