using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysPro.Interface
{
    public interface IView
    {
        event EventHandler<FileInitEventArgs> OnFileInitialized;
        event EventHandler<NodeAddedEventArgs> OnNodeAdded;
        void UpdateNodesList(List<Node> nodes);
        event Action OnFileSaved;
        event EventHandler<NodeRemovedEventArgs> OnNodeRemoved;
        event EventHandler<NodeEditedEventArgs> OnNodeEdited;


        event Action OnLogsOpened;
        void ShowLogs(string Logs);

        event EventHandler<LowLevelEventArgs> LowLevelStarted;
        void ShowLowLvlResult(string result);


        void ShowErrorMessage(string message);



        event EventHandler<AnalysisEventArgs> OnAnalysisStarted;
        void ShowResult(string result);


    }
}
