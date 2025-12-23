class Program
{
    const int WIDTH = 9;
    const int HEIGHT = 9;

    public static void Main(string[] args)
    {
        // Hard-Coded paths for testing
        string[] seedPaths = [
            "./SamplePuzzleSeed.csv",
            "./SamplePuzzleSeed2.csv",
            "./SampleInvalidSeed.csv"
        ];

        // Parse the CLI input and try to select a file path from the above list
        int pathNumber = 0;

        if (args.Length > 0)
        {
            if (int.TryParse(args[0], out pathNumber))
            {
                if (pathNumber >= 0 && pathNumber < seedPaths.Length)
                { 
                    System.Console.WriteLine($"Using {seedPaths[pathNumber]}");
                } 
                else
                {
                    System.Console.WriteLine($"Provided argument {pathNumber} invalid. Using {seedPaths[pathNumber]} as path.");
                    pathNumber = 0;
                }
            }
        }

        string path = seedPaths[pathNumber];
        
        // Verify that the chosen path actually exists
        if (!System.IO.File.Exists(path))
        {
            System.Console.WriteLine($"File path '{seedPaths[pathNumber]}' not found. Verify the file exists, and that the permissions are correct.");
        }

        // Read the input seed file and generate a sudoku grid array
        Grid grid = new(path);

        // Solve the sudoku puzzle
        List<int[][]> solutions = new();
        Backtracker.Backtrack(grid.Vertices, grid.Vertices, solutions);

        // Distill the list of solutions to filter out the duplicates
        // Ideally, this step is redundant. However, if there is something wrong in the solving process,
        // then this will highlight a discrepancy.
        List<int[][]> distinctSolutions = GetDistinct2DArrays(solutions);
        
        // Output the results
       System.Console.WriteLine("Original Puzzle:");
       System.Console.WriteLine(GetAsciiReprGrid(grid.Vertices));
       System.Console.WriteLine($"Found {solutions.Count} solutions");
        if (solutions.Count > 0)
        {
           System.Console.WriteLine(GetAsciiReprGrid(solutions[0]));
        }
        // foreach (var solution in solutions)
        // {
        //     System.Console.WriteLine(GetAsciiReprGrid(solution));
        // }
       System.Console.WriteLine($"Found {distinctSolutions.Count} distinct solutions");
        // foreach (var solution in distinctSolutions)
        // {
        //    System.Console.WriteLine(GetAsciiReprGrid(solution));
        // }
        
    }

    private static string GetAsciiReprGrid(int[][] grid)
    {
        return new AsciiGrid(grid).ToString();
    }

    private static List<int[][]> GetDistinct2DArrays(List<int[][]> arrayList)
    {
        List<int[][]> distinctArrays = new();
        // foreach(var array1 in arrayList)
        for (int i = 0; i < arrayList.Count; i ++)
        {
            var array1 = arrayList[i];
            bool exists = false;
            // foreach(var array2 in distinctArrays)
            for (int j = 0; j < distinctArrays.Count; j++)
            {
                //System.Console.WriteLine($"Comparing arrayList[{i}] to distinctArrays[{j}]");
                var array2 = distinctArrays[j];
                // If the lengths don't match, they clearly aren't the same.
                if (array1.Length != array2.Length)
                {
                    exists = false;
                    continue;
                }

                bool match = false;
                for (int y = 0; y < HEIGHT; y++)
                {
                    for (int x = 0; x < WIDTH; x++)
                    {
                        match = array1[y][x] == array2[y][x];
                        if (!match)
                        {
                            exists = false;
                            break;
                        }
                    }
                }
                if (match)
                {
                    exists = true;
                }
            }

            if (!exists)
            {
                distinctArrays.Add(array1);
            }
        }

        return distinctArrays;
    }
}