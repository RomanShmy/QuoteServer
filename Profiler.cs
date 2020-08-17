using System.Text;
using System;
using System.Collections.Generic;

namespace MainServer
{
    public class Profiler
    {
        private List<TimeSpan> proccessTime;

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
            StringBuilder result = new StringBuilder();
            foreach(var time in proccessTime)
            {
                result.Append(time).Append("<br>\n");   
            }
            if(proccessTime.Count == 10)
            {
                result.Append($"Average time: {GetAvg(proccessTime)}").Append("<br>\n"); 
            }

            return result.ToString();
        }

        private TimeSpan GetAvg(List<TimeSpan> times)
        {
            TimeSpan timeSpan = new TimeSpan(0,0,0,0,0);
            foreach(var time in proccessTime)
            {
                timeSpan+=time;
            }
            return timeSpan/times.Count;
        }
        
    }
}