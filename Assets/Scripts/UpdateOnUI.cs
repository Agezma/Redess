using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateOnUI : MonoBehaviour
{
    public Text lifeText;
    private void Start()
    {
        lifeText = Main.instance.GetLifeText();
    }

    public void UpdateLifeText(float life)
    {
        lifeText.text = life.ToString();
    }

}
