using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProject
{
    internal class Interactions
    {

        //prends la grille en parametre et l'affiche à chaque fois
        //prends la taille d'un bateau en paramètre et pose x fois la question de quel cellule utilisée (x = taille du bateau)
        //crée le bateau et l'ajoute à la grille
        public void PlaceBoatOnGrid(Grid grid, int boatSize)
        {
            List<Cell> boatCells = new ();

            grid.Write();

            for (int size = 0; size < boatSize; size++)
            {
                int ligne;
                int colonne;
                Console.WriteLine("Quelles cellules entre 1 et {0}?", grid.Width);

                Console.Write("Ligne : ");
                ligne = Convert.ToInt32(Console.ReadLine()) - 1;

                Console.Write("Colonne : ");
                colonne = Convert.ToInt32(Console.ReadLine()) - 1;

                boatCells.Add(grid.GetCell(ligne, colonne));

                grid.Write();
            }

            Boat boat = new (boatCells);

            grid.AddBoat(boat);
        }
    }
}
