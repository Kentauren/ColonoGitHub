using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalStorageInfo : MonoBehaviour
{
    public DataOverview dataOverview;
    public GameObject resourceObject;
    public GameObject timberSticker;
    public List<GameObject> timberStickerList = new List<GameObject>();
    public string currentResource = "null";
    public int buildingHealth;
    public int maxStorageAmount;
    public int currentStorageAmount;

    void Start(){
        GameObject findScript = GameObject.Find("Canvas");
        dataOverview = findScript.GetComponent<DataOverview>(); 
    }

    public void AddResource(GameObject resource){
        if(currentResource == "null"){
            currentResource = resource.name;
            resourceObject = resource;

            if(currentResource == "WoodenLog"){
                maxStorageAmount = 30;
            }
            if(currentResource == "Timber"){
                maxStorageAmount = 60;
            }
        }

        if(resource.name == "WoodenLog"){
            GameObject newResource = Instantiate(resource, transform.position, transform.rotation);
            newResource.transform.Rotate(transform.rotation.x, transform.rotation.y + 90, transform.rotation.z);
            dataOverview.woodenLogsAmount += 1;
            newResource.transform.localScale = new Vector3(20, 20, 120);
            newResource.layer = 0;
            newResource.transform.SetParent(this.transform);
            var startPos = 0f;
            var newPos = (float)currentStorageAmount;
            if(currentStorageAmount < 8){
                startPos = 9f;
                newResource.transform.localPosition = new Vector3(0, 1.5f, (newPos * 2.5f) - startPos); 
            }
            if(currentStorageAmount >= 8 && currentStorageAmount < 15){
                startPos = 7.75f;
                newPos = newPos - 8;
                newResource.transform.localPosition = new Vector3(0, 2.5f, (newPos * 2.5f) - startPos); 
            }
            if(currentStorageAmount >= 15 && currentStorageAmount < 21){
                startPos = 6.5f;
                newPos = newPos - 15;
                newResource.transform.localPosition = new Vector3(0, 3.5f, (newPos * 2.5f) - startPos); 
            }
            if(currentStorageAmount >= 21 && currentStorageAmount < 26){
                startPos = 5.25f;
                newPos = newPos - 21;
                newResource.transform.localPosition = new Vector3(0, 4.5f, (newPos * 2.5f) - startPos); 
            }
            if(currentStorageAmount >= 26 && currentStorageAmount < 30){
                startPos = 4f;
                newPos = newPos - 26;
                newResource.transform.localPosition = new Vector3(0, 5.5f, (newPos * 2.5f) - startPos); 
            }
        }

        if(resource.name == "Timber"){
            GameObject newResource = Instantiate(resource, transform.position, transform.rotation);
            newResource.transform.Rotate(transform.rotation.x, transform.rotation.y + 90, transform.rotation.z);
            dataOverview.woodenLogsAmount += 1;
            newResource.transform.localScale = new Vector3(2.3f, 0.03f, 0.2f);
            newResource.layer = 0;
            newResource.transform.SetParent(this.transform);
            var startPos = 10f;
            var newPos = (float)currentStorageAmount;
            if(currentStorageAmount < 12){
                if(currentStorageAmount == 0){
                    PlaceSticker(0.95f);
                }
                newResource.transform.localPosition = new Vector3(0, 1.06f, (newPos * 1.8f) - startPos); 
            }
            if(currentStorageAmount >= 12 && currentStorageAmount < 24){
                if(currentStorageAmount == 12){
                    PlaceSticker(1.17f);
                }
                newPos = newPos - 12;
                newResource.transform.localPosition = new Vector3(0, 1.28f, (newPos * 1.8f) - startPos); 
            }
            if(currentStorageAmount >= 24 && currentStorageAmount < 36){
                if(currentStorageAmount == 24){
                    PlaceSticker(1.39f);
                }
                newPos = newPos - 24;
                newResource.transform.localPosition = new Vector3(0, 1.5f, (newPos * 1.8f) - startPos); 
            }
            if(currentStorageAmount >= 36 && currentStorageAmount < 48){
                if(currentStorageAmount == 36){
                    PlaceSticker(1.61f);
                }
                newPos = newPos - 36;
                newResource.transform.localPosition = new Vector3(0, 1.72f, (newPos * 1.8f) - startPos); 
            }
            if(currentStorageAmount >= 48 && currentStorageAmount < 60){
                if(currentStorageAmount == 48){
                    PlaceSticker(1.83f);
                }
                newPos = newPos - 48;
                newResource.transform.localPosition = new Vector3(0, 1.94f, (newPos * 1.8f) - startPos); 
            }  
        }
        currentStorageAmount += 1;
    }

    public void TakeResource(){ 
        Destroy(gameObject.transform.GetChild(currentStorageAmount - 1).gameObject);
        currentStorageAmount -= 1;
        dataOverview.woodenLogsAmount -= 1;
        
        if(currentStorageAmount == 0 || currentStorageAmount == 12 || currentStorageAmount == 24 || currentStorageAmount == 36 || currentStorageAmount == 48){
            Destroy(timberStickerList[timberStickerList.Count - 1]);
            Destroy(timberStickerList[timberStickerList.Count - 1]);
        }

        if(currentStorageAmount == 0){
            currentResource = "null";
        }
    }

    void PlaceSticker(float height){
        GameObject newTimberSticker = Instantiate(timberSticker, transform.position, transform.rotation);
        newTimberSticker.transform.localScale = new Vector3(0.028f, 0.033f, 2.78f);
        newTimberSticker.transform.SetParent(this.transform);
        newTimberSticker.transform.localPosition = new Vector3(8, height, 0);
        timberStickerList.Add(newTimberSticker);
        newTimberSticker = Instantiate(timberSticker, transform.position, transform.rotation);
        newTimberSticker.transform.localScale = new Vector3(0.028f, 0.033f, 2.78f);
        newTimberSticker.transform.SetParent(this.transform);
        newTimberSticker.transform.localPosition = new Vector3(-8, height, 0);
        timberStickerList.Add(newTimberSticker);  
    }
}
