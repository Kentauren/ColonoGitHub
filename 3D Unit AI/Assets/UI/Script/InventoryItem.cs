using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler{
    
    
    public Canvas canvas;
    public InventoryCharacter inventoryCharacter;
    public InventorySystem inventorySystem;
    public UnitSpawnSystem unitSpawnSystem;
    public GameObject dragObject;
    public GameObject cloneDragObject;
    public GameObject itemObject;
    public RectTransform hoverInfo;
    public TextMeshProUGUI nameInfoText;
    public Text weightInfoText;
    public Text damageInfoText;
    public Text weaponSpeedInfoText;
    public Text armorInfoText;
    public Transform characterHead;
    public Transform characterBody;
    public Transform characterBodyClothing;
    public Transform characterRightHand;
    public Transform characterLeftHand;
    public Transform characterBack;
    public List<GameObject> handObjects = new List<GameObject>();
    GraphicRaycaster raycaster;
    EventSystem eventSystem;
    PointerEventData pointerEventData;
    public bool drag;
    public bool hoveringHoverInfo;
    public bool hoveringItemButton;
    public bool checkingHover;
    
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
                ShowHoverInfoOnExit();
                string dragObjectName = gameObject.name;
                for(int j = 0; j < handObjects.Count; j++){
                    if(handObjects[j].name == dragObjectName){
                        dragObject = handObjects[j].gameObject;
                        break;
                    }
                }
                cloneDragObject = Instantiate(dragObject, canvas.transform, false);
                cloneDragObject.GetComponent<EventTrigger>().enabled = false;
                drag = true;
                Debug.Log("DragButton:" + gameObject.name + "is active");            
            }
        }
        canvas.GetComponent<InventorySystem>().ShowCompatibleSlot(drag, itemObject.GetComponent<Weapon>().itemInfo.itemSlot, itemObject.GetComponent<Weapon>().itemInfo.leftHand);
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
                //Checkcs if there is a card in the board, if there is'nt it will instantiate a new card
                if(inventorySystem.selectedCard == null){
                    inventorySystem.NewButtonOnClick();
                }
                Debug.Log("Object has been dropped in" + result.gameObject.name);
                if(itemObject.GetComponent<Weapon>().itemInfo.itemSlot == "HeadSlot"){
                    result.gameObject.transform.GetChild(0).GetComponent<Image>().material = itemObject.GetComponent<Weapon>().itemInfo.itemIconMaterial;
                    if(characterHead.childCount > 0){
                        Destroy(characterHead.GetChild(0).gameObject);
                    }
                    var itemInfo = itemObject.gameObject.GetComponent<Weapon>().itemInfo;
                    inventorySystem.selectedCard.GetComponent<UnitCard>().characterHeadCard = itemObject;
                    unitSpawnSystem.selectedSpawnCard.GetComponent<UnitSpawnCard>().characterHeadCard = itemObject; 
                    GameObject newItemObject = Instantiate(itemObject, characterHead.transform.position, characterHead.transform.rotation);
                    newItemObject.transform.Rotate(itemInfo.eulerAnglex, itemInfo.eulerAngley, itemInfo.eulerAnglez);
                    
                    newItemObject.transform.SetParent(characterHead);
                    newItemObject.transform.localScale = new Vector3(itemInfo.scalex, itemInfo.scaley, itemInfo.scalez);
                    newItemObject.transform.localPosition = new Vector3(itemInfo.posx, itemInfo.posy, itemInfo.posz);
                }
                if(itemObject.GetComponent<Weapon>().itemInfo.itemSlot == "BodySlot"){
                    result.gameObject.transform.GetChild(0).GetComponent<Image>().material = itemObject.GetComponent<Weapon>().itemInfo.itemIconMaterial;
                    if(characterBody.childCount > 0){
                        Destroy(characterBody.GetChild(0).gameObject);
                    }
                    var itemInfo = itemObject.gameObject.GetComponent<Weapon>().itemInfo;
                    inventorySystem.selectedCard.GetComponent<UnitCard>().characterBodyCard = itemObject;
                    unitSpawnSystem.selectedSpawnCard.GetComponent<UnitSpawnCard>().characterBodyCard = itemObject; 
                    GameObject newItemObject = Instantiate(itemObject, characterBody.transform.position, characterBody.transform.rotation);
                    newItemObject.transform.Rotate(itemInfo.eulerAnglex, itemInfo.eulerAngley, itemInfo.eulerAnglez);
                    
                    newItemObject.transform.SetParent(characterBody);
                    newItemObject.transform.localScale = new Vector3(itemInfo.scalex, itemInfo.scaley, itemInfo.scalez);
                    newItemObject.transform.localPosition = new Vector3(itemInfo.posx, itemInfo.posy, itemInfo.posz);
                }
                if(itemObject.GetComponent<Weapon>().itemInfo.itemSlot == "ClothingSlot"){
                    result.gameObject.transform.GetChild(0).GetComponent<Image>().material = itemObject.GetComponent<Weapon>().itemInfo.itemIconMaterial;
                    inventorySystem.selectedCard.GetComponent<UnitCard>().characterClothingCard = itemObject;
                    unitSpawnSystem.selectedSpawnCard.GetComponent<UnitSpawnCard>().characterClothingCard = itemObject; 
                    characterBody.GetComponent<MeshRenderer>().material = itemObject.GetComponent<Weapon>().itemInfo.itemMaterial;
                }
                if(itemObject.gameObject.GetComponent<Weapon>().itemInfo.itemSlot == "HandSlot"){
                    if(itemObject.gameObject.GetComponent<Weapon>().itemInfo.leftHand == false){
                        result.gameObject.transform.GetChild(0).GetComponent<Image>().material = itemObject.GetComponent<Weapon>().itemInfo.itemIconMaterial;
                        if(result.gameObject.GetComponent<HandSlotID>().handSlotID == "HandSlot1"){
                            if(characterRightHand.childCount > 0){
                                Destroy(characterRightHand.GetChild(0).gameObject);
                            }
                            var itemInfo = itemObject.gameObject.GetComponent<Weapon>().itemInfo;
                            inventorySystem.selectedCard.GetComponent<UnitCard>().characterRightHandCard = itemObject;
                            unitSpawnSystem.selectedSpawnCard.GetComponent<UnitSpawnCard>().characterRightHandCard = itemObject; 
                            GameObject newItemObject = Instantiate(itemObject, characterRightHand.transform.position, characterRightHand.transform.rotation);
                            newItemObject.transform.Rotate(itemInfo.eulerAnglex, itemInfo.eulerAngley, itemInfo.eulerAnglez);

                            newItemObject.transform.SetParent(characterRightHand);
                            newItemObject.transform.localScale = new Vector3(itemInfo.scalex, itemInfo.scaley, itemInfo.scalez);
                        }
                        if(result.gameObject.GetComponent<HandSlotID>().handSlotID == "HandSlot2"){
                            if(characterLeftHand.childCount > 0){
                                Destroy(characterLeftHand.GetChild(0).gameObject);
                            }
                            var itemInfo = itemObject.gameObject.GetComponent<Weapon>().itemInfo;
                            inventorySystem.selectedCard.GetComponent<UnitCard>().characterLeftHandCard = itemObject;
                            unitSpawnSystem.selectedSpawnCard.GetComponent<UnitSpawnCard>().characterLeftHandCard = itemObject; 
                            GameObject newItemObject = Instantiate(itemObject, characterLeftHand.transform.position, characterLeftHand.transform.rotation);
                            newItemObject.transform.Rotate(itemInfo.eulerAnglex, itemInfo.eulerAngley, itemInfo.eulerAnglez);
                            
                            newItemObject.transform.SetParent(characterLeftHand);
                            newItemObject.transform.localScale = new Vector3(itemInfo.scalex, itemInfo.scaley, itemInfo.scalez);
                        }
                        if(result.gameObject.GetComponent<HandSlotID>().handSlotID == "HandSlot3"){
                            if(characterBack.childCount > 0){
                                Destroy(characterBack.GetChild(0).gameObject);
                            }
                            var itemInfo = itemObject.gameObject.GetComponent<Weapon>().itemInfo;
                            inventorySystem.selectedCard.GetComponent<UnitCard>().characterBackCard = itemObject;
                            unitSpawnSystem.selectedSpawnCard.GetComponent<UnitSpawnCard>().characterBackCard = itemObject; 
                            GameObject newItemObject = Instantiate(itemObject, characterBack.transform.position, characterBack.transform.rotation);
                            newItemObject.transform.Rotate(itemInfo.eulerAnglex, itemInfo.eulerAngley, itemInfo.eulerAnglez);
                            
                            newItemObject.transform.SetParent(characterBack);
                            newItemObject.transform.localScale = new Vector3(itemInfo.scalex, itemInfo.scaley, itemInfo.scalez);
                        }  
                    }
                    else{
                        
                        if(result.gameObject.GetComponent<HandSlotID>().handSlotID == "HandSlot2"){
                            result.gameObject.transform.GetChild(0).GetComponent<Image>().material = itemObject.GetComponent<Weapon>().itemInfo.itemIconMaterial;
                            if(characterLeftHand.childCount > 0){
                                Destroy(characterLeftHand.GetChild(0).gameObject);
                            }
                            var itemInfo = itemObject.gameObject.GetComponent<Weapon>().itemInfo;
                            inventorySystem.selectedCard.GetComponent<UnitCard>().characterLeftHandCard = itemObject;
                            unitSpawnSystem.selectedSpawnCard.GetComponent<UnitSpawnCard>().characterLeftHandCard = itemObject; 
                            GameObject newItemObject = Instantiate(itemObject, characterLeftHand.transform.position, characterLeftHand.transform.rotation);
                            newItemObject.transform.Rotate(itemInfo.eulerAnglex, itemInfo.eulerAngley, itemInfo.eulerAnglez);
                            
                            newItemObject.transform.SetParent(characterLeftHand);
                            newItemObject.transform.GetChild(0).transform.localScale = new Vector3(itemInfo.scalex, itemInfo.scaley, itemInfo.scalez);
                        }
                        if(result.gameObject.GetComponent<HandSlotID>().handSlotID == "HandSlot3"){
                            result.gameObject.transform.GetChild(0).GetComponent<Image>().material = itemObject.GetComponent<Weapon>().itemInfo.itemIconMaterial;
                            if(characterBack.childCount > 0){
                                Destroy(characterBack.GetChild(0).gameObject);
                            }
                            var itemInfo = itemObject.gameObject.GetComponent<Weapon>().itemInfo;
                            inventorySystem.selectedCard.GetComponent<UnitCard>().characterBackCard = itemObject;
                            unitSpawnSystem.selectedSpawnCard.GetComponent<UnitSpawnCard>().characterBackCard = itemObject; 
                            GameObject newItemObject = Instantiate(itemObject, characterBack.transform.position, characterBack.transform.rotation);
                            newItemObject.transform.Rotate(itemInfo.eulerAnglex, itemInfo.eulerAngley, itemInfo.eulerAnglez);

                            newItemObject.transform.SetParent(characterBack);
                            newItemObject.transform.GetChild(0).transform.localScale = new Vector3(itemInfo.scalex, itemInfo.scaley, itemInfo.scalez);
                        }  
                    }
                }
                inventoryCharacter.Animator(true);
                inventorySystem.UpdateStatsInfo();
            }
            Debug.Log("Hit " + result.gameObject.name);
        }
        drag = false;
        Debug.Log("ItemButton is inactive");
        Destroy(cloneDragObject.gameObject);
        inventorySystem.ShowCompatibleSlot(drag, itemObject.GetComponent<Weapon>().itemInfo.itemSlot, itemObject.GetComponent<Weapon>().itemInfo.leftHand);
    }

    public void ShowHoverInfoOnEnter(){
        if(hoveringItemButton != true || hoveringHoverInfo != true){
            //StartCoroutine(ShowHoverInfo());
            hoverInfo.GetComponent<HoverInfo>().hoveringItemObject = gameObject;
        }
        hoveringItemButton = true;  
    }

    public void ShowHoverInfoOnExit(){
        hoveringItemButton = false;
    }

    public IEnumerator CheckHoverInfo(){
        checkingHover = true;
        yield return new WaitForSeconds(.1f);
        checkingHover = false;
        if(hoveringHoverInfo != true && hoveringItemButton != true){
            StopCoroutine(ShowHoverInfo());
            hoverInfo.gameObject.SetActive(false); 
        }
    }

    public IEnumerator ShowHoverInfo(){
        bool showHoverInfo = false;
        yield return new WaitForSeconds(1f);
        pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerEventData, results);
        for(int i = 0; i < results.Count; i++){
            if(results[i].gameObject.name == gameObject.name){
                showHoverInfo = true;
                Debug.Log("Mouse is still hovering" + gameObject.name);            
            }
        }
        if(showHoverInfo == true && drag == false){
            hoverInfo.gameObject.SetActive(true);
            hoverInfo.transform.position = Input.mousePosition;
            nameInfoText.text = itemObject.GetComponent<Weapon>().itemInfo.itemName;
            weightInfoText.text = itemObject.GetComponent<Weapon>().itemInfo.weight.ToString();
            damageInfoText.text = itemObject.GetComponent<Weapon>().itemInfo.damage.ToString();
            weaponSpeedInfoText.text = itemObject.GetComponent<Weapon>().itemInfo.weaponSpeed.ToString();
            armorInfoText.text = itemObject.GetComponent<Weapon>().itemInfo.armor.ToString();            
        }
        if(drag == true){
            hoverInfo.gameObject.SetActive(false);
        }       
    }
}
