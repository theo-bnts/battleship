namespace ConsoleProject.Entities
{
    internal class Boat
    {
        private readonly List<Cell> cells;

        public List<Cell> Cells
        {
            get { return cells; }
        }

        public Boat(List<Cell> cells)
        {
            if (cells.Count == 0)
            {
                throw new Exception("No boat cell");
            }

            if (!Cell.AreHorizonltallyOrVerticallyAligned(cells))
            {
                throw new Exception("Boat cells are not neighbours or horizontally / vertically aligned");
            }

            this.cells = cells;
        }

        public static bool CanBePlacedOnGrid(Grid grid, List<Cell> firstsCells, int size)
        {
            if (firstsCells.Count == 0)
            {
                throw new Exception("No boat cell");
            }

            if (!Cell.AreHorizonltallyOrVerticallyAligned(firstsCells))
            {
                throw new Exception("Boat cells are not neighbours or horizontally / vertically aligned");
            }

            firstsCells = Cell.SortByAbscissaAndOrdinate(firstsCells);

            bool canBePlaced = false;
            int availableCellsInAlignment = 0;

            if (firstsCells.Count == 1 || firstsCells[0].IsHorizontalNeighbour(firstsCells[1]))
            {
                for (int x = firstsCells[0].X - 1; x >= 0; x--)
                {
                    if (x >= 0 && x < grid.Width)
                    {
                        if (!grid.GetCell(x, firstsCells[0].Y).Selected)
                        {
                            availableCellsInAlignment++;
                        }
                        else
                        {
                            availableCellsInAlignment--;

                            break;
                        }
                    }
                }

                for (int x = firstsCells[firstsCells.Count - 1].X + 1; x < grid.Width; x++)
                {
                    if (x >= 0 && x < grid.Width)
                    {
                        if (!grid.GetCell(x, firstsCells[0].Y).Selected)
                        {
                            availableCellsInAlignment++;
                        }
                        else
                        {
                            availableCellsInAlignment--;

                            break;
                        }
                    }
                }

                if (availableCellsInAlignment >= size - firstsCells.Count)
                {
                    canBePlaced = true;
                }
            }

            availableCellsInAlignment = 0;

            if (firstsCells.Count == 1 || firstsCells[0].IsVerticalNeighbour(firstsCells[1]))
            {
                for (int y = firstsCells[0].Y - 1; y >= 0; y--)
                {
                    if (y >= 0 && y < grid.Height)
                    {
                        if (!grid.GetCell(firstsCells[0].X, y).Selected)
                        {
                            availableCellsInAlignment++;
                        }
                        else
                        {
                            availableCellsInAlignment--;

                            break;
                        }
                    }
                }

                for (int y = firstsCells[firstsCells.Count - 1].Y + 1; y < grid.Height; y++)
                {
                    if (y >= 0 && y < grid.Height)
                    {
                        if (!grid.GetCell(firstsCells[0].X, y).Selected)
                        {
                            availableCellsInAlignment++;
                        }
                        else
                        {
                            availableCellsInAlignment--;

                            break;
                        }
                    }
                }

                if (availableCellsInAlignment >= size - firstsCells.Count)
                {
                    canBePlaced = true;
                }
            }

            return canBePlaced;
        }

        public bool IsDestroyed()
        {
            return cells.All(cell => cell.Discover == true);
        }
    }
}
