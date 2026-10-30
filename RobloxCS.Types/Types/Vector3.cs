#pragma warning disable CS0660, CS0661

namespace RobloxCS.Types;

[RobloxNative("Vector3", RobloxNativeType.DataType)]
public readonly struct Vector3 {
    public Vector3(double x, double y, double z) { }

    [RobloxName("zero")]
    public static Vector3 Zero => ThrowHelper.ThrowTranspiledMethod();

    [RobloxName("one")]
    public static Vector3 One => ThrowHelper.ThrowTranspiledMethod();

    [RobloxName("xAxis")]
    public static Vector3 XAxis => ThrowHelper.ThrowTranspiledMethod();

    [RobloxName("yAxis")]
    public static Vector3 YAxis => ThrowHelper.ThrowTranspiledMethod();

    [RobloxName("zAxis")]
    public static Vector3 ZAxis => ThrowHelper.ThrowTranspiledMethod();

    public double X => ThrowHelper.ThrowTranspiledMethod();
    public double Y => ThrowHelper.ThrowTranspiledMethod();
    public double Z => ThrowHelper.ThrowTranspiledMethod();
    public double Magnitude => ThrowHelper.ThrowTranspiledMethod();
    public Vector3 Unit => ThrowHelper.ThrowTranspiledMethod();

    public static Vector3 FromNormalId(Enums.NormalId normal) => ThrowHelper.ThrowTranspiledMethod();
    public static Vector3 FromAxis(Enums.Axis axis) => ThrowHelper.ThrowTranspiledMethod();

    public Vector3 Abs() => ThrowHelper.ThrowTranspiledMethod();
    public Vector3 Ceil() => ThrowHelper.ThrowTranspiledMethod();
    public Vector3 Floor() => ThrowHelper.ThrowTranspiledMethod();
    public Vector3 Sign() => ThrowHelper.ThrowTranspiledMethod();
    public Vector3 Cross(Vector3 other) => ThrowHelper.ThrowTranspiledMethod();
    public double Angle(Vector3 other, Vector3? axis = null) => ThrowHelper.ThrowTranspiledMethod();
    public double Dot(Vector3 other) => ThrowHelper.ThrowTranspiledMethod();
    public bool FuzzyEq(Vector3 other, double epsilon = 1e-5) => ThrowHelper.ThrowTranspiledMethod();
    public Vector3 Lerp(Vector3 goal, double alpha) => ThrowHelper.ThrowTranspiledMethod();
    public Vector3 Max(Vector3 other) => ThrowHelper.ThrowTranspiledMethod();
    public Vector3 Min(Vector3 other) => ThrowHelper.ThrowTranspiledMethod();

    public static Vector3 operator +(Vector3 a, Vector3 b) => ThrowHelper.ThrowTranspiledMethod();
    public static Vector3 operator -(Vector3 a, Vector3 b) => ThrowHelper.ThrowTranspiledMethod();
    public static Vector3 operator *(Vector3 a, Vector3 b) => ThrowHelper.ThrowTranspiledMethod();
    public static Vector3 operator *(Vector3 a, double b) => ThrowHelper.ThrowTranspiledMethod();
    public static Vector3 operator *(double a, Vector3 b) => ThrowHelper.ThrowTranspiledMethod();
    public static Vector3 operator /(Vector3 a, Vector3 b) => ThrowHelper.ThrowTranspiledMethod();
    public static Vector3 operator /(Vector3 a, double b) => ThrowHelper.ThrowTranspiledMethod();
    public static Vector3 operator -(Vector3 a) => ThrowHelper.ThrowTranspiledMethod();
    public static bool operator ==(Vector3 a, Vector3 b) => ThrowHelper.ThrowTranspiledMethod();
    public static bool operator !=(Vector3 a, Vector3 b) => ThrowHelper.ThrowTranspiledMethod();
}