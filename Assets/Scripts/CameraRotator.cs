using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CameraRotator : MonoBehaviour
{
    float speed = 5f;

    public float timeLerp = 0.1f;
    public GameObject myWeapon;
    public Camera myCamera;
    [SerializeField] float xAxisClamp;
    PhotonView phView;
    public IController controller;

    public float mouseSensivity;

    private void Start()
    {
        controller = new CharacterInput();
        phView = GetComponentInParent<PhotonView>();
    }

    void  FixedUpdate()
    {
        if (!phView.IsMine) return;

        RotateCamera();
    }

    void RotateCamera()
    {
        float mouseX = controller.HorizontalRotation() * mouseSensivity;
        float mouseY = controller.VerticalRotation() * mouseSensivity;

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

