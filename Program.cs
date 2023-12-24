using System;
using System.Data;
using System.Text.Json;
using Task1;
using static Task1.Programm;

namespace Task1
{

    public class Programm
    {
        public delegate void FValues(double x, ref double y1, ref double y2);
        public delegate DataItem FDI(double x);
        public static DataItem FDI_F(double x)
        {
            DataItem item = new(x, 2 * x, 0);
            return item;
        }
        public static void FValues_F(double x, ref double y1, ref double y2)
        {
            y1 = x + 3;
            y2 = 0;
        }

        static void Main()
        {

            Task3_tests();

        }

        public static void Task3_tests()
        {
            double[] x1 = { 1, 2, 4};
            double[] x2 = { 1, 5, 6, 10, 100 };


            DataArray arr1 = new DataArray("x1", DateTime.Today, x1, FValues_F);
            SplineData data1 = new SplineData(arr1, 3, 100);
            SplineData.SplineBuilding(data1);
            data1.Save("C:\\Users\\HONOR\\source\\repos\\Solution1\\x1.txt", "f3");
            Console.WriteLine(data1.ToLongString("0.##"));

            DataArray arr2 = new DataArray("x2", DateTime.Today, x2, FValues_F);
            SplineData data2 = new SplineData(arr2, 4, 1000);
            SplineData.SplineBuilding(data2);
            data2.Save("C:\\Users\\HONOR\\source\\repos\\Solution1\\x2.txt", "f3");
            Console.WriteLine(data2.ToLongString("0.##"));

            


            

        }
        
    }
}

































/*
 public static void Task1_tests()
        {
            double[] x = { 10.5, 20.5, 30.5 };

            Console.WriteLine("========================== 1 ==========================");
            DataList mylist = new("DataList_word1", DateTime.Today, x, FDI_F);
            Console.WriteLine(mylist.ToLongString(""));
            DataArray myarray = (DataArray)mylist;
            Console.WriteLine(myarray.ToLongString(""));

            Console.WriteLine("========================== 2 ==========================");
            DataArray myarray2 = new("DataArray_word1", DateTime.Today, x, FValues_F);
            Console.WriteLine(myarray2.ToLongString(""));
            Console.WriteLine(myarray2.Property.ToLongString(""));

            Console.WriteLine("========================== 3 ==========================");
            MainCollection mycollection = new(2, 2);
            Console.WriteLine(mycollection.ToLongString(""));

            Console.WriteLine("========================== 4 ==========================");
            foreach (Data data in mycollection)
            {
                Console.WriteLine(data.MaxDistance);
            }

            Console.WriteLine("========================== 5 ==========================");
            MainCollection mycollection2 = new();
            Console.WriteLine(mycollection2.ToLongString(""));
            foreach (Data data in mycollection2)
            {
                Console.WriteLine(data.Y2ForXMax);
            }
            
        }

        public static void Read_write_tests()
        {
            var x = new double[] { 1.0, 2, 3.5, 4.4, 5.8 };
            var array_1 = new DataArray($"test_array_key", DateTime.Today, x, FValues_F);
            Console.WriteLine(array_1.ToLongString(""));

            DataArray.Save("store.json", ref array_1);

            var array_2 = new DataArray($"new_test_array_key", DateTime.Today);
            DataArray.Load("store.json", ref array_2);

            Console.WriteLine(array_2.ToLongString(""));
        }
        public static void Linq_tests()
        {
            var Collection_1 = new MainCollection(0, 0)
            {
                new DataList("key1", DateTime.Today, new double[] { 1, 2, 3 }, FDI_F),
                new DataArray("key2", DateTime.Today, new double[] { -1, 1, 2}, FValues_F),
                new DataList("key3", DateTime.Today, new double[] { 5, 5, 3 }, FDI_F)
            };

            Console.WriteLine(Collection_1.ToLongString(""));
            Console.WriteLine("Test Mean_Norm:\n" + Collection_1.Mean_Norm);
            Console.WriteLine("Test Max_Deviation:\n" + Collection_1.Max_Deviation);
            

            Console.WriteLine("Test Repetitive_grid:");
            if (Collection_1.Repetitive_grid == null)
            {
                return;
            }
            foreach (var x in Collection_1.Repetitive_grid)
            {
                Console.WriteLine(x);
            }
            Console.WriteLine("Test Lower_min:" );
            if(Collection_1.Lower_min == null)
            {
                return;
            }
            foreach(var di in Collection_1.Lower_min) 
            { 
                Console.WriteLine(di); 
            }
        }

        public static void Mytests()
        {
            var x = new double[] { 1.0, 2, 3.5, 4.4, 5.8 };
            var array_3 = new DataArray($"test_array_key", DateTime.Today, x, FValues_F);
            Console.WriteLine(array_3.ToLongString(""));

            using (StreamWriter fs = new("store.json"))
            {
                string? s = JsonSerializer.Serialize(array_3.Key);
                fs.WriteLine(s);
                s = JsonSerializer.Serialize(array_3.Date_Time);
                s = null;
                fs.WriteLine(s);
                s = JsonSerializer.Serialize(array_3.Grid);
                fs.WriteLine(s);
                s = JsonSerializer.Serialize(array_3.Fields[0]);
                fs.WriteLine(s);
                s = JsonSerializer.Serialize(array_3.Fields[1]);
                fs.WriteLine(s);
            }

            var array_4 = new DataArray($"test_array_key", DateTime.Today);
            DataArray.Load("store.json", ref array_4);

            Console.WriteLine(array_4.ToLongString(""));
        }
*/


