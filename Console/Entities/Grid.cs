using ConsoleProject.Tools;
using ConsoleTables;

namespace ConsoleProject.Entities
{
    /// <summary>
    /// Represents a grid for the game.
    /// </summary>
    internal class Grid
    {
        private readonly int width;
        private readonly int height;
        private readonly List<Cell> cells;
        private readonly List<Boat> boats;

        /// <summary>
        /// Gets the width of the grid.
        /// </summary>
        public int Width
        {
            get { return width; }
        }

        /// <summary>
        /// Gets the height of the grid.
        /// </summary>
        public int Height
        {
            get { return height; }
        }

        /// <summary>
        /// Gets the list of boats on the grid.
        /// </summary>
        public List<Boat> Boats
        {
            get { return boats; }
        }

        /// <summary>
        /// Initializes a new instance of the Grid class with the specified width and height.
        /// </summary>
        /// <param name="width">The width of the grid.</param>
        /// <param name="height">The height of the grid.</param>
        public Grid(int width, int heigth)
        {
            this.width = width;
            height = heigth;
            cells = new List<Cell>();
            boats = new List<Boat>();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < this.width; x++)
                {
                    cells.Add(new Cell(x, y));
                }
            }
        }

        /// <summary>
        /// Gets the cell at the specified coordinates.
        /// </summary>
        /// <param name="x">The x-coordinate of the cell.</param>
        /// <param name="y">The y-coordinate of the cell.</param>
        /// <returns>The cell at the specified coordinates.</returns>
        /// <exception cref="Exception">Thrown when the cell is out of the grid.</exception>
        public Cell GetCell(int x, int y)
        {
            if (x < 0 || x > width - 1 || y < 0 || y > height - 1)
            {
                throw new Exception("Cell is out of grid");
            }

            int index = x + y * height;

            return cells[index];
        }

        /// <summary>
        /// Adds a boat to the grid.
        /// </summary>
        /// <param name="boat">The boat to add.</param>
        /// <exception cref="Exception">Thrown when the boat is overlapping or too close to another boat.</exception>
        public void AddBoat(Boat boat)
        {
            foreach (Cell cell in boat.Cells)
            {
                if (cell.IsBoat(this) || cell.IsBoatNeighbour(this))
                {
                    throw new Exception("Boat is overlapping or too close to another boat");
                }
            }

            boats.Add(boat);
        }

        /// <summary>
        /// Checks if all boats on the grid are destroyed.
        /// </summary>
        /// <returns>True if all boats are destroyed, otherwise false.</returns>
        public bool AllBoatsDestroyed()
        {
            return boats.All(boat => boat.IsDestroyed() == true);
        }

        /// <summary>
        /// Returns a string representation of the grid.
        /// </summary>
        /// <param name="boatPlacementMode">Flag indicating whether to show the grid in boat placement mode.</param>
        /// <returns>The string representation of the grid.</returns>
        public string ToString(bool boatPlacementMode)
        {
            List<string> columnNames = new () { " " };

            columnNames.AddRange(Alphabet.Take(width).Select(c => c.ToString()));

            ConsoleTable table = new (columnNames.ToArray());

            for (int y = 0; y < height; y++)
            {
                List<string> row = new()
                {
                    (y + 1).ToString()
                };

                for (int x = 0; x < width; x++)
                {
                    Cell cell = GetCell(x, y);

                    if (boatPlacementMode)
                    {
                        if (cell.Discover)
                        {
                            if (cell.IsBoat(this))
                            {
                                row.Add("X");
                            }
                            else
                            {
                                row.Add("-");
                            }
                        }
                        else
                        {
                            if (cell.Selected)
                            {
                                row.Add("@");
                            }
                            else
                            {
                                row.Add(" ");
                            }
                        }
                    }
                    else
                    {
                        if (cell.Discover)
                        {
                            if (cell.IsBoat(this))
                            {
                                Boat boat = cell.GetRelativeBoat(this);

                                if (boat.IsDestroyed())
                                {
                                    row.Add("@");
                                }
                                else
                                {
                                    row.Add("X");
                                }
                            }
                            else
                            {
                                row.Add("-");
                            }
                        }
                        else
                        {
                            row.Add(" ");
                        }
                    }
                }

                table.AddRow(row.ToArray());
            }

            return table.ToStringAlternative();
        }

        /// <summary>
        /// Writes the grid to the console.
        /// </summary>
        /// <param name="boatPlacementMode">Flag indicating whether to show the grid in boat placement mode.</param>
        /// <param name="topOffset">The offset from the top of the console.</param>
        /// <param name="leftOffset">The offset from the left of the console.</param>
        public void Write(bool boatPlacementMode, int topOffset = 0, int leftOffset = 0)
        {
            string[] lines = ToString(boatPlacementMode).Split("\n");

            Console.CursorTop += topOffset;

            foreach (string line in lines)
            {
                Console.SetCursorPosition(leftOffset, Console.CursorTop);
                Console.WriteLine(line);
            }
        }
    }
}
