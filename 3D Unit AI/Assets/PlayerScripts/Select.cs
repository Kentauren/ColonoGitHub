using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Select : MonoBehaviour{

    public GameObject selectedObject;
    public ObjectInfo selectedInfo;
    public UnitUI unitUI;
    public RTS_Camera rTS_Camera; 
    public List<ObjectInfo> selectedInfos = new List<ObjectInfo>();
    public List<GameObject> selectedObjects = new List<GameObject>();
    public List<GameObject> selectables = new List<GameObject>();
    public List<Transform> selectedTransforms = new List<Transform>();
    public RectTransform selectionBox;
    public bool selectionBoxIsActive;
    public Vector3 startPos;
    private Vector3 endPos;
    private Camera cam;
    Color32 defaultColorIcon = new Color32(255, 255, 255, 255);

    void Start(){
        selectionBoxIsActive = true;
    }

    void Awake(){
        cam = Camera.main;
    }

    public void Update(){
        if(Input.GetMouseButtonDown(0)){
            startPos = Input.mousePosition;
            if(!EventSystem.current.IsPointerOverGameObject()){
                LeftClick();
            }
        }
        if(Input.GetMouseButtonUp(0) && selectionBoxIsActive == true){
            ReleaseSelectionBox();
        }
        if(Input.GetMouseButton(0) && selectionBoxIsActive == true){
            UpdateSelectionBox(Input.mousePosition);                
        }
    }

    public void LeftClick(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100)){
            if(hit.collider.tag == "Ground" && !Input.GetKey(KeyCode.LeftShift)){
                Debug.Log("Deselected");
                foreach (ObjectInfo item in selectedInfos){
                    item.isSelected = false;
                }
                selectedObjects.Clear();
                selectedInfos.Clear();
                selectedTransforms.Clear(); 
                foreach (GameObject item in unitUI.GetComponent<UnitUI>().selectedUnitsUI){
                    item.SetActive(false); //Hides all the gameObjects in selectedUnitsUI 
                }
                
                unitUI.GetComponent<UnitUI>().ActivateUnitUI(); //Checks if selectedObjects > 0 and deactivate or activates the correct UnitUI
                Debug.Log("All objects are deselected");
            }
            if(hit.collider.tag == "Selectable"){
                if(Input.GetKey(KeyCode.LeftShift)){
                    selectedObject = hit.collider.gameObject;
                    //Checks if selectedobject is already selected and if the maximum amount of units selected are below x
                    if (selectedObject.GetComponent<ObjectInfo>().isSelected == false && selectedObjects.Count < 29){
                        selectedObjects.Add(hit.collider.gameObject);
                        selectedTransforms.Add(selectedObject.GetComponent<Transform>());
                        selectedInfos.Add(selectedObject.GetComponent<ObjectInfo>());
                        foreach (ObjectInfo item in selectedInfos){
                            item.isSelected = true;
                        }
                        unitUI.GetComponent<UnitUI>().selectedUnitsUI[selectedInfos.Count - 1].SetActive(true); //Sets the amount of selected units to active in the UnitsUI
                        unitUI.GetComponent<UnitUI>().ActivateUnitUI(); //Checks if selectedObjects > 0 and deactivate or activates the correct UnitUI
                        Debug.Log("Selected with shift");
                    }
                    else{
                        Debug.Log("Maximum of units are selected (50)");
                    }
                }
                else {
                    foreach(ObjectInfo item in selectedInfos){
                        item.isSelected = false; 
                        Debug.Log("Deselected the other units");
                    }
                    selectedObjects.Clear();
                    selectedInfos.Clear();
                    selectedTransforms.Clear(); 
                    foreach (GameObject item in unitUI.GetComponent<UnitUI>().selectedUnitsUI){
                        item.SetActive(false); //Hides all the gameObjects in selectedUnitsUI 
                    }
                    unitUI.GetComponent<UnitUI>().ActivateUnitUI(); //Checks if selectedObjects > 0 and deactivate or activates the correct UnitUI

                    selectedObject = hit.collider.gameObject;
                    selectedObjects.Add(hit.collider.gameObject);
                    selectedTransforms.Add(selectedObject.GetComponent<Transform>());
                    selectedInfos.Add(selectedObject.GetComponent<ObjectInfo>());
                    foreach (ObjectInfo item in selectedInfos){
                        item.isSelected = true;
                    }
                    unitUI.GetComponent<UnitUI>().selectedUnitsUI[selectedInfos.Count - 1].SetActive(true); //Sets the amount of selected units to active in the UnitsUI
                    unitUI.GetComponent<UnitUI>().ActivateUnitUI(); //Checks if selectedObjects > 0 and deactivate or activates the correct UnitUI
                    Debug.Log("Selected");
                }
            }
            else{
                Debug.Log("You're not clicking on anything...");
            } 
        }
    }

    void UpdateSelectionBox(Vector2 curMousePos){
        selectionBox.gameObject.SetActive(true);
        float width = curMousePos.x - startPos.x;
        float height = curMousePos.y - startPos.y;
        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        selectionBox.anchoredPosition = startPos + new Vector3(width / 2, height / 2);
    }
    void ReleaseSelectionBox(){
        selectionBox.gameObject.SetActive(false);
        Vector2 min = selectionBox.anchoredPosition - (selectionBox.sizeDelta / 2);
        Vector2 max = selectionBox.anchoredPosition + (selectionBox.sizeDelta / 2);
        foreach (GameObject unit in selectables){
            Vector3 screenPos = cam.WorldToScreenPoint(unit.transform.position);
            if(screenPos.x > min.x && screenPos.x < max.x && screenPos.y > min.y && screenPos.y < max.y){
                //Checks if selectedobject is already selected and if the maximum amount of units selected are below x
                if(unit.GetComponent<ObjectInfo>().isSelected == false && selectedObjects.Count < 30){
                    selectedObjects.Add(unit);
                    selectedTransforms.Add(unit.GetComponent<Transform>());
                    selectedInfos.Add(unit.GetComponent<ObjectInfo>());
                        foreach (ObjectInfo item in selectedInfos){
                            item.isSelected = true;
                        }
                    unitUI.GetComponent<UnitUI>().selectedUnitsUI[selectedInfos.Count - 1].SetActive(true); //Sets the amount of selected units to active in the UnitsUI
                    unitUI.GetComponent<UnitUI>().selectedUnitsUI[selectedInfos.Count - 1].GetComponent<Image>().color = defaultColorIcon;
                    unitUI.GetComponent<UnitUI>().ActivateUnitUI(); //Checks if selectedObjects > 0 and deactivate or activates the correct UnitUI
                    Debug.Log("You have selected multiply units");
                }
                else{
                    Debug.Log("Maximum of units are selected (50)");
                }
            }
        }
    }
}