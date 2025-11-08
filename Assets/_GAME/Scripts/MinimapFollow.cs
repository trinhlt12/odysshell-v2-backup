using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    public Transform player;

    public float cameraHeight;

    void LateUpdate()
    {
        if (player == null)
        {
            return;
        }

        Vector3 newPosition = player.position;

        newPosition.y = cameraHeight;

        transform.position = newPosition;

        Quaternion playerRotation = player.rotation;
        Quaternion newRotation    = Quaternion.Euler(90f, playerRotation.eulerAngles.y, 0f);
        transform.rotation = newRotation;

    }
}