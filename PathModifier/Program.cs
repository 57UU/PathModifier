//load setting file


using System.Text.Json;
using ConsoleTables;
using Microsoft.Win32;
using PathModifier;




Assets assets;
try
{
    Console.WriteLine("Loading configures");
    assets = Utils.loadAssets();
    if (assets.enable_gross_move)
    {
        assets.download_path = assets.gross_move_floder+@"\Downloads";
        assets.desktop_path = assets.gross_move_floder + @"\Desktop";
        assets.document_path = assets.gross_move_floder + @"\Documents";
        assets.music_path = assets.gross_move_floder + @"\Music";
        assets.video_path = assets.gross_move_floder + @"\Videos";
        assets.picture_path = assets.gross_move_floder + @"\Pictures";
    }
}
catch (Exception e)
{
    Console.WriteLine("There are no existing configuration, preparing to generate");
    assets = new();
    while (true)
    {
        Console.WriteLine("Do you want move all the relative folders to a new place?(y/n)");
        var input = Console.ReadLine().ToLower();
        if (input == "y")
        {
            assets.enable_gross_move=true;
            Console.WriteLine("Where do you want to move to?(absolute path)");
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
                    assets.gross_move_floder=p;
                    Utils.saveAssets(assets);
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



Console.WriteLine("Opening current registry");
RegEdit regEdit = new RegEdit();


Console.WriteLine("Inspect current windows config\n");

var currentDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
var currentVideo = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
var currentDocument = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
var currentPicture = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
var currentMusic = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);


var currentDownload = regEdit.Read("{374DE290-123F-4565-9164-39C4925E467B}");

var table = new ConsoleTable("Key","Current Path","New Path");

table.AddRow(Strings.desktop, currentDesktop,assets.desktop_path);
table.AddRow(Strings.video,currentVideo,assets.video_path);
table.AddRow(Strings.download,currentDownload,assets.download_path);
table.AddRow(Strings.document,currentDocument,assets.document_path);
table.AddRow(Strings.pictures,currentPicture,assets.picture_path);
table.AddRow(Strings.music,currentMusic,assets.music_path);

table.Write(Format.MarkDown);


Console.Write("override those system path? ");
Console.Write("press y to confirm (waiting for key to be pressed)");

if (Console.ReadKey().Key!=ConsoleKey.Y)
{
    Console.WriteLine("Press Any Key to Continue...");
    Environment.Exit(0);
}
Console.WriteLine();
//init floder
Utils.CreateIfNotExists(assets.desktop_path);
Utils.CreateIfNotExists(assets.video_path);
Utils.CreateIfNotExists(assets.document_path);
Utils.CreateIfNotExists(assets.download_path);
Utils.CreateIfNotExists(assets.picture_path);
Utils.CreateIfNotExists(assets.music_path);
//replace
regEdit.ReplaceValue(currentDesktop, assets.desktop_path);
regEdit.ReplaceValue(currentVideo, assets.video_path);
regEdit.ReplaceValue(currentDocument, assets.document_path);
regEdit.ReplaceValue(currentPicture, assets.picture_path);
regEdit.ReplaceValue(currentDownload, assets.download_path);
regEdit.ReplaceValue(currentMusic,assets.music_path);

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Path modify done, you may need rebooting to apply");
Console.ResetColor();


