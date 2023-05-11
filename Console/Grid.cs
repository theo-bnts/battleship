using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ConsoleTables;

namespace ConsoleProject
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
            this.height = heigth;
            this.cells = new List<Cell>();
            this.boats = new List<Boat>();

            for (int y = 0; y < this.height; y++)
            {
                for (int x = 0; x < this.width; x++)
                {
                    cells.Add(new Cell(x, y));
                }
            }
        }

        public Cell GetCell(int x, int y)
        {
            if (x < 0 || x > this.width - 1 || y < 0 || y > this.height - 1)
            {
                throw new Exception("Cell is out of grid");
            }

            int index = x + (y * height);

            return cells[index];
        }

        public bool IsABoat(Cell cell)
        {
            bool isABoat = false;

            foreach (Boat boat in this.boats)
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

        /*
        public void SelectCell(int x, int y)
        {
            foreach (Boat boat in this.boats)
            {
                for (int xOffset = -1; xOffset <= 1; xOffset++)
                {
                    for (int yOffset = -1; yOffset <= 1; yOffset++)
                    {
                        int newX = x + xOffset;
                        int newY = y + yOffset;

                        if (newX >= 0 && newX < width && newY >= 0 && newY < height)
                        {

                        }
                    }
                }
            }
        }
        */

        public void AddBoat(Boat newBoat)
        {
            foreach (Boat boat in this.boats)
            {
                foreach (Cell cell in boat.Cells)
                {
                    foreach (Cell newCell in newBoat.Cells)
                    {
                        if (cell.HaveSamePosition(newCell) || cell.IsNeighbour(newCell))
                        {
                            throw new Exception("Boat is overlapping or too close to another boat");
                        }
                    }
                }
            }

            this.boats.Add(newBoat);
        }


        public void Write(bool hideBoats = true)
        {
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string[] columnNames = (' ' + alphabet.Substring(0, this.width)).ToCharArray().Select(c => c.ToString()).ToArray();

            ConsoleTable table = new (columnNames);

            for (int y = 0; y < this.height; y++)
            {
                List<string> row = new ();

                row.Add((y + 1).ToString());

                for (int x = 0; x < this.width; x++)
                {
                    Cell cell = GetCell(x, y);

                    if (hideBoats || cell.Hit)
                    {
                        if (IsABoat(cell))
                        {
                            row.Add("X");
                        }
                        else
                        {
                            row.Add("O");
                        }
                    }
                    else
                    {
                        row.Add(" ");
                    }
                }

                table.AddRow(row.ToArray());
            }

            table.Write();
        }

    }
}
