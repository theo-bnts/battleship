using ConsoleProject.Tools;
using ConsoleTables;

namespace ConsoleProject.Entities
{
    internal class Grid
    {
        private readonly int width;
        private readonly int height;
        private readonly List<Cell> cells;
        private readonly List<Boat> boats;

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }

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

        public Cell GetCell(int x, int y)
        {
            if (x < 0 || x > width - 1 || y < 0 || y > height - 1)
            {
                throw new Exception("Cell is out of grid");
            }

            int index = x + y * height;

            return cells[index];
        }

        public bool IsBoat(Cell cell)
        {
            bool isABoat = false;

            foreach (Boat boat in boats)
            {
                foreach (Cell boatCell in boat.Cells)
                {
                    if (cell.HaveSamePosition(boatCell))
                    {
                        isABoat = true;
                    }
                }
            }

            return isABoat;
        }

        public bool IsBoatNeighbour(Cell cell)
        {
            bool isNeighbourOfABoat = false;

            foreach (Boat boat in boats)
            {
                foreach (Cell boatCell in boat.Cells)
                {
                    if (cell.IsNeighbour(boatCell))
                    {
                        isNeighbourOfABoat = true;
                    }
                }
            }
            return isNeighbourOfABoat;
        }

        public void AddBoat(Boat boat)
        {
            foreach (Cell cell in boat.Cells)
            {
                if (IsBoat(cell) || IsBoatNeighbour(cell))
                {
                    throw new Exception("Boat is overlapping or too close to another boat");
                }
            }

            boats.Add(boat);
        }


        public void Write(bool boatPlacementMode = false)
        {
            List<string> columnNames = new List<string>() { " " };

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
                        if (cell.Selected)
                        {
                            row.Add("@");
                        }
                        else
                        {
                            row.Add(" ");
                        }
                    }
                    else
                    {
                        if (cell.Hit)
                        {
                            if (IsBoat(cell))
                            {
                                row.Add("@");
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

            table.Write(Format.Alternative);
        }

    }
}
