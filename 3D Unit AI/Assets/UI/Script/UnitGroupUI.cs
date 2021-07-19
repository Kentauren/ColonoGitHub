using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnitGroupUI : MonoBehaviour{

    public GameObject mainCamera;
    public GameObject unitUI;
    public TextMeshProUGUI group1Text;
    public TextMeshProUGUI group2Text;
    public TextMeshProUGUI group3Text;
    public TextMeshProUGUI group4Text;
    public TextMeshProUGUI group5Text;
    public TextMeshProUGUI group6Text;
    public GameObject group1UI;
    public GameObject group2UI;
    public GameObject group3UI;
    public GameObject group4UI;
    public GameObject group5UI;
    public GameObject group6UI;
    public List<GameObject> group1 = new List<GameObject>();
    public List<GameObject> group2 = new List<GameObject>();
    public List<GameObject> group3 = new List<GameObject>();
    public List<GameObject> group4 = new List<GameObject>();
    public List<GameObject> group5 = new List<GameObject>();
    public List<GameObject> group6 = new List<GameObject>();
    public bool groupUIActive;

    void Start(){
        group1UI.SetActive(false);
        group2UI.SetActive(false);
        group3UI.SetActive(false);
        group4UI.SetActive(false);
        group5UI.SetActive(false);
        group6UI.SetActive(false);
    }

    // Update is called once per frame
    void Update(){
        //Group 1   
        if (Input.GetKey(KeyCode.Alpha1)){
            if (Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftAlt)){
                foreach(GameObject unit in mainCamera.GetComponent<Select>().selectedObjects){
                    if (!unit.GetComponent<ObjectInfo>().group.Contains(1)){
                        group1.Add(unit); 
                        unit.GetComponent<ObjectInfo>().group.Add(1);
                    }
                }
                int i = group1.Count;
                group1Text.text = i.ToString();
                if (group1.Count > 0){
                    group1UI.SetActive(true);
                    groupUIActive = true;
                }  
            }
            if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.LeftShift)){
                foreach(GameObject unit in group1){
                    if (unit.GetComponent<ObjectInfo>().group.Contains(1)){
                       unit.GetComponent<ObjectInfo>().group.Remove(1); 
                    }    
                }
                group1.Clear();
                foreach(GameObject unit in mainCamera.GetComponent<Select>().selectedObjects){
                    group1.Add(unit);
                    unit.GetComponent<ObjectInfo>().group.Add(1);
                }
                int i = group1.Count;
                group1Text.text = i.ToString();
                if (group1.Count > 0){
                    group1UI.SetActive(true);
                    groupUIActive = true;
                }
                if (group1.Count == 0){
                    group1UI.SetActive(false);
                    groupUIActive = false;
                }
            } 
            if (!Input.GetKey(KeyCode.LeftShift)){
               Group1Button(); 
            }         
        }
        //Group 2        
        if (Input.GetKey(KeyCode.Alpha2)) {
            if (Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftAlt)){
                foreach(GameObject unit in mainCamera.GetComponent<Select>().selectedObjects){
                    if (!unit.GetComponent<ObjectInfo>().group.Contains(2)){
                        group2.Add(unit); 
                        unit.GetComponent<ObjectInfo>().group.Add(2);
                    }
                }
                int i = group2.Count;
                group2Text.text = i.ToString();
                if (group2.Count > 0){
                    group2UI.SetActive(true);
                    groupUIActive = true;
                }   
            }
            if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.LeftShift)){
                foreach(GameObject unit in group2){
                    if (unit.GetComponent<ObjectInfo>().group.Contains(2)){
                       unit.GetComponent<ObjectInfo>().group.Remove(2); 
                    }    
                }
                group2.Clear();
                foreach(GameObject unit in mainCamera.GetComponent<Select>().selectedObjects){
                    group2.Add(unit);
                    unit.GetComponent<ObjectInfo>().group.Add(2);
                }
                int i = group2.Count;
                group2Text.text = i.ToString();
                if (group2.Count > 0){
                    group2UI.SetActive(true);
                    groupUIActive = true;
                }
                if (group2.Count == 0){
                    group2UI.SetActive(false);
                    groupUIActive = false;
                }
            } 
            if (!Input.GetKey(KeyCode.LeftShift)){
               Group2Button(); 
            }         
        }
        //Group 3
        if (Input.GetKey(KeyCode.Alpha3)){
            if (Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftAlt)){
                foreach(GameObject unit in mainCamera.GetComponent<Select>().selectedObjects){
                    if (!unit.GetComponent<ObjectInfo>().group.Contains(3)){
                        group3.Add(unit); 
                        unit.GetComponent<ObjectInfo>().group.Add(3);
                    }
                }
                int i = group3.Count;
                group3Text.text = i.ToString();
                if (group3.Count > 0){
                    group3UI.SetActive(true);
                    groupUIActive = true;
                }  
            }
            if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.LeftShift)){
                foreach(GameObject unit in group3){
                    if (unit.GetComponent<ObjectInfo>().group.Contains(3)){
                       unit.GetComponent<ObjectInfo>().group.Remove(3); 
                    }    
                }
                group3.Clear();
                foreach(GameObject unit in mainCamera.GetComponent<Select>().selectedObjects){
                    group3.Add(unit);
                    unit.GetComponent<ObjectInfo>().group.Add(3);
                }
                int i = group3.Count;
                group3Text.text = i.ToString();
                if (group3.Count > 0){
                    group3UI.SetActive(true);
                    groupUIActive = true;
                }
                if (group3.Count == 0){
                    group3UI.SetActive(false);
                    groupUIActive = false;
                }
            } 
            if (!Input.GetKey(KeyCode.LeftShift)){
               Group3Button(); 
            }         
        }
        //Group 4
        if (Input.GetKey(KeyCode.Alpha4)) {
            if (Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftAlt)){
                foreach(GameObject unit in mainCamera.GetComponent<Select>().selectedObjects){
                    if (!unit.GetComponent<ObjectInfo>().group.Contains(4)){
                        group4.Add(unit); 
                        unit.GetComponent<ObjectInfo>().group.Add(4);
                    }
                }
                int i = group4.Count;
                group4Text.text = i.ToString();
                if (group4.Count > 0){
                    group4UI.SetActive(true);
                    groupUIActive = true;
                }                  
            }
            if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.LeftShift)){
                foreach(GameObject unit in group4){
                    if (unit.GetComponent<ObjectInfo>().group.Contains(4)){
                       unit.GetComponent<ObjectInfo>().group.Remove(4); 
                    }    
                }
                group4.Clear();
                foreach(GameObject unit in mainCamera.GetComponent<Select>().selectedObjects){
                    group4.Add(unit);
                    unit.GetComponent<ObjectInfo>().group.Add(4);
                }
                int i = group4.Count;
                group4Text.text = i.ToString();
                if (group4.Count > 0){
                    group4UI.SetActive(true);
                    groupUIActive = true;
                }
                if (group4.Count == 0){
                    group4UI.SetActive(false);
                    groupUIActive = false;
                }
            } 
            if (!Input.GetKey(KeyCode.LeftShift)){
               Group4Button(); 
            }         
        }
        //Group 5
        if (Input.GetKey(KeyCode.Alpha5)){
            if (Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftAlt)){
                foreach(GameObject unit in mainCamera.GetComponent<Select>().selectedObjects){
                    if (!unit.GetComponent<ObjectInfo>().group.Contains(5)){
                        group5.Add(unit); 
                        unit.GetComponent<ObjectInfo>().group.Add(5);
                    }
                }
                int i = group5.Count;
                group5Text.text = i.ToString();
                if (group5.Count > 0){
                    group5UI.SetActive(true);
                    groupUIActive = true;
                }  
            }
            if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.LeftShift)){
                foreach(GameObject unit in group5){
                    if (unit.GetComponent<ObjectInfo>().group.Contains(5)){
                       unit.GetComponent<ObjectInfo>().group.Remove(5); 
                    }    
                }
                group5.Clear();
                foreach(GameObject unit in mainCamera.GetComponent<Select>().selectedObjects){
                    group5.Add(unit);
                    unit.GetComponent<ObjectInfo>().group.Add(5);
                }
                int i = group5.Count;
                group5Text.text = i.ToString();
                if (group5.Count > 0){
                    group5UI.SetActive(true);
                    groupUIActive = true;
                }
                if (group5.Count == 0){
                    group5UI.SetActive(false);
                    groupUIActive = false;
                }
            } 
            if (!Input.GetKey(KeyCode.LeftShift)){
               Group5Button(); 
            }         
        }
        //Group 6
        if (Input.GetKey(KeyCode.Alpha6)) {
            if (Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftAlt)){
                foreach(GameObject unit in mainCamera.GetComponent<Select>().selectedObjects){
                    if (!unit.GetComponent<ObjectInfo>().group.Contains(6)){
                        group6.Add(unit); 
                        unit.GetComponent<ObjectInfo>().group.Add(6);
                    }
                }
                int i = group6.Count;
                group6Text.text = i.ToString();
                if (group6.Count > 0){
                    group6UI.SetActive(true);
                    groupUIActive = true;
                }  
            }
            if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.LeftShift)){
                foreach(GameObject unit in group6){
                    if (unit.GetComponent<ObjectInfo>().group.Contains(6)){
                       unit.GetComponent<ObjectInfo>().group.Remove(6); 
                    }    
                }
                group6.Clear();
                foreach(GameObject unit in mainCamera.GetComponent<Select>().selectedObjects){
                    group6.Add(unit);
                    unit.GetComponent<ObjectInfo>().group.Add(6);
                }
                int i = group6.Count;
                group6Text.text = i.ToString();
                if (group6.Count > 0){
                    group6UI.SetActive(true);
                    groupUIActive = true;
                }
                if (group6.Count == 0){
                    group6UI.SetActive(false);
                    groupUIActive = false;
                }
            } 
            if (!Input.GetKey(KeyCode.LeftShift)){
               Group6Button(); 
            }         
        }                        
    }

    public void Group1Button()
    {
        if (group1.Count > 0){
            foreach(ObjectInfo unit in mainCamera.GetComponent<Select>().selectedInfos){
                unit.isSelected = false; 
                Debug.Log("Deselected the other units");
            }
            mainCamera.GetComponent<Select>().selectedObjects.Clear();
            mainCamera.GetComponent<Select>().selectedInfos.Clear();
            foreach (GameObject item in unitUI.GetComponent<UnitUI>().selectedUnitsUI){
                item.SetActive(false); //Hides all the gameObjects in selectedUnitsUI 
            }
            unitUI.GetComponent<UnitUI>().ActivateUnitUI(); //Checks if selectedObjects > 0 and deactivate or activates the correct UnitUI

            foreach (GameObject unit in group1){
                mainCamera.GetComponent<Select>().selectedObjects.Add(unit);
                mainCamera.GetComponent<Select>().selectedInfos.Add(unit.GetComponent<ObjectInfo>());
                unitUI.GetComponent<UnitUI>().selectedUnitsUI[mainCamera.GetComponent<Select>().selectedInfos.Count - 1].SetActive(true); //Sets the amount of selected units to active in the UnitsUI
            }
            foreach (ObjectInfo unit in mainCamera.GetComponent<Select>().selectedInfos){
                unit.isSelected = true;
            }
            unitUI.GetComponent<UnitUI>().ActivateUnitUI(); //Checks if selectedObjects > 0 and deactivate or activates the correct UnitUI
            Debug.Log("The units in group 1 has been selected");
        }   
    }

    public void Group2Button(){
        if (group2.Count > 0){
            foreach(ObjectInfo unit in mainCamera.GetComponent<Select>().selectedInfos){
                unit.isSelected = false; 
                Debug.Log("Deselected the other units");
            }
            mainCamera.GetComponent<Select>().selectedObjects.Clear();
            mainCamera.GetComponent<Select>().selectedInfos.Clear();
            foreach (GameObject item in unitUI.GetComponent<UnitUI>().selectedUnitsUI){
                item.SetActive(false); //Hides all the gameObjects in selectedUnitsUI 
            }
            unitUI.GetComponent<UnitUI>().ActivateUnitUI(); //Checks if selectedObjects > 0 and deactivate or activates the correct UnitUI

            foreach (GameObject unit in group2){
                mainCamera.GetComponent<Select>().selectedObjects.Add(unit);
                mainCamera.GetComponent<Select>().selectedInfos.Add(unit.GetComponent<ObjectInfo>());
                unitUI.GetComponent<UnitUI>().selectedUnitsUI[mainCamera.GetComponent<Select>().selectedInfos.Count - 1].SetActive(true); //Sets the amount of selected units to active in the UnitsUI
            }
            foreach (ObjectInfo unit in mainCamera.GetComponent<Select>().selectedInfos){
                unit.isSelected = true;
            }
            unitUI.GetComponent<UnitUI>().ActivateUnitUI(); //Checks if selectedObjects > 0 and deactivate or activates the correct UnitUI
            Debug.Log("The units in group 1 has been selected");
        }
        
    }

    public void Group3Button(){
        if (group3.Count > 0){
            foreach(ObjectInfo unit in mainCamera.GetComponent<Select>().selectedInfos){
                unit.isSelected = false; 
                Debug.Log("Deselected the other units");
            }
            mainCamera.GetComponent<Select>().selectedObjects.Clear();
            mainCamera.GetComponent<Select>().selectedInfos.Clear();
            foreach (GameObject item in unitUI.GetComponent<UnitUI>().selectedUnitsUI){
                item.SetActive(false); //Hides all the gameObjects in selectedUnitsUI 
            }
            unitUI.GetComponent<UnitUI>().ActivateUnitUI(); //Checks if selectedObjects > 0 and deactivate or activates the correct UnitUI

            foreach (GameObject unit in group3){
                mainCamera.GetComponent<Select>().selectedObjects.Add(unit);
                mainCamera.GetComponent<Select>().selectedInfos.Add(unit.GetComponent<ObjectInfo>());
                unitUI.GetComponent<UnitUI>().selectedUnitsUI[mainCamera.GetComponent<Select>().selectedInfos.Count - 1].SetActive(true); //Sets the amount of selected units to active in the UnitsUI
            }
            foreach (ObjectInfo unit in mainCamera.GetComponent<Select>().selectedInfos){
                unit.isSelected = true;
            }
            unitUI.GetComponent<UnitUI>().ActivateUnitUI(); //Checks if selectedObjects > 0 and deactivate or activates the correct UnitUI
            Debug.Log("The units in group 1 has been selected");
        }        
    }

    public void Group4Button(){
        if (group4.Count > 0){
            foreach(ObjectInfo unit in mainCamera.GetComponent<Select>().selectedInfos){
                unit.isSelected = false; 
                Debug.Log("Deselected the other units");
            }
            mainCamera.GetComponent<Select>().selectedObjects.Clear();
            mainCamera.GetComponent<Select>().selectedInfos.Clear();
            foreach (GameObject item in unitUI.GetComponent<UnitUI>().selectedUnitsUI){
                item.SetActive(false); //Hides all the gameObjects in selectedUnitsUI 
            }
            unitUI.GetComponent<UnitUI>().ActivateUnitUI(); //Checks if selectedObjects > 0 and deactivate or activates the correct UnitUI

            foreach (GameObject unit in group4){
                mainCamera.GetComponent<Select>().selectedObjects.Add(unit);
                mainCamera.GetComponent<Select>().selectedInfos.Add(unit.GetComponent<ObjectInfo>());
                unitUI.GetComponent<UnitUI>().selectedUnitsUI[mainCamera.GetComponent<Select>().selectedInfos.Count - 1].SetActive(true); //Sets the amount of selected units to active in the UnitsUI
            }
            foreach (ObjectInfo unit in mainCamera.GetComponent<Select>().selectedInfos){
                unit.isSelected = true;
            }
            unitUI.GetComponent<UnitUI>().ActivateUnitUI(); //Checks if selectedObjects > 0 and deactivate or activates the correct UnitUI
            Debug.Log("The units in group 1 has been selected");
        }        
    }

    public void Group5Button(){
        if (group5.Count > 0)
        {
            foreach(ObjectInfo unit in mainCamera.GetComponent<Select>().selectedInfos){
                unit.isSelected = false; 
                Debug.Log("Deselected the other units");
            }
            mainCamera.GetComponent<Select>().selectedObjects.Clear();
            mainCamera.GetComponent<Select>().selectedInfos.Clear();
            foreach (GameObject item in unitUI.GetComponent<UnitUI>().selectedUnitsUI){
                item.SetActive(false); //Hides all the gameObjects in selectedUnitsUI 
            }
            unitUI.GetComponent<UnitUI>().ActivateUnitUI(); //Checks if selectedObjects > 0 and deactivate or activates the correct UnitUI

            foreach (GameObject unit in group5){
                mainCamera.GetComponent<Select>().selectedObjects.Add(unit);
                mainCamera.GetComponent<Select>().selectedInfos.Add(unit.GetComponent<ObjectInfo>());
                unitUI.GetComponent<UnitUI>().selectedUnitsUI[mainCamera.GetComponent<Select>().selectedInfos.Count - 1].SetActive(true); //Sets the amount of selected units to active in the UnitsUI
            }
            foreach (ObjectInfo unit in mainCamera.GetComponent<Select>().selectedInfos){
                unit.isSelected = true;
            }
            unitUI.GetComponent<UnitUI>().ActivateUnitUI(); //Checks if selectedObjects > 0 and deactivate or activates the correct UnitUI
            Debug.Log("The units in group 1 has been selected");
        }        
    }

    public void Group6Button(){
        if (group6.Count > 0){
            foreach(ObjectInfo unit in mainCamera.GetComponent<Select>().selectedInfos){
                unit.isSelected = false; 
                Debug.Log("Deselected the other units");
            }
            mainCamera.GetComponent<Select>().selectedObjects.Clear();
            mainCamera.GetComponent<Select>().selectedInfos.Clear();
            foreach (GameObject item in unitUI.GetComponent<UnitUI>().selectedUnitsUI){
                item.SetActive(false); //Hides all the gameObjects in selectedUnitsUI 
            }
            unitUI.GetComponent<UnitUI>().ActivateUnitUI(); //Checks if selectedObjects > 0 and deactivate or activates the correct UnitUI

            foreach (GameObject unit in group6){
                mainCamera.GetComponent<Select>().selectedObjects.Add(unit);
                mainCamera.GetComponent<Select>().selectedInfos.Add(unit.GetComponent<ObjectInfo>());
                unitUI.GetComponent<UnitUI>().selectedUnitsUI[mainCamera.GetComponent<Select>().selectedInfos.Count - 1].SetActive(true); //Sets the amount of selected units to active in the UnitsUI
            }
            foreach (ObjectInfo unit in mainCamera.GetComponent<Select>().selectedInfos){
                unit.isSelected = true;
            }
            unitUI.GetComponent<UnitUI>().ActivateUnitUI(); //Checks if selectedObjects > 0 and deactivate or activates the correct UnitUI
            Debug.Log("The units in group 1 has been selected");
        }        
    }          
}
