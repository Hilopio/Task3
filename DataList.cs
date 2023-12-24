using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Task1.Programm;

namespace Task1
{
    public class DataList : Data
    {
        public override IEnumerator<DataItem> GetEnumerator()//
        {
            return AllData.GetEnumerator();
        }
        public List<DataItem> AllData { get; set; }
        public DataList(string key, DateTime date_time) : base(key, date_time)
        {
            AllData = new List<DataItem>();
        }
        public DataList(string key, DateTime date_time, double[] x, FDI F) : this(key, date_time)
        {
            foreach(double val in x.Distinct().ToArray()) { 
                DataItem point = F(val);
                AllData.Add(point);
            }
        }
        public override double Y2ForXMax
        {
            get
            {
                if (AllData.Count == 0) return 0;
                double max = AllData[0].x;
                int max_index = 0;
                for(int i = 0; i < AllData.Count; ++i)
                {
                    if (AllData[i].x > max) { max = AllData[i].x; max_index = i; }
                }
                return AllData[max_index].y2;
            }
        }
        public override double MaxDistance { 
            get 
            {
                if (AllData.Count == 0) return 0;
                double min = AllData[0].x, max = min;
                foreach(DataItem _ in AllData)
                {
                    if (_.x > max) { max = _.x; }
                    if (_.x < min) { min = _.x; }
                }

                return max - min;
            } 
        }

        public static explicit operator DataArray(DataList list)
        {
            DataArray array = new DataArray(list.Key, list.Date_Time);
            array.Grid = new double[list.AllData.Count];
            array.Fields = new double[2][];
            array.Fields[0] = new double[list.AllData.Count];
            array.Fields[1] = new double[list.AllData.Count];

            for (int i = 0; i < list.AllData.Count; ++i)
            {
                array.Grid[i] = list.AllData[i].x;
                array.Fields[0][i] = list.AllData[i].y1;
                array.Fields[1][i] = list.AllData[i].y2;
            }
            return array;
        }
        public override string ToString()
        {
            return $"type = DataList \ninfo = {base.ToString()} \ntotal = {AllData.Count}";
        }

        public override string ToLongString(string format)
        {
            StringBuilder ans = new StringBuilder();

            foreach (DataItem _ in AllData)
            {
                ans.Append($"{_.ToLongString(format)}\n");

            }

            return $"{this}\ndata:\n{ans}"; 
        }
    };
}
