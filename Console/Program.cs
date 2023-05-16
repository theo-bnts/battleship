using ConsoleProject.Entities;
using ConsoleProject.Tools;

Console.ForegroundColor = ConsoleColor.Red;
Console.WriteLine("Please maximise your console to continue");
Console.ResetColor();

while (Console.WindowWidth < Console.LargestWindowWidth * 0.9 || Console.WindowHeight < Console.LargestWindowHeight * 0.9)
{
    Thread.Sleep(10);
}

API api = new ();
await api.GetData();

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
    InputHandler.DisplayPlayerName("Player A");

    InputHandler.DiscoverCell(gridB);

    if (gridB.AllBoatsDestroyed())
    {
        InputHandler.Win();
    }

    InputHandler.DisplayPlayerName("Player B");

    InputHandler.DiscoverCell(gridA);

    if (gridA.AllBoatsDestroyed())
    {
        InputHandler.Win();
    }
}
while (!gridA.AllBoatsDestroyed() && !gridB.AllBoatsDestroyed());