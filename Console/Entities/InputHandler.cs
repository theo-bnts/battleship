﻿using ConsoleProject.Tools;
using System.Text.RegularExpressions;

namespace ConsoleProject.Entities
{
    internal abstract class InputHandler
    {
        public static void DisplayPlayerName(string name, int topOffset = 0, int leftOffset = 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(leftOffset, Console.CursorTop + topOffset);
            Console.WriteLine("{0}\n", name);
            Console.ResetColor();
        }

        public static Cell ReadCell(Grid grid)
        {
            string? coordinates = Console.ReadLine();

            if (coordinates == null)
            {
                throw new Exception("Coordinates not provided");
            }

            Regex regex = new("^([A-Z]{1})([0-9]+)$");

            if (!regex.IsMatch(coordinates))
            {
                throw new Exception("Coordinates are not valid");
            }

            Match match = regex.Match(coordinates);

            int colonne = Alphabet.IndexOf(Convert.ToChar(match.Groups[1].Value));
            int ligne = Convert.ToInt32(match.Groups[2].Value) - 1;

            return grid.GetCell(colonne, ligne);
        }

        public static void PlaceBoatOnGrid(string player, Grid grid, int size)
        {
            static void DisplayCellChoiceConsigns(string player, Grid grid, int size, int cellsCount)
            {
                Console.Clear();

                DisplayPlayerName(player);

                grid.Write(true);

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Please specify the coordinates of the next cell and press enter (e.g. A1)\nBoat size : {0} - Cells to place : {1}\n", size, size - cellsCount);
                Console.ResetColor();
            }

            DisplayCellChoiceConsigns(player, grid, size, 0);

            List<Cell> cells = new ();

            while (cells.Count < size)
            {
                try
                {
                    Cell cell = ReadCell(grid);

                    if (cell.Selected)
                    {
                        throw new Exception("Cell is already selected");
                    }

                    if (cell.IsBoatNeighbour(grid))
                    {
                        throw new Exception("Cell is too close to another boat");
                    }

                    if (!Cell.AreHorizonltallyOrVerticallyAligned(cells.Concat(new List<Cell> { cell }).ToList()))
                    {
                        throw new Exception("Boat cells are not neighbours or horizontally / vertically aligned");
                    }

                    if (!Boat.CanBePlacedOnGrid(grid, cells.Concat(new List<Cell> { cell }).ToList(), size))
                    {
                        throw new Exception("Boat can't be placed on grid if you select this cell");
                    }

                    cell.Selected = true;

                    cells.Add(cell);

                    DisplayCellChoiceConsigns(player, grid, size, cells.Count);
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Message);
                    Console.ResetColor();
                }
            }

            Boat boat = new (cells);

            grid.AddBoat(boat);
        }

        public static void DisplayGrids(Grid gridA, Grid gridB, bool aBoatPlacementMode, bool bBoatPlacementMode)
        {
            Console.Clear();

            DisplayPlayerName("Player A");
            DisplayPlayerName("Player B", -2, 4 * (gridB.Width + 1) + (gridB.Height.ToString().Length - 1) + 10);

            gridA.Write(aBoatPlacementMode, 0, 0);
            gridB.Write(bBoatPlacementMode, 0 - (2 * (gridA.Height + 1) + 2), 4 * (gridB.Width + 1) + (gridB.Height.ToString().Length - 1) + 10);
        }

        public static void DiscoverCell(Grid grid)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Please specify the coordinates of the cell you would like to discover and press enter (e.g. A1)\n");
            Console.ResetColor();

            while (true)
            {
                try
                {
                    Cell cell = ReadCell(grid);

                    if (cell.Discover)
                    {
                        throw new Exception("Cell is already discovered");
                    }
                    else
                    {
                        cell.Discover = true;

                        Console.ForegroundColor = ConsoleColor.Green;

                        if (cell.IsBoat(grid))
                        {
                            if (cell.GetRelativeBoat(grid).IsDestroyed())
                            {
                                Console.WriteLine("\nTouché-coulé");
                            }
                            else
                            {
                                Console.WriteLine("\nTouché");
                            }
                        }
                        else
                        {
                            Console.WriteLine("\nPlouf");
                        }

                        Console.ResetColor();

                        Thread.Sleep(1_500);

                        break;
                    }
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Message);
                    Console.ResetColor();
                }
            }
        }

        public static void Win()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\nYou win !");
            Console.ResetColor();

            Environment.Exit(0);
        }
    }
}