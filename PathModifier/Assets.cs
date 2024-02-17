using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PathModifier;

public class Assets
{
    public const string FILE_NAME="Path_Setting.json";
    public bool enable_gross_move=false;
    public string? gross_move_floder;
    public string? video_path;
    public string? desktop_path;
    public string? download_path;
    public string? document_path;
    public string? photo_path;
    public string? music_path;
}

[JsonSourceGenerationOptions(WriteIndented = true,IncludeFields = true)]
[JsonSerializable(typeof(Assets))]
internal partial class SourceGenerationContext : JsonSerializerContext
{
}

