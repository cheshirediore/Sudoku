// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

int xSize, ySize;
xSize = 9;
ySize = 9;
AsciiGrid asciiGrid = new(xSize, ySize);
asciiGrid.SetAll();
// for (int x = 0; x < xSize; x++)
// {
//     for (int y = 0; y < xSize; y++)
//     {
//         asciiGrid.SetGridCell(x, y, $"{x}x{y}y");
//     }
// }
Console.WriteLine(asciiGrid);