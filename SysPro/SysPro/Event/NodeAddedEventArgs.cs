
namespace SysPro
{
    public class NodeAddedEventArgs
    {
        public string NodePath { get; set; }

        public NodeAddedEventArgs(string nodePath)
        {
            NodePath = nodePath;
        }

    }
}