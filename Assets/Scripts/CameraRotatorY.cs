using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotatorY : MonoBehaviour
{

    private Vector3 initialVector;

    public Transform target;

    public float clampYUp;
    public float clampYDown;
    public float mouseSensivity;

    private void Start()
    {
        initialVector = transform.localPosition - target.localPosition;
        initialVector.x = 0;
    }

    private void Update()
    {
        Rotate();
    }

    void Rotate()
    {

        float rotateDegrees = 0f;

        float mouseY = Input.GetAxis("Mouse Y") * mouseSensivity;
        rotateDegrees-= mouseY;

        Vector3 currentVector = transform.localPosition - target.localPosition;
        currentVector.x = 0;
        float angleBetween = Vector3.Angle(initialVector, currentVector) * (Vector3.Cross(initialVector, currentVector).x > 0 ? 1 : -1);
        float newAngle = Mathf.Clamp(angleBetween + rotateDegrees, -clampYUp, clampYDown);
        rotateDegrees = newAngle - angleBetween;

        transform.RotateAround(target.position, target.transform.right, rotateDegrees);
    }
}
