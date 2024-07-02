using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysPro.Logs
{
    public class Logi
    {
        public List<Log> Log { get; set; }

        public Logi()
        {
            Log = new List<Log>();
        }
        public void AddLog(string info)
        {
            Log log = new Log(info);
            Log.Add(log);
        }

    }
}
