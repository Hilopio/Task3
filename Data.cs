using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1
{
    public abstract class Data: IEnumerable<DataItem>//
    {
        public abstract IEnumerator<DataItem> GetEnumerator();//
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }//
        public string Key { get; set; }
        public DateTime Date_Time { get; set; }
        public Data(string key, DateTime date_time)
        {
            this.Key = key;
            this.Date_Time = date_time;
        }
        public abstract double MaxDistance { get; }
        
        public abstract double Y2ForXMax { get; }
        public override string ToString()
        {
            return $"(key is {Key}   Date_Time is {Date_Time})"; 
        }
        public virtual string ToLongString(string format)
        {
            return $"{this}";

        }
    };
}
