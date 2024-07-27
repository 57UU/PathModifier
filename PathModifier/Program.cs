//load setting file


using System.Text.Json;
using ConsoleTables;
using Microsoft.Win32;
using PathModifier;


Console.ForegroundColor=ConsoleColor.Green;
Console.Write("view more at ");
Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("https://github.com/57UU/PathModifier");
Console.ResetColor();


Assets assets;
try
{
    Console.WriteLine("Loading configurations");
    assets = Utils.loadAssets();

}
catch (Exception e)
{
    if(e is JsonException)
    {
        PrintRed("The configuration is corrupt, press y to override/replace it");
        PressYtoContinue();
    }
    else
    {
        Console.WriteLine("There are no existing configuration, preparing to generate");
    }
    
    assets = new();
    while (true)
    {
        Console.WriteLine("Do you want move all the relative folders to a new place?(y/n)");
        var input = Console.ReadKey().Key;
        Console.WriteLine();
        if (input == ConsoleKey.Y)
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

        if (input == ConsoleKey.N)
        {
            assets.enable_gross_move = false;
            Utils.saveAssets(assets);
            Console.WriteLine($"Please Modify the {Assets.FILE_NAME},then re-run");
            PressAnyKeyToExit();

        }

        Console.WriteLine("Please input 'y' or 'n'");
    }
}
if (assets.enable_gross_move)
{
    SetPath();
}
Utils.saveAssets(assets);


//firewall 
Console.WriteLine("---firewall---");
if (assets.open_port != null)
{
    if (!assets.auto_confirm)
    {
        Console.WriteLine($"Are you sure opening firewall port {assets.open_port}?(y/other)");
        if (!PressYtoConfirm())
        {
            goto firewall_done;
        }
    }
    //Console.WriteLine("setting firewall port ");
    FirewallEdit.Open(assets.open_port.Value);
}
else
{
    Console.WriteLine("open firewall port is not set, ignore");
}
Console.WriteLine("---firewall-done---\n");
firewall_done:

Console.WriteLine("Opening current registry");
RegEdit regEdit = new();


Console.WriteLine("Inspecting current config\n");

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

if (!assets.auto_confirm)
{
    Console.Write("override those system path? ");
    Console.Write("press y to confirm.");
    PressYtoContinue();
}



try
{
    //init floder
    Utils.CreateIfNotExists(assets.desktop_path);
    Utils.CreateIfNotExists(assets.video_path);
    Utils.CreateIfNotExists(assets.document_path);
    Utils.CreateIfNotExists(assets.download_path);
    Utils.CreateIfNotExists(assets.picture_path);
    Utils.CreateIfNotExists(assets.music_path);
}
catch (Exception e) {
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("An error occurred while inspecting those folders! ");
    Console.Error.WriteLine(e.Message);
    
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("Try to inspect the configuration");
    Console.ResetColor();
    Console.WriteLine("Exiting...");
    Console.ReadKey();
    Environment.Exit(0);
}

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


PressAnyKeyToExit();

void PrintRed(string text)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(text);
    Console.ResetColor();
}
void PressAnyKeyToExit()
{
    Console.WriteLine("Press Any Key to Continue...");
    Console.ReadKey();
    Environment.Exit(0);
}
void PressYtoContinue()
{
    var key = Console.ReadKey().Key;
    Console.WriteLine();
    if (key != ConsoleKey.Y)
    {
        PressAnyKeyToExit();
    }
}
bool PressYtoConfirm()
{
    var key = Console.ReadKey().Key;
    Console.WriteLine();
    return key == ConsoleKey.Y;

}
void SetPath()
{
    assets.download_path = assets.gross_move_floder + @"\Downloads";
    assets.desktop_path = assets.gross_move_floder + @"\Desktop";
    assets.document_path = assets.gross_move_floder + @"\Documents";
    assets.music_path = assets.gross_move_floder + @"\Music";
    assets.video_path = assets.gross_move_floder + @"\Videos";
    assets.picture_path = assets.gross_move_floder + @"\Pictures";
}