using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
    public Transform _cameraTransform;

    void LateUpdate()
    {
        Vector3 lookDirection = _cameraTransform.position - transform.position;

        lookDirection.y = 0;
        if (lookDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }
    }
}