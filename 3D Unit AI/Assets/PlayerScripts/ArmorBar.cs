using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmorBar : MonoBehaviour{

    public Slider armorSlider;

    public void SetMaxHealth(float armorHealth, float armorMaxHealth){
        armorSlider.maxValue = armorMaxHealth;
        armorSlider.value = armorHealth;
    }

    public void SetHealth(float armorHealth){
        armorSlider.value = armorHealth;
    }   
}
