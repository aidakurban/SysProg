namespace SysPro
{
    public class NodeRemovedEventArgs
    {
        public object SelectedNode { get; set; }

        public NodeRemovedEventArgs(object selectedNode)
        {
            SelectedNode = selectedNode;
        }


    }
}