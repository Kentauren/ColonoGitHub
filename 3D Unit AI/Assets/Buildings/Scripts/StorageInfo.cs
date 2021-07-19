using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageInfo : MonoBehaviour{   

    public DataOverview dataOverview;
    public GameObject resourceObject;
    public string currentResource = "WoodenLog";
    public int buildingHealth;
    public int maxStorageAmount;
    public int currentStorageAmount;

    void Start(){
        GameObject findScript = GameObject.Find("Canvas");
        dataOverview = findScript.GetComponent<DataOverview>();
    }

    public void AddResource(GameObject resource){
        GameObject newResource = Instantiate(resource, transform.position, transform.rotation);
        newResource.transform.Rotate(transform.rotation.x, transform.rotation.y + 90, transform.rotation.z);
        dataOverview.woodenLogsAmount += 1;
        newResource.transform.localScale = new Vector3(20, 20, 120);
        newResource.layer = 0;
        newResource.transform.SetParent(this.transform);
        var startPos = 0f;
        var newPos = (float)currentStorageAmount;
        if(currentStorageAmount < 16){
            startPos = 19f;
            newResource.transform.localPosition = new Vector3(0, 0.4f, (newPos * 2.5f) - startPos); 
        }
        if(currentStorageAmount >= 16 && currentStorageAmount < 31){
            startPos = 17.75f;
            newPos = newPos - 16;
            newResource.transform.localPosition = new Vector3(0, 1.1f, (newPos * 2.5f) - startPos); 
        }
        if(currentStorageAmount >= 31 && currentStorageAmount < 45){
            startPos = 16.5f;
            newPos = newPos - 31;
            newResource.transform.localPosition = new Vector3(0, 1.8f, (newPos * 2.5f) - startPos);
        }
        if(currentStorageAmount >= 45 && currentStorageAmount < 58){
            startPos = 15.25f;
            newPos = newPos - 45;
            newResource.transform.localPosition = new Vector3(0, 2.5f, (newPos * 2.5f) - startPos);
        }
        if(currentStorageAmount >= 58 && currentStorageAmount < 70){
            startPos = 14f;
            newPos = newPos - 58;
            newResource.transform.localPosition = new Vector3(0, 3.2f, (newPos * 2.5f) - startPos);
        }
        if(currentStorageAmount >= 70 && currentStorageAmount < 81){
            startPos = 12.75f;
            newPos = newPos - 70;
            newResource.transform.localPosition = new Vector3(0, 3.9f, (newPos * 2.5f) - startPos);
        }
        if(currentStorageAmount >= 81 && currentStorageAmount < 91){
            startPos = 11.5f;
            newPos = newPos - 81;
            newResource.transform.localPosition = new Vector3(0, 4.6f, (newPos * 2.5f) - startPos);
        }
        if(currentStorageAmount >= 91 && currentStorageAmount < 100){
            startPos = 10.25f;
            newPos = newPos - 91;
            newResource.transform.localPosition = new Vector3(0, 5.3f, (newPos * 2.5f) - startPos);
        }
    }

    public void TakeResource(){ 
        Destroy(gameObject.transform.GetChild(currentStorageAmount - 1).gameObject);
        currentStorageAmount -= 1;
        dataOverview.woodenLogsAmount -= 1;
    }
}
