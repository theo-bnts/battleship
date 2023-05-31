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

        public List<Boat> Boats
        {
            get { return boats; }
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

        public bool AllBoatsDestroyed()
        {
            return boats.All(boat => boat.IsDestroyed() == true);
        }

        public string ToString(bool boatPlacementMode = false)
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

        public void Write(bool boatPlacementMode = false, int topOffset = 0, int leftOffset = 0)
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
