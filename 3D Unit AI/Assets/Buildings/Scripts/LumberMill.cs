using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LumberMill : MonoBehaviour{
    public Transform woodenLogStorage;
    public Transform timberStorage;
    public Transform workbenchA;
    public Transform workbenchB;
    public GameObject woodenLogObject;
    public GameObject timberObject;
    public List<GameObject> currentWorkersList = new List<GameObject>();
    public bool fill;
    public bool empty;
    public bool lumberMillisOccupied;    
    public int buildingHealth;
    public int maxTimberAmount;
    public int timberOutput;
    public int currentTimberAmount;
    public int maxWoodenLogAmount;
    public int currentWoodenLogAmount;
    public int currentWorkersAmount;

    void Start(){
        currentWorkersAmount = 0;
        lumberMillisOccupied = false;
    }

    public void AddWorker(GameObject worker){
        if(currentWorkersAmount < 2){
            currentWorkersAmount += 1;
            currentWorkersList.Add(worker);
        }
        if(currentWorkersAmount >= 2){
            lumberMillisOccupied = true;
        }
    }

    public void RemoveWorker(GameObject worker){
        if(currentWorkersAmount > 0){
            currentWorkersAmount -= 1;
            currentWorkersList.Remove(worker);
        }
        if(currentWorkersAmount < 2 && lumberMillisOccupied == true){
           lumberMillisOccupied = false; 
        }
    }

    public void AddResource(GameObject resource){
        
        if(resource.name == "WoodenLog"){
            GameObject newResource = Instantiate(resource, transform.position, transform.rotation);
            newResource.transform.localScale = new Vector3(20, 20, 120);
            newResource.layer = 0;
            newResource.transform.SetParent(woodenLogStorage);
            newResource.transform.localPosition = new Vector3(0, 0, 0);
            newResource.transform.localRotation = Quaternion.Euler(0, 90, 0);
            var startPos = 0f;
            var newPos = (float)currentWoodenLogAmount;

            if(currentWoodenLogAmount < 3){
                newResource.transform.localPosition = new Vector3(0, 0.8f, -(newPos * 1.5f) - startPos);
            }
            if(currentWoodenLogAmount >= 3 && currentWoodenLogAmount < 5){
                startPos = 1f;
                newPos = newPos - 3;
                newResource.transform.localPosition = new Vector3(0, 2f, -(newPos * 1.5f) - startPos);
            }
            if(currentWoodenLogAmount >= 5 && currentWoodenLogAmount < 6){
                startPos = 2f;
                newPos = newPos - 5;
                newResource.transform.localPosition = new Vector3(0, 3f, -(newPos * 1.5f) - startPos);
            }

            currentWoodenLogAmount += 1;
            Debug.Log("Woodenlog has been added to mill storage");
        }

        if(resource.name == "Timber"){
            GameObject newResource = Instantiate(resource, transform.position, transform.rotation);
            newResource.transform.localScale = new Vector3(2.3f, 0.03f, 0.2f);
            newResource.layer = 0;
            newResource.transform.SetParent(timberStorage);
            newResource.transform.localPosition = new Vector3(0, 0, 0);
            newResource.transform.localRotation = Quaternion.Euler(0, 90, 0);
            var newPos = (float)currentTimberAmount;

            if(currentTimberAmount < 9){
                newResource.transform.localPosition = new Vector3(- newPos * 0.9f, 0, 0);
            }
            if(currentTimberAmount >= 9 && currentTimberAmount < 18){
                newPos = newPos - 9;
                newResource.transform.localRotation = Quaternion.Euler(0, 0, 0);
                newResource.transform.localPosition = new Vector3(-3.65f, 0.12f, -(4 - newPos) * 0.9f);
            }
            if(currentTimberAmount >= 18 && currentTimberAmount < 27){
                newPos = newPos - 18;
                newResource.transform.localPosition = new Vector3(- newPos * 0.9f, 0.24f, 0);
            }
            if(currentTimberAmount >= 27 && currentTimberAmount < 36){
                newPos = newPos - 27;
                newResource.transform.localRotation = Quaternion.Euler(0, 0, 0);
                newResource.transform.localPosition = new Vector3(-3.65f, 0.36f, -(4 - newPos) * 0.9f);
            }
            if(currentTimberAmount >= 36 && currentTimberAmount < 45){
                newPos = newPos - 36;
                newResource.transform.localPosition = new Vector3(- newPos * 0.9f, 0.48f, 0);
            }
            if(currentTimberAmount >= 45 && currentTimberAmount < 54){
                newPos = newPos - 45;
                newResource.transform.localRotation = Quaternion.Euler(0, 0, 0);
                newResource.transform.localPosition = new Vector3(-3.65f, 0.60f, -(4 - newPos) * 0.9f);
            }
            if(currentTimberAmount >= 54 && currentTimberAmount < 63){
                newPos = newPos - 54;
                newResource.transform.localPosition = new Vector3(- newPos * 0.9f, 0.72f, 0);
            }
            if(currentTimberAmount >= 63 && currentTimberAmount < 72){
                newPos = newPos - 63;
                newResource.transform.localRotation = Quaternion.Euler(0, 0, 0);
                newResource.transform.localPosition = new Vector3(-3.65f, 0.84f, -(4 - newPos) * 0.9f);
            }

            currentTimberAmount += 1;
            Debug.Log("Timber has been added to mill storage");
        }
    }

    public void TakeResourceWoodenLog(){ 
        Destroy(woodenLogStorage.transform.GetChild(currentWoodenLogAmount - 1).gameObject);
        currentWoodenLogAmount -= 1;
    }

    public void TakeResourceTimber(){
        Destroy(timberStorage.transform.GetChild(currentTimberAmount - 1).gameObject);
        currentTimberAmount -= 1;
    }

    public void AddResourceWorkbench(Transform workbench){
        GameObject newWoodenLog = Instantiate(woodenLogObject, transform.position, transform.rotation);
        newWoodenLog.layer = 0;
        newWoodenLog.name = "WoodenLog";
        newWoodenLog.transform.SetParent(workbench);
        newWoodenLog.transform.localPosition = new Vector3(0, 8, 0);
    }

    public void TakeResourceWorkbench(Transform workbench){
        Destroy(workbench.transform.GetChild(workbench.childCount - 1).gameObject);
    }

    public void ProduceTimber(Transform workbench){
        Destroy(workbench.GetChild(0).gameObject);
        for(int i = 0; i < timberOutput; i++){
            GameObject newTimber = Instantiate(timberObject, transform.position, transform.rotation);
            newTimber.name = "Timber";
            newTimber.transform.SetParent(workbench);
            newTimber.transform.localScale = new Vector3(13.16644f, 0.7005091f, 0.5879322f);
            var newHeight = 0.5f * (float)i;
            newTimber.transform.localPosition = new Vector3(0, 4.27f + newHeight, 0);
            newTimber.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }    
    }
}
