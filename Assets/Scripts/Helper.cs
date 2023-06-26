using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper
{
    public static bool PointInCameraView(Vector3 point)
    {
        Vector3 viewport = Camera.main.WorldToViewportPoint(point);
        bool inCameraFrustum = Is01(viewport.x) && Is01(viewport.y);
        bool inFrontOfCamera = viewport.z > 0;

        //RaycastHit depthCheck;
        //bool objectBlockingPoint = false;

        /*Vector3 directionBetween = point - camera.transform.position;
        directionBetween = directionBetween.normalized;

        float distance = Vector3.Distance(camera.transform.position, point);

        if (Physics.Raycast(camera.transform.position, directionBetween, out depthCheck, distance + 0.05f))
        {
            if (depthCheck.point != point)
            {
                objectBlockingPoint = true;
            }
        }*/

        //return inCameraFrustum && inFrontOfCamera && !objectBlockingPoint;

        return inCameraFrustum && inFrontOfCamera;
    }

    private static bool Is01(float a)
    {
        return a > 0 && a < 1;
    }
}