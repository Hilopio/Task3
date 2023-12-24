using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Task1.Programm;


namespace Task1
{
    public class DataArray : Data
    {
        public override IEnumerator<DataItem> GetEnumerator()
        {
            for (int i = 0; i < Grid.Length; ++i)
            {
                yield return new DataItem(Grid[i], Fields[0][i], Fields[1][i]);
            }
        }
        public double[] Grid { get; set; }
        public double[][] Fields { get; set; }
        public DataArray(string key, DateTime date_time) : base(key, date_time)
        {
            Grid = Array.Empty<double>();
            Fields = Array.Empty<double[]>();

        }
        public DataArray(string key, DateTime date_time, double[] x, FValues F) : this(key, date_time)
        {
            Grid = new double[x.Length];
            x.CopyTo(Grid, 0);
            Fields = new double[2][];
            Fields[0] = new double[x.Length];
            Fields[1] = new double[x.Length];

            for (int i = 0; i < Grid.Length; ++i)
            {
                F(Grid[i], ref Fields[0][i], ref Fields[1][i]);
            }
        }

        public DataArray(string key, DateTime date, int nX, double xL, double xR, FValues F) : base(key, date)
        {
            Grid = new double[nX];
            Fields = new double[Grid.Length][];
            double h = (xR - xL) / (nX - 1);

            for (int i = 0; i < nX; ++i)
            {
                Fields[i] = new double[2];
                Grid[i] = xL + i * h;
;               F(Grid[i], ref Fields[0][i], ref Fields[1][i]);
            }
        }

        public double[] this[int i]
        {
            get => Fields[i];
        }

        public DataList Property
        {
            get {
                DataList list = new(this.Key, this.Date_Time);
                for (int i = 0; i < Grid.Length; ++i)
                {
                    DataItem temp = new(Grid[i], Fields[0][i], Fields[1][i]);
                    list.AllData.Add(temp);
                }
                return list; 
            }
        }

        public override double Y2ForXMax
        {
            get
            {
                if (Grid.Length == 0) return 0;
                double max = Grid[0];
                int max_index = 0;
                for (int i = 0; i < Grid.Length; ++i)
                {
                    if (Grid[i] > max) { max = Grid[i]; max_index = i; }
                }
                return Fields[1][max_index];
            }
        }
        public override double MaxDistance
        {
            get
            {
                if (Grid.Length == 0) return 0;
                double min = Grid[0], max = min;
                foreach (double _ in Grid)
                {
                    if (_ > max) { max = _; }
                    if (_ < min) { min = _; }
                }

                return max - min;
            }
        }
        public override string ToString()
        {
            return $"type = DataArray \ninfo = {base.ToString()} \ntotal = {Grid.Length}";
        }

        public override string ToLongString(string format)
        {
            StringBuilder ans = new StringBuilder();
            for (int i = 0; i < Grid.Length; ++i)
            {
                ans.Append($"{Grid[i].ToString(format)} {Fields[0][i]} {Fields[1][i]}\n");
            }

            return $"{this}\ndata:\n{ans}";
        }

        public static bool Save(string filename, ref DataArray array)
        {
            try
            {
                Console.WriteLine($"Saving to {filename}");
                using (StreamWriter fs = new(filename))
                {
                    string s = JsonSerializer.Serialize(array.Key);
                    fs.WriteLine(s);
                    s = JsonSerializer.Serialize(array.Date_Time);
                    fs.WriteLine(s);
                    s = JsonSerializer.Serialize(array.Grid);
                    fs.WriteLine(s);
                    s = JsonSerializer.Serialize(array.Fields[0]);
                    fs.WriteLine(s);
                    s = JsonSerializer.Serialize(array.Fields[1]);
                    fs.WriteLine(s);
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Saving faileded");
                Console.WriteLine(ex.Message);
                return false;
            }
            Console.WriteLine("Saving completed");
            return true;
        }

        public static bool Load(string filename, ref DataArray array)
        {
            try
            {
                Console.WriteLine($"Reading from {filename}");
                using (StreamReader fs = new(filename))///???
                {
                    string s = fs.ReadLine();
                    array.Key = JsonSerializer.Deserialize<string>(s);
                    s = fs.ReadLine();
                    array.Date_Time = JsonSerializer.Deserialize<DateTime>(s);
                    s = fs.ReadLine();
                    array.Grid = JsonSerializer.Deserialize<double[]>(s);
                    s = fs.ReadLine();
                    var tmp1 = JsonSerializer.Deserialize<double[]>(s);
                    s = fs.ReadLine();
                    var tmp2 = JsonSerializer.Deserialize<double[]>(s);
                    array.Fields = new double[2][] { tmp1, tmp2 };
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Reading failed");
                Console.WriteLine(ex.Message);
                return false;
            }
            Console.WriteLine("Reading completed");
            return true;
        }
    };
}
