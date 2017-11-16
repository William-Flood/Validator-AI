using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions
{
                                                            // Used to report that a file
                                                            // contains erronious content
    public class FileCorruptionException : Exception
    {
        public int badLine { get; set; }
        public FileCorruptionException()
        {

        }

        public FileCorruptionException(string message)
            : base(message)
        {

        }

        public FileCorruptionException(string message, Exception inner) :
            base(message, inner)
        {

        }

        public FileCorruptionException(int badLine) : 
            base()
        {
            this.badLine = badLine;
        }
    }
}
