using System.Drawing;

public class Program
{
    public static void Main()
    {
        //definição do mapa
        bool[,] map = new bool[,]
        {
            { true, true, true, false, true,  true, true },
            { true, true, true, false, true,  true, true },
            { true, true, true, false, true,  true, true },
            { true, true, true, false, false, true, true },
            { true, true, true, true,  true,  true, true },
        };
        // Devido ao acesso ser [linha, coluna], os atributos de obj da 
        // classe Point (x = coluna, y = linha) são invertidos.
        // Ex: Point(1,2) = coluna 1 linha 2
        /*
        for (int row = 0; row < map.GetLength(0); row++)
        {
            for (int column = 0; column < map.GetLength(1); column++)
            {
                Console.Write(map[row,column] ? ". " : "█ ");
            }
            Console.WriteLine();
        }
        */
        Point start = new Point(1,2);   //  coluna 1 linha 2
        Point end = new Point(5,2); //  coluna 5 linha 2

        SearchParameters search = new SearchParameters(start, end, map);

        List<Point> path = search.FindPath();

        Console.WriteLine("Path found:");
        foreach (var p in path)
        {
            Console.WriteLine($"({p.Y}, {p.X})"); // Invertendo para exibir como linha, coluna
        }

        Console.WriteLine("\nGrid with path:");
        for (int row = 0; row < map.GetLength(0); row++)
        {
            for (int column = 0; column < map.GetLength(1); column++)
            {
                Point current = new Point(column, row);
                if (current == start)
                    Console.Write("S "); // Start
                else if (current == end)
                    Console.Write("E "); // End
                else if (!map[row, column])
                    Console.Write("█ "); // Wall
                else if (path.Contains(current))
                    Console.Write("* "); // Path
                else
                    Console.Write(". "); // Empty space
            }
            Console.WriteLine();
        }
    }
}
