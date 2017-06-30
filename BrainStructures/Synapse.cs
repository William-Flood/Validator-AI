using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainStructures
{
    public class Synapse
    {
        public int Weight { get; set; }
        public INode Target { get; set; }
        public int TargetSensoryIndex { get; set; }
        public int MotorIndex { get; set; }
        public int? ComponentIndex { get; set; }
    }
}
