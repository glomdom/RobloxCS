namespace RobloxCS.Transpiler.Builders;

public static class BuilderResultExtensions {
    public static BuilderResult Flatten(this IEnumerable<BuilderResult> source) {
        var results = source as BuilderResult[] ?? source.ToArray();

        if (results.Length == 0) {
            return BuilderResult.Empty();
        }

        return results.Aggregate((acc, next) => {
            acc.Add(next);

            return acc;
        });
    }
}