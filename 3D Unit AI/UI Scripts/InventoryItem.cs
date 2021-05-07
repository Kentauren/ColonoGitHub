using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler{
    
    public bool drag;
    public Canvas canvas;
    public GameObject dragObject;
    public GameObject cloneDragObject;
    public GameObject itemObject;
    public Transform characterHead;
    public Transform characterBody;
    public Transform characterRightHand;
    public Transform characterLeftHand;
    public Transform characterback;
    private List<GameObject> handObjects = new List<GameObject>();
    GraphicRaycaster raycaster;
    EventSystem eventSystem;
    PointerEventData pointerEventData;
    
    void Start(){
        eventSystem = canvas.GetComponent<EventSystem>();
        raycaster = canvas.GetComponent<GraphicRaycaster>();
    }
    void Update(){
        if(drag == true){
            cloneDragObject.transform.position = Input.mousePosition;
        }
    }

    public void OnPointerDown(PointerEventData eventData){
        pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerEventData, results);
        for(int i = 0; i < results.Count; i++){
            if(results[i].gameObject.name == gameObject.name){
                string dragObjectName = gameObject.name;
                dragObject = GameObject.Find(dragObjectName);
                cloneDragObject = Instantiate(dragObject, canvas.transform, false);
                drag = true;
                Debug.Log("DragButton:" + gameObject.name + "is active");            
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData){
        //Set up the new Pointer Event
        pointerEventData = new PointerEventData(eventSystem);
        //Set the Pointer Event Position to that of the mouse position
        pointerEventData.position = Input.mousePosition;
        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();
        //Raycast using the Graphics Raycaster and mouse click position
        raycaster.Raycast(pointerEventData, results);
        //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
        foreach (RaycastResult result in results){
            if(itemObject.GetComponent<Weapon>().itemInfo.itemSlot == result.gameObject.name){
                result.gameObject.transform.GetChild(1).GetComponent<Image>().material = itemObject.GetComponent<Weapon>().itemInfo.itemMaterial;
                Debug.Log("Object has been dropped in" + result.gameObject.name);
                if(itemObject.GetComponent<Weapon>().itemInfo.itemSlot == "HeadSlot"){ 
                    GameObject newItemObject = Instantiate(itemObject, characterHead.transform.position, Quaternion.identity);
                    newItemObject.transform.SetParent(characterHead);
                }
                if(itemObject.GetComponent<Weapon>().itemInfo.itemSlot == "BodySlot"){
                    GameObject newItemObject = Instantiate(itemObject, characterBody.transform.position, Quaternion.identity);
                    newItemObject.transform.SetParent(characterBody);
                }
                if(itemObject.gameObject.GetComponent<Weapon>().itemInfo.itemSlot == "HandSlot"){
                    if(result.gameObject.GetComponent<HandSlotID>().handSlotID == "HandSlot1"){
                        GameObject newItemObject = Instantiate(itemObject, characterRightHand.transform.position, Quaternion.identity);
                        newItemObject.transform.SetParent(characterRightHand);
                    }
                    if(result.gameObject.GetComponent<HandSlotID>().handSlotID == "HandSlot2"){
                        GameObject newItemObject = Instantiate(itemObject, characterLeftHand.transform.position, Quaternion.identity);
                        newItemObject.transform.SetParent(characterLeftHand);
                    }
                    if(result.gameObject.GetComponent<HandSlotID>().handSlotID == "HandSlot3"){
                        GameObject newItemObject = Instantiate(itemObject, characterback.transform.position, Quaternion.identity);
                        newItemObject.transform.SetParent(characterBody);
                    }
                }
            }
            Debug.Log("Hit " + result.gameObject.name);
        }
        drag = false;
        Debug.Log("ItemButton is inactive");
        Destroy(cloneDragObject.gameObject);
    }
}
