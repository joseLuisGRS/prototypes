using System.ComponentModel;
using System.Reflection;
using System.Text.Json;

namespace Utilities.Extensions;

/// <summary>
/// This class is a utility that helps with the management and correct operation of the system.
/// </summary>
public static class GenericTypeExtension
{
    public static string DescriptionAttr<T>(this T source)
    {
        FieldInfo fi = source.GetType().GetField(source.ToString());

        DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
            typeof(DescriptionAttribute), false);

        if (attributes != null && attributes.Length > 0) return attributes[0].Description;
        else return source.ToString();
    }

    public static string Serialize<T>(this T source) => JsonSerializer.Serialize(source);

    public static T Deserialize<T>(this string txt)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        return JsonSerializer.Deserialize<T>(txt, options);
    }

}
