//load setting file


using System.Text.Json;
using PathModifier;




Assets assets;
try
{
    assets = Utils.loadAssets();
}
catch (Exception e)
{
    Console.WriteLine("There are no existing setting file, preparing to generate");
    assets = new();
    while (true)
    {
        Console.WriteLine("Do you want move all the relative folder to a new place?(y/n)");
        var input = Console.ReadLine().ToLower();
        if (input == "y")
        {
            assets.enable_gross_move=true;
            Console.WriteLine("Where do you want to move to?(absolute path)");
            string path;
            while (true)
            {
                var p = Console.ReadLine();
                try
                {
                    DirectoryInfo f = new(p);
                    
                    if (!f.Exists)
                    {
                        f.Create();
                    }
                    break;
                    
                }
                catch (Exception)
                {
                    Console.WriteLine("Not a valid path");
                    // ignored
                }
            }
            break;
        }

        if (input == "n")
        {
            assets.enable_gross_move = false;
            Utils.saveAssets(assets);
            Console.WriteLine($"Please Modify the {Assets.FILE_NAME},then re-run");
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            Environment.Exit(0);
            
        }

        Console.WriteLine("Please input 'y' or 'n'");

    }


    
}
