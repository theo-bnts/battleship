using ConsoleProject.Tools;
using System.Text.RegularExpressions;

namespace ConsoleProject.Entities
{
    internal abstract class InputHandler
    {
        private static void ClearAllLinesExceptTwoFirsts()
        {
            for (int cursorHeight = 2; cursorHeight < Console.BufferHeight; cursorHeight++)
            {
                Console.SetCursorPosition(0, cursorHeight);
                Console.Write(new string(' ', Console.BufferWidth));
            }

            Console.SetCursorPosition(0, 2);
        }

        private static void DisplayCellChoiceConsigns(Grid grid, int size, int cellsCount)
        {
            ClearAllLinesExceptTwoFirsts();

            grid.Write(true);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Please specify the coordinates of the next cell and press enter (e.g. A1)\nBoat size : {0} - Cells to place : {1}\n", size, size - cellsCount);
            Console.ResetColor();
        }

        public static void PlaceBoatOnGrid(Grid grid, int size)
        {
            DisplayCellChoiceConsigns(grid, size, 0);

            List<Cell> cells = new ();

            while (cells.Count < size)
            {
                try
                {
                    string? coordinates = Console.ReadLine();

                    if (coordinates == null)
                    {
                        throw new Exception("Coordinates not provided");
                    }

                    Regex regex = new ("^([A-Z]{1})([0-9]+)$");

                    if (!regex.IsMatch(coordinates))
                    {
                        throw new Exception("Coordinates are not valid");
                    }

                    Match match = regex.Match(coordinates);

                    int colonne = Alphabet.IndexOf(Convert.ToChar(match.Groups[1].Value));
                    int ligne = Convert.ToInt32(match.Groups[2].Value) - 1;

                    Cell cell = grid.GetCell(colonne, ligne);

                    if (cell.Selected)
                    {
                        throw new Exception("Cell is already selected");
                    }

                    if (grid.IsBoatNeighbour(cell))
                    {
                        throw new Exception("Cell is too close to another boat");
                    }

                    if (!Cell.AreHorizonltalyOrVerticallyAligned(cells.Concat(new List<Cell> { cell }).ToList()))
                    {
                        throw new Exception("Boat cells are not neighbours or horizontally / vertically aligned");
                    }

                    cell.Selected = true;

                    cells.Add(cell);

                    DisplayCellChoiceConsigns(grid, size, cells.Count);
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n{0}\n", e.Message);
                    Console.ResetColor();
                }
            }

            Boat boat = new (cells);

            grid.AddBoat(boat);
        }
    }
}
