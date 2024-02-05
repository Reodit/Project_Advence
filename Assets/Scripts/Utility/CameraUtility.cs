using UnityEngine;

public static class CameraUtility
{
    public static bool IsTargetInCameraView(Camera camera, Vector2 targetPos)
    {
        float cameraHeight = 2f * camera.orthographicSize;
        float cameraWidth = cameraHeight * camera.aspect;

        Vector2 cameraPosition = camera.transform.position;
        float leftBound = cameraPosition.x - cameraWidth / 2;
        float rightBound = cameraPosition.x + cameraWidth / 2;
        float lowerBound = cameraPosition.y - cameraHeight / 2;
        float upperBound = cameraPosition.y + cameraHeight / 2;

        bool isInView = targetPos.x >= leftBound && targetPos.x <= rightBound &&
                        targetPos.y >= lowerBound && targetPos.y <= upperBound;

        return isInView;
    }
}
