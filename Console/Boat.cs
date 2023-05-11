using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProject
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
            string alignementException = "Boat cells are not neighbours or horizontaly / verticaly aligned";

            if (cells.Count == 2)
            {
                if (!((cells[0].IsHorizontalNeighbour(cells[1])) || (cells[0].IsVerticalNeighbour(cells[1]))))
                {
                    throw new Exception(alignementException);
                }
            }

            for (int i = 0; i < cells.Count - 2; i++)
            {
                if (!((cells[i].IsHorizontalNeighbour(cells[i + 1]) && cells[i + 1].IsHorizontalNeighbour(cells[i + 2])) || (cells[i].IsVerticalNeighbour(cells[i + 1]) && cells[i + 1].IsVerticalNeighbour(cells[i + 2]))))
                {
                    throw new Exception(alignementException);
                }
            }

            this.cells = cells;
        }

        public bool IsDestroyed()
        {
            return cells.All(cell => cell.Hit == true);
        }
    }
}
