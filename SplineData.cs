using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Task1;
using task3;

namespace Task1
{
    public class SplineData
    {
        public DataArray Items { get; set; }    // Входные данные
        public int m { get; set; }    // узлов в равномерной сетке
        public double[] nu_spline { get; set; }   // значения функции в сплайне
        public int IterationsLimit { get; set; }
        public int StopInfo { get; set; }
        public int Iterations { get; set; }
        public double ResidualMinimum { get; set; }
        public List<SplineDataItem> Spline { get; set; }  // итоговый сплайн


        [DllImport("C:\\Users\\HONOR\\source\\repos\\Solution1\\Build\\x64\\Debug\\Dll1.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Interpolation(int n, double[] X, double[] Y, int m, double[] StartValues, double[] splineValues,
            ref int StopInfo, int IterationsLimit, ref int Iterations);

        public SplineData(DataArray Items, int m, int IterationsLimit)
        {
            this.Items = Items;
            this.m = m;
            this.IterationsLimit = IterationsLimit;
            this.nu_spline = new double[Items.Grid.Length];
            Spline = [];
        }

        public static void SplineBuilding(SplineData data)
        {
            int _StopInfo = 0;
            int _Iterations = 0;

            double[] StartValues = new double[data.m];
            for (int i = 0; i < StartValues.Length; ++i) StartValues[i] = i;

            Interpolation(data.Items.Grid.Length, data.Items.Grid, data.Items.Fields[0],  data.m,
                StartValues, data.nu_spline, ref _StopInfo, data.IterationsLimit, ref _Iterations);

            double s = 0;
            data.Spline.Clear();
            for (int i = 0; i < data.nu_spline.Length; ++i)
            {
                s += (data.nu_spline[i] - data.Items.Fields[0][i]) * (data.nu_spline[i] - data.Items.Fields[0][i]);
                data.Spline =  data.Spline.Append(new SplineDataItem(data.Items.Grid[i], data.Items.Fields[0][i], data.nu_spline[i])).ToList();
            }
            s = Math.Sqrt(s);
            data.ResidualMinimum = s;
            data.StopInfo = _StopInfo;
            data.Iterations = _Iterations;
        }
        public string ToLongString(string format)
        {
            string str = "";
            str += "Входные значения:\n";
            str += Items.ToLongString(format);
            str += "Вычисленная аппроксимация:\n";
            foreach (var point in Spline) str += point.ToString(format);
            str += $"Минимальная невязка: {ResidualMinimum}\n";
            str += "Причина остановки: ";

            switch (StopInfo)
            {
                case 1:
                    str += "Достигнут максимум итераций";
                    break;
                case 2:
                    str += "Достигнут размер доверительной области";
                    break;
                case 3:
                    str += "Достигнута необходимая точность";
                    break;
                case 4:
                    str += "Достигнута точность нормы матрицы Якоби";
                    break;
                case 5:
                    str += "Достигнут указанный размер пробного шага";
                    break;
                case 6:
                    str += "Достигнута необходимая разность норм функции и ошибки";
                    break;
                default:
                    str += "Неизвестная причина остановки";
                    break;
            }
            str += $"\nПроизведено итераций: {Iterations}\n";
            return str;
        }
        public void Save(string filename, string format)
        {
            using (StreamWriter fs = new StreamWriter(filename))
            {
                fs.WriteLine(ToLongString(format));
            }
        }
    }
}   
   
