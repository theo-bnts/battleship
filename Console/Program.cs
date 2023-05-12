using ConsoleProject.Entities;
using ConsoleProject.Tools;

API api = new ();
await api.GetDataAsync();

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Player A");
Console.ResetColor();

Grid gridA = new (api.Width, api.Height);

foreach (int boatSize in api.BoatSizes)
{
    InputHandler.PlaceBoatOnGrid(gridA, boatSize);
}

Console.Clear();

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Player B");
Console.ResetColor();

Grid gridB = new (api.Width, api.Height);

foreach (int boatSize in api.BoatSizes)
{
    InputHandler.PlaceBoatOnGrid(gridB, boatSize);
}