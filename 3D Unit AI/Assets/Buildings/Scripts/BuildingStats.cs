using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New BuildingInfo", menuName = "BuildingInfo")]
public class BuildingStats : ScriptableObject{

    public string buildingName;
    public int buildingHealth;
    public int maxStorageAmount;
    public int currentStorageAmount;
    public float eulerAnglex;
    public float eulerAngley;
    public float eulerAnglez; 
}
