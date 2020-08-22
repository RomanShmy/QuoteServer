using System.Text;
using System;
using System.Collections.Generic;

namespace MainServer
{
    public class Profiler
    {
        private List<TimeSpan> proccessTime;
        private string type;

        public Profiler()
        {
            proccessTime = new List<TimeSpan>();
        }

        public void AddTime(TimeSpan time)
        {
            proccessTime.Add(time);
        }
        public string GetProccessTime()
        {
            return $"Average time ({type.ToUpper()}): {GetAvg(proccessTime)}";
        }

        private TimeSpan GetAvg(List<TimeSpan> times)
        {
            TimeSpan timeSpan = new TimeSpan(0,0,0,0,0);
            foreach(var time in times)
            {
                timeSpan+=time;
            }
            return timeSpan/times.Count;
        }

        internal void AddType(string type = "sync")
        {
            this.type = type;
        }
    }
}