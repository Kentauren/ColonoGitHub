using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempoaryCollisionChecker : MonoBehaviour
{
    public BuildingSystem buildingSystem;

    void Start(){
        GameObject findScript = GameObject.Find("Canvas");
        buildingSystem = findScript.GetComponent<BuildingSystem>();
    }

    private void OnTriggerEnter(Collider other){
        if(other.tag != "Ground"){
            buildingSystem.currentCollisionsList.Add(other.gameObject);
            Debug.Log(other.name + (" has been added to CurrentCollisionList"));   
        }
        
    }

    private void OnTriggerExit(Collider other){
        buildingSystem.currentCollisionsList.Remove(other.gameObject);
        Debug.Log(other.name + (" has been removed from CurrentCollisionList"));
    }
}
