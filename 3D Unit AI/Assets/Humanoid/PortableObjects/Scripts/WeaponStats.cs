using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New ItemInfo", menuName = "ItemInfo")]
public class WeaponStats : ScriptableObject {

    public string itemName;
    public string itemSlot;
    public bool leftHand;
    public Material itemIconMaterial;
    public Material itemMaterial;
    public float weight;
    public float damage;
    public float weaponSpeed;
    public float armor;
    public float eulerAnglex;
    public float eulerAngley;
    public float eulerAnglez;
    public float scalex;
    public float scaley;
    public float scalez; 
    public float posx;
    public float posy;
    public float posz;
}
