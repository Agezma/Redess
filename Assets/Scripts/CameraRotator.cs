using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CameraRotator : MonoBehaviour
{
    float speed = 5f;

    public float timeLerp = 0.1f;
    public Camera myCamera;
    [SerializeField] float xAxisClamp;
   
    public float mouseSensivity;
           
    public void RotateCamera(float mouseHorizontal, float mouseVertical)
    {
        float mouseX = mouseHorizontal * mouseSensivity;
        float mouseY = mouseVertical * mouseSensivity;

        Vector3 rotateCameraVector3 = myCamera.transform.rotation.eulerAngles;
        Vector3 rotateBodyVector3 = transform.rotation.eulerAngles;
        rotateCameraVector3.x -= mouseY;
        rotateBodyVector3.y += mouseX;
                
        xAxisClamp -= mouseY;
        if (xAxisClamp > 90)
        {
            xAxisClamp = 90;
            rotateCameraVector3.x = xAxisClamp;
        }
        else if (xAxisClamp < -90)
        {
            xAxisClamp = -90;
            rotateCameraVector3.x = 270;
        }
        myCamera.transform.rotation = Quaternion.Euler(rotateCameraVector3);
        transform.rotation = Quaternion.Euler(rotateBodyVector3);

    }
}

