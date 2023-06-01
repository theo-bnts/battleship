namespace ConsoleProject.Entities
{
    /// <summary>
    /// Represents a boat entity.
    /// </summary>
    internal class Boat
    {
        private readonly List<Cell> cells;

        /// <summary>
        /// Gets the list of cells that make up the boat.
        /// </summary>
        public List<Cell> Cells
        {
            get { return cells; }
        }

        /// <summary>
        /// Initializes a new instance of the Boat class with the specified cells.
        /// </summary>
        /// <param name="cells">The cells that make up the boat.</param>
        public Boat(List<Cell> cells)
        {
            if (cells.Count == 0)
            {
                throw new Exception("No boat cell");
            }

            if (!Cell.AreHorizontallyOrVerticallyAligned(cells))
            {
                throw new Exception("Boat cells are not neighbours or horizontally / vertically aligned");
            }

            this.cells = cells;
        }

        /// <summary>
        /// Checks if a boat can be placed on the grid based on the specified first cells and size.
        /// </summary>
        /// <param name="grid">The grid on which to place the boat.</param>
        /// <param name="firstsCells">The first cells of the boat.</param>
        /// <param name="size">The size of the boat.</param>
        /// <returns>True if the boat can be placed on the grid; otherwise, false.</returns>
        public static bool CanBePlacedOnGrid(Grid grid, List<Cell> firstCells, int size)
        {
            static int CountAvailableCellsInAlignment(Grid grid, int startX, int startY, int deltaX, int deltaY)
            {
                int count = 0;
                int x = startX;
                int y = startY;

                while (x >= 0 && x < grid.Width && y >= 0 && y < grid.Height)
                {
                    if (!grid.GetCell(x, y).Selected)
                    {
                        count++;
                    }
                    else
                    {
                        count--;
                        break;
                    }

                    x += deltaX;
                    y += deltaY;
                }

                return count;
            }

            if (firstCells.Count == 0)
            {
                throw new Exception("No boat cell");
            }

            if (!Cell.AreHorizontallyOrVerticallyAligned(firstCells))
            {
                throw new Exception("Boat cells are not neighbors or horizontally/vertically aligned");
            }

            var sortedCells = Cell.SortByAbscissaAndOrdinate(firstCells);

            bool canBePlaced = false;
            int availableCellsInAlignment;

            if (sortedCells.Count == 1 || sortedCells[0].IsHorizontalNeighbour(sortedCells[1]))
            {
                availableCellsInAlignment = CountAvailableCellsInAlignment(grid, sortedCells[0].X - 1, sortedCells[0].Y, -1, 0);
                availableCellsInAlignment += CountAvailableCellsInAlignment(grid, sortedCells[^1].X + 1, sortedCells[0].Y, 1, 0);

                if (availableCellsInAlignment >= size - sortedCells.Count)
                {
                    canBePlaced = true;
                }
            }
            else if (sortedCells.Count == 1 || sortedCells[0].IsVerticalNeighbour(sortedCells[1]))
            {
                availableCellsInAlignment = CountAvailableCellsInAlignment(grid, sortedCells[0].X, sortedCells[0].Y - 1, 0, -1);
                availableCellsInAlignment += CountAvailableCellsInAlignment(grid, sortedCells[0].X, sortedCells[^1].Y + 1, 0, 1);

                if (availableCellsInAlignment >= size - sortedCells.Count)
                {
                    canBePlaced = true;
                }
            }

            return canBePlaced;
        }

        /// <summary>
        /// Checks if the boat is destroyed, meaning all of its cells have been discovered.
        /// </summary>
        /// <returns>True if the boat is destroyed; otherwise, false.</returns>
        public bool IsDestroyed()
        {
            return cells.All(cell => cell.Discover == true);
        }
    }
}
