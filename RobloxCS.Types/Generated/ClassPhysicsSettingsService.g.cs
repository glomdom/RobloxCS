#nullable enable
namespace RobloxCS.Types;
[RobloxNative("PhysicsSettings", RobloxNativeType.Service)]
public static partial class PhysicsSettingsService {
    public static bool AllowSleep { get; set; } = default!;
    public static bool AreAnchorsShown { get; set; } = default!;
    public static bool AreAssembliesShown { get; set; } = default!;
    public static bool AreAwakePartsHighlighted { get; set; } = default!;
    public static bool AreBodyTypesShown { get; set; } = default!;
    public static bool AreContactIslandsShown { get; set; } = default!;
    public static bool AreContactPointsShown { get; set; } = default!;
    public static bool AreJointCoordinatesShown { get; set; } = default!;
    public static bool AreMechanismsShown { get; set; } = default!;
    public static bool AreModelCoordsShown { get; set; } = default!;
    public static bool AreNonAnchorsShown { get; set; } = default!;
    public static bool AreOwnersShown { get; set; } = default!;
    public static bool ArePartCoordsShown { get; set; } = default!;
    public static bool AreRegionsShown { get; set; } = default!;
    public static bool AreTerrainReplicationRegionsShown { get; set; } = default!;
    public static bool AreUnalignedPartsShown { get; set; } = default!;
    public static bool AreWorldCoordsShown { get; set; } = default!;
    public static bool DisableCSGv2 { get; set; } = default!;
    public static bool DisableCSGv3ForPlugins { get; set; } = default!;
    public static bool IsInterpolationThrottleShown { get; set; } = default!;
    public static bool IsReceiveAgeShown { get; set; } = default!;
    public static bool IsTreeShown { get; set; } = default!;
    public static Enums.EnviromentalPhysicsThrottle PhysicsEnvironmentalThrottle { get; set; } = default!;
    public static bool ShowDecompositionGeometry { get; set; } = default!;
    public static double ThrottleAdjustTime { get; set; } = default!;
    public static bool UseCSGv2 { get; set; } = default!;
}
