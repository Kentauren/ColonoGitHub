using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour{

    public Select select;
    public Canvas canvas;
    public UnitSpawnSystem unitSpawnSystem;
    public GameObject inventorySystem;
    public Material minusIcon;
    public Material plusIcon;
    public Button itemObject;
    public RectTransform hoverInfo;
    public RectTransform card;
    public RectTransform board;
    public RectTransform selectedCard;
    public Text nameText;
    public Text weightText;
    public Text armorText;
    public Material defaultHeadSlotMaterial;
    public Material defaultBodySlotMaterial;
    public Material defaultHandSlotMaterial;
    public Material defaultClothingSlotMaterial;
    public Material defaultClothing;
    public Image headSlotImage;
    public Image bodySlotImage;
    public Image clothingSlotImage;
    public Image rightHandSlotImage;
    public Image leftHandSlotImage;
    public Image backSlotImage;
    public Button headSlotButton;
    public Button bodySlotButton;
    public Button clothingSlotButton;
    public Button rightHandSlotButton;
    public Button leftHandSlotButton;
    public Button backSlotButton;
    public Transform characterHead;
    public Transform characterBody;
    public Transform characterRightHand;
    public Transform characterLeftHand;
    public Transform characterBack;
    Color32 noItemsColor = new Color32(124, 124, 124, 255);
    Color32 itemsColor = new Color32(252, 244, 228, 255);
    Color32 uncompatibleColor = new Color32(140, 140, 140, 255);
    Color32 compatibleColor = new Color32(255, 255, 255, 255);
    public List<RectTransform> boardList = new List<RectTransform>();
    public Image headArmorShowListIcon;
    public Image bodyArmorShowListIcon;
    public Image swordsShowListIcon;
    public Image axesShowListIcon;
    public Image spearsShowListIcon;
    public Image shieldsShowListIcon;
    public Image bowsShowListIcon;
    public Image crossbowShowListIcon;
    public Image ammunitionShowListIcon;
    public Image clothingShowListIcon;
    public Image toolsShowListIcon;
    public List<GameObject> headArmorItemList = new List<GameObject>();   
    public List<GameObject> bodyArmorItemList = new List<GameObject>();
    public List<GameObject> swordsItemList = new List<GameObject>();
    public List<GameObject> axesItemList = new List<GameObject>();
    public List<GameObject> spearsItemList = new List<GameObject>();
    public List<GameObject> shieldsItemList = new List<GameObject>();
    public List<GameObject> bowsItemList = new List<GameObject>();
    public List<GameObject> crossbowItemList = new List<GameObject>();
    public List<GameObject> ammunitionItemList = new List<GameObject>();
    public List<GameObject> clothingItemList = new List<GameObject>();
    public List<GameObject> toolsItemList = new List<GameObject>();
    List<Button> headArmorItemButtonList = new List<Button>();
    List<Button> bodyArmorItemButtonList = new List<Button>();
    List<Button> swordsItemButtonList = new List<Button>();
    List<Button> axesItemButtonList = new List<Button>();
    List<Button> spearsItemButtonList = new List<Button>();
    List<Button> shieldsItemButtonList = new List<Button>();
    List<Button> bowsItemButtonList = new List<Button>();
    List<Button> crossbowItemButtonList = new List<Button>();
    List<Button> ammunitionItemButtonList = new List<Button>();
    List<Button> clothingItemButtonList = new List<Button>();
    List<Button> toolsItemButtonList = new List<Button>();
    public RectTransform headArmorParent;
    public RectTransform bodyArmorParent;
    public RectTransform swordsParent;
    public RectTransform axesParent;
    public RectTransform spearsParent;
    public RectTransform shieldsParent;
    public RectTransform bowsParent;
    public RectTransform crossbowParent;
    public RectTransform ammunitionParent;
    public RectTransform clothingParent;
    public RectTransform toolsParent;

    public Button headArmorButton;
    public Button bodyArmorButton;
    public Button swordsButton;
    public Button axesButton;
    public Button spearsButton;
    public Button shieldsButton;
    public Button bowsButton;
    public Button crossbowButton;
    public Button ammunitionButton;
    public Button clothingButton;
    public Button toolsButton;
    
    bool invenSysIsActive;
    bool headArmorShowList;
    bool bodyArmorShowList;
    bool swordsShowList;
    bool axesShowList;
    bool spearsShowList;
    bool shieldsShowList;
    bool bowsShowList;
    bool crossbowsShowList;
    bool ammunitionShowList;
    bool clothingShowList;
    bool toolsShowList;
    GraphicRaycaster raycaster;
    PointerEventData pointerEventData;
    EventSystem eventSystem;

    void Start(){
        inventorySystem.SetActive(false);
        hoverInfo.gameObject.SetActive(false);    
        invenSysIsActive = false;
        headArmorShowList = false;
        bodyArmorShowList = false;
        swordsShowList = false;
        axesShowList = false;
        spearsShowList = false;
        shieldsShowList = false;
        bowsShowList = false;
        crossbowsShowList = false;
        ammunitionShowList = false;
        clothingShowList = false;
        toolsShowList = false;
        raycaster = GetComponent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(1))
        {
            pointerEventData = new PointerEventData(eventSystem);
            pointerEventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerEventData, results);
            foreach (RaycastResult result in results){
                if(result.gameObject.name == "HeadSlot"){
                    result.gameObject.transform.GetChild(1).GetComponent<Image>().material = defaultHeadSlotMaterial;
                    Destroy(characterHead.GetChild(0).gameObject);
                    selectedCard.GetComponent<UnitCard>().characterHeadCard = null;
                }
                if(result.gameObject.name == "BodySlot"){
                    result.gameObject.transform.GetChild(1).GetComponent<Image>().material = defaultBodySlotMaterial;
                    Destroy(characterBody.GetChild(0).gameObject);
                    selectedCard.GetComponent<UnitCard>().characterBodyCard = null;
                }
                if(result.gameObject.name == "ClothingSlot"){
                    result.gameObject.transform.GetChild(1).GetComponent<Image>().material = defaultClothingSlotMaterial;
                    characterBody.GetComponent<MeshRenderer>().material = defaultClothing;
                    selectedCard.GetComponent<UnitCard>().characterClothingCard = null;
                }
                if(result.gameObject.name == "HandSlot"){
                    if(result.gameObject.GetComponent<HandSlotID>().handSlotID == "HandSlot1"){
                        result.gameObject.transform.GetChild(1).GetComponent<Image>().material = defaultHandSlotMaterial;
                        Destroy(characterRightHand.GetChild(0).gameObject);
                        selectedCard.GetComponent<UnitCard>().characterRightHandCard = null;
                    }
                    if(result.gameObject.GetComponent<HandSlotID>().handSlotID == "HandSlot2"){
                        result.gameObject.transform.GetChild(1).GetComponent<Image>().material = defaultHandSlotMaterial;
                        Destroy(characterLeftHand.GetChild(0).gameObject);
                        selectedCard.GetComponent<UnitCard>().characterLeftHandCard = null;
                    }
                    if(result.gameObject.GetComponent<HandSlotID>().handSlotID == "HandSlot3"){
                        result.gameObject.transform.GetChild(1).GetComponent<Image>().material = defaultHandSlotMaterial;
                        Destroy(characterBack.GetChild(0).gameObject);
                        selectedCard.GetComponent<UnitCard>().characterBackCard = null;
                    }                    
                }
                Debug.Log("Hit " + result.gameObject.name);
            }
            UpdateStatsInfo();
        }
    }         

    public void ActivateInventorySystem(){
        if(invenSysIsActive == false){
            inventorySystem.SetActive(true);
            invenSysIsActive = true;
            select.selectionBoxIsActive = false;
            select.rTS_Camera.enabled = false;
            if(headArmorItemList.Count == 0){
                headArmorButton.GetComponent<Image>().color = noItemsColor;
            }
            if(headArmorItemList.Count > 0){
                headArmorButton.GetComponent<Image>().color = itemsColor;
            }
            if(bodyArmorItemList.Count == 0){
                bodyArmorButton.GetComponent<Image>().color = noItemsColor;
            }
            if(bodyArmorItemList.Count > 0){
                bodyArmorButton.GetComponent<Image>().color = itemsColor;
            }            
            if(swordsItemList.Count == 0){
                swordsButton.GetComponent<Image>().color = noItemsColor;
            }
            if(swordsItemList.Count > 0){
                swordsButton.GetComponent<Image>().color = itemsColor;
            }            
            if(axesItemList.Count == 0){
                axesButton.GetComponent<Image>().color = noItemsColor;
            }
            if(axesItemList.Count > 0){
                axesButton.GetComponent<Image>().color = itemsColor;
            }            
            if(spearsItemList.Count == 0){
                spearsButton.GetComponent<Image>().color = noItemsColor;
            }
            if(spearsItemList.Count > 0){
                spearsButton.GetComponent<Image>().color = itemsColor;
            }            
            if(shieldsItemList.Count == 0){
                shieldsButton.GetComponent<Image>().color = noItemsColor;
            }
            if(shieldsItemList.Count > 0){
                shieldsButton.GetComponent<Image>().color = itemsColor;
            }            
            if(bowsItemList.Count == 0){
                bowsButton.GetComponent<Image>().color = noItemsColor;
            }
            if(bowsItemList.Count > 0){
                bowsButton.GetComponent<Image>().color = itemsColor;
            }            
            if(crossbowItemList.Count == 0){
                crossbowButton.GetComponent<Image>().color = noItemsColor;
            }
            if(crossbowItemList.Count > 0){
                crossbowButton.GetComponent<Image>().color = itemsColor;
            }            
            if(ammunitionItemList.Count == 0){
                ammunitionButton.GetComponent<Image>().color = noItemsColor;
            }
            if(ammunitionItemList.Count > 0){
                ammunitionButton.GetComponent<Image>().color = itemsColor;
            }            
            if(clothingItemList.Count == 0){
                clothingButton.GetComponent<Image>().color = noItemsColor;
            }
            if(clothingItemList.Count > 0){
                clothingButton.GetComponent<Image>().color = itemsColor;
            }            
            if(toolsItemList.Count == 0){
                toolsButton.GetComponent<Image>().color = noItemsColor;
            }
            if(toolsItemList.Count > 0){
                toolsButton.GetComponent<Image>().color = itemsColor;
            }            
        }
        else{
            invenSysIsActive = false;
            inventorySystem.SetActive(false);
            select.selectionBoxIsActive = true;
            select.rTS_Camera.enabled = true;
        }
    }

    public void NewButtonOnClick(){
        if(boardList.Count < 9){
            var newCard = Instantiate(card, board.transform);
            boardList.Add(newCard);
            selectedCard = newCard;
            unitSpawnSystem.AddNewCardToBoardList();
            ResetOnClick();
        }
        else{
            Debug.Log("The maximum of cards has been reached (9)");
        }
    }

    public void DeleteButtonOnClick(){
        if(selectedCard != null){
            int i = boardList.IndexOf(selectedCard);
            boardList.RemoveAt(i);
            Destroy(selectedCard.gameObject);
            unitSpawnSystem.spawnBoardList.RemoveAt(i);
            Destroy(unitSpawnSystem.selectedSpawnCard.gameObject);       
            if(boardList.Count > 0){
                selectedCard = boardList[0];
                unitSpawnSystem.selectedSpawnCard = unitSpawnSystem.spawnBoardList[0];
            }
        }
        else{
            Debug.Log("There are no cards left to remove");
        }
    }

    public void ResetOnClick(){
        if(characterHead.childCount > 0){ //Head
            Destroy(characterHead.GetChild(0).gameObject);
            headSlotImage.material = defaultHeadSlotMaterial;
            selectedCard.GetComponent<UnitCard>().characterHeadCard = null;
            unitSpawnSystem.selectedSpawnCard.GetComponent<UnitSpawnCard>().characterHeadCard = null;
        }
        if(characterBody.childCount > 0){ //Body
            Destroy(characterBody.GetChild(0).gameObject);
            bodySlotImage.material = defaultBodySlotMaterial;
            selectedCard.GetComponent<UnitCard>().characterBodyCard = null;
            unitSpawnSystem.selectedSpawnCard.GetComponent<UnitSpawnCard>().characterBodyCard = null;
        }

        characterBody.GetComponent<MeshRenderer>().material = defaultClothing;
        clothingSlotImage.material = defaultClothingSlotMaterial;
        selectedCard.GetComponent<UnitCard>().characterClothingCard = null;
        unitSpawnSystem.selectedSpawnCard.GetComponent<UnitSpawnCard>().characterClothingCard = null;

        if(characterRightHand.childCount > 0){ //RightHand
            Destroy(characterRightHand.GetChild(0).gameObject);
            rightHandSlotImage.material = defaultHandSlotMaterial;
            selectedCard.GetComponent<UnitCard>().characterRightHandCard = null;
            unitSpawnSystem.selectedSpawnCard.GetComponent<UnitSpawnCard>().characterRightHandCard = null;
        }
        if(characterLeftHand.childCount > 0){ //LeftHand
            Destroy(characterLeftHand.GetChild(0).gameObject);
            leftHandSlotImage.material = defaultHandSlotMaterial;
            selectedCard.GetComponent<UnitCard>().characterLeftHandCard = null;
            unitSpawnSystem.selectedSpawnCard.GetComponent<UnitSpawnCard>().characterLeftHandCard = null;
        }
        if(characterBack.childCount > 0){ //Back
            Destroy(characterBack.GetChild(0).gameObject);
            backSlotImage.material = defaultHandSlotMaterial;
            selectedCard.GetComponent<UnitCard>().characterBackCard = null;
            unitSpawnSystem.selectedSpawnCard.GetComponent<UnitSpawnCard>().characterBackCard = null;
        }
        UpdateStatsInfo();
    }

    public void EditNameOnClick(){
        selectedCard.GetComponent<UnitCard>().nameField.text = nameText.text.ToString();
        unitSpawnSystem.selectedSpawnCard.GetComponent<UnitSpawnCard>().cardName.text = nameText.text.ToString();
        Debug.Log("Name has been edited");
    }

    public void UpdateStatsInfo(){
        float headArmor = 0f;
        float bodyArmor = 0f;
        float clothingArmor = 0f;
        float rightHandArmor = 0f;
        float leftHandArmor = 0f;
        float backArmor = 0f;
        float headWeight = 0f;
        float bodyWeight = 0f;
        float clothingWeight = 0f;
        float rightHandWeight = 0f;
        float leftHandWeight = 0f;
        float backWeight = 0f;

        if(selectedCard.GetComponent<UnitCard>().characterHeadCard != null){
            headArmor = selectedCard.GetComponent<UnitCard>().characterHeadCard.GetComponent<Weapon>().itemInfo.armor;
            headWeight = selectedCard.GetComponent<UnitCard>().characterHeadCard.GetComponent<Weapon>().itemInfo.weight; 
        }
        if(selectedCard.GetComponent<UnitCard>().characterBodyCard != null){
            bodyArmor = selectedCard.GetComponent<UnitCard>().characterBodyCard.GetComponent<Weapon>().itemInfo.armor;
            bodyWeight = selectedCard.GetComponent<UnitCard>().characterBodyCard.GetComponent<Weapon>().itemInfo.weight; 
        }
        if(selectedCard.GetComponent<UnitCard>().characterClothingCard != null){
            clothingArmor = selectedCard.GetComponent<UnitCard>().characterClothingCard.GetComponent<Weapon>().itemInfo.armor;
            clothingWeight = selectedCard.GetComponent<UnitCard>().characterClothingCard.GetComponent<Weapon>().itemInfo.weight; 
        }
        if(selectedCard.GetComponent<UnitCard>().characterRightHandCard != null){
            rightHandArmor = selectedCard.GetComponent<UnitCard>().characterRightHandCard.GetComponent<Weapon>().itemInfo.armor;
            rightHandWeight = selectedCard.GetComponent<UnitCard>().characterRightHandCard.GetComponent<Weapon>().itemInfo.weight;
        }
        if(selectedCard.GetComponent<UnitCard>().characterLeftHandCard != null){
            leftHandArmor = selectedCard.GetComponent<UnitCard>().characterLeftHandCard.GetComponent<Weapon>().itemInfo.armor;
            leftHandWeight = selectedCard.GetComponent<UnitCard>().characterLeftHandCard.GetComponent<Weapon>().itemInfo.weight;
        }
        if(selectedCard.GetComponent<UnitCard>().characterBackCard != null){
            backArmor = selectedCard.GetComponent<UnitCard>().characterBackCard.GetComponent<Weapon>().itemInfo.armor;
            backWeight = selectedCard.GetComponent<UnitCard>().characterBackCard.GetComponent<Weapon>().itemInfo.weight;
        }
        float totalArmor = headArmor + bodyArmor + clothingArmor + rightHandArmor + leftHandArmor + backArmor;
        float totalWeight = headWeight + bodyWeight + clothingWeight + rightHandWeight + leftHandWeight + backWeight;
        weightText.text = totalWeight.ToString();
        armorText.text = totalArmor.ToString();
        nameText.text = selectedCard.GetComponent<UnitCard>().name.ToString();
    }

    public void ShowCompatibleSlot(bool drag, string itemSlotName){
        if(drag == true){
            if(itemSlotName == "HeadSlot"){
                bodySlotButton.GetComponent<Image>().color = uncompatibleColor;
                clothingSlotButton.GetComponent<Image>().color = uncompatibleColor;
                rightHandSlotButton.GetComponent<Image>().color = uncompatibleColor;
                leftHandSlotButton.GetComponent<Image>().color = uncompatibleColor;
                backSlotButton.GetComponent<Image>().color = uncompatibleColor;
            }
            if(itemSlotName == "BodySlot"){
                headSlotButton.GetComponent<Image>().color = uncompatibleColor;
                clothingSlotButton.GetComponent<Image>().color = uncompatibleColor;
                rightHandSlotButton.GetComponent<Image>().color = uncompatibleColor;
                leftHandSlotButton.GetComponent<Image>().color = uncompatibleColor;
                backSlotButton.GetComponent<Image>().color = uncompatibleColor;
            }
            if(itemSlotName == "ClothingSlot"){
                headSlotButton.GetComponent<Image>().color = uncompatibleColor;
                bodySlotButton.GetComponent<Image>().color = uncompatibleColor;
                rightHandSlotButton.GetComponent<Image>().color = uncompatibleColor;
                leftHandSlotButton.GetComponent<Image>().color = uncompatibleColor;
                backSlotButton.GetComponent<Image>().color = uncompatibleColor;
            }
            if(itemSlotName == "HandSlot"){
                headSlotButton.GetComponent<Image>().color = uncompatibleColor;
                bodySlotButton.GetComponent<Image>().color = uncompatibleColor;
                clothingSlotButton.GetComponent<Image>().color = uncompatibleColor;
            }            
        }
        if(drag == false){
            headSlotButton.GetComponent<Image>().color = compatibleColor;
            bodySlotButton.GetComponent<Image>().color = compatibleColor;
            clothingSlotButton.GetComponent<Image>().color = compatibleColor;
            rightHandSlotButton.GetComponent<Image>().color = compatibleColor;
            leftHandSlotButton.GetComponent<Image>().color = compatibleColor;
            backSlotButton.GetComponent<Image>().color = compatibleColor;
        }
    }

    public void HeadArmorOnClick(){
        if(headArmorItemList.Count > 0){
            if(headArmorShowList == false){
                headArmorShowListIcon.material = minusIcon;
                headArmorShowList = true;
                float newSize = headArmorItemList.Count * 31;
                headArmorParent.sizeDelta += new Vector2(0,newSize);
                for(int i = 0; i < headArmorItemList.Count; i++){
                    itemObject.GetComponentInChildren<Text>().text = headArmorItemList[i].name;
                    itemObject.name = headArmorItemList[i].name;
                    itemObject.GetComponent<InventoryItem>().itemObject = headArmorItemList[i].gameObject; 
                    var cloneItemObject = Instantiate(itemObject, headArmorParent.transform);
                    headArmorItemButtonList.Add(cloneItemObject);
                }
            }
            else{
                headArmorShowListIcon.material = plusIcon;
                headArmorShowList = false;
                headArmorParent.sizeDelta = new Vector2(200,30);
                for(int i = 0; i < headArmorItemButtonList.Count; i++)
                {
                    Destroy(headArmorItemButtonList[i].gameObject);
                }
                headArmorItemButtonList.Clear();
            }            
        }
        else{
            Debug.Log("No Head Armor are available");
        }
    }

    public void BodyArmorOnClick(){
        if(bodyArmorItemList.Count > 0){
            if(bodyArmorShowList == false){
                bodyArmorShowListIcon.material = minusIcon;
                bodyArmorShowList = true;
                float newSize = bodyArmorItemList.Count * 31;
                bodyArmorParent.sizeDelta += new Vector2(0,newSize);
                for(int i = 0; i < bodyArmorItemList.Count; i++){
                    itemObject.GetComponentInChildren<Text>().text = bodyArmorItemList[i].name;
                    itemObject.name = bodyArmorItemList[i].name;
                    itemObject.GetComponent<InventoryItem>().itemObject = bodyArmorItemList[i].gameObject;
                    var cloneItemObject = Instantiate(itemObject, bodyArmorParent.transform);
                    bodyArmorItemButtonList.Add(cloneItemObject);
                }
            }
            else{
                bodyArmorShowListIcon.material = plusIcon;
                bodyArmorShowList = false;
                bodyArmorParent.sizeDelta = new Vector2(200,30);
                for(int i = 0; i < bodyArmorItemButtonList.Count; i++)
                {
                    Destroy(bodyArmorItemButtonList[i].gameObject);
                }
                bodyArmorItemButtonList.Clear();
            }            
        }
        else{
            Debug.Log("No Body Armor are available");
        }
    }

    public void AxesOnClick(){
        if(axesItemList.Count > 0){
            if(axesShowList == false){
                axesShowListIcon.material = minusIcon;
                axesShowList = true;
                float newSize = axesItemList.Count * 31;
                axesParent.sizeDelta += new Vector2(0,newSize);
                for(int i = 0; i < axesItemList.Count; i++){
                    itemObject.GetComponentInChildren<Text>().text = axesItemList[i].name;
                    itemObject.name = axesItemList[i].name;
                    itemObject.GetComponent<InventoryItem>().itemObject = axesItemList[i].gameObject;
                    var cloneItemObject = Instantiate(itemObject, axesParent.transform);
                    axesItemButtonList.Add(cloneItemObject);
                }
            }
            else{
                axesShowListIcon.material = plusIcon;
                axesShowList = false;
                axesParent.sizeDelta = new Vector2(200,30);
                for(int i = 0; i < axesItemButtonList.Count; i++){
                    Destroy(axesItemButtonList[i].gameObject);
                }
                axesItemButtonList.Clear();
            }            
        }
        else{
            Debug.Log("No Axes are available");
        }
    }

    public void SwordsOnClick(){
        if(swordsItemList.Count > 0){
            if(swordsShowList == false){
                swordsShowListIcon.material = minusIcon;
                swordsShowList = true;
                float newSize = swordsItemList.Count * 31;
                swordsParent.sizeDelta += new Vector2(0,newSize);
                for(int i = 0; i < swordsItemList.Count; i++){
                    itemObject.GetComponentInChildren<Text>().text = swordsItemList[i].name;
                    itemObject.name = swordsItemList[i].name;
                    itemObject.GetComponent<InventoryItem>().itemObject = swordsItemList[i].gameObject;
                    var cloneItemObject = Instantiate(itemObject, swordsParent.transform);
                    swordsItemButtonList.Add(cloneItemObject);
                }
            }
            else{
                swordsShowListIcon.material = plusIcon;
                swordsShowList = false;
                swordsParent.sizeDelta = new Vector2(200,30);
                for(int i = 0; i < swordsItemButtonList.Count; i++){
                    Destroy(swordsItemButtonList[i].gameObject);
                }
                swordsItemButtonList.Clear();
            }            
        }
        else{
            Debug.Log("No Swords are available");
        }
    }

    public void SpearsOnClick(){
        if(spearsItemList.Count > 0){
            if(spearsShowList == false){
                spearsShowListIcon.material = minusIcon;
                spearsShowList = true;
                float newSize = spearsItemList.Count * 31;
                spearsParent.sizeDelta += new Vector2(0,newSize);
                for(int i = 0; i < spearsItemList.Count; i++){
                    itemObject.GetComponentInChildren<Text>().text = spearsItemList[i].name;
                    itemObject.name = spearsItemList[i].name;
                    itemObject.GetComponent<InventoryItem>().itemObject = spearsItemList[i].gameObject;
                    var cloneItemObject = Instantiate(itemObject, spearsParent.transform);
                    spearsItemButtonList.Add(cloneItemObject);
                }
            }
            else{
                spearsShowListIcon.material = plusIcon;
                spearsShowList = false;
                spearsParent.sizeDelta = new Vector2(200,30);
                for(int i = 0; i < spearsItemButtonList.Count; i++){
                    Destroy(spearsItemButtonList[i].gameObject);
                }
                spearsItemButtonList.Clear();
            }            
        }
        else{
            Debug.Log("No Spears are available");
        }
    }

    public void ShieldsOnClick(){
        if(shieldsItemList.Count > 0){
            if(shieldsShowList == false){
                shieldsShowListIcon.material = minusIcon;
                shieldsShowList = true;
                float newSize = shieldsItemList.Count * 31;
                shieldsParent.sizeDelta += new Vector2(0,newSize);
                for(int i = 0; i < shieldsItemList.Count; i++){
                    itemObject.GetComponentInChildren<Text>().text = shieldsItemList[i].name;
                    itemObject.name = shieldsItemList[i].name;
                    itemObject.GetComponent<InventoryItem>().itemObject = shieldsItemList[i].gameObject;
                    var cloneItemObject = Instantiate(itemObject, shieldsParent.transform);
                    shieldsItemButtonList.Add(cloneItemObject);
                }
            }
            else{
                shieldsShowListIcon.material = plusIcon;
                shieldsShowList = false;
                shieldsParent.sizeDelta = new Vector2(200,30);
                for(int i = 0; i < shieldsItemButtonList.Count; i++){
                    Destroy(shieldsItemButtonList[i].gameObject);
                }
                shieldsItemButtonList.Clear();
            }            
        }
        else{
            Debug.Log("No Shields are available");
        }
    }    

    public void BowsOnClick(){
        if(bowsItemList.Count > 0){
            if(bowsShowList == false){
                bowsShowListIcon.material = minusIcon;
                bowsShowList = true;
                float newSize = bowsItemList.Count * 31;
                bowsParent.sizeDelta += new Vector2(0,newSize);
                for(int i = 0; i < bowsItemList.Count; i++){
                    itemObject.GetComponentInChildren<Text>().text = bowsItemList[i].name;
                    itemObject.name = bowsItemList[i].name;
                    itemObject.GetComponent<InventoryItem>().itemObject = bowsItemList[i].gameObject;
                    var cloneItemObject = Instantiate(itemObject, bowsParent.transform);
                    bowsItemButtonList.Add(cloneItemObject);
                }
            }
            else{
                bowsShowListIcon.material = plusIcon;
                bowsShowList = false;
                bowsParent.sizeDelta = new Vector2(200,30);
                for(int i = 0; i < bowsItemButtonList.Count; i++){
                    Destroy(bowsItemButtonList[i].gameObject);
                }
                bowsItemButtonList.Clear();
            }            
        }
        else{
            Debug.Log("No bows are avaiable");
        }
    }

    public void CrossbowOnClick(){
        if(crossbowItemList.Count > 0){
            if(crossbowsShowList == false){
                crossbowShowListIcon.material = minusIcon;
                crossbowsShowList = true;
                float newSize = crossbowItemList.Count * 31;
                crossbowParent.sizeDelta += new Vector2(0,newSize);
                for(int i = 0; i < crossbowItemList.Count; i++){
                    itemObject.GetComponentInChildren<Text>().text = crossbowItemList[i].name;
                    itemObject.name = crossbowItemList[i].name;
                    itemObject.GetComponent<InventoryItem>().itemObject = crossbowItemList[i].gameObject;
                    var cloneItemObject = Instantiate(itemObject, crossbowParent.transform);
                    crossbowItemButtonList.Add(cloneItemObject);
                }
            }
            else{
                crossbowShowListIcon.material = plusIcon;
                crossbowsShowList = false;
                crossbowParent.sizeDelta = new Vector2(200,30);
                for(int i = 0; i < crossbowItemButtonList.Count; i++){
                    Destroy(crossbowItemButtonList[i].gameObject);
                }
                crossbowItemButtonList.Clear();
            }            
        }
        else{
            Debug.Log("No Crossbows are available");
        }
    }  

    public void AmmunitionOnClick(){
        if(ammunitionItemList.Count > 0){
            if(ammunitionShowList == false){
                ammunitionShowListIcon.material = minusIcon;
                ammunitionShowList = true;
                float newSize = ammunitionItemList.Count * 31;
                ammunitionParent.sizeDelta += new Vector2(0,newSize);
                for(int i = 0; i < ammunitionItemList.Count; i++){
                    itemObject.GetComponentInChildren<Text>().text = ammunitionItemList[i].name;
                    itemObject.name = ammunitionItemList[i].name;
                    itemObject.GetComponent<InventoryItem>().itemObject = ammunitionItemList[i].gameObject;
                    var cloneItemObject = Instantiate(itemObject, ammunitionParent.transform);
                    ammunitionItemButtonList.Add(cloneItemObject);
                }
            }
            else{
                ammunitionShowListIcon.material = plusIcon;
                ammunitionShowList = false;
                ammunitionParent.sizeDelta = new Vector2(200,30);
                for(int i = 0; i < ammunitionItemButtonList.Count; i++){
                    Destroy(ammunitionItemButtonList[i].gameObject);
                }
                ammunitionItemButtonList.Clear();
            }            
        }
        else{
            Debug.Log("No Ammunition are avaiable");
        }
    }

    public void ClothingOnClick(){
        if(clothingItemList.Count > 0){
            if(clothingShowList == false){
                clothingShowListIcon.material = minusIcon;
                clothingShowList = true;
                float newSize = clothingItemList.Count * 31;
                clothingParent.sizeDelta += new Vector2(0,newSize);
                for(int i = 0; i < clothingItemList.Count; i++){
                    itemObject.GetComponentInChildren<Text>().text = clothingItemList[i].name;
                    itemObject.name = clothingItemList[i].name;
                    itemObject.GetComponent<InventoryItem>().itemObject = clothingItemList[i].gameObject;
                    var cloneItemObject = Instantiate(itemObject, clothingParent.transform);
                    clothingItemButtonList.Add(cloneItemObject);
                }
            }
            else{
                clothingShowListIcon.material = plusIcon;
                clothingShowList = false;
                clothingParent.sizeDelta = new Vector2(200,30);
                for(int i = 0; i < clothingItemButtonList.Count; i++){
                    Destroy(clothingItemButtonList[i].gameObject);
                }
                clothingItemButtonList.Clear();
            }            
        }
        else{
            Debug.Log("No Clothing are available");
        }
    }

    public void ToolsOnClick(){
        if(toolsItemList.Count > 0){
            if(toolsShowList == false){
                toolsShowListIcon.material = minusIcon;
                toolsShowList = true;
                float newSize = toolsItemList.Count * 31;
                toolsParent.sizeDelta += new Vector2(0,newSize);
                for(int i = 0; i < toolsItemList.Count; i++){
                    itemObject.GetComponentInChildren<Text>().text = toolsItemList[i].name;
                    itemObject.name = toolsItemList[i].name;
                    itemObject.GetComponent<InventoryItem>().itemObject = toolsItemList[i].gameObject;
                    var cloneItemObject = Instantiate(itemObject, toolsParent.transform);
                    toolsItemButtonList.Add(cloneItemObject);
                }
            }
            else{
                toolsShowListIcon.material = plusIcon;
                toolsShowList = false;
                toolsParent.sizeDelta = new Vector2(200,30);
                for(int i = 0; i < toolsItemButtonList.Count; i++){
                    Destroy(toolsItemButtonList[i].gameObject);
                }
                toolsItemButtonList.Clear();
            }        
        }
        else{
            Debug.Log("No Tools are available");
        }            
    }
}
