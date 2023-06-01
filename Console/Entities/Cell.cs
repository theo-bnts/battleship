namespace ConsoleProject.Entities
{
    /// <summary>
    /// Represents a cell on a grid.
    /// </summary>
    internal class Cell
    {
        private readonly int x;
        private readonly int y;
        private bool selected;
        private bool discover;

        /// <summary>
        /// Gets the x-coordinate of the cell.
        /// </summary>
        public int X
        {
            get { return x; }
        }

        /// <summary>
        /// Gets the y-coordinate of the cell.
        /// </summary>
        public int Y
        {
            get { return y; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the cell is selected.
        /// </summary>
        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the cell is discovered.
        /// </summary>
        public bool Discover
        {
            get { return discover; }
            set { discover = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> class.
        /// </summary>
        /// <param name="x">The x-coordinate of the cell.</param>
        /// <param name="y">The y-coordinate of the cell.</param>
        public Cell(int x, int y)
        {
            this.x = x;
            this.y = y;
            selected = false;
            discover = false;
        }

        /// <summary>
        /// Checks if the cell has the same position as the specified cell.
        /// </summary>
        /// <param name="cell">The cell to compare.</param>
        /// <returns>True if the cells have the same position, otherwise false.</returns>
        public bool HaveSamePosition(Cell cell)
        {
            return cell.x == x && cell.y == y;
        }

        /// <summary>
        /// Checks if the cell is a neighbor of the specified cell.
        /// </summary>
        /// <param name="cell">The cell to compare.</param>
        /// <returns>True if the cells are neighbors, otherwise false.</returns>
        public bool IsNeighbour(Cell cell)
        {
            bool isNeighbour = true;

            if (HaveSamePosition(cell))
            {
                isNeighbour = false;
            }

            if (cell.x > x + 1 || cell.x < x - 1)
            {
                isNeighbour = false;
            }

            if (cell.y > y + 1 || cell.y < y - 1)
            {
                isNeighbour = false;
            }

            return isNeighbour;
        }

        /// <summary>
        /// Checks if the cell is a horizontal neighbor of the specified cell.
        /// </summary>
        /// <param name="cell">The cell to compare.</param>
        /// <returns>True if the cells are horizontal neighbors, otherwise false.</returns>
        public bool IsHorizontalNeighbour(Cell cell)
        {
            return IsNeighbour(cell) && cell.y == y;
        }

        /// <summary>
        /// Checks if the cell is a vertical neighbor of the specified cell.
        /// </summary>
        /// <param name="cell">The cell to compare.</param>
        /// <returns>True if the cells are vertical neighbors, otherwise false.</returns>
        public bool IsVerticalNeighbour(Cell cell)
        {
            return IsNeighbour(cell) && cell.x == x;
        }

        /// <summary>
        /// Checks if the cell is part of a boat on the grid.
        /// </summary>
        /// <param name="grid">The grid to search for boats.</param>
        /// <returns>True if the cell is part of a boat, otherwise false.</returns>
        public bool IsBoat(Grid grid)
        {
            bool isBoat = false;

            foreach (Boat boat in grid.Boats)
            {
                foreach (Cell boatCell in boat.Cells)
                {
                    if (HaveSamePosition(boatCell))
                    {
                        isBoat = true;
                        break;
                    }
                }
            }

            return isBoat;
        }

        /// <summary>
        /// Gets the boat that the cell is part of on the grid.
        /// </summary>
        /// <param name="grid">The grid to search for boats.</param>
        /// <returns>The boat that the cell is part of.</returns>
        /// <exception cref="Exception">Thrown when the cell is not part of a boat.</exception>
        public Boat GetRelativeBoat(Grid grid)
        {
            foreach (Boat boat in grid.Boats)
            {
                foreach (Cell boatCell in boat.Cells)
                {
                    if (HaveSamePosition(boatCell))
                    {
                        return boat;
                    }
                }
            }

            throw new Exception("Cell is not part of a boat");
        }

        /// <summary>
        /// Checks if the cell is a neighbor of any boat on the grid.
        /// </summary>
        /// <param name="grid">The grid to search for boats.</param>
        /// <returns>True if the cell is a neighbor of any boat, otherwise false.</returns>
        public bool IsBoatNeighbour(Grid grid)
        {
            bool isNeighbourOfABoat = false;

            foreach (Boat boat in grid.Boats)
            {
                foreach (Cell boatCell in boat.Cells)
                {
                    if (IsNeighbour(boatCell))
                    {
                        isNeighbourOfABoat = true;
                    }
                }
            }

            return isNeighbourOfABoat;
        }

        /// <summary>
        /// Sorts the list of cells by abscissa and ordinate.
        /// </summary>
        /// <param name="cells">The list of cells to sort.</param>
        /// <returns>The sorted list of cells.</returns>
        public static List<Cell> SortByAbscissaAndOrdinate(List<Cell> cells)
        {
            return cells.OrderBy(cell => cell.x).ThenBy(cell => cell.y).ToList();
        }

        /// <summary>
        /// Checks if the cells in the list are horizontally or vertically aligned.
        /// </summary>
        /// <param name="cells">The list of cells to check.</param>
        /// <returns>True if the cells are horizontally or vertically aligned, otherwise false.</returns>
        public static bool AreHorizontallyOrVerticallyAligned(List<Cell> cells)
        {
            bool areAligned = true;

            if (cells.Count == 2)
            {
                if (!(cells[0].IsHorizontalNeighbour(cells[1]) || cells[0].IsVerticalNeighbour(cells[1])))
                {
                    areAligned = false;
                }
            }

            if (cells.Count >= 3)
            {
                cells = SortByAbscissaAndOrdinate(cells);

                for (int i = 0; i < cells.Count - 2; i++)
                {
                    if (!(cells[i].IsHorizontalNeighbour(cells[i + 1]) && cells[i + 1].IsHorizontalNeighbour(cells[i + 2]) || cells[i].IsVerticalNeighbour(cells[i + 1]) && cells[i + 1].IsVerticalNeighbour(cells[i + 2])))
                    {
                        areAligned = false;
                    }
                }
            }

            return areAligned;
        }
    }
}
