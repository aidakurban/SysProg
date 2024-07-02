namespace SysPro
{
    public class AnalysisEventArgs
    {
        public string LoopCode { get; set; }

        public AnalysisEventArgs(string loopCode)
        {
            LoopCode = loopCode;
        }


    }
}