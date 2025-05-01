using System.Drawing;
public class Node
{
    public Point Location { get; private set; }
    public bool IsWalkable { get; set; }

    public float G { get; set; } // Custo acumulado desde o início até este nó
    public float H { get; set; } // Heurística (estimativa até o destino)
    public float F => G + H;     // Custo total estimado

    public NodeState State { get; set; }

    public Node ParentNode { get; set; }

    public Node(Point location, bool isWalkable)
    {
        this.Location = location;
        this.IsWalkable = isWalkable;
        this.State = NodeState.Untested;
        this.G = float.MaxValue; // Inicialmente, o custo é infinito
    }

    // Distância de Manhattan (ideal para grid com movimento apenas em 4 direções)
    public static float GetTraversalCost(Point from, Point to)
    {
        return Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y);
    }
}
