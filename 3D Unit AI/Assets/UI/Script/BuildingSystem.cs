using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuildingSystem : MonoBehaviour
{
    public InventorySystem inventorySystem;
    public UnitSpawnSystem unitSpawnSystem;
    public RectTransform buildingSystem;
    public RectTransform selectedBuildingRect;
    public GameObject selectedBuildingObject;
    public GameObject spawnBuilding;
    public List<GameObject> currentCollisionsList = new List<GameObject>();
    public bool buildingSystemIsActive;
    public bool buildingReadyToSpawn;

    void Start(){
        buildingSystemIsActive = false;
        buildingReadyToSpawn = false;
        buildingSystem.gameObject.SetActive(false);
    }

    void Update(){    
        if(selectedBuildingRect != null){
            if(buildingReadyToSpawn == false){
                buildingReadyToSpawn = true;
                var eulerAngleX = selectedBuildingObject.GetComponent<Building>().buildingInfo.eulerAnglex;
                var eulerAngleY = selectedBuildingObject.GetComponent<Building>().buildingInfo.eulerAngley;
                var eulerAngleZ = selectedBuildingObject.GetComponent<Building>().buildingInfo.eulerAnglez;                
                spawnBuilding = Instantiate(selectedBuildingObject, transform.position, Quaternion.identity);      
                spawnBuilding.transform.Rotate(eulerAngleX, eulerAngleY, eulerAngleZ);
                spawnBuilding.AddComponent<TempoaryCollisionChecker>();
                spawnBuilding.AddComponent<Rigidbody>();
                spawnBuilding.GetComponent<Rigidbody>().isKinematic = true;
                spawnBuilding.GetComponent<NavMeshObstacle>().enabled = false;
                spawnBuilding.layer = 2;
            }
            else{
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100)){
                    if(hit.collider.tag == "Ground"){
                        spawnBuilding.transform.position = hit.point;
                    }
                }
            }
            
            if(Input.GetKey(KeyCode.LeftControl)){
                if(Input.GetKey(KeyCode.Q)){
                    spawnBuilding.transform.Rotate(Vector3.down * Time.deltaTime * 150);
                }

                if(Input.GetKey(KeyCode.E)){
                    spawnBuilding.transform.Rotate(Vector3.up * Time.deltaTime * 150);
                }
            }

            //Instantiates the building when left click is pressed
            if(Input.GetMouseButtonDown(0)){
                if(currentCollisionsList.Count == 0){
                    GameObject newBuilding = Instantiate(spawnBuilding, spawnBuilding.transform.position, spawnBuilding.transform.rotation);
                    newBuilding.layer = selectedBuildingObject.layer;
                    newBuilding.name = newBuilding.GetComponent<Building>().buildingInfo.buildingName;
                    Destroy(newBuilding.GetComponent<TempoaryCollisionChecker>());
                    Destroy(newBuilding.GetComponent<Rigidbody>());
                    newBuilding.GetComponent<NavMeshObstacle>().enabled = true;
                    currentCollisionsList.Clear();
                    Debug.Log("Nothing is interfering with the spawn placement. " + newBuilding.name + " Has been spawned.");                    
                }
                else{
                    for(int i = 0; i < currentCollisionsList.Count; i++){
                        Debug.Log(currentCollisionsList[i].name + " is interfering with the placement");
                    }     
                }
            }

            if(Input.GetMouseButtonUp(1)){
                DeactivateBuildingPlacement();
            }
        }
    }

    public void ActivateBuildingSystemOnClick(){
        if(buildingSystemIsActive == true){
            if(selectedBuildingRect != null){
                DeactivateBuildingPlacement();
            }           
            buildingSystem.gameObject.SetActive(false);
            buildingSystemIsActive = false;
        }
        else{
            buildingSystem.gameObject.SetActive(true);
            buildingSystemIsActive = true;
            if(inventorySystem.invenSysIsActive == true){
                inventorySystem.ActivateInventorySystem();
            }
            if(unitSpawnSystem.unitSpawnSystemIsActive == true){
                unitSpawnSystem.ActivateUnitSpawnSystemOnClick();
            }
        }
    }

    void DeactivateBuildingPlacement(){
        Destroy(spawnBuilding);
        selectedBuildingRect = null;
        buildingReadyToSpawn = false;
        currentCollisionsList.Clear();        
    }
}
