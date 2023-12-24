using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task3
{
    public struct SplineDataItem
    {
        double x {  get; set; }
        double y_true { get; set; }
        double y_calc { get; set; }

        public SplineDataItem(double x, double y1, double y2)
        {
            this.x = x;
            this.y_true = y1;
            this.y_calc = y2;
        }
        public string ToString(string format)
        {
            return $"{x.ToString(format)}, {y_true.ToString(format)}, {y_calc.ToString(format)}\n";
        }
        public override string ToString()
        {
            return $"{x.ToString()}, {y_true.ToString()}, {y_calc.ToString()}\n";
        }
    }
}
