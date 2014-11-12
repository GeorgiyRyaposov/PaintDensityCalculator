using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaintDensityCalculator.Models
{
    public class Density
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public float DensityValue { get; set; }
        public int AccordingThinner { get; set; }
        public int AccordingHardener { get; set; }
        public float ProportionThinner { get; set; }
        public float ProportionHardener { get; set; }
    }
}
