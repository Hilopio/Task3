using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.VisualBasic;

namespace Task1
{
    public class MainCollection : System.Collections.ObjectModel.ObservableCollection<Data>
    {
        public bool Contains(string key)
        {
            return Items.Any(data => data.Key == key);
        }

        public new bool Add(Data data)
        {
            foreach (var item in Items)
            {
                if (item.Key == data.Key)
                {
                    return false;
                }
            }

            base.Add(data);
            return true;
        }

        public double Mean_Norm
        {
            get
            {
                var data = (from i in Items  //i is a DataArray or DataList
                           from j in i      //j is a DataItem
                           select Math.Sqrt(j.y1 * j.y1 + j.y2 * j.y2));
                if (!data.Any())
                {
                    return double.NaN;
                }
                var res = data.Sum() / data.Count();
                return res;
            }
        }

        public DataItem? Max_Deviation
        {
            get
            {
                if (Items.Count == 0)
                {
                    return null;
                }
                IEnumerable<DataItem> data = (from i in Items
                                              from j in i
                                              select j).OrderBy(x => Math.Abs(Math.Sqrt(x.y1 * x.y1 + x.y2 * x.y2) - Mean_Norm));

                return data.Last();
            }
        }

        public IEnumerable<double> Lower_min
        {
            get
            {
                var list_data = (from i in Items
                                from j in i
                                where i is DataList  select j.x);
                var min = list_data.Min();

                var array_data = (from i in Items
                                 from j in i
                                 where i is DataArray
                                 where j.x < min select j.x);
                return array_data;
            }
        }

        public IEnumerable<double>? Repetitive_grid
        {
            get
            {
                if (Items.Count == 0)
                {
                    return null;
                }

                var grid_copy  = (from i in Items
                             from j in i 
                             where i is DataList select j.x);
                IEnumerable<double> grid = (from i in Items
                           from j in i
                           where (grid_copy.Count(y => y == j.x) > 1) select j.x).OrderBy(x => x).Distinct();///??? 
                return grid;

            }
        }
        public MainCollection(int nDataArray, int nDataList)
        {
            void F1(double x, ref double y1, ref double y2)
            {
                y1 = 2 * x;
                y2 = x * x;
            }
            DataItem F2(double x)
            {
                DataItem item = new(x, 2 * x, x * x);
                return item;
            }
            double[] x = new double[] {0,1,2,3,4};
            for (int i = 0; i < nDataArray; ++i)
            {
                for( int j = 0; j < x.Length; ++j ) { x[j] += i * j; }
                DataArray temp_arr = new($"DataArray_{i}Key", DateTime.Today, x, F1);
                Add(temp_arr);
                for (int j = 0; j < x.Length; ++j) { x[j] -= i * j; }
            }
            for (int i = 0; i < nDataList; ++i)
            {
                for (int j = 0; j < x.Length; ++j) { x[j] += i * j; }
                DataList temp_list = new($"DataList_{i}Key", DateTime.Today, x, F2);
                Add(temp_list);
                for (int j = 0; j < x.Length; ++j) { x[j] -= i * j; }
            }
        }

        public MainCollection()
        {
            void F1(double x, ref double y1, ref double y2)
            {
                y1 = 2 * x;
                y2 = x * x;
            }
            DataItem F2(double x)
            {
                DataItem item = new(x, 10 * x, -9 * x);
                return item;
            }
            void F3(double x, ref double y1, ref double y2)
            {
                y1 = 1 + x;
                y2 = 2 * x;
            }
            double[] x = new double[] { 0, 1, 2, 4, 3 };

            DataArray da = new("DataArray_word9", DateTime.Today, x, F1);
            Add(da);
            
            Data13Array d13a = new("Data13Array_word8", DateTime.Today, x, F3);
            Add(d13a);

            DataList dl = new("DataList_word7", DateTime.Today, x, F2);
            Add(dl);
        }

        public string ToLongString(string format)
        {
            StringBuilder ans = new();


            foreach (var item in Items)
            {
                ans.Append($"{item.ToLongString(format)}\n");
            }

            return ans.ToString();
        }

        public override string ToString()
        {
            StringBuilder ans = new();

            foreach (var item in Items)
            {
                ans.Append($"{item}\n");
            }

            return ans.ToString();
        }
    }
}