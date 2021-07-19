using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageBuildingUI : MonoBehaviour{
    public BuildingSystem buildingSystem;
    public RectTransform storageBuildingRect;
    public GameObject storageBuildingObject;

    public void BuildingPlacementOnClick(){
        buildingSystem.selectedBuildingRect = storageBuildingRect;
        buildingSystem.selectedBuildingObject = storageBuildingObject;
    }
}
