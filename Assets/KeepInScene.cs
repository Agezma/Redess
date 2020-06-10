using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepInScene : MonoBehaviour
{
    public static KeepInScene Instance;

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

}
