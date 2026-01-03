#nullable enable
namespace RobloxCS.Types;
[RobloxNative("SoundService", RobloxNativeType.Service)]
public static partial class SoundService {
    public static bool AcousticSimulationEnabled { get; } = default!;
    public static Enums.ReverbType AmbientReverb { get; } = default!;
    public static Enums.RolloutState CharacterSoundsUseNewApi { get; } = default!;
    public static float DistanceFactor { get; } = default!;
    public static float DopplerScale { get; } = default!;
    public static bool RespectFilteringEnabled { get; } = default!;
    public static float RolloffScale { get; } = default!;
    public static Enums.VolumetricAudio VolumetricAudio { get; } = default!;
    public static LuaTuple GetListener() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void PlayLocalSound() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void SetListener() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
