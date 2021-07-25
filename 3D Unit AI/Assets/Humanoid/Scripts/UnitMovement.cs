using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour{

    public Select select;
    public UnitUI unitUI;
    public GameObject endPoint;
    public GameObject emptyPoint;
    public float wanderRadius;
    public GameObject leader;
    public GameObject reversedLeader;
    public GameObject leaderClone;
    public Vector3 leaderCloneDirection;
    public GameObject lookAtTarget;
    public GameObject cloneLookAtTarget;
    public List<Transform> formationList = new List<Transform>(); 
    public List<GameObject> tempoaryList = new List<GameObject>();
    public List<GameObject> endPointListX = new List<GameObject>();
    public List<GameObject> endPointListZ = new List<GameObject>();
    public List<GameObject> rowEndPoint = new List<GameObject>();
    public List<GameObject> newFormationList = new List<GameObject>();
    public bool dynamicFormationActive;
    public bool rightclickPressed;

    void Start(){
        rightclickPressed = false;
        dynamicFormationActive = false;
    }

    // Update is called once per frame
    void Update(){
        if (Input.GetMouseButtonDown(1)){
            if(GetComponent<Select>().selectedObjects.Count > 1 && !Input.GetKey(KeyCode.LeftControl)){
                RightClick();
            }            
            if (GetComponent<Select>().selectedObjects.Count > 1 && !Input.GetKey(KeyCode.LeftControl) && rightclickPressed != true && unitUI.dynamicFormation == true){
                rightclickPressed = true;
                if(GetComponent<Select>().selectedObjects.Count > 1){
                    StartCoroutine(CheckMouseMovement());    
                }                
            }
            if (GetComponent<Select>().selectedObjects.Count > 1 && Input.GetKey(KeyCode.LeftControl) && unitUI.dynamicFormation == true){
                FaceDirection();
            }           
        }
        if (Input.GetMouseButtonUp(1) && GetComponent<Select>().selectedObjects.Count > 1 && unitUI.dynamicFormation == true){
            if(rightclickPressed == true && dynamicFormationActive == false){
                Debug.Log("Premade formation is instantiated");
                PremadeFormation();
                StopCoroutine(CheckMouseMovement());
                rightclickPressed = false;                
            }
            if(rightclickPressed == true){
                rightclickPressed = false;
                dynamicFormationActive = false;
            }
        } 
    }

    public void RightClick(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100)){
            if (hit.collider.tag == "Ground"){
                if (GetComponent<Select>().selectedObjects.Count > 1){   
                    //RandomFormation
                    if (unitUI.randomFormation == true){
                        //if PatrolActive = true, make false and reset endPoints
                        if(!Input.GetKey(KeyCode.LeftShift)){
                            ResetEndPoints();
                        }                    
                        wanderRadius = GetComponent<Select>().selectedObjects.Count / 4 + 2;
                        //If endpoints are added to a list
                        if (Input.GetKey(KeyCode.LeftShift)) { 
                            for (int i = 0; i < GetComponent<Select>().selectedObjects.Count; i++){
                                Vector3 newPos = RandomNavSphere(hit.point, wanderRadius, 1);

                                GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().currentFormation = "RandomFormation";
                                GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().inFormation = true;
                                GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().Animator();
                                newPos.y += .1f; //sets the EndPoint slightly above the ground
                                GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().cloneEndPoints.Add(Instantiate(endPoint, newPos, Quaternion.identity)); 
                                GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().NextEndPoint();          
                            }
                        }
                        else{
                            foreach (GameObject unit in GetComponent<Select>().selectedObjects){   
                                Vector3 newPos = RandomNavSphere(hit.point, wanderRadius, 1);

                                unit.GetComponent<Movement>().currentFormation = "RandomFormation";
                                unit.GetComponent<Movement>().inFormation = true;
                                unit.GetComponent<Movement>().agent.SetDestination(newPos);
                                unit.GetComponent<Movement>().task = TaskList.Moving;
                                unit.GetComponent<Movement>().Animator();
                                newPos.y += 0.1f; //sets the EndPoint slightly above the ground
                                unit.GetComponent<Movement>().cloneEndPoints.Add(Instantiate(endPoint, newPos, Quaternion.identity)); //Creates EndPoint for unit destination                        
                            }                            
                        }                        
                        Debug.Log("Multiply units has been selected - Formation instantiated");                        
                    }
                    //CircleFormation
                    if (unitUI.circleFormation == true){   
                        //if PatrolActive = true, make false and reset endPoints
                        if(!Input.GetKey(KeyCode.LeftShift)){
                            ResetEndPoints();
                        }
                        
                        int amountInRow = 5;
                        int currentAmountOfUnits = 0;
                        float angleIncrement = 360 / amountInRow;
                        //If endpoints are added to a list
                        if (Input.GetKey(KeyCode.LeftShift)) {
                            for (int i = 0; i < GetComponent<Select>().selectedObjects.Count; i++){
                                if(amountInRow == currentAmountOfUnits){
                                    currentAmountOfUnits = 0;
                                    amountInRow += 5;
                                    angleIncrement = 360 / amountInRow;
                                }
                                float radius = amountInRow / 6 + 0.5f;
                                Vector3 newPos = FormationCircle(hit.point, radius, i, angleIncrement);
                                currentAmountOfUnits += 1;

                                GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().currentFormation = "CircleFormation";
                                GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().inFormation = true;
                                GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().Animator();
                                newPos.y += .1f; //sets the EndPoint slightly above the ground
                                GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().cloneEndPoints.Add(Instantiate(endPoint, newPos, Quaternion.identity));
                                GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().cloneEndPoints[0].transform.LookAt(hit.point, Vector3.up); 
                                GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().NextEndPoint();          
                            }
                        }
                        else{
                            
                            for (int i = 0; i < GetComponent<Select>().selectedObjects.Count; i++){
                                if(amountInRow == currentAmountOfUnits){
                                    currentAmountOfUnits = 0;
                                    amountInRow += 5;
                                    angleIncrement = 360 / amountInRow;
                                }
                                float radius = amountInRow / 6 + 0.5f;
                                Vector3 newPos = FormationCircle(hit.point, radius, i, angleIncrement);
                                currentAmountOfUnits += 1;

                                GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().currentFormation = "CircleFormation";
                                GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().inFormation = true;
                                GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().agent.SetDestination(newPos);
                                GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().task = TaskList.Moving;
                                GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().Animator();
                                newPos.y += 0.1f; //sets the EndPoint slightly above the ground
                                
                                GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().cloneEndPoints.Add(Instantiate(endPoint, newPos, Quaternion.identity)); //Creates EndPoint for unit destination
                                GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().cloneEndPoints[0].transform.LookAt(hit.point, Vector3.up);
                            }
                        }                        
                    }
                }
            }
        }                
    }

    public IEnumerator DynamicFormation(){

        dynamicFormationActive = true;
        int selectedUnits = GetComponent<Select>().selectedObjects.Count;
        float maxWidth = (float)selectedUnits * 2;
        float maxDepth = (float)selectedUnits * 2;
        float width;
        float depth;
        
        Ray rayA = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitA;
        
        if (Physics.Raycast(rayA, out hitA, 50)){
          
            if (hitA.collider.tag == "Ground"){
                
                Debug.Log("hitA" + " X: " + hitA.point.x + " Y: " + hitA.point.y + " Z: " + hitA.point.z);
                GameObject startPos = Instantiate(emptyPoint, hitA.point, Quaternion.identity);
                int newRowsAmount = 0;
                int currentWidth = -1;

                GameObject firstCloneEndPoint = Instantiate(endPoint, startPos.transform, false);
                firstCloneEndPoint.transform.SetParent(startPos.transform);
                firstCloneEndPoint.transform.localPosition = new Vector3 (0, 0.05f, 0);
                
                endPointListX.Add(firstCloneEndPoint);                

                while(dynamicFormationActive == true){

                    yield return new WaitForSeconds(.1f);
                    Ray rayB = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hitB;

                    if (Physics.Raycast(rayB, out hitB, 50)){

                        if (hitB.collider.tag == "Ground"){
                            
                            startPos.transform.LookAt(hitB.point, Vector3.up);
                            var distance = Vector3.Distance(hitA.point, hitB.point);

                            Debug.Log("hitB" + " X: " + hitB.point.x + " Y: " + hitB.point.y + " Z: " + hitB.point.z);
                            float newPosX = hitB.point.x - hitA.point.x;
                            float newPosZ = hitB.point.z - hitA.point.z;
                            Debug.Log("hitBLocalPosition" + " X: " + newPosX + " Y: " + hitB.point.y + " Z: " + newPosZ);
                            Debug.Log(distance);
                            Debug.DrawLine(hitA.point, hitB.point, Color.red, 50, false);

                            width = distance + 1;
                            depth = selectedUnits - width;
                            Debug.Log("width: " + (int)width);
                            Debug.Log("currentWidth: " + currentWidth);
                            Debug.Log("selectedUnits: " + selectedUnits);
                            Debug.Log("endPointListX.Count: " + endPointListX.Count);

                            int rowsAmount = (int)selectedUnits / (int)width;
                            Debug.Log("rowsAmount: " + rowsAmount);
                            Debug.Log("newRowsAmount: " + newRowsAmount);

                            if((int)width != currentWidth && (int)width <= selectedUnits){
                                for(int i = 0; i < rowEndPoint.Count; i++){
                                    Destroy(rowEndPoint[i]);
                                }
                                rowEndPoint.Clear();
                                                                        
                                for(int k = 0; k < (int)width; k++){
                                    for(int o = 1; o < rowsAmount; o++){    
                                        GameObject newCloneEndPoint = Instantiate(endPoint, startPos.transform, false);
                                        newCloneEndPoint.transform.SetParent(startPos.transform);
                                        newCloneEndPoint.transform.localPosition = new Vector3 (o, .05f, k);
                                        rowEndPoint.Add(newCloneEndPoint);
                                    }
                                }
                                newRowsAmount = rowsAmount;
                            }
                            if((int)width > selectedUnits){
                                for(int i = 0; i < rowEndPoint.Count; i++){
                                    Destroy(rowEndPoint[i]);
                                }
                                rowEndPoint.Clear();    
                            }

                            //Checks if there is any leftover points which do not complete a full row
                            if((int)width != currentWidth && (int)width <= selectedUnits){
                                int leftoverEndPoints = selectedUnits - (rowsAmount * (int)width);
                                float center = 0;
                                if(leftoverEndPoints == 1){
                                    center = ((int)width - 1) / 2f;
                                }
                                if(leftoverEndPoints > 1){
                                    center = ((int)width) / 2f - (leftoverEndPoints / 2f); 
                                }
                                Debug.Log("<b>Center: </b>" + center);

                                if(rowsAmount < 0){
                                    for(int i = 0; i < rowEndPoint.Count; i++){
                                        Destroy(rowEndPoint[i]);
                                    }
                                    rowEndPoint.Clear();                                    
                                }

                                Debug.Log("<b>LeftoverEndPoints: </b>" + leftoverEndPoints);
                                if(leftoverEndPoints != 0){
                                    for(int i = 0; i < leftoverEndPoints; i++){
                                        GameObject newCloneEndPoint = Instantiate(endPoint, startPos.transform, false);
                                        newCloneEndPoint.transform.SetParent(startPos.transform);
                                        newCloneEndPoint.transform.localPosition = new Vector3 (rowsAmount, .05f, i + center);
                                        rowEndPoint.Add(newCloneEndPoint);                                          
                                    }  
                                }                                
                            }

                            for(int i = 0; i < (int)width; i++){                                
                                if(selectedUnits > endPointListX.Count && endPointListX.Count < (int)width){
                                    GameObject cloneEndPoint = Instantiate(endPoint, startPos.transform, false);
                                    cloneEndPoint.transform.SetParent(startPos.transform);
                                    cloneEndPoint.transform.localPosition = new Vector3 (0, 0.05f, endPointListX.Count);
                                   
                                    endPointListX.Add(cloneEndPoint);
                                    currentWidth = (int)width;                                        
                                }                               
                            }
                            if(currentWidth > (int)width){
                                if(endPointListX.Count != 0){
                                    Destroy(endPointListX[endPointListX.Count - 1]);
                                    endPointListX.Remove(endPointListX[endPointListX.Count - 1]);
                                    currentWidth = (int)width;                                         
                                }                                         
                            }
                            if(endPointListX.Count != (int)width){
                                if(endPointListX.Count > (int)width){
                                    int difference = endPointListX.Count - (int)width;
                                    for(int i = 0; i < difference; i++){
                                    Destroy(endPointListX[endPointListX.Count - 1]);
                                    endPointListX.Remove(endPointListX[endPointListX.Count - 1]);
                                    currentWidth = (int)width;                                         
                                    } 
                                }
                            }                                                                                    
                        }
                    }
                    if(dynamicFormationActive == false){
                        for(int i = 0; i < GetComponent<Select>().selectedObjects.Count; i++){
                            if(GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().cloneEndPoints.Count != 0){
                                for(int j = 0; j < GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().cloneEndPoints.Count; j++){
                                    Destroy(GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().cloneEndPoints[j]);
                                }
                                GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().cloneEndPoints.Clear();
                            }
                        }
                        newFormationList.Clear();

                        for(int i = 0; i < endPointListX.Count; i++){
                            GameObject newPos = endPointListX[i].gameObject;
                            newFormationList.Add(newPos);
    
                        }
                        for(int i = 0; i < rowEndPoint.Count; i++){
                            GameObject newPos = rowEndPoint[i].gameObject;
                            newFormationList.Add(newPos);

                        }
                        for(int i = 0; i < newFormationList.Count; i++){
                            GameObject newPos = newFormationList[i].gameObject;
                            GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().currentFormation = "DynamicSquareFormation";
                            GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().inFormation = true;
                            GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().agent.SetDestination(newPos.transform.position);
                            GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().task = TaskList.Moving;
                            GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().Animator();
                            GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().cloneEndPoints.Add(Instantiate(endPoint, newPos.transform.position, newPos.transform.rotation));
                        }
                        Destroy(startPos);
                        endPointListX.Clear();
                        rowEndPoint.Clear();
                        break;
                    }
                    if(Input.GetMouseButton(0)){
                        newFormationList.Clear();
                        Destroy(startPos);
                        endPointListX.Clear();
                        rowEndPoint.Clear();
                        break;
                    }
                    if(hitB.collider == null){
                        Destroy(startPos);
                        endPointListX.Clear();
                        endPointListZ.Clear();
                        break;
                    }   
                }
            }        
        }
    }

    void PremadeFormation(){
        newFormationList.Clear();

        int selectedUnits = select.selectedObjects.Count;
        
        float calculatedAmountInRow = Mathf.Ceil(((float)selectedUnits / 3f));
        int amountInRow = (int)calculatedAmountInRow;
        Debug.Log("AmountInRow: " + amountInRow);

        float center = (float)amountInRow / 2;
        
        int leftoverEndPoints = selectedUnits - (amountInRow * 2);
        if(leftoverEndPoints == amountInRow){
            leftoverEndPoints = 0;
        }
        float leftoverEndPointsCenter = ((float)amountInRow - 1) / 2f;
        Debug.Log("leftoverEndPoints: " + leftoverEndPoints);
        Debug.Log("leftoverEndPointCenter: " + leftoverEndPointsCenter);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100)){
            if (hit.collider.tag == "Ground"){
                GameObject startPos = Instantiate(emptyPoint, hit.point, Quaternion.identity);
                GameObject lookAtTarget = select.selectedObjects[selectedUnits / 2].gameObject;
                startPos.transform.LookAt(lookAtTarget.transform.position, Vector3.up);

                if(leftoverEndPoints == 0){
                    for(int i = 0; i < 3; i++){
                        for(int k = 0; k < amountInRow; k++){
                            GameObject cloneEndPoint = Instantiate(endPoint, startPos.transform, false);
                            cloneEndPoint.transform.SetParent(startPos.transform);
                            cloneEndPoint.transform.localPosition = new Vector3(k - center, 0.05f, i);
                            newFormationList.Add(cloneEndPoint);
                        }
                    }                    
                }
                else{
                    for(int i = 0; i < 2; i++){
                        for(int k = 0; k < amountInRow; k++){     
                            GameObject cloneEndPoint = Instantiate(endPoint, startPos.transform, false);
                            cloneEndPoint.transform.SetParent(startPos.transform);
                            cloneEndPoint.transform.localPosition = new Vector3(k - center, 0.05f, i);
                            newFormationList.Add(cloneEndPoint);   
                        }
                    } 
                    for(int k = 0; k < leftoverEndPoints; k++){
                        GameObject cloneEndPoint = Instantiate(endPoint, startPos.transform, false);
                        cloneEndPoint.transform.SetParent(startPos.transform);
                        cloneEndPoint.transform.localPosition = new Vector3 ((float)k - leftoverEndPointsCenter, .05f, 2);
                        newFormationList.Add(cloneEndPoint);   
                    } 
                }
                for(int i = 0; i < newFormationList.Count; i++){
                    GameObject newPos = newFormationList[i].gameObject;
                    GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().currentFormation = "PremadeSquareFormation";
                    GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().inFormation = true;
                    GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().agent.SetDestination(newPos.transform.position);
                    GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().task = TaskList.Moving;
                    GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().Animator();
                    GetComponent<Select>().selectedObjects[i].GetComponent<Movement>().cloneEndPoints.Add(Instantiate(endPoint, newPos.transform.position, newPos.transform.rotation));  
                }
                Destroy(startPos);
            }                
        }        
    }

    public IEnumerator EnclosedAttack(GameObject unitA, GameObject unitB){
        unitA.GetComponent<ObjectInfo>().enclosedAttackMode = true;
        unitB.GetComponent<ObjectInfo>().enclosedAttackMode = true;

        Vector3 savePosition = unitA.transform.position;
        bool escapeeMode = false;
        bool enclosedAttackMode = true;

        List<GameObject> attackGroupList = new List<GameObject>();
        attackGroupList.Add(unitA);
        attackGroupList.Add(unitB);

        var unitAStartNumber = Random.Range(unitA.GetComponent<ObjectInfo>().weaponSkill, 100);
        var unitBStartNumber = Random.Range(unitB.GetComponent<ObjectInfo>().weaponSkill, 100);
        Debug.Log("<b>UnitA rolled: </b>" + unitAStartNumber + ". <b>UnitB rolled: </b>" + unitBStartNumber);

        GameObject unitsTurnToAttack = null;
        GameObject target = null;
        GameObject unitStorage = null;
        if(unitAStartNumber >= unitBStartNumber){
            unitsTurnToAttack = unitA;
            target = unitB;
            Debug.Log("Unit A has the highest number");
        }
        if(unitAStartNumber < unitBStartNumber){
            unitsTurnToAttack = unitB;
            target = unitA;
            Debug.Log("UnitB has the highest number");
        }

        float dist = 0;

        while(enclosedAttackMode == true){
            if(unitA != null && unitB != null){
                if(unitA.GetComponent<Movement>().attackMate != true || unitB.GetComponent<Movement>().attackMate != true){
                    unitA.GetComponent<Movement>().attackMate = false;
                    unitB.GetComponent<Movement>().attackMate = false;
                    Debug.Log("Something wrong with attackMate, breaking enclosedAttack"); 
                    break;
                }                
            }

            if(unitA == null){
                unitB.GetComponent<Movement>().attackMate = false;
                Debug.Log("unitA == null. Breaking coroutine");
                break;
            }
            if(unitB == null){
                unitA.GetComponent<Movement>().attackMate = false;
                Debug.Log("unitB == null. Breaking coroutine");
                break;
            }

            if(unitA != null && unitB != null){
                dist = Vector3.Distance(unitA.transform.position, unitB.transform.position);
                Debug.Log("<b> Distance between units </b>" + dist);
                
                //Checks if a unit is escaping. If a unit is escaping, the other unit will follow until x amount of newDist
                if(unitA.GetComponent<Movement>().escaping == true && escapeeMode == false){
                    if(escapeeMode == false){
                        savePosition = unitB.transform.position;
                        escapeeMode = true;                        
                    }
                    else{
                        float newDist = Vector3.Distance(unitA.transform.position, savePosition);
                        if(newDist > 10){
                            escapeeMode = false;
                            unitA.GetComponent<Movement>().attackMate = false;
                            unitB.GetComponent<Movement>().attackMate = false;
                            unitA.GetComponent<ObjectInfo>().enclosedAttackMode = false;
                            unitA = null;
                            unitB.GetComponent<Movement>().agent.SetDestination(savePosition);
                            StartCoroutine(unitB.GetComponent<Movement>().Aggro());
                            break;
                        }
                    }
                }
                if(unitB.GetComponent<Movement>().escaping == true && escapeeMode == false){
                    if(escapeeMode == false){
                        savePosition = unitA.transform.position;
                        escapeeMode = true;                        
                    }
                    else{
                        float newDist = Vector3.Distance(unitB.transform.position, savePosition);
                        if(newDist > 10){
                            escapeeMode = false;
                            unitA.GetComponent<Movement>().attackMate = false;
                            unitB.GetComponent<Movement>().attackMate = false;
                            unitB.GetComponent<ObjectInfo>().enclosedAttackMode = false;
                            unitB = null;
                            unitA.GetComponent<Movement>().agent.SetDestination(savePosition);
                            StartCoroutine(unitA.GetComponent<Movement>().Aggro());
                            break;
                        }
                    }
                }       
            }

            if(dist > 3 && unitA != null && unitB != null){
                if(unitA.GetComponent<Movement>().escaping == true){
                    unitA.GetComponent<Movement>().agent.SetDestination(unitA.transform.position);
                }
                if(unitB.GetComponent<Movement>().escaping == true){
                    unitA.GetComponent<Movement>().agent.SetDestination(unitB.transform.position);
                }
                else{
                    unitA.GetComponent<Movement>().agent.SetDestination(unitB.transform.position);
                }  
            }

            //If unitA or unitB are too close to eachother, unitA will move 1 back
            if(dist < 1 && unitA != null && unitB != null){
                Vector3 newPos = Vector3.MoveTowards(unitA.transform.position, unitB.transform.position, -1);
                unitA.GetComponent<Movement>().agent.SetDestination(newPos);
                Debug.Log("Units are too close, UnitA is moving back");
                yield return new WaitForSeconds(.5f);
                Debug.Log("1 second has passed, coroutine will continue");
                dist = Vector3.Distance(unitA.transform.position, unitB.transform.position);
                Debug.Log("<b> New distance between units </b>" + dist); 
            }

            //Start attack sequence
            if(dist <= 3 && dist >= 1 && unitA != null && unitB != null){
                unitA.GetComponent<Movement>().agent.ResetPath();
                StartCoroutine(unitA.GetComponent<Movement>().FaceTarget(300, unitB.transform.position));
                StartCoroutine(unitB.GetComponent<Movement>().FaceTarget(300, unitA.transform.position));
                Debug.Log(unitsTurnToAttack + "will attack");
                unitsTurnToAttack.GetComponent<Movement>().ready = false;
                StartCoroutine(unitsTurnToAttack.GetComponent<Movement>().Attack(target));
                yield return new WaitUntil(() => unitsTurnToAttack.GetComponent<Movement>().ready);
                
                if(target != null){
                    unitStorage = unitsTurnToAttack;
                    unitsTurnToAttack = target;
                    target = unitStorage;
                    Debug.Log("First sequence completed");                   
                }
                //If unit dies, activate Aggro()
                else{
                    unitsTurnToAttack.GetComponent<Movement>().attackMate = false;
                    StartCoroutine(unitsTurnToAttack.GetComponent<Movement>().Aggro());
                    Debug.Log("Target has been killed, stopping coroutine EnclosedAttack");
                    break;
                } 
            }

            else{
                unitA.GetComponent<Movement>().attackMate = false;
                unitB.GetComponent<Movement>().attackMate = false;
                Debug.Log(" <b> Enclosed Attack: </b> Something went wrong");
                break;
            }
        }
    }

    void ResetEndPoints(){
        foreach (GameObject unit in GetComponent<Select>().selectedObjects){
            Destroy(unit.GetComponent<Movement>().cloneEndPoint);
        }
        for (int i = 0; i < select.selectedObjects.Count; i++){
            for (int j = 0; j < select.selectedObjects[i].GetComponent<Movement>().cloneEndPoints.Count; j++){
                Destroy(select.selectedObjects[i].GetComponent<Movement>().cloneEndPoints[j].gameObject);  
            }
            select.selectedObjects[i].GetComponent<Movement>().patrolActive = false;
            select.selectedObjects[i].GetComponent<Movement>().cloneEndPoints.Clear();
        }
        Debug.Log("More units are selected and leaving patrol. Deleting endPoints");
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
            leaderClone = Instantiate(reversedLeader, target.transform.position, target.transform.rotation);
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

    IEnumerator CheckMouseMovement(){
        while(rightclickPressed == true){
            yield return new WaitForSeconds(0.005f);
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0){
                StartCoroutine(DynamicFormation());
                StopCoroutine(CheckMouseMovement());
                break;
            }
        }
    }
}
