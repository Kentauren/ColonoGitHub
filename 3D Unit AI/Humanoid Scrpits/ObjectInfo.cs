using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectInfo : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject smallBloodPool;
    public GameObject mediumBloodPool;
    public GameObject largeBloodPool;
    public GameObject selectionCircle;
    public Texture unitIcon;
    public UnitUI unitUI;
    public UnitGroupUI unitGroupUI;
    public bool isSelected = false;
    public string objectName;
    public int maxHealth = 100;
    public int currentHealth;
    public float team;
    public List<int> group = new List<int>();

    void Start(){
        currentHealth = maxHealth;
        int intTeam = (int)team;
        gameObject.layer = intTeam;
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
    
    public void TakeDamage(int damage, GameObject attacker){
        currentHealth -= damage;
        int layerMask = 1 << 8;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, layerMask)){
            Vector3 newHit = hit.point;
            newHit.y = .05f;            
            if (hit.collider.tag != "SmallBloodPool" && hit.collider.tag != "MediumBloodPool" && hit.collider.tag != "LargeBloodPool"){
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
            if (hit.collider.tag == "SmallBloodPool"){
                Destroy(hit.collider.gameObject);
                if (damage <= 10){
                    Instantiate(mediumBloodPool, newHit, Quaternion.identity);
                }
                if (damage > 10){
                    Instantiate(largeBloodPool, newHit, Quaternion.identity);
                }
            }
            if (hit.collider.tag == "MediumBloodPool"){
                Destroy(hit.collider.gameObject);
                Instantiate(largeBloodPool, newHit, Quaternion.identity);
            }
        }
        if (gameObject.GetComponent<Movement>().standGround == true && gameObject.GetComponent<Movement>().standGroundDefend == false){
            StartCoroutine(gameObject.GetComponent<Movement>().StandGroundDefend(attacker));
            Debug.Log("Unit is defending itself");
        }
        if (currentHealth <= 0){
            if (isSelected == true){
                if (mainCamera.GetComponent<Select>().selectedObjects.Count == 1){
                    unitUI.unitInfo.SetActive(false);
                }
            }
            Destroy(GetComponent<Movement>().cloneEndPoint);
            mainCamera.GetComponent<Select>().selectedObjects.Remove(gameObject);
            mainCamera.GetComponent<Select>().selectedInfos.Remove(this);
            mainCamera.GetComponent<Select>().selectables.Remove(gameObject);
            unitGroupUI.group1.Remove(gameObject);  //Removes the object from the group if it is added to any
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
