using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateOnUI : MonoBehaviour
{
    public Text lifeText;
    public Text grenadeCounter;
    public Image rewindImage;

    public 

    CharacterHead myChar;


    private void Start()
    {
        lifeText = Main.instance.GetLifeText();
        grenadeCounter = Main.instance.GetGrenadeText();
        rewindImage = Main.instance.GetRewindImg();
        
        myChar = GetComponent<CharacterHead>();
    }

    public void UpdateLifeText(float life)
    {
        lifeText.text = life.ToString();
    }
    
    public void UpdateRewindCD(float currentCD, float maxCd)
    {
       rewindImage.fillAmount = currentCD / maxCd;
    }
    public void UpdateGrenades(int grenadeAmmount)
    {
        grenadeCounter.text = grenadeAmmount.ToString();
    }

}
