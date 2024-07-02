using SysPro.Event;
using SysPro.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysPro.Interface
{
    public interface IModel
    {
        string FilePath { get; set; }
        List<Node> Nodes { get; set; }
        double InputValue1 { get; set; }
        double InputValue2 { get; set; }
        int OutputValue { get; set; }


        void InitFile();
        void SaveFile();
        void AddNode(string name, string lastUpdateTime, string version);
        void RemoveNode(Node node);
        void EditNode(Node node);

            
        void AddLog(string info);
        IEnumerable<Log> GetLogs();


        void LowLevel();


        event EventHandler<ErrorThrowedEventArgs> OnErrorThrowed;
        int IterationsCount { get; set; }
        void ToAnalyze(string loopText);

    }
}
