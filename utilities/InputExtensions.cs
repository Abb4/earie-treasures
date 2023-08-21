using Godot;

namespace utilities;

public static class InputExtensions {
    
    // NOTE: Capture should be used with 3d games to rotate player
    public static bool IsMouseCaptured() => Input.MouseMode == Input.MouseModeEnum.Captured;
    public static void CaptureMouse() => Input.MouseMode = Input.MouseModeEnum.Captured;
    public static void UncaptureMouse() => Input.MouseMode = Input.MouseModeEnum.Visible;


    // NOTE: Confined should be used with 2d games to track mouse position
    public static bool IsMouseConfined() => Input.MouseMode == Input.MouseModeEnum.Confined;
    public static void ConfineMouse() => Input.MouseMode = Input.MouseModeEnum.Confined;
    public static void UnconfineMouse() => UncaptureMouse();
    
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
