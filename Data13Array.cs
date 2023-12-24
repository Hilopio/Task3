using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Task1.Programm;

namespace Task1
{
    public class Data13Array : Data
    {
        public double[] TotalData { get; set; }
        public override IEnumerator<DataItem> GetEnumerator()//
        {
            for (int i = 0; i < TotalData.Length / 3; ++i)
            {
                yield return new DataItem(TotalData[i], TotalData[TotalData.Length + i], TotalData[TotalData.Length * 2 + i]);
            }
        }
        public Data13Array(string key, DateTime date_time) : base(key, date_time)
        {
            TotalData = new double[] { };

        }
        public Data13Array(string key, DateTime date_time, double[] x, FValues F) : this(key, date_time)
        {
            TotalData = new double[3 * x.Length];

            for (int i = 0; i < x.Length; ++i)
            {
                TotalData[i] = x[i];
                F(x[i], ref TotalData[i + x.Length], ref TotalData[i + 2 * x.Length]);
            }
        }
        public override double MaxDistance
        {
            get
            {
                return 0;
            }
        }
        public override double Y2ForXMax
        {
            get
            {
                if (TotalData.Length == 0) return 0;
                double max = TotalData[0];
                int max_index = 0;
                int len = TotalData.Length / 3;
                for (int i = 0; i < len; ++i)
                {
                    if (TotalData[i] > max) { max = TotalData[i]; max_index = i; }
                }
                return TotalData[max_index + 2 * len];
            }
        }
        public override string ToString()
        {
            return $"type = Data13Array \ninfo = {base.ToString()} \ntotal = {TotalData.Length / 3}";
        }

        public override string ToLongString(string format)
        {
            StringBuilder ans = new StringBuilder();
            int len = TotalData.Length / 3;
            for (int i = 0; i < len; ++i)
            {
                ans.Append($"{TotalData[i].ToString(format)} {TotalData[i + len]} {TotalData[i + 2 * len]}\n");
            }

            return $"{this}\ndata:\n{ans}";
        }
    };
}
