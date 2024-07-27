using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Text.Json;




List<CLSID> pairs = new();
{
    var reg = Registry.ClassesRoot.OpenSubKey("CLSID");
    var subs = reg.GetSubKeyNames();
    foreach (var i in subs)
    {
        var name = reg.OpenSubKey(i).GetValue("")?.ToString();
        pairs.Add(new CLSID
        {
            guid = i,
            name = name
        });
    }
}
{
    var reg = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Classes\\CLSID");
    var subs = reg.GetSubKeyNames();
    foreach (var i in subs)
    {
        var name = reg.OpenSubKey(i).GetValue("")?.ToString();
        pairs.Add(new CLSID
        {
            guid = i,
            name = name
        });
    }
}

var json= JsonSerializer.Serialize(pairs,options:new JsonSerializerOptions() {WriteIndented=true });

File.WriteAllText("CLSID.json", json, encoding: Encoding.UTF8);


struct CLSID
{
    public string guid { set; get; }
    public string name { set; get; }
}