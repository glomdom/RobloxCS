using System.ComponentModel;

namespace RobloxCS.Common;

public static class EnumExtensions {
    public static string ToFriendlyString(this Enum value) {
        var info = value.GetType().GetField(value.ToString())!;

        return Attribute.GetCustomAttribute(info, typeof(DescriptionAttribute)) is DescriptionAttribute attr ? attr.Description : value.ToString();
    }
}