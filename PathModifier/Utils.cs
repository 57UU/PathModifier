using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PathModifier;

public static class Utils
{
    public static string ReadFile(string path)
    {
        return File.ReadAllText(path,encoding:Encoding.UTF8);
    }

    public static void WriteFile(string path, string content)
    {
        File.WriteAllTextAsync(path, content, encoding: Encoding.UTF8);
    }

    public static Assets loadAssets()
    {
        var json = Utils.ReadFile(Assets.FILE_NAME);

        var assets = JsonSerializer.Deserialize<Assets>
            (json, SourceGenerationContext.Default.Assets);
        return assets;
    }

    public static void saveAssets(Assets assets)
    {
        var json = JsonSerializer.Serialize<Assets>
            (assets, SourceGenerationContext.Default.Assets);
        WriteFile(Assets.FILE_NAME,json);
    }
}

