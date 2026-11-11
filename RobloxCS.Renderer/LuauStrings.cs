using System.Globalization;
using System.Text;

namespace RobloxCS.Renderer;

internal static class LuauStrings {
    public static string Escape(string value, bool interpolated = false) {
        var output = new StringBuilder();

        foreach (var character in value) {
            if (character == '\\' || character == '"' || interpolated && character is '`' or '{' or '}') {
                output.Append('\\').Append(character);
            } else if (character < ' ') {
                output.Append('\\').Append(((int)character).ToString("D3", CultureInfo.InvariantCulture));
            } else {
                output.Append(character);
            }
        }

        return output.ToString();
    }
}