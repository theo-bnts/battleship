using ConsoleProject.Entities;
using ConsoleProject.Tools;

Console.ForegroundColor = ConsoleColor.Red;
Console.WriteLine("Please maximise your console to continue.");
Console.WriteLine("Do not use Microsoft Terminal.");
Console.ResetColor();

while (Console.WindowWidth < Console.LargestWindowWidth * 0.9 || Console.WindowHeight < Console.LargestWindowHeight * 0.9)
{
    Thread.Sleep(1_000);
}

API api = new ();
await api.FetchData();

Grid gridA = new (api.Width, api.Height);

foreach (int boatSize in api.BoatSizes)
{
    InputHandler.PlaceBoatOnGrid("Player A", gridA, boatSize);
}

Grid gridB = new (api.Width, api.Height);

foreach (int boatSize in api.BoatSizes)
{
    InputHandler.PlaceBoatOnGrid("Player B", gridB, boatSize);
}

do
{
    Console.Clear();

    InputHandler.DisplayGrids(gridA, gridB, true, false);

    InputHandler.DisplayPlayerName("Player A turn");

    InputHandler.DiscoverCell(gridB);

    if (gridB.AllBoatsDestroyed())
    {
        InputHandler.Win();
    }

    Console.Clear();

    InputHandler.DisplayGrids(gridA, gridB, false, true);

    InputHandler.DisplayPlayerName("Player B turn");

    InputHandler.DiscoverCell(gridA);

    if (gridA.AllBoatsDestroyed())
    {
        InputHandler.Win();
    }
}
while (!gridA.AllBoatsDestroyed() && !gridB.AllBoatsDestroyed());