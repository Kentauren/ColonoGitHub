using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSpawnSystem : MonoBehaviour{

    public Select select;
    public InventorySystem inventorySystem;
    public GameObject unitSpawnSystem;
    public GameObject unit;
    public GameObject newUnit;
    public RectTransform selectedSpawnCard;
    public RectTransform unitSpawnCard;
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
        }
    }

    public void AddNewCardToBoardList(){
        var newCard = Instantiate(unitSpawnCard, boardListPanel.transform);
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
        newUnit.GetComponent<ObjectInfo>().team = newTeam;
        select.selectables.Add(newUnit);
        Debug.Log("Unit has been spawned on Team" + newTeam);
    }
}
