using System.Collections.Generic;

namespace ConsoleProject.Entities
{
    internal class Cell
    {
        private readonly int x;
        private readonly int y;
        private bool selected;
        private bool discover;

        public int X
        {
            get { return x; }
        }

        public int Y
        {
            get { return y; }
        }

        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        public bool Discover
        {
            get { return discover; }
            set { discover = value; }
        }

        public Cell(int x, int y)
        {
            this.x = x;
            this.y = y;
            selected = false;
            discover = false;
        }

        public bool HaveSamePosition(Cell cell)
        {
            return cell.x == x && cell.y == y;
        }

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

        public bool IsHorizontalNeighbour(Cell cell)
        {
            return IsNeighbour(cell) && cell.y == y;
        }

        public bool IsVerticalNeighbour(Cell cell)
        {
            return IsNeighbour(cell) && cell.x == x;
        }

        public Boat GetRelativeBoat(Grid grid)
        {
            Boat result = null;

            foreach (Boat boat in grid.Boats)
            {
                foreach (Cell boatCell in boat.Cells)
                {
                    if (HaveSamePosition(boatCell))
                    {
                        result = boat;
                        break;
                    }
                }
            }

            return result;
        }

        public bool IsBoat(Grid grid)
        {
            return GetRelativeBoat(grid) != null;
        }

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

        public static List<Cell> SortByAbscissaAndOrdinate(List<Cell> cells)
        {
            return cells.OrderBy(cell => cell.x).ThenBy(cell => cell.y).ToList();
        }

        public static bool AreHorizonltallyOrVerticallyAligned(List<Cell> cells)
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
