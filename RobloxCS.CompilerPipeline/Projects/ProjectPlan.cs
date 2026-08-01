using Microsoft.CodeAnalysis;
using RobloxCS.Common.Rojo;

namespace RobloxCS.CompilerPipeline.Projects;

public sealed record ProjectPlan(Project Project, Compilation Compilation, RojoAnchor Anchor);