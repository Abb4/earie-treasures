using Godot;
using static Godot.Mathf;

namespace utilities;

public static class InputExtensions {
    public static bool IsMouseCaptured() => Input.MouseMode == Input.MouseModeEnum.Captured;
    public static void CaptureMouse() => Input.MouseMode = Input.MouseModeEnum.Captured;
    public static void UncaptureMouse() => Input.MouseMode = Input.MouseModeEnum.Visible;
    
    public static Vector3 AsRotation3dDeg(this InputEventMouseMotion mouseMotion)
    {
        // Note: Y in 2d corresponds to X in 3d, same swap for X
        return new Vector3(
            - mouseMotion.Relative.Y, 
            - mouseMotion.Relative.X, 
            0
        ); 
    }

    public static Vector3 AsRotation3dRad(this InputEventMouseMotion mouseMotion)
    {
        return AsRotation3dDeg(mouseMotion).DegToRad();
    }
}
