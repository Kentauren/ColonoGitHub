using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ObjectInfo : MonoBehaviour
{
    public GameObject mainCamera;
    public Canvas canvas;
    public GameObject smallBloodPool;
    public GameObject mediumBloodPool;
    public GameObject largeBloodPool;
    public GameObject selectionCircle;
    public GameObject card; 
    public GameObject head;
    public GameObject body;
    public GameObject clothing;
    public GameObject rightHand;
    public GameObject leftHand;
    public GameObject back;
    public Transform headTransform;
    public Transform bodyTransform;
    public Transform rightHandTransform;
    public Transform leftHandTransform;
    public Transform backTransform;    
    public Material unitIcon;
    public UnitUI unitUI;
    public UnitGroupUI unitGroupUI;
    public bool isSelected = false;
    public bool enclosedAttackMode = false;
    public string objectName;
    public string cardName;
    public float maxHealth = 100;
    public float maxArmor;
    public float currentHealth;
    public float currentArmor;
    public float maxWeight;
    public float team;
    public int weaponSkill;
    public int shieldBlock;
    public int movementSpeed;
    public List<int> group = new List<int>();
    public List<GameObject> attackers = new List<GameObject>();
    public List<GameObject> equipmentList = new List<GameObject>();
    Color32 deadColorIcon = new Color32(255, 108, 108, 255);

    void Start(){
        currentArmor = maxArmor;
        currentHealth = maxHealth;
        int intTeam = (int)team;
        gameObject.layer = intTeam;
        UpdateEquipmentList();
    }

    // Update is called once per frame
    void Update(){
        if (isSelected == false){
            selectionCircle.SetActive(false);
        }
        if (isSelected == true){
            selectionCircle.SetActive(true);
        }
    }

    public void UpdateEquipmentList(){
        if(rightHandTransform.childCount > 0){
            equipmentList.Add(rightHandTransform.GetChild(0).gameObject);
        }
        if(leftHandTransform.childCount > 0){
            equipmentList.Add(leftHandTransform.GetChild(0).gameObject);
        }
        if(backTransform.childCount > 0){
            equipmentList.Add(backTransform.GetChild(0).gameObject);
        }
    }
    
    public void TakeDamage(float damage, GameObject attacker){
        
        if(currentArmor > 0){
            Debug.Log("Damage: " + damage);
            float totalDamage = damage / currentArmor;
            Debug.Log("DamageTaken" + totalDamage);
            currentHealth -= totalDamage;
        }
        if(currentArmor <= 0){
            currentHealth -= damage;
        }
        
        int layerMask = 1 << 8;
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, layerMask)){
            Vector3 newHit = hit.point;
            newHit.y = .01f;
            Debug.Log("Checks for blood");
            Debug.Log(hit.collider.tag);            
            if(hit.collider.tag != "SmallBloodPool" && hit.collider.tag != "MediumBloodPool" && hit.collider.tag != "LargeBloodPool"){
                if (damage <= 5){
                    Instantiate(smallBloodPool, newHit, Quaternion.identity);
                }
                if (damage > 5 && 10 >= damage){
                    Instantiate(mediumBloodPool, newHit, Quaternion.identity);
                }
                if (damage > 10){
                    Instantiate(largeBloodPool, newHit, Quaternion.identity);
                }                        
            }
            if(hit.collider.tag == "SmallBloodPool"){
                Destroy(hit.collider.gameObject);
                if (damage <= 10){
                    Instantiate(mediumBloodPool, newHit, Quaternion.identity);
                }
                if (damage > 10){
                    Instantiate(largeBloodPool, newHit, Quaternion.identity);
                }
            }
            if(hit.collider.tag == "MediumBloodPool"){
                Destroy(hit.collider.gameObject);
                Instantiate(largeBloodPool, newHit, Quaternion.identity);
            }
        }
        if(currentHealth <= 0){
            if(isSelected == true){
                if(mainCamera.GetComponent<Select>().selectedObjects.Count == 1){
                    unitUI.unitInfo.SetActive(false);
                }
                else if(mainCamera.GetComponent<Select>().selectedObjects.Count > 1){
                    int i = mainCamera.GetComponent<Select>().selectedObjects.IndexOf(gameObject);
                    Debug.Log(i);
                    canvas.GetComponent<UnitUI>().selectedUnitsUI[i].GetComponent<RawImage>().color = deadColorIcon;
                }
            }
            Destroy(GetComponent<Movement>().cloneEndPoint);
            mainCamera.GetComponent<Select>().selectedObjects.Remove(gameObject);
            mainCamera.GetComponent<Select>().selectedInfos.Remove(this);
            mainCamera.GetComponent<Select>().selectables.Remove(gameObject);

            //Removes the object from the group if it is added to any
            unitGroupUI.group1.Remove(gameObject);  
            unitGroupUI.group1Text.text = unitGroupUI.group1.Count.ToString();
            unitGroupUI.group2.Remove(gameObject);
            unitGroupUI.group2Text.text = unitGroupUI.group2.Count.ToString();
            unitGroupUI.group3.Remove(gameObject);
            unitGroupUI.group3Text.text = unitGroupUI.group3.Count.ToString();
            unitGroupUI.group4.Remove(gameObject);
            unitGroupUI.group4Text.text = unitGroupUI.group4.Count.ToString();
            unitGroupUI.group5.Remove(gameObject);
            unitGroupUI.group5Text.text = unitGroupUI.group5.Count.ToString();
            unitGroupUI.group6.Remove(gameObject);
            unitGroupUI.group6Text.text = unitGroupUI.group6.Count.ToString();

            Destroy(gameObject);
        }
    }
}
