using System.Drawing;

public class SearchParameters
{
    private Node[,] nodes;
    private int height;
    private int width;
    private Node startNode;
    private Node endNode;
    private bool[,] map;

    public Point StartLocation { get; set; }
    public Point EndLocation { get; set; }

    public SearchParameters(Point startLocation, Point endLocation, bool[,] map)
    {
        this.StartLocation = startLocation;
        this.EndLocation = endLocation;
        this.map = map;

        this.height = map.GetLength(0); // número de linhas
        this.width = map.GetLength(1);  // número de colunas
        this.nodes = new Node[height, width];

        InitializeNodes();
    }

    private void InitializeNodes()
    {
        for (int row = 0; row < height; row++)
        {
            for (int column = 0; column < width; column++)
            {
                Point location = new Point(column, row);
                bool isWalkable = map[row, column];

                Node node = new Node(location, isWalkable);
                nodes[row, column] = node;

                if (location == StartLocation)
                    startNode = node;

                if (location == EndLocation)
                    endNode = node;

                node.H = Node.GetTraversalCost(location, EndLocation);
            }
        }

        startNode.G = 0;
        startNode.State = NodeState.Open;
    }

    private List<Node> GetAdjacentWalkableNodes(Node fromNode)
    {
        List<Node> walkableNodes = new List<Node>();
        IEnumerable<Point> adjacentLocations = GetAdjacentLocations(fromNode.Location);

        foreach (var location in adjacentLocations)
        {
            int row = location.Y;
            int column = location.X;
            // Verifica se a posição está dentro dos limites do mapa
            if (row < 0 || row >= height || column < 0 || column >= width)
                continue;

            Node node = nodes[row, column];
            // Verifica se o nó é caminhável e não está fechado
            if (!node.IsWalkable || node.State == NodeState.Closed)
                continue;
            // 
            float traversalCost = Node.GetTraversalCost(node.Location, fromNode.Location);
            float gTemp = fromNode.G + traversalCost;
            // Verifica se o nó já está aberto e se o novo custo é menor que o atual
            if (node.State == NodeState.Open)
            {
                if (gTemp < node.G)
                {
                    node.ParentNode = fromNode;
                    node.G = gTemp;
                    walkableNodes.Add(node);
                }
            }
            else
            {
                node.ParentNode = fromNode;
                node.G = gTemp;
                node.State = NodeState.Open;
                walkableNodes.Add(node);
            }
        }

        return walkableNodes;
    }

    private IEnumerable<Point> GetAdjacentLocations(Point location)
    {
        return new List<Point>
        {
            new Point(location.X - 1, location.Y),
            new Point(location.X + 1, location.Y),
            new Point(location.X, location.Y - 1),
            new Point(location.X, location.Y + 1)
        };
    }
    // Método recursivo para busca em profundidade

    private bool Search(Node currentNode)
    {
        currentNode.State = NodeState.Closed;
        List<Node> nextNodes = GetAdjacentWalkableNodes(currentNode);
        // Ordena os nós adjacentes com base no custo total estimado (F = G + H)
        nextNodes.Sort((node1, node2) => node1.F.CompareTo(node2.F));

        foreach (var nextNode in nextNodes)
        {
            if (nextNode.Location == EndLocation)
                return true;

            if (Search(nextNode))
                return true;
        }

        return false;
    }

    public List<Point> FindPath()
    {
        List<Point> path = new List<Point>();
        bool success = Search(startNode);

        if (success)
        {
            Node node = endNode;
            while (node.ParentNode != null)
            {
                path.Add(node.Location);
                node = node.ParentNode;
            }
            path.Reverse();
        }
        return path;
    }
}
