#nullable enable
namespace RobloxCS.Types;
[RobloxNative("SoundService", RobloxNativeType.Service)]
public static partial class SoundService {
    public static bool AcousticSimulationEnabled { get; set; } = default!;
    public static Enums.ReverbType AmbientReverb { get; set; } = default!;
    public static Enums.RolloutState CharacterSoundsUseNewApi { get; } = default!;
    public static float DistanceFactor { get; set; } = default!;
    public static float DopplerScale { get; set; } = default!;
    public static bool RespectFilteringEnabled { get; set; } = default!;
    public static float RolloffScale { get; set; } = default!;
    public static Enums.VolumetricAudio VolumetricAudio { get; set; } = default!;
    public static LuaTuple GetListener() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void PlayLocalSound() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void SetListener() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
