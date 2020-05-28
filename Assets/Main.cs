using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Main : MonoBehaviour
{
    public Text lifeText;

    private Main _instance;

    Text GetLifeText() { return lifeText; }

}
