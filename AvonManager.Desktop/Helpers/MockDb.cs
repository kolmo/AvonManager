using System.Collections.ObjectModel;

namespace AvonManager.Helpers
{
    public class Node
    {
        public string Name { get; set; }
        public ObservableCollection<Node> Subserien { get; private set; }
        public Node()
        {
            Subserien = new ObservableCollection<Node>();
        }
    }
    public class MockDb
    {

        public static ObservableCollection<Node> GetNodesTree()
        {
             ObservableCollection<Node> Nodes = new ObservableCollection<Node>();
      Node n = new Node { Name = "Peter" };
            Node n2 = new Node { Name = "Klaus" };
            Node n3 = new Node { Name = "Heinz" };
            Node n4 = new Node { Name = "Frida" };
            Node n5 = new Node { Name = "Maria" };
            n.Subserien.Add(n2);
            n.Subserien.Add(n3);
            Nodes.Add(n);
            Nodes.Add(n4);
            Nodes.Add(n5);
            return Nodes ;
        }
    }
}
