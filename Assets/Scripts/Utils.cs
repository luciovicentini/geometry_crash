using UnityEngine;

public class Utils
{
    public static Vector3 ScreenToWorld(Camera camera, Vector3 position) {
        return camera.ScreenToWorldPoint(position);
    }

    public static GameObject DetectObject(Camera mainCamera, Vector2 position)
    {
        Ray ray = mainCamera.ScreenPointToRay(position);
        RaycastHit2D hits2D = Physics2D.GetRayIntersection(ray);
        if (hits2D.collider != null)
        {
            return hits2D.collider.gameObject;
        }
        return null;
    }
}
