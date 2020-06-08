using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canvasreda : MonoBehaviour
{
    public static canvasreda Instance;

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
