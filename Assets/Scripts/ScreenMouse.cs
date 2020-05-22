using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenMouse : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Cursor.lockState == CursorLockMode.Locked)
            LockMouse();
        else if (Input.GetKeyDown(KeyCode.Escape))
            UnlockMouse();
    }

    public void LockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    public void UnlockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

}
