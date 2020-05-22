using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInput : IController
{
    public float Horizontal()
    {
        return Input.GetAxis("Horizontal");
    }

    public float Vertical()
    {
        return Input.GetAxis("Vertical");
    }
    public bool Jump()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    public float HorizontalRotation()
    {
        return Input.GetAxis("Mouse X");
    }

    public float VerticalRotation()
    {
        return Input.GetAxis("Mouse Y");
    }

    public bool Shoot()
    {
        return Input.GetMouseButtonDown(0);
    }
    public bool ThrowGranade()
    {
        return Input.GetKeyDown(KeyCode.Q);
    }
    public bool Rewind()
    {
        return Input.GetKeyDown(KeyCode.E);
    }
}
