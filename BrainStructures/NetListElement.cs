using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainStructures
{
                                                            //Provides a description of a neural net
    public class NetListElement
    {               
        public String name { get; set; }                    //The file name of the neural net

        public double percentRan { get; set; }              //The percentages of execution that
                                                            //result in the neural net taking any action

        public double percentCorrect { get; set; }          //The percentages of execution that
                                                            //result in the neural net making
                                                            //a correct guess.
                                                            
        public NetListElement(String name, double percentRan, double percentCorrect)
        {
            this.name = name;
            this.percentCorrect = percentCorrect;
            this.percentRan = percentRan;
        }
    }
}
