using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour{

    public Select select;
    public UnitUI unitUI;
    public GameObject endPoint;
    public float wanderRadius;
    public GameObject leader;
    public GameObject leaderClone;
    public Vector3 leaderCloneDirection;
    public GameObject lookAtTarget;
    public GameObject cloneLookAtTarget;
    public List<Transform> formationList = new List<Transform>(); 

    // Update is called once per frame
    void Update(){
        if (Input.GetMouseButtonDown(1)){
            if (GetComponent<Select>().selectedObjects.Count > 1 && !Input.GetKey(KeyCode.LeftControl)){
                RightClick();
            }
            if (GetComponent<Select>().selectedObjects.Count > 1 && Input.GetKey(KeyCode.LeftControl) && unitUI.squareFormation == true){
                FaceDirection();
            }           
        }
    }

    public void RightClick(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100)){
            if (hit.collider.tag == "Ground"){
                if (GetComponent<Select>().selectedObjects.Count > 1){   
                    foreach (GameObject unit in GetComponent<Select>().selectedObjects){
                        Destroy(unit.GetComponent<Movement>().cloneEndPoint);
                    }
                    //if PatrolActive = true, make false and reset endPoints
                    if(!Input.GetKey(KeyCode.LeftShift)){
                        for (int i = 0; i < select.selectedObjects.Count; i++){
                            for (int j = 0; j < select.selectedObjects[i].GetComponent<Movement>().cloneEndPoints.Count; j++){
                                Destroy(select.selectedObjects[i].GetComponent<Movement>().cloneEndPoints[j].gameObject);  
                            }
                            select.selectedObjects[i].GetComponent<Movement>().patrolActive = false;
                            select.selectedObjects[i].GetComponent<Movement>().cloneEndPoints.Clear();
                        }
                        Debug.Log("More units are selected and leaving patrol. Deleting endPoints");
                    }
                    //RandomFormation
                    if (unitUI.unorganizedFormation == true){
                        wanderRadius = GetComponent<Select>().selectedObjects.Count / 4 + 2;
                        //If endpoints are added to a list
                        if (Input.GetKey(KeyCode.LeftShift)) {
                            for (int i = 0; i < GetComponent<Select>().selectedObjects.Count; i++){
                                Vector3 newPos = RandomNavSphere(hit.point, wanderRadius, 1);
                                GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().Animator();
                                newPos.y += .1f; //sets the EndPoint slightly above the ground
                                GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().cloneEndPoints.Add(Instantiate(endPoint, newPos, Quaternion.identity)); 
                                GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().NextEndPoint();          
                            }
                        }
                        else{
                            foreach (GameObject unit in GetComponent<Select>().selectedObjects){   
                                Vector3 newPos = RandomNavSphere(hit.point, wanderRadius, 1);
                                unit.GetComponent<Movement>().agent.SetDestination(newPos);
                                unit.GetComponent<Movement>().task = TaskList.Moving;
                                unit.GetComponent<Movement>().Animator();
                                newPos.y += 0.1f; //sets the EndPoint slightly above the ground
                                unit.GetComponent<Movement>().cloneEndPoint = Instantiate(endPoint, newPos, Quaternion.identity); //Creates EndPoint for unit destination                        
                            }                            
                        }                        
                        Debug.Log("Multiply units has been selected - Formation instantiated");                        
                    }
                    //SquareFormation         
                    if (unitUI.squareFormation == true){      
                        float center = GetComponent<Select>().selectedObjects.Count / 2;
                        GameObject target = GetComponent<Select>().selectedObjects[0];
                        leaderClone = Instantiate(leader, hit.point, Quaternion.identity);
                        leaderClone.transform.LookAt(target.transform);

                        leaderCloneDirection = leaderClone.transform.position;
                        foreach (Transform child in leaderClone.transform) {
                            if (child.CompareTag("FormationPoint"))
                            {
                                formationList.Add(child.transform);
                            }
                        }
                        //If endpoints are added to a list
                        if (Input.GetKey(KeyCode.LeftShift)){
                            for (int i = 0; i < GetComponent<Select>().selectedObjects.Count; i++){
                                Vector3 newPos = formationList[i].transform.position;
                                GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().Animator();
                                newPos.y += .1f; //sets the EndPoint slightly above the ground
                                GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().cloneEndPoints.Add(Instantiate(endPoint, newPos, Quaternion.identity)); 
                                GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().NextEndPoint();          
                            }
                        }
                        else{
                            for (int i = 0; i < GetComponent<Select>().selectedObjects.Count; i++){
                                Vector3 newPos = formationList[i].transform.position;
                                GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().agent.SetDestination(newPos);
                                GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().task = TaskList.Moving;
                                GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().Animator();
                                newPos.y += 0.1f; //sets the EndPoint slightly above the ground
                                GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().cloneEndPoint = Instantiate(endPoint, newPos, Quaternion.identity); //Creates EndPoint for unit destination
                            }
                        }
                        Destroy(leaderClone.gameObject);
                        formationList.Clear();
                        Debug.Log("Square formation instantiated");
                    }
                    //CircleFormation
                    if (unitUI.circleFormation == true){   
                        float radius = GetComponent<Select>().selectedObjects.Count / 4 + 0.5f;
                        float angleIncrement = 360 / GetComponent<Select>().selectedObjects.Count;
                        //If endpoints are added to a list
                        if (Input.GetKey(KeyCode.LeftShift)) {
                            for (int i = 0; i < GetComponent<Select>().selectedObjects.Count; i++){
                                Vector3 newPos = FormationCircle(hit.point, radius, i, angleIncrement);
                                GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().Animator();
                                newPos.y += .1f; //sets the EndPoint slightly above the ground
                                GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().cloneEndPoints.Add(Instantiate(endPoint, newPos, Quaternion.identity)); 
                                GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().NextEndPoint();          
                            }
                        }
                        else{
                            for (int i = 0; i < GetComponent<Select>().selectedObjects.Count; i++){
                                Vector3 newPos = FormationCircle(hit.point, radius, i, angleIncrement);
                                GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().agent.SetDestination(newPos);
                                GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().task = TaskList.Moving;
                                GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().Animator();
                                newPos.y += 0.1f; //sets the EndPoint slightly above the ground
                                GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().cloneEndPoint = Instantiate(endPoint, newPos, Quaternion.identity); //Creates EndPoint for unit destination
                            } 
                        }                        
                    }
                }
            }
        }                
    }

    //Unorganized Unit formation
    public Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask){   
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        UnityEngine.AI.NavMeshHit navHit;
        UnityEngine.AI.NavMesh.SamplePosition (randDirection, out navHit, dist +1f, layermask);
        
        if (!GameObject.Find("EndPoint(Clone)")){
            Debug.Log("First Endpoint is found");
            return navHit.position;
        }
        GameObject[] endPoints;
        endPoints = GameObject.FindGameObjectsWithTag("EndPoint");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = navHit.position;

        foreach (GameObject go in endPoints){
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance){
                closest = go;
                distance = curDistance;
            }
        }
            print(closest);
            print(distance);

        if (Vector3.Distance(closest.transform.position, navHit.position) > 1f){
            Debug.Log("None of these EndPoint are too close to eachother");
            return navHit.position;
        }
     
        return RandomNavSphere(origin, dist, layermask);  
    }

    Vector3 FormationCircle(Vector3 center, float radius, int index, float angleIncrement){
        float ang = index * angleIncrement;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.y = center.y;
        return pos;
    }
    
    void FaceDirection(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100)){
            //Destroys all related endpoints before instantiating new ones
            foreach (GameObject unit in GetComponent<Select>().selectedObjects){
                Destroy(unit.GetComponent<Movement>().cloneEndPoint);
            }
            float center = GetComponent<Select>().selectedObjects.Count / 2;
            GameObject target = GetComponent<Select>().selectedObjects[0];
            leaderClone = Instantiate(leader, target.transform.position, Quaternion.identity);
            leaderClone.transform.LookAt(hit.point);
            foreach (Transform child in leaderClone.transform) {
                if (child.CompareTag("FormationPoint")){
                    formationList.Add(child.transform);
                }
            }
            for (int i = 0; i < GetComponent<Select>().selectedObjects.Count; i++){
                Vector3 newPos = formationList[i].transform.position;
                GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().agent.SetDestination(newPos);
                GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().task = TaskList.Moving;
                GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().Animator();
                newPos.y += 0.1f; //sets the EndPoint slightly above the ground
                GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().cloneEndPoint = Instantiate(endPoint, newPos, Quaternion.identity); //Creates EndPoint for unit destination
            }
            Destroy(leaderClone.gameObject);
            formationList.Clear();
            Debug.Log("Square formation instantiated");  
        }
    }
}
