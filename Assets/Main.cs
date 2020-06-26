using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Main : MonoBehaviour
{
    public Text lifeText;
    public Text GrenadeCounter;
    public Image RewindTimeImg;
    public ScoreManager ScoreBoard;

    public static Main instance;

    public Text GetLifeText() { return lifeText; }
    public Text GetGrenadeText() { return GrenadeCounter; }
    public Image GetRewindImg() { return RewindTimeImg; }
    public ScoreManager GetScoreboard() { return ScoreBoard; }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
