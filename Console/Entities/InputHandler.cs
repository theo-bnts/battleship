using ConsoleProject.Tools;
using System.Text.RegularExpressions;

namespace ConsoleProject.Entities
{
    /// <summary>
    /// Handles user input and interaction with the game.
    /// </summary>
    internal abstract class InputHandler
    {
        /// <summary>
        /// Displays the player's name on the console.
        /// </summary>
        /// <param name="name">The name of the player.</param>
        /// <param name="topOffset">The offset from the top of the console.</param>
        /// <param name="leftOffset">The offset from the left of the console.</param>
        public static void DisplayPlayerName(string name, int topOffset = 0, int leftOffset = 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(leftOffset, Console.CursorTop + topOffset);
            Console.WriteLine("{0}\n", name);
            Console.ResetColor();
        }

        /// <summary>
        /// Reads the user's input for cell coordinates and returns the corresponding cell on the grid.
        /// </summary>
        /// <param name="grid">The grid on which the cell is located.</param>
        /// <returns>The cell specified by the user.</returns>
        /// <exception cref="Exception">Thrown when the input coordinates are not valid.</exception>
        public static Cell ReadCell(Grid grid)
        {
            string coordinates = Console.ReadLine() ?? throw new Exception("Coordinates not provided");

            Regex regex = new("^([a-zA-Z]{1})([0-9]+)$");

            if (!regex.IsMatch(coordinates))
            {
                throw new Exception("Coordinates are not valid");
            }

            Match match = regex.Match(coordinates);

            int colonne = Alphabet.IndexOf(Convert.ToChar(match.Groups[1].Value));
            int ligne = Convert.ToInt32(match.Groups[2].Value) - 1;

            return grid.GetCell(colonne, ligne);
        }

        /// <summary>
        /// Places a boat on the grid based on the user's input.
        /// </summary>
        /// <param name="player">The name of the player.</param>
        /// <param name="grid">The grid on which to place the boat.</param>
        /// <param name="size">The size of the boat.</param>
        /// <exception cref="Exception">Thrown when the selected cell is invalid or the boat placement is not allowed.</exception>
        public static void PlaceBoatOnGrid(string player, Grid grid, int size)
        {
            static void DisplayCellChoiceConsigns(string player, Grid grid, int size, int cellsCount)
            {
                Console.Clear();

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

                    if (cell.IsBoatNeighbour(grid))
                    {
                        throw new Exception("Cell is too close to another boat");
                    }

                    if (!Cell.AreHorizontallyOrVerticallyAligned(cells.Concat(new List<Cell> { cell }).ToList()))
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

        /// <summary>
        /// Displays the grids of both players on the console.
        /// </summary>
        /// <param name="gridA">The grid of Player A.</param>
        /// <param name="gridB">The grid of Player B.</param>
        /// <param name="aBoatPlacementMode">Flag indicating whether to show Player A's grid in boat placement mode.</param>
        /// <param name="bBoatPlacementMode">Flag indicating whether to show Player B's grid in boat placement mode.</param>
        public static void DisplayGrids(Grid gridA, Grid gridB, bool aBoatPlacementMode, bool bBoatPlacementMode)
        {
            Console.Clear();

            DisplayPlayerName("Player A");
            DisplayPlayerName("Player B", -2, 4 * (gridB.Width + 1) + (gridB.Height.ToString().Length - 1) + 10);

            gridA.Write(aBoatPlacementMode);
            gridB.Write(bBoatPlacementMode, 0 - (2 * (gridA.Height + 1) + 2), 4 * (gridB.Width + 1) + (gridB.Height.ToString().Length - 1) + 10);
        }

        /// <summary>
        /// Allows the user to discover a cell on the grid and provides feedback on the result.
        /// </summary>
        /// <param name="grid">The grid on which to discover the cell.</param>
        /// <exception cref="Exception">Thrown when the selected cell is already discovered.</exception>
        public static void DiscoverCell(Grid grid)
        {
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

                        Console.ForegroundColor = ConsoleColor.Green;

                        if (cell.IsBoat(grid))
                        {
                            if (cell.GetRelativeBoat(grid).IsDestroyed())
                            {
                                Console.WriteLine("\nSunk");
                            }
                            else
                            {
                                Console.WriteLine("\nHit");
                            }
                        }
                        else
                        {
                            Console.WriteLine("\nMissed");
                        }

                        Console.ResetColor();

                        Thread.Sleep(1_000);

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

        /// <summary>
        /// Displays a message indicating that the player has won the game and exits the application.
        /// </summary>
        public static void Win()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\nYou win !");
            Console.ResetColor();

            Environment.Exit(0);
        }
    }
}