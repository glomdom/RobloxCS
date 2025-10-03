namespace RobloxCS.Transpiler.Builders;

public static class BuilderResultExtensions {
    public static BuilderResult Flatten(this IEnumerable<BuilderResult> source) {
        return source.Aggregate((acc, next) => {
            acc.Add(next);

            return acc;
        });
    }
}