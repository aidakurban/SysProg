using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysPro.Logs
{
    public class Log
    {
        public string Time { get; set; }
        public string Info { get; set; }
        public Log(string info)
        {
            Time = DateTime.Now.ToString("HH:mm:ss");
            Info = info;
        }


    }
}
