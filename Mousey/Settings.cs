using System;
using System.IO;
using Newtonsoft.Json;

namespace Mousey;

public record Settings
{
    public TimeSpan Interval { get; set; }


    public static Settings Load()
    {
        try
        {
            var payload = File.ReadAllText("Settings.json");
            return JsonConvert.DeserializeObject<Settings>(payload);
        }
        catch
        {
            return new Settings()
            {
                Interval = TimeSpan.FromSeconds(5)
            };
        }
    }

    public void SaveToFile()
    {
        File.WriteAllText("Settings.json", JsonConvert.SerializeObject(this));
    }
}