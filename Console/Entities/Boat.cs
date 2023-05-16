namespace ConsoleProject.Entities
{
    internal class Boat
    {
        private readonly List<Cell> cells;

        public List<Cell> Cells
        {
            get { return this.cells; }
        }

        public Boat(List<Cell> cells)
        {
            if (!Cell.AreHorizonltalyOrVerticallyAligned(cells))
            {
                throw new Exception("Boat cells are not neighbours or horizontally / vertically aligned");
            }

            this.cells = cells;
        }

        public bool CanBePlacedOnGrid(List<Cell> firstsCells, int size)
        {
            // Vérifier si le bateau peut être placé sur la grille
            // Vérifier si c'est possible de le placer horizontalement ou verticalement
            // Prendre en compte les bordures de la grid
            // Prendre en compte l'espace d'une case si un bateau est à proximité


            return true;
        }

        public bool IsDestroyed()
        {
            return cells.All(cell => cell.Discover == true);
        }
    }
}
