using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace WebUI.Models;

public static class ExtensionHelper
{
    public static void Set<T>(this ISession session, string key, T value)
    {
        JsonSerializerOptions options = new()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            WriteIndented = true
        };

        session.SetString(key, JsonSerializer.Serialize(value,options));

    }

    public static T Get<T>(this ISession session, string key)
    {

        var value = session.GetString(key);
        return value == null ? default : JsonSerializer.Deserialize<T>(value);
    }
}
