using ConsoleProject;

API api = new ();
await api.GetDataAsync();

Grid gridA = new (api.Width, api.Height);

/*
Boat boat1 = new (new List<Cell>() { gridA.GetCell(0, 0), gridA.GetCell(0, 1), gridA.GetCell(0, 2), gridA.GetCell(0, 3) });
Boat boat2 = new (new List<Cell>() { gridA.GetCell(2, 1), gridA.GetCell(2, 2) });

gridA.AddBoat(boat1);
gridA.AddBoat(boat2);

gridA.Write();
*/

foreach (int boatSize in api.BoatSizes)
{
    Interactions interactions = new ();
    interactions.PlaceBoatOnGrid(gridA, boatSize);
}

