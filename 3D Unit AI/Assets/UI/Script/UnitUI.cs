using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UnitUI : MonoBehaviour{

    public GameObject mainCamera;
    public GameObject unitInfo;
    public GameObject unitsInfo;
    public UnitGroupUI groupUI;
    public RectTransform groupUITransform;
    public Image unitIcon;
    public Button formationButton;
    public Button patrolButtonUnitInfo;
    public Button patrolButtonUnitInfos;
    public Button standGroundButtonUnitInfo;
    public Button standGroundButtonUnitUnfos;
    public Image formationButtonIcon;
    public Material dynamicFormationMaterial;
    public Material randomFormationMaterial;
    public Material circleFormationMaterial;
    private bool patrolButtonActive;
    public bool dynamicFormation;
    public bool randomFormation;
    public bool circleFormation; 
    public TextMeshProUGUI currentTaskUI;
    public TextMeshProUGUI teamTextUI;
    public TextMeshProUGUI cardNameTextUI;
    public TextMeshProUGUI jobTextUI;
    public HealthBar healthBar;

    public List<GameObject> selectedUnitsUI = new List<GameObject>();

    void Start(){
        unitInfo.SetActive(false);
        unitsInfo.SetActive(false);
        randomFormation = true;
        dynamicFormation = false;
        circleFormation = false;
        patrolButtonActive = false;
    }

    void Update(){
        if (Input.GetMouseButtonDown(1) && !Input.GetKey(KeyCode.LeftShift)){
            patrolButtonActive = false;
            patrolButtonUnitInfo.GetComponent<Image>().color = Color.white;
            patrolButtonUnitInfos.GetComponent<Image>().color = Color.white;            
        }
        //Checks if only 1 unit is selected
        if (mainCamera.GetComponent<Select>().selectedObjects.Count == 1) {
            GameObject unit1 = mainCamera.GetComponent<Select>().selectedObjects[0]; //Access the first object in the list
            Movement movement = unit1.GetComponent<Movement>();
            ObjectInfo objectInfo = unit1.GetComponent<ObjectInfo>();
            healthBar.SetHealth(objectInfo.currentHealth);          
            if(movement.task == TaskList.Idle) {
                currentTaskUI.text = "Idle"; //Changes the text to Idle in UnitUI
            }
            if(movement.task == TaskList.Moving){
                currentTaskUI.text = "Moving"; //Changes the text to Moving in UnitUI
            }
            if(movement.task == TaskList.Attacking){
                currentTaskUI.text = "Attacking"; //Changes the text to Attacking in UnitUI
            }
            if(movement.task == TaskList.Blocking){
                currentTaskUI.text = "Blocking"; //Changes the text to Attacking in UnitUI
            }
            if(movement.task == TaskList.ReadyForAttack){
                currentTaskUI.text = "Ready For Attack"; //Changes the text to Ready For Attack in UnitUI
            }
            if(movement.task == TaskList.PreparingForattack){
                currentTaskUI.text = "Preparing For Attack"; //Changes the text to Preparing For Attack in UnitUI
            }
            if(movement.task == TaskList.HeadingToTree){
                currentTaskUI.text = "Heading To Tree"; //Changes the text to Heading To Tree in UnitUI
            }
            if(movement.task == TaskList.HeadingToLog){
                currentTaskUI.text = "Heading To Log"; //Changes the text to Heading To Log in UnitUI
            }
            if(movement.task == TaskList.CuttingTree){
                currentTaskUI.text = "Cutting Tree"; //Changes the text to Cutting Tree in UnitUI
            }
            if(movement.task == TaskList.MovingLogToStorage){
                currentTaskUI.text = "Moving Log To Storage"; //Changes the text to Moving Log To Storage in UnitUI
            }
        }
        //Checks if more than 1 unit is selected 
        if (mainCamera.GetComponent<Select>().selectedObjects.Count > 1){
            for (int i = 0; i < mainCamera.GetComponent<Select>().selectedObjects.Count ; i++){
                ObjectInfo newObjectInfo = mainCamera.GetComponent<Select>().selectedObjects[i].GetComponent<ObjectInfo>();
                selectedUnitsUI[i].GetComponent<HealthBar>().SetHealth(newObjectInfo.currentHealth);
                selectedUnitsUI[i].GetComponent<RawImage>().material = newObjectInfo.unitIcon; //Gets the texture from the select unit and shows it in the unitsicon
            }
        }
    }

    public void ActivateUnitUI(){
        if (mainCamera.GetComponent<Select>().selectedObjects.Count == 1){
            unitInfo.SetActive(true);
            GameObject unit1 = mainCamera.GetComponent<Select>().selectedObjects[0];
            ObjectInfo objectInfo = unit1.GetComponent<ObjectInfo>();
            unitIcon.material = objectInfo.unitIcon;
            UpdateText();

            //If groupUI = true: move groupUI to appropiate position
            groupUITransform.anchorMin = new Vector2(0.5f, 1);
            groupUITransform.anchorMax = new Vector2(0.5f, 1);              
            if (mainCamera.GetComponent<Select>().selectedObjects[0].GetComponent<Movement>().standGround == true){
                standGroundButtonUnitInfo.GetComponent<Image>().color = Color.red; //Placeholder Color
                standGroundButtonUnitUnfos.GetComponent<Image>().color = Color.red; //Placeholder Color                
            }
            else{
                standGroundButtonUnitInfo.GetComponent<Image>().color = Color.white; //Placeholder Color
                standGroundButtonUnitUnfos.GetComponent<Image>().color = Color.white; //Placeholder Color                
            }
            Debug.Log("UnitInfo has been activated");

        }
        if (mainCamera.GetComponent<Select>().selectedObjects.Count == 0){
            unitInfo.SetActive(false);
            patrolButtonActive = false;
            patrolButtonUnitInfo.GetComponent<Image>().color = Color.white;
            patrolButtonUnitInfos.GetComponent<Image>().color = Color.white;
            Debug.Log("UnitInfo has been deactivated");

            //If groupUI = true: move groupUI to appropiate position
            groupUITransform.anchorMin = new Vector2(0.5f, 0);
            groupUITransform.anchorMax = new Vector2(0.5f, 0);             
        }
        if (mainCamera.GetComponent<Select>().selectedObjects.Count > 1){
            unitInfo.SetActive(false);
            unitsInfo.SetActive(true);
            Debug.Log("UnitInfo has been deactivated and UnitsInfo has been activated");

            //If groupUI = true: move groupUI to appropiate position
            groupUITransform.anchorMin = new Vector2(0.5f, 1);
            groupUITransform.anchorMax = new Vector2(0.5f, 1); 
        }
        else if (mainCamera.GetComponent<Select>().selectedObjects.Count <= 1){
            unitsInfo.SetActive(false);
            patrolButtonActive = false;
            patrolButtonUnitInfo.GetComponent<Image>().color = Color.white;
            patrolButtonUnitInfos.GetComponent<Image>().color = Color.white;
            Debug.Log("UnitsInfo has been deactivated");
        }
    }

    public void UpdateText(){
        GameObject selectedUnit = mainCamera.GetComponent<Select>().selectedObjects[0]; //Access the first object in the list
        ObjectInfo objectInfo = selectedUnit.GetComponent<ObjectInfo>();
        teamTextUI.text = (objectInfo.team - 10f).ToString();
        cardNameTextUI.text = objectInfo.cardName;
    }

    public void FormationOnClick(){

        if(dynamicFormation == true){
            CircleFormation();
        }
        else if(circleFormation == true){
            RandomFormation();
        }
        else if(randomFormation == true){
            DynamicFormation();
        }
    }

    public void DynamicFormation(){
        formationButtonIcon.material = dynamicFormationMaterial;
        dynamicFormation = true;
        randomFormation = false;
        circleFormation = false;
    }

    public void RandomFormation(){
        formationButtonIcon.material = randomFormationMaterial;
        randomFormation = true;
        dynamicFormation = false;
        circleFormation = false;
    }

    public void CircleFormation(){
        formationButtonIcon.material = circleFormationMaterial;
        circleFormation = true;
        dynamicFormation = false;
        randomFormation = false;
    }

    public void PatrolOnClick(){
        if (patrolButtonActive == false){
            patrolButtonActive = true;
            patrolButtonUnitInfo.GetComponent<Image>().color = Color.red;
            patrolButtonUnitInfos.GetComponent<Image>().color = Color.red;
            for (int i = 0; i < mainCamera.GetComponent<Select>().selectedObjects.Count; i++){
                StartCoroutine(mainCamera.GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().Patrol());
            }          
        }
        else{
            patrolButtonActive = false;
            patrolButtonUnitInfo.GetComponent<Image>().color = Color.white;
            patrolButtonUnitInfos.GetComponent<Image>().color = Color.white;
        }
    }

    public void StandGroundOnClick(){
        if (mainCamera.GetComponent<Select>().selectedObjects[0].GetComponent<Movement>().standGround == false){
            for (int i = 0; i < mainCamera.GetComponent<Select>().selectedObjects.Count; i++){
                mainCamera.GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().standGround = true;               
            }
            standGroundButtonUnitInfo.GetComponent<Image>().color = Color.red; //Placeholder Color
            standGroundButtonUnitUnfos.GetComponent<Image>().color = Color.red; //Placeholder Color
        }
        else{
            for (int i = 0; i < mainCamera.GetComponent<Select>().selectedObjects.Count; i++){
                mainCamera.GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().standGround = false;
                StartCoroutine(mainCamera.GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().Aggro());
            }
            standGroundButtonUnitInfo.GetComponent<Image>().color = Color.white; //Placeholder Color
            standGroundButtonUnitUnfos.GetComponent<Image>().color = Color.white; //Placeholder Color
        }
    }

    //Selects the unit that has been clicked on in the unit UI and deselects the rest.
    public void UnitIconButtonUI(){
        int i = EventSystem.current.currentSelectedGameObject.transform.GetSiblingIndex();
        if (!Input.GetKey(KeyCode.LeftShift)){
            mainCamera.GetComponent<Select>().selectedObject = mainCamera.GetComponent<Select>().selectedObjects[i];
            foreach(ObjectInfo unit in mainCamera.GetComponent<Select>().selectedInfos){
                unit.isSelected = false; //Deselect all the units
            }
            mainCamera.GetComponent<Select>().selectedObjects.Clear();
            mainCamera.GetComponent<Select>().selectedInfos.Clear();
            foreach(GameObject unit in selectedUnitsUI){
                unit.SetActive(false); //Hides all the gameObjects in selectedUnitsUI 
            }
            mainCamera.GetComponent<Select>().selectedObjects.Add(mainCamera.GetComponent<Select>().selectedObject);
            mainCamera.GetComponent<Select>().selectedInfos.Add(mainCamera.GetComponent<Select>().selectedObject.GetComponent<ObjectInfo>());
            selectedUnitsUI[mainCamera.GetComponent<Select>().selectedInfos.Count - 1].SetActive(true);
            foreach(ObjectInfo unit in mainCamera.GetComponent<Select>().selectedInfos){
                unit.isSelected = true; //Selected the chosen Unit
            }
            ActivateUnitUI();    
            Debug.Log("1 Unit has been selected from the Units UI");
        }
        if (Input.GetKey(KeyCode.LeftShift)){
            foreach(GameObject unit in selectedUnitsUI){
                unit.SetActive(false);
            }
            mainCamera.GetComponent<Select>().selectedInfos[i].GetComponent<ObjectInfo>().isSelected = false;
            mainCamera.GetComponent<Select>().selectedObjects.RemoveAt(i);
            mainCamera.GetComponent<Select>().selectedInfos.RemoveAt(i);

            foreach (GameObject unit in mainCamera.GetComponent<Select>().selectedObjects){
                for (int o = 0; o < mainCamera.GetComponent<Select>().selectedInfos.Count; o++){
                    selectedUnitsUI[o].SetActive(true); 
                }
                ActivateUnitUI();  
            }
        }
    }
}
