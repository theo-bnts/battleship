using ConsoleProject.Tools;
using System.Text.RegularExpressions;

namespace ConsoleProject.Entities
{
    internal abstract class InputHandler
    {
        public static void DisplayPlayerName(string name)
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("{0}\n", name);
            Console.ResetColor();
        }

        public static Cell ReadCell(Grid grid)
        {
            string? coordinates = Console.ReadLine();

            if (coordinates == null)
            {
                throw new Exception("Coordinates not provided");
            }

            Regex regex = new("^([A-Z]{1})([0-9]+)$");

            if (!regex.IsMatch(coordinates))
            {
                throw new Exception("Coordinates are not valid");
            }

            Match match = regex.Match(coordinates);

            int colonne = Alphabet.IndexOf(Convert.ToChar(match.Groups[1].Value));
            int ligne = Convert.ToInt32(match.Groups[2].Value) - 1;

            return grid.GetCell(colonne, ligne);
        }

        public static void PlaceBoatOnGrid(string player, Grid grid, int size)
        {
            static void DisplayCellChoiceConsigns(string player, Grid grid, int size, int cellsCount)
            {
                DisplayPlayerName(player);

                grid.Write(true);

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Please specify the coordinates of the next cell and press enter (e.g. A1)\nBoat size : {0} - Cells to place : {1}\n", size, size - cellsCount);
                Console.ResetColor();
            }

            DisplayCellChoiceConsigns(player, grid, size, 0);

            List<Cell> cells = new ();

            while (cells.Count < size)
            {
                try
                {
                    Cell cell = ReadCell(grid);

                    if (cell.Selected)
                    {
                        throw new Exception("Cell is already selected");
                    }

                    if (grid.IsBoatNeighbour(cell))
                    {
                        throw new Exception("Cell is too close to another boat");
                    }

                    if (!Cell.AreHorizonltallyOrVerticallyAligned(cells.Concat(new List<Cell> { cell }).ToList()))
                    {
                        throw new Exception("Boat cells are not neighbours or horizontally / vertically aligned");
                    }

                    if (!Boat.CanBePlacedOnGrid(grid, cells.Concat(new List<Cell> { cell }).ToList(), size))
                    {
                        throw new Exception("Boat can't be placed on grid if you select this cell");
                    }

                    cell.Selected = true;

                    cells.Add(cell);

                    DisplayCellChoiceConsigns(player, grid, size, cells.Count);
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Message);
                    Console.ResetColor();
                }
            }

            Boat boat = new (cells);

            grid.AddBoat(boat);
        }

        public static void DiscoverCell(Grid grid)
        {
            grid.Write();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Please specify the coordinates of the cell you would like to discover and press enter (e.g. A1)\n");
            Console.ResetColor();

            while (true)
            {
                try
                {
                    Cell cell = ReadCell(grid);

                    if (cell.Discover)
                    {
                        throw new Exception("Cell is already discovered");
                    }
                    else
                    {
                        cell.Discover = true;

                        break;
                    }
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Message);
                    Console.ResetColor();
                }
            }
        }

        public static void Win()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\nYou win !");
            Console.ResetColor();

            Environment.Exit(0);
        }
    }
}