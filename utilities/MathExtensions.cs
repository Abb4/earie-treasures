using Godot;

using static Godot.Mathf;

namespace utilities;

public static class MathExtensions {

    public static Vector3 DegToRad(this Vector3 vector)
    {
        return new Vector3(
            Mathf.DegToRad(vector.X), 
            Mathf.DegToRad(vector.Y), 
            Mathf.DegToRad(vector.Z)
        );
    }
}
