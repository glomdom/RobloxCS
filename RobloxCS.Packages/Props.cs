using System.Text;

namespace RobloxCS.Packages;

public static class Props {
    public const string FileName = "rbxcs.props";

    public static bool IsStale(string root) {
        var props = Path.Combine(root, FileName);
        if (!File.Exists(props)) {
            return true;
        }

        var written = File.GetLastWriteTimeUtc(props);

        return new[] { ProjectManifest.FileName, "rbxcs.lock" }
            .Select(name => Path.Combine(root, name))
            .Any(path => File.Exists(path) && File.GetLastWriteTimeUtc(path) > written);
    }

    public static void Write(string root, string robloxCSVersion, IEnumerable<string> packageProjects) {
        var sb = new StringBuilder($"""
            <Project>
            
                <PropertyGroup>
                    <TargetFramework>net10.0</TargetFramework>
                    <ImplicitUsings>enable</ImplicitUsings>
                    <Nullable>enable</Nullable>
                    <RobloxCSVersion>{robloxCSVersion}</RobloxCSVersion>
                    <RbxcsHome Condition="'$(RBXCS_HOME)' != ''">$(RBXCS_HOME)</RbxcsHome>
                    <RbxcsHome Condition="'$(RbxcsHome)' == ''">$([System.Environment]::GetFolderPath(SpecialFolder.UserProfile))/.rbxcs</RbxcsHome>
                </PropertyGroup>

                <ItemGroup>
                    <Reference Include="RobloxCS.Types">
                        <HintPath>$(RbxcsHome)/$(RobloxCSVersion)/RobloxCS.Types.dll</HintPath>
                        <Private>false</Private>
                    </Reference>
                    <Using Include="RobloxCS.Types" />
                    <Using Include="RobloxCS.Types.Attributes" />
                </ItemGroup>
            
            
            """
        );

        sb.AppendLine("    <ItemGroup>");

        foreach (var project in packageProjects) {
            sb.AppendLine($"""        <ProjectReference Include="{project}" />""");
        }
        
        sb.AppendLine("    </ItemGroup>");
        sb.AppendLine();
        sb.AppendLine("</Project>");

        var props = Path.Combine(root, FileName);
        File.WriteAllText(props, sb.ToString());
    }
}