using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeInfo : MonoBehaviour{
    private Quaternion targetRotation;
    public GameObject treeStump;
    public GameObject treeLog;  
    public int amountOfLogs;
    public int objectMaxHealth;
    public int objectCurrentHealth;
    public bool treeIsOccupied;
    public bool treeHasFallen;
    public bool treeIsFallingNow;
    private float rotX;
    private float rotY;
    private float rotZ;

    void Start(){
        objectCurrentHealth = objectMaxHealth;
        treeHasFallen = false;
        rotX = transform.rotation.x;
        rotY = transform.rotation.y;
        rotZ = transform.rotation.z;
    }

    public void TakeDamage(int damage, GameObject unit){
        objectCurrentHealth -= damage;
        
        if(objectCurrentHealth < 100 && treeHasFallen == false){
            treeHasFallen = true;
            GameObject cloneTreeStump = Instantiate(treeStump, transform.position, transform.rotation);
            cloneTreeStump.transform.eulerAngles = new Vector3 (-90, 0, 0);
            StartCoroutine(TreeFall(unit));
        }

        if(objectCurrentHealth < 0){
            for(int i = 0; i < amountOfLogs; i++){
                GameObject newWoodenLog = Instantiate(treeLog, new Vector3(transform.position.x, transform.position.y, transform.position.z + i), treeLog.transform.rotation);
                newWoodenLog.name = "WoodenLog";
            }
            Destroy(gameObject);
        }
    }

    IEnumerator TreeFall(GameObject unit){
        treeIsFallingNow = true;
        float rotationSpeed = 50f;
        Debug.Log("Tree is falling now");

        Vector3 target = Vector3.MoveTowards(transform.position, unit.transform.position, -1);

        targetRotation = Quaternion.LookRotation(target - transform.position, Vector3.forward);
        float dur = Quaternion.Angle(transform.rotation, targetRotation) / rotationSpeed;
        Quaternion start = transform.rotation;
        float t = 0f;

        while(t < dur){
            yield return null;
            t += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(start, targetRotation, t / dur);
        }
        transform.rotation = targetRotation;
        treeIsFallingNow = false; 
    }
}
