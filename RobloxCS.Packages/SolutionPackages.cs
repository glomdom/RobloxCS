using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace RobloxCS.Packages;

// Keeps a /Packages/ folder in the project's .slnx listing every installed package project.
// MSBuild finds packages through rbxcs.props alone, but IDEs only load projects that are in the solution.
public static class SolutionPackages {
    public const string FolderName = "/Packages/";

    public static IEnumerable<string> FindSolutions(string root) {
        return Directory.Exists(root) ? Directory.EnumerateFiles(root, "*.slnx", SearchOption.TopDirectoryOnly).Order(StringComparer.Ordinal) : [];
    }

    // returns the solutions that were rewritten
    public static List<string> Sync(string root, IEnumerable<string> packageProjects) {
        var projects = packageProjects.Select(Path.GetFullPath).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        var changed = new List<string>();

        foreach (var solution in FindSolutions(root)) {
            if (SyncSolution(solution, projects)) {
                changed.Add(solution);
            }
        }

        return changed;
    }

    private static bool SyncSolution(string solution, List<string> projects) {
        XDocument document;

        try {
            document = XDocument.Load(solution);
        } catch (XmlException ex) {
            throw new PackageException($"Could not update '{Path.GetFileName(solution)}': {ex.Message}");
        }

        if (document.Root is not { Name.LocalName: "Solution" } root) {
            throw new PackageException($"Could not update '{Path.GetFileName(solution)}': its root element isn't <Solution>");
        }

        var directory = Path.GetDirectoryName(solution)!;
        var folder = root.Elements("Folder").FirstOrDefault(element => (string?)element.Attribute("Name") == FolderName);

        // a package the user already added somewhere else stays where they put it
        var listedElsewhere = root.Descendants("Project")
            .Where(project => project.Parent != folder)
            .Select(project => (string?)project.Attribute("Path"))
            .OfType<string>()
            .Select(path => Path.GetFullPath(Path.Combine(directory, path)))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var wanted = projects
            .Where(project => !listedElsewhere.Contains(project))
            .Select(project => Path.GetRelativePath(directory, project).Replace('\\', '/'))
            .Order(StringComparer.Ordinal)
            .ToList();

        var current = folder?.Elements("Project").Select(project => (string?)project.Attribute("Path") ?? "").ToList() ?? [];
        if (current.SequenceEqual(wanted, StringComparer.Ordinal) && (folder is null || folder.Elements().All(element => element.Name.LocalName == "Project"))) {
            return false;
        }

        if (wanted.Count == 0) {
            folder?.Remove();
        } else {
            if (folder is null) {
                folder = new XElement("Folder", new XAttribute("Name", FolderName));
                root.Add(folder);
            }

            folder.RemoveNodes();
            foreach (var path in wanted) {
                folder.Add(new XElement("Project", new XAttribute("Path", path)));
            }
        }

        Save(document, solution);

        return true;
    }

    private static void Save(XDocument document, string path) {
        var settings = new XmlWriterSettings {
            Indent = true,
            IndentChars = "  ",
            OmitXmlDeclaration = document.Declaration is null,
            Encoding = new UTF8Encoding(false),
        };

        using (var writer = XmlWriter.Create(path, settings)) {
            document.Save(writer);
        }

        File.AppendAllText(path, Environment.NewLine);
    }
}
