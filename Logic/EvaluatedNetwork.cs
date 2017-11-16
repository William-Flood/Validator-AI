using BrainStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class EvaluatedNetwork
    {
        public Collective Trainee { get; set; }

        // The number of times this neural
        // net proposed a correct guess
        public int TimesCorrect { get; set; }

        // The number of times this neural
        // net proposed any guess
        public int TimesGuessed { get; set; }

        // The number of times the
        // neural net was cycled through
        public int TotalCyclesTaken { get; set; }

        // A value to set neuron firing threshholds
        // to.  Future versions of the program
        // may use neurons with varying
        // threshholds.
        public const int SUGGESTED_NEURON_THRESHHHOLD = 5;

        // Compares this neural net against another.
        // Returns true if this neural net produced
        // better results than the one passed
        // as an argument
        public bool OutPerformed(EvaluatedNetwork contender, int numberOfSentences)
        {
            if (
                (this.TimesCorrect > contender.TimesCorrect)
                )
            {
                return true;
            }
            else if (
                (this.TimesCorrect == numberOfSentences) &&
                (this.TotalCyclesTaken <
                contender.TotalCyclesTaken)
                )
            {
                return true;
            }
            return false;
        }

        public void Dealloc()
        {
            this.Trainee.Dealloc();
            this.Trainee = null;
        }
    }
}
