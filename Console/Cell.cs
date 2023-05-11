using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProject
{
    internal class Cell
    {
        private readonly int x;
        private readonly int y;
        private bool selected;
        private bool hit;

        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        public bool Hit
        {
            get { return hit; }
            set { hit = value; }
        }

        public Cell(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.selected = false;
            this.hit = false;
        }

        public bool HaveSamePosition(Cell cell)
        {
            return cell.x == this.x && cell.y == this.y;
        }

        public bool IsNeighbour(Cell cell)
        {
            bool isNeighbour = true;

            if (this.HaveSamePosition(cell))
            {
                isNeighbour = false;
            }

            if (cell.x > this.x + 1 || cell.x < this.x - 1)
            {
                isNeighbour = false;
            }

            if (cell.y > this.y + 1 || cell.y < this.y - 1)
            {
                isNeighbour = false;
            }

            return isNeighbour;
        }

        public bool IsHorizontalNeighbour(Cell cell)
        {
            return this.IsNeighbour(cell) && cell.x == this.x;
        }

        public bool IsVerticalNeighbour(Cell cell)
        {
            return this.IsNeighbour(cell) && cell.y == this.y;
        }
    }
}
