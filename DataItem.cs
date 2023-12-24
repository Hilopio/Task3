using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1
{
    public struct DataItem
    {
        public double x { get; set; }
        public double y1 { get; set; }
        public double y2 { get; set; }
        public DataItem(double x, double y1, double y2)
        {
            this.x = x;
            this.y1 = y1;
            this.y2 = y2;
        }
        
        public override string ToString()
        {
            return $"{x} {y1} {y2}";
        }

        public string ToLongString(string format)
        {
            return $"{this}";
        }
    }
}
