namespace SysPro
{
    public class NodeEditedEventArgs
    {
        public object Node { get; set; }

        public NodeEditedEventArgs(object node)
        {
            Node = node;
        }

    }
}