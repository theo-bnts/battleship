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
            if (!Cell.AreHorizonltalyOrVerticallyAligned(cells))
            {
                throw new Exception("Boat cells are not neighbours or horizontally / vertically aligned");
            }

            this.cells = cells;
        }

        public bool IsDestroyed()
        {
            return cells.All(cell => cell.Hit == true);
        }
    }
}
