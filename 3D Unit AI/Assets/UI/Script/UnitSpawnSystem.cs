using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSpawnSystem : MonoBehaviour{

    public Select select;
    public InventorySystem inventorySystem;
    public BuildingSystem buildingSystem;
    public GameObject unitSpawnSystem;
    public GameObject unit;
    private GameObject newUnit;
    public GameObject character;
    public RectTransform selectedSpawnCard;
    public RectTransform newUnitSpawnCard;
    public RectTransform boardListPanel;
    public Transform selectedSpawn;
    public Transform team1SpawnPoint;
    public Transform team2SpawnPoint;
    public Button team1Button;
    public Button team2Button;
    public Button selectedButton; 
    Color32 teamButtonSelectedColor = new Color32(140, 140, 140, 255);
    Color32 teamButtonDefaultColor = new Color32(255, 255, 255, 255);
    public List<RectTransform> spawnBoardList = new List<RectTransform>(); 
    public float newTeam;
    public bool unitSpawnSystemIsActive;

    void Start(){
        unitSpawnSystemIsActive = false;
        unitSpawnSystem.SetActive(false);
    }

    public void ActivateUnitSpawnSystemOnClick(){

        if(unitSpawnSystemIsActive == true){
            unitSpawnSystem.SetActive(false);
            unitSpawnSystemIsActive = false;
        }
        else{
            unitSpawnSystem.SetActive(true);
            unitSpawnSystemIsActive = true;
            if(inventorySystem.invenSysIsActive == true){
                inventorySystem.ActivateInventorySystem();
            }
            if(buildingSystem.buildingSystemIsActive == true){
                buildingSystem.ActivateBuildingSystemOnClick();
            }
            selectedSpawnCard.GetComponent<UnitSpawnCard>().UpdateText();
        }
    }

    public void AddNewCardToBoardList(){
        var newCard = Instantiate(newUnitSpawnCard, boardListPanel.transform);
        spawnBoardList.Add(newCard);
        selectedSpawnCard = newCard;
    }

    public void Team1SelectedOnClick(){
        selectedSpawn = team1SpawnPoint;
        newTeam = 11;
        team1Button.GetComponent<Image>().color = teamButtonSelectedColor;
        team2Button.GetComponent<Image>().color = teamButtonDefaultColor;
        Debug.Log("Team 1 has been selected");
    }

    public void Team2SelectedOnClick(){
        selectedSpawn = team2SpawnPoint;
        newTeam = 12;
        team2Button.GetComponent<Image>().color = teamButtonSelectedColor;
        team1Button.GetComponent<Image>().color = teamButtonDefaultColor;
        Debug.Log("Team 2 has been selected");
    }

    public void SpawnUnitOnClick(){
        newUnit = Instantiate(unit, selectedSpawn.position, Quaternion.identity);
        newUnit.GetComponent<ObjectInfo>().head = selectedSpawnCard.GetComponent<UnitSpawnCard>().characterHeadCard;
        newUnit.GetComponent<ObjectInfo>().body = selectedSpawnCard.GetComponent<UnitSpawnCard>().characterBodyCard;
        newUnit.GetComponent<ObjectInfo>().clothing = selectedSpawnCard.GetComponent<UnitSpawnCard>().characterClothingCard;
        newUnit.GetComponent<ObjectInfo>().rightHand = selectedSpawnCard.GetComponent<UnitSpawnCard>().characterRightHandCard;
        newUnit.GetComponent<ObjectInfo>().leftHand = selectedSpawnCard.GetComponent<UnitSpawnCard>().characterLeftHandCard;
        newUnit.GetComponent<ObjectInfo>().back = selectedSpawnCard.GetComponent<UnitSpawnCard>().characterBackCard;

        if(newUnit.GetComponent<ObjectInfo>().head != null){
            var itemAngle = newUnit.GetComponent<ObjectInfo>().head.GetComponent<Weapon>().itemInfo;
            GameObject newItemObject = Instantiate(newUnit.GetComponent<ObjectInfo>().head, newUnit.GetComponent<ObjectInfo>().headTransform.position, newUnit.GetComponent<ObjectInfo>().headTransform.rotation);
            newItemObject.transform.Rotate(itemAngle.eulerAnglex, itemAngle.eulerAngley, itemAngle.eulerAnglez);
            newItemObject.transform.SetParent(newUnit.GetComponent<ObjectInfo>().headTransform);
        }
        if(newUnit.GetComponent<ObjectInfo>().body != null){
            var itemAngle = newUnit.GetComponent<ObjectInfo>().body.GetComponent<Weapon>().itemInfo;
            GameObject newItemObject = Instantiate(newUnit.GetComponent<ObjectInfo>().body, newUnit.GetComponent<ObjectInfo>().bodyTransform.position, newUnit.GetComponent<ObjectInfo>().bodyTransform.rotation);
            newItemObject.transform.Rotate(itemAngle.eulerAnglex, itemAngle.eulerAngley, itemAngle.eulerAnglez);
            newItemObject.transform.SetParent(newUnit.GetComponent<ObjectInfo>().bodyTransform);   
        }
        if(newUnit.GetComponent<ObjectInfo>().clothing != null){
            newUnit.GetComponent<ObjectInfo>().bodyTransform.GetComponent<MeshRenderer>().material = selectedSpawnCard.GetComponent<UnitSpawnCard>().characterClothingCard.GetComponent<Weapon>().itemInfo.itemMaterial;
        }
        if(newUnit.GetComponent<ObjectInfo>().rightHand != null){
            var itemAngle = newUnit.GetComponent<ObjectInfo>().rightHand.GetComponent<Weapon>().itemInfo;
            GameObject newItemObject = Instantiate(newUnit.GetComponent<ObjectInfo>().rightHand, newUnit.GetComponent<ObjectInfo>().rightHandTransform.position, newUnit.GetComponent<ObjectInfo>().rightHandTransform.rotation);
            newItemObject.transform.Rotate(itemAngle.eulerAnglex, itemAngle.eulerAngley, itemAngle.eulerAnglez);
            newItemObject.transform.SetParent(newUnit.GetComponent<ObjectInfo>().rightHandTransform);   
        }
        if(newUnit.GetComponent<ObjectInfo>().leftHand != null){
            var itemAngle = newUnit.GetComponent<ObjectInfo>().leftHand.GetComponent<Weapon>().itemInfo;
            GameObject newItemObject = Instantiate(newUnit.GetComponent<ObjectInfo>().leftHand, newUnit.GetComponent<ObjectInfo>().leftHandTransform.position, newUnit.GetComponent<ObjectInfo>().leftHandTransform.rotation);
            newItemObject.transform.Rotate(itemAngle.eulerAnglex, itemAngle.eulerAngley, itemAngle.eulerAnglez);
            newItemObject.transform.SetParent(newUnit.GetComponent<ObjectInfo>().leftHandTransform);   
        }
        if(newUnit.GetComponent<ObjectInfo>().back != null){
            var itemAngle = newUnit.GetComponent<ObjectInfo>().back.GetComponent<Weapon>().itemInfo;
            GameObject newItemObject = Instantiate(newUnit.GetComponent<ObjectInfo>().back, newUnit.GetComponent<ObjectInfo>().backTransform.position, newUnit.GetComponent<ObjectInfo>().backTransform.rotation);
            newItemObject.transform.Rotate(itemAngle.eulerAnglex, itemAngle.eulerAngley, itemAngle.eulerAnglez);
            newItemObject.transform.SetParent(newUnit.GetComponent<ObjectInfo>().backTransform);   
        }
        
        newUnit.GetComponent<ObjectInfo>().team = newTeam;
        newUnit.GetComponent<ObjectInfo>().maxArmor = (int)selectedSpawnCard.GetComponent<UnitSpawnCard>().totalArmor;
        newUnit.GetComponent<ObjectInfo>().maxWeight = selectedSpawnCard.GetComponent<UnitSpawnCard>().totalWeight;
        select.selectables.Add(newUnit);
        Debug.Log("Unit has been spawned on Team" + newTeam);
    }
}
