using UnityEngine;

public class Utils
{
    public static Vector3 ScreenToWorld(Camera camera, Vector3 position) {
        return camera.ScreenToWorldPoint(position);
    }
}
