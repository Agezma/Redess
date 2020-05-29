using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Main : MonoBehaviour
{
    public Text lifeText;

    public static Main instance;

    public Text GetLifeText() { return lifeText; }

    private void Awake()
    {
        instance = this;
    }

}
