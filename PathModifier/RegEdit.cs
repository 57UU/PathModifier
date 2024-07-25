using Microsoft.Win32;
namespace PathModifier;

public class RegEdit
{
    private RegistryKey reg1;
    private RegistryKey reg2;
    public RegEdit()
    {
        reg1 = Registry.CurrentUser!.OpenSubKey(Strings.reg,true)!;
        reg2 = Registry.CurrentUser!.OpenSubKey(Strings.reg2,true)!;
    }
    public void Write(string key,string value)
    {
        reg1.SetValue(key, value);
        reg2.SetValue(key, value);
    }
    public string? Read(string key) {
        return (string?)reg1.GetValue(key);
    }
    public void ReplaceValue(string oldValue,string newValue)
    {
        //List<string> keys = new List<string>();
        _replaceValue(reg1);
        _replaceValue(reg2);
        void _replaceValue(RegistryKey reg)
        {
            var subs = reg.GetValueNames();
            foreach (var subKey in subs)
            {
                string value = (string)(reg.GetValue(subKey));
                if (value == oldValue)
                {
                    //keys.Add(subKey);
                    reg.SetValue(subKey, newValue);
                }
            }
        }

    }
}
