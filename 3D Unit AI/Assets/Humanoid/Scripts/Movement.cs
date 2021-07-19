using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class Movement : MonoBehaviour{
    
    private ActionList AL; //List for movenment
    public TaskList task; //List for Tasks
    public JobList job; //List for Jobs
    public Select select;
    public UnitUI unitUI;
    public DataOverview dataOverview;
    public UnitMovement unitMovement;
    public ObjectInfo objectInfo;
    private Quaternion targetRotation;
    public NavMeshAgent agent;
    public GameObject mainCamera; 
    public GameObject endPoint;
    public GameObject cloneEndPoint;
    public GameObject targetNode;
    public GameObject[] drops;
    public GameObject log;        
    public List<GameObject> cloneEndPoints = new List<GameObject>();
    public List<GameObject> target = new List<GameObject>();
    public Animation m_Animator;
    public Transform rightHand;
    public Transform leftHand;
    public Transform back;
    [SerializeField] private LayerMask checkSurroundingsLayer;

    public string currentFormation;
    public float rotationSpeed;
    public float wanderRadius = 2;
    public float weaponDamage;
    public bool escaping = false;    
    public bool inFormation = false;
    public bool attackMate = false;
    public bool standGround = false;
    public bool standGroundDefend = false;
    public bool followTarget;
    public bool goToNextEndPoint = false; 
    public bool patrolActive = false;
    public bool onlyMove = false;
    public bool aggro = false;
    public bool ready = true;

    // Start is called before the first frame update
    void Start(){
        agent = GetComponent<NavMeshAgent>();
        AL = FindObjectOfType<ActionList>();
        m_Animator = gameObject.GetComponent<Animation>();
        followTarget = false;
        task = TaskList.Idle;
        Animator();
    }

    // Update is called once per frame
    public void Update(){
        if(Input.GetMouseButtonDown(1) && GetComponent<ObjectInfo>().isSelected){
            if(!Input.GetKey(KeyCode.LeftControl)){
                RightClick();
            }
            if(Input.GetKey(KeyCode.LeftControl) && select.GetComponent<Select>().selectedObjects.Count >= 1){        
                StartCoroutine(FaceDirection(rotationSpeed));
            }
        }
        // Check if unit has reached the destination
        if (task == TaskList.Moving){
            if (!agent.pathPending){
                if (agent.remainingDistance < 0.5){
                    //Code below is when unit has arrived to the endPoint position
                    if (!agent.hasPath || agent.velocity.sqrMagnitude >= 0.5){ 
                        Debug.Log("Unit has arrived");
                        task = TaskList.Idle;
                        agent.ResetPath();

                        //Checks which formation is current for the individual unit at gives the unit the correct direction variable.
                        if(currentFormation != "RandomFormation" && inFormation == true){
                            GameObject target = Instantiate(endPoint, cloneEndPoints[0].transform, false);
                            target.transform.SetParent(cloneEndPoints[0].transform);
                            if(currentFormation == "DynamicSquareFormation"){
                                target.transform.localPosition = new Vector3(-1000, 2, 0);  
                            }
                            if(currentFormation == "CircleFormation"){
                                target.transform.localPosition = new Vector3(0, 2, -1000);
                            }
                            if(currentFormation == "PremadeSquareFormation"){
                                target.transform.localPosition = new Vector3(0, 2, -1000);
                            }
                            StartCoroutine(FaceTarget(rotationSpeed, target.transform.position));                            
                        }

                        if(followTarget == false){
                            Animator();
                        }
                        if(onlyMove == true && standGroundDefend == false){
                            onlyMove = false;
                            StartCoroutine(Aggro());
                        }
                        if(patrolActive == true){
                            goToNextEndPoint = true;
                        }
                        if(cloneEndPoint != null){
                            Destroy(cloneEndPoint);
                        }
                        if(cloneEndPoints.Count > 0){
                            for(int i = 0; i < cloneEndPoints.Count; i++){
                                Destroy(cloneEndPoints[i].gameObject);
                                cloneEndPoints.Remove(cloneEndPoints[i].gameObject);                                
                            }

                        } 
                        if(cloneEndPoints.Count > 0 && patrolActive == false){
                            Destroy(cloneEndPoints[0].gameObject);
                            cloneEndPoints.Remove(cloneEndPoints[0].gameObject);
                            if (cloneEndPoints.Count > 0){
                                NextEndPoint();
                            }
                        } 
                    }
                }
            }
        }
    }
    
    public void RightClick(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100)){
            target.Clear();
            //Here the unit moves to destinationen when clicked on ground
            if (hit.collider.tag == "Ground"){
                if (mainCamera.GetComponent<Select>().selectedObjects.Count <= 1){
                    if (!Input.GetKey(KeyCode.LeftShift)){
                        Destroy(cloneEndPoint);
                        for(int i = 0; i < cloneEndPoints.Count; i++){
                            Destroy(cloneEndPoints[i].gameObject);
                        }
                        if(patrolActive == true){
                            patrolActive = false;
                        }
                        //If unit carries a TreeLog. Unit will drop TreeLog
                        if(job == JobList.WoodCutter){
                            if(task == TaskList.MovingLogToStorage){
                                if(rightHand.childCount > 0 && rightHand.GetChild(0).name == "TreeLog(Clone)"){
                                    int layerMask = 1 << 8;
                                    RaycastHit hitDrop;
                                    if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hitDrop, Mathf.Infinity, layerMask)){
                                        Vector3 newHitDrop = hitDrop.point;
                                        Transform newDrop = Instantiate(rightHand.GetChild(0), newHitDrop, rightHand.GetChild(0).transform.rotation);
                                        newDrop.localScale = new Vector3(20, 20, 120);
                                        Destroy(rightHand.GetChild(0).gameObject);
                                        agent.speed = 4;
                                        UpdateJob(job, JobList.Unemployed);
                                    }   
                                }
                            }
                        }
                        if(attackMate == true){
                            escaping = true;
                        }
                        cloneEndPoints.Clear();
                        followTarget = false;
                        onlyMove = true;
                        inFormation = false;
                        AL.Move(agent, hit, task);
                        task = TaskList.Moving;         
                        Vector3 newHit = hit.point;
                        newHit.y += 0.1f;
                        cloneEndPoint = Instantiate(endPoint, newHit, Quaternion.identity); //Creates EndPoint for unit destination
                        Animator();
                    }
                    //Adds endpoints to a list which unit will walk to from 0 and up 
                    if (Input.GetKey(KeyCode.LeftShift)){
                        onlyMove = true;
                        inFormation = false;
                        Vector3 newHit = hit.point;
                        newHit.y += .1f;
                        //cloneEndPoint = Instantiate(endPoint,newHit,Quaternion.identity); //Creates EndPoint for unit destination
                        cloneEndPoints.Add(Instantiate(endPoint,newHit,Quaternion.identity));
                        if (patrolActive == false){
                            NextEndPoint(); 
                        }
                    }
                }
            }
            //Here the unit moves to the location of the clicked object
            if (hit.collider.tag == "Selectable"){
                target.Add(hit.collider.gameObject);
                if (target[0].GetComponent<ObjectInfo>().team == objectInfo.team){
                    AL.Move(agent, hit, task);
                    targetNode = target[0];
                    onlyMove = true;
                    inFormation = false;
                    task = TaskList.Moving;
                    Debug.Log("Unit moving towards friendly");
                    Animator();
                }
                //Checks if target is on the same team. If not it attacks target
                if (target[0].GetComponent<ObjectInfo>().team > objectInfo.team || target[0].GetComponent<ObjectInfo>().team < objectInfo.team && !Input.GetKey(KeyCode.LeftShift)) {
                    if (targetNode != target[0] || followTarget == false){
                        StartCoroutine(FollowTarget(target[0]));
                        targetNode = target[0];
                        Debug.Log("Target is not the same as the current target, FollowTarget has been executed");
                    }
                    for (int i = 0; i < cloneEndPoints.Count; i++){
                        Destroy(cloneEndPoints[i].gameObject);
                    }
                    cloneEndPoints.Clear();
                    onlyMove = true;
                    inFormation = false;
                    task = TaskList.Moving; 
                    Debug.Log("Unit moving towards enemy");
                    Animator();
                }
            }
            if(hit.collider.tag == "Tree"){
                StartCoroutine(WoodCutter(hit.collider.gameObject));
                Debug.Log("Clicked on tree. Units job is now WoodCutting");
            }
            if(hit.collider.tag == "LumberMill"){
                StartCoroutine(MillWorker(hit.collider.gameObject));
                Debug.Log("Clicked on Lumber Mill. Units job is now Millworker");
            }
            if (hit.collider.tag == "Resource"){
                AL.Move(agent, hit, task);
                onlyMove = true;
                targetNode = hit.collider.gameObject;
                task = TaskList.Gathering;
                Debug.Log("Gathering");
            }
        }
    }

    private void UpdateJob(JobList currentJob, JobList newJob){
        if(currentJob == JobList.WoodCutter){
            dataOverview.workerAmount -= 1;
        }
        if(currentJob == JobList.Unemployed){
            dataOverview.unemployedAmount -= 1;
        }
        if(currentJob == JobList.Soldier){
            dataOverview.soldierAmount -= 1;
        }
        if(newJob == JobList.WoodCutter){
            dataOverview.workerAmount += 1;
        }
        if(newJob == JobList.Unemployed){
            dataOverview.unemployedAmount += 1;
        }
        if(newJob == JobList.Soldier){
            dataOverview.soldierAmount += 1;
        }
        job = newJob;
    }

    public void CheckWeapon(){
        if(task == TaskList.HeadingToLog || task == TaskList.HeadingTowardsStorage || task == TaskList.TakeTimberWorkbench){
            if(rightHand.childCount > 0){
                GameObject objectToMove = rightHand.GetChild(0).gameObject;
                objectToMove.transform.SetParent(back);
                objectToMove.transform.localPosition = new Vector3(0, 0, 0);
                objectToMove.transform.localRotation = Quaternion.Euler(0, 0, 0);    
            }
            if(leftHand.childCount > 0){

            }
        }
        if(task == TaskList.HeadingToTree || task == TaskList.ProducingTimber){
            if(rightHand.childCount > 0){
                if(rightHand.GetChild(0).gameObject.tag != "Axe"){
                    
                }                
            }
            else{
                for(int i = 0; i < objectInfo.equipmentList.Count; i++){
                    if(objectInfo.equipmentList[i].tag == "Axe"){
                        GameObject objectToMove = objectInfo.equipmentList[i];
                        objectToMove.transform.SetParent(rightHand);
                        objectToMove.transform.localPosition = new Vector3(0, 0, 0);
                        var objectEulerAngleX = objectToMove.GetComponent<Weapon>().itemInfo.eulerAnglex;
                        var objectEulerAngleY = objectToMove.GetComponent<Weapon>().itemInfo.eulerAngley;
                        var objectEulerAngleZ = objectToMove.GetComponent<Weapon>().itemInfo.eulerAnglez;
                        objectToMove.transform.localRotation = Quaternion.Euler(objectEulerAngleX, objectEulerAngleY, objectEulerAngleZ);
                        var objectScalex = objectToMove.GetComponent<Weapon>().itemInfo.scalex;
                        var objectScaley = objectToMove.GetComponent<Weapon>().itemInfo.scaley;
                        var objectScalez = objectToMove.GetComponent<Weapon>().itemInfo.scalez;
                        objectToMove.transform.localScale = new Vector3(objectScalex, objectScaley, objectScalez);                         
                    }
                }
            }

        }
        weaponDamage = objectInfo.rightHand.GetComponent<Weapon>().itemInfo.damage;
    }

    IEnumerator FaceDirection(float rotationSpeed){
        if(Input.GetMouseButtonDown(1)){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100)){   
                if (hit.collider.tag == "Ground"){
                    targetRotation = Quaternion.LookRotation(hit.point - transform.position);
                    Debug.Log("New Direction: Looking at hit.point"); 
                }
            }
        }
        float dur = Quaternion.Angle(transform.rotation, targetRotation) / rotationSpeed;
        Quaternion start = transform.rotation;
        float t = 0f;
        while(t < dur){
            yield return null;
            if(Input.GetMouseButtonDown(1)){
               break;
            }
            t += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(start, targetRotation, t / dur);
        }
        transform.rotation = targetRotation;  
    }

    public IEnumerator FaceTarget(float rotationSpeed, Vector3 target){
          
        targetRotation = Quaternion.LookRotation(target - transform.position);
        float dur = Quaternion.Angle(transform.rotation, targetRotation) / rotationSpeed;
        Quaternion start = transform.rotation;
        float t = 0f;
        while(t < dur){
            yield return null;
            if(Input.GetMouseButtonDown(1)){
               break;
            }
            t += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(start, targetRotation, t / dur);
        }
        transform.rotation = targetRotation; 
        Debug.Log("<b> Facing towards relevant position </b>");
    }
    
    //Follows target and attacks it
    IEnumerator FollowTarget(GameObject newTarget){
        GameObject currentTarget = null;
        followTarget = false;
        followTarget = true;
        Debug.Log("Unit follows target");
        float dist = Vector3.Distance(newTarget.transform.position, transform.position); //Calculates the distance between the selected unit and the target unit
        Debug.Log(dist);
        while (followTarget == true && onlyMove == false){
            yield return new WaitForSeconds(.5f);
            if (newTarget == null){
                followTarget = false;
                task = TaskList.Idle;
                if(patrolActive == true){
                    Animator();
                    goToNextEndPoint = true;
                }
                StartCoroutine(Aggro());                      
                break;
            }
            if (followTarget == false || onlyMove == true){
                newTarget.GetComponent<ObjectInfo>().attackers.Remove(gameObject);
                break;
            }
            dist = Vector3.Distance(newTarget.transform.position, transform.position);             
            if (dist > 50){
                newTarget.GetComponent<ObjectInfo>().attackers.Remove(gameObject);
                followTarget = false;
                task = TaskList.Idle;
                if(patrolActive == true){
                    Animator();
                    goToNextEndPoint = true;
                }
                StartCoroutine(Aggro());                
                break;
            }
            if (dist < 50){
                if(newTarget.GetComponent<ObjectInfo>().attackers.Count < 3 || newTarget == currentTarget){
                    agent.SetDestination(newTarget.transform.position);
                    if (dist < 3){
                        agent.ResetPath();
                        Debug.Log("Unit has reached target, Follow target cancelled. EnclosedAttack has begun");
                        StartCoroutine(unitMovement.EnclosedAttack(gameObject, newTarget));
                        followTarget = false;
                        break;   
                    }                         
                }      
            }
            else{
                break;
            }
        }
    }

    public IEnumerator Attack(GameObject target){
        if(target != null){
            CheckWeapon();
            task = TaskList.Attacking;
            
            var attackerRoll = Random.Range(gameObject.GetComponent<ObjectInfo>().weaponSkill, 200);
            var targetRoll = Random.Range(target.GetComponent<ObjectInfo>().weaponSkill / 2 + target.GetComponent<ObjectInfo>().shieldBlock, 200);
            Debug.Log("<b>attackerRoll: </b>" + attackerRoll + " <b>targetRoll : </b>" + targetRoll);

            if(attackerRoll >= targetRoll){
                Animator(); 
                yield return new WaitForSeconds(.5f);
                target.GetComponent<ObjectInfo>().TakeDamage(weaponDamage, gameObject);
                yield return new WaitForSeconds(.5f);
                if(target == null){
                    attackMate = false;
                    if(standGround == false){
                        StartCoroutine(Aggro());    
                    }  
                }
                Debug.Log("AttackerRoll was the highest, attack has been executed and was succesful");                
            }
            
            if(attackerRoll < targetRoll){
                Animator();
                target.GetComponent<Movement>().task = TaskList.Blocking;
                target.GetComponent<Movement>().Animator();
                yield return new WaitForSeconds(1f);
                target.GetComponent<Movement>().task = TaskList.Idle;
                target.GetComponent<Movement>().Animator();
                Debug.Log("TargetRoll was the highest, attack has been executed but was blocked");
            }                  
            ready = true;
            task = TaskList.Idle;
            Animator();
        }
        else{
            ready = true;
        }
    }

    //Defends itself if unit is attacked and StandGround == true
    public IEnumerator StandGroundDefend(GameObject attacker){
        standGroundDefend = true;
        goToNextEndPoint = false;
        float dist = Vector3.Distance(attacker.transform.position, transform.position);
        Debug.Log(dist);
        while(standGroundDefend == true){
            yield return new WaitForSeconds(.5f);
            if(attacker == null){
                standGroundDefend = false;
                task = TaskList.Idle;
                Animator();
                if(patrolActive == true){
                    goToNextEndPoint = true;
                }
                break;
            }
            if(dist <= 5){
                agent.SetDestination(attacker.transform.position);
                if(dist <= 3){
                    agent.ResetPath();
                    StartCoroutine(FaceTarget(rotationSpeed, attacker.transform.position));
                    task = TaskList.Attacking;
                    Animator();
                    yield return new WaitForSeconds(.5f);
                    attacker.GetComponent<ObjectInfo>().TakeDamage(5, gameObject);
                }                
            }
        }
    }

    //Sets the destination for the next endPoint in a list
    public void NextEndPoint(){
        Destroy(cloneEndPoint);
        agent.SetDestination(cloneEndPoints[0].transform.position);
        task = TaskList.Moving;
        Animator();
        Debug.Log("Unit is moving the next waypoint");
    }
    
    public IEnumerator Patrol(){
        Destroy(cloneEndPoint);
        for(int j = 0; j < cloneEndPoints.Count; j++){
            Destroy(cloneEndPoints[j].gameObject);
        }
        cloneEndPoints.Clear();
        bool waitForClick = true;
        while(waitForClick == true){
            yield return new WaitUntil(() => Input.GetMouseButtonDown(1));
            patrolActive = true;
            waitForClick = false;
        }
        agent.SetDestination(cloneEndPoints[0].transform.position);
        bool goingBack = false;
        int i = 0;
        while(patrolActive == true){
            yield return new WaitUntil(() => goToNextEndPoint == true);
            goToNextEndPoint = false;
            Debug.Log("Patrol: Arrived at waypoint");
            if (i == 1 && goingBack == true){
                goingBack = false;
            }
            if (i == cloneEndPoints.Count && goingBack == false){
                goingBack = true;
            }
            if (i < cloneEndPoints.Count && goingBack == false){
                i++;
            }  
            if (goingBack == true){
                i--;
            }
            yield return new WaitForSeconds(1);
            agent.SetDestination(cloneEndPoints[i - 1].transform.position);
            Debug.Log("Patrol: Going to the next waypoint");
            task = TaskList.Moving;
            Animator();
        }
    }

    public void CheckSurroundings(GameObject unitA, GameObject unitB){
        Debug.Log("CheckSurroundings has been executed");
        target.Clear();
        Collider[] aggroRadius = Physics.OverlapSphere(transform.position, 2, checkSurroundingsLayer, QueryTriggerInteraction.Collide);
        for(int i = 0; i < aggroRadius.Length; i++){
            if(aggroRadius[i].gameObject != unitA.gameObject && aggroRadius[i].gameObject != unitB.gameObject){
                //Der mangler noget her
                Debug.Log("A unit is too close. Moving away from unit");
                break;
            }
        }
    }

    public IEnumerator Aggro(){
        Debug.Log("Aggro has been executed"); 
        target.Clear();
        int layer = 12;
        if (objectInfo.team == 12){
            layer = 1 << 11;
        }
        if(objectInfo.team == 11){
            layer = 1 << 12;
        }
        while (standGround == false && onlyMove == false){
            yield return new WaitForSeconds(1); 
            Collider[] aggroRadius = Physics.OverlapSphere(transform.position, 25, layer, QueryTriggerInteraction.Collide);
            if (0 < aggroRadius.Length){
                if (target.Count == 0){
                    for(int i = 0; i < aggroRadius.Length; i++){ 
                        //Checks if the target already has the maximum amount of attackers. If not, unit will attack target or else unit will check next target in list
                        if(aggroRadius[i].gameObject.GetComponent<Movement>().attackMate != true){
                            target.Add(aggroRadius[i].gameObject);
                            aggroRadius[i].GetComponent<Movement>().attackMate = true;
                            attackMate = true;
                            break;
                        }    
                    }
                    if(target.Count != 0){
                        StartCoroutine(FollowTarget(target[0]));
                        Debug.Log("Found a target. FollowTarget has begun"); 
                        break;                          
                    }
                }
            }
            else{   
                Animator();
                target.Clear();           
                Debug.Log("No enemies neaby");               
            }
        }       
    }

    public IEnumerator WoodCutter(GameObject target){
        if(cloneEndPoint != null){
            Destroy(cloneEndPoint);
        }
        if(cloneEndPoints.Count > 0){
            for(int i = 0; i < cloneEndPoints.Count; i++){
                Destroy(cloneEndPoints[i].gameObject);
                cloneEndPoints.Remove(cloneEndPoints[i].gameObject);                                
            }
        } 
        task = TaskList.Idle;
        float distance = 0;
        float lowestDistance = Mathf.Infinity;
        Debug.Log("WoodCutter has been executed");

        if(target == null || target.GetComponent<TreeInfo>().treeIsOccupied == true){ 
            int layer = 1 << 14;
            Collider[] findTrees = Physics.OverlapSphere(transform.position, 50, layer, QueryTriggerInteraction.Collide);
            if(findTrees.Length > 0){
                for(int i = 0; i < findTrees.Length; i++){
                    if(findTrees[i].gameObject.GetComponent<TreeInfo>().treeIsOccupied == false){
                        distance = Vector3.Distance(findTrees[i].gameObject.transform.position, transform.position);
                        if(distance < lowestDistance){
                            lowestDistance = distance;
                            target = findTrees[i].gameObject;
                        }
                    }
                }
            }
            else{
                UpdateJob(job, JobList.Unemployed);        
                Debug.Log("There are no trees nearby. Unit stays as unemployed");
            }
        }        
        if(target != null && target.GetComponent<TreeInfo>().treeIsOccupied == false){
            UpdateJob(job, JobList.WoodCutter);
            CheckWeapon();            
            target.GetComponent<TreeInfo>().treeIsOccupied = true;
            agent.SetDestination(target.transform.position);
            task = TaskList.HeadingToTree;
            Debug.Log("Heading to tree. Distance to tree: " + lowestDistance);              
        }

        while(job == JobList.WoodCutter){
            yield return new WaitForSeconds(.1f);
            
            //when unit is within distance of tree, task will change to CuttingTree
            if(task == TaskList.HeadingToTree){
                distance = Vector3.Distance(target.transform.position, transform.position);
                if(distance <= 2){
                    agent.ResetPath();
                    CheckWeapon();
                    task = TaskList.CuttingTree;                    
                }    
            }

            //Unit deals damage to tree
            if(task == TaskList.CuttingTree && target != null){
                Vector3 lookAtPos = new Vector3(target.transform.position.x, target.transform.position.y + .15f, target.transform.position.z);
                StartCoroutine(FaceTarget(300, lookAtPos));
                Animator();
                yield return new WaitForSeconds(.5f);
                target.GetComponent<TreeInfo>().TakeDamage((int)weaponDamage, gameObject);
                yield return new WaitForSeconds(1.5f);
            }

            //When tree is destroyed unit will look for the closest log and heads to it when found
            if(task == TaskList.CuttingTree && target == null || task == TaskList.LookingForLog){
                lowestDistance = Mathf.Infinity;
                int layer = 1 << 15;
                target = null;
                Collider[] findLogs = Physics.OverlapSphere(transform.position, 50, layer, QueryTriggerInteraction.Collide);
                if(findLogs.Length > 0){
                    for(int i = 0; i < findLogs.Length; i++){
                        if(findLogs[i].gameObject.GetComponent<LogInfo>().logIsOccupied == false){
                            distance = Vector3.Distance(findLogs[i].gameObject.transform.position, transform.position);
                            if(distance < lowestDistance){
                                lowestDistance = distance;
                                target = findLogs[i].gameObject;
                            }
                        }
                    }                  
                }
                if(target != null){
                    task = TaskList.HeadingToLog;
                    target.GetComponent<LogInfo>().logIsOccupied = true;
                    agent.SetDestination(target.transform.position);
                    CheckWeapon();
                    Animator();
                    Debug.Log("Found a log. Heading to Log. Distance to log: " + distance);                         
                }                
                //If there is no logs nearby, unit will look for a tree
                else{
                    Debug.Log("No logs nearby. Will look for a tree instead");
                    layer = 1 << 14;
                    target = null;
                    Collider[] findTrees = Physics.OverlapSphere(transform.position, 50, layer, QueryTriggerInteraction.Collide);
                    if(findTrees.Length > 0){
                        for(int i = 0; i < findTrees.Length; i++){
                            if(findTrees[i].gameObject.GetComponent<TreeInfo>().treeIsOccupied == false){
                                distance = Vector3.Distance(findTrees[i].gameObject.transform.position, transform.position);
                                if(distance < lowestDistance){
                                    lowestDistance = distance;
                                    target = findTrees[i].gameObject;
                                }
                            }
                        }
                    }
                    if(target != null){
                        task = TaskList.HeadingToTree;
                        CheckWeapon();
                        Animator();            
                        target.GetComponent<TreeInfo>().treeIsOccupied = true;
                        agent.SetDestination(target.transform.position);                 
                        Debug.Log("Heading to tree. Distance to tree: " + lowestDistance);                        
                    }
                    //If there are no trees nearby coroutine will break
                    else{
                        UpdateJob(job, JobList.Unemployed);
                        Debug.Log("There are no trees nearby. Job has been switched to Unemployed");  
                        break;
                    }
                }
            }

            //When unit arrives to the target(Log), it then picks it up and looks for a nearby storage
            if(task == TaskList.HeadingToLog && target != null || task == TaskList.LookingForStorage){
                distance = Vector3.Distance(target.transform.position, transform.position);
                if(distance <= 1 || task == TaskList.LookingForStorage){
                    if(task == TaskList.HeadingToLog){
                        agent.ResetPath();
                        GameObject cloneLog = target;
                        yield return new WaitForSeconds(.5f);
                        cloneLog.transform.SetParent(rightHand);
                        cloneLog.transform.localPosition = new Vector3(0, 0, 0);
                        cloneLog.transform.localRotation = Quaternion.Euler(0, 0, 0);                          
                    }

                    yield return new WaitForSeconds(.5f);
                    int layer = 1 << 16;
                    target = null;
                    lowestDistance = Mathf.Infinity;
                    Collider[] findStorage = Physics.OverlapSphere(transform.position, 50, layer, QueryTriggerInteraction.Collide);
                    if(findStorage.Length > 0){
                        for(int i = 0; i < findStorage.Length; i++){

                            Debug.Log("Name of found storage: " + findStorage[i].name);
                            int currentStorageAmountleft = 0;
                            string storageResource = "null";
                            if(findStorage[i].name == "WoodenLogStorage"){
                                currentStorageAmountleft = findStorage[i].gameObject.GetComponent<StorageInfo>().maxStorageAmount - findStorage[i].gameObject.GetComponent<StorageInfo>().currentStorageAmount;
                                storageResource = findStorage[i].gameObject.GetComponent<StorageInfo>().currentResource;
                            }
                            else if(findStorage[i].name == "NormalStorage"){
                                currentStorageAmountleft = findStorage[i].gameObject.GetComponent<NormalStorageInfo>().maxStorageAmount - findStorage[i].gameObject.GetComponent<NormalStorageInfo>().currentStorageAmount;
                                storageResource = findStorage[i].gameObject.GetComponent<NormalStorageInfo>().currentResource;
                            }

                            if(currentStorageAmountleft > 0 && storageResource == "WoodenLog" || currentStorageAmountleft > 0 && storageResource == "null"){
                                distance = Vector3.Distance(findStorage[i].gameObject.transform.position, transform.position);
                                if(distance < lowestDistance){
                                    lowestDistance = distance;
                                    target = findStorage[i].gameObject;
                                }
                            }
                        }
                    }
                    if(target != null){
                        task = TaskList.MovingLogToStorage;
                        Animator();
                        agent.SetDestination(target.transform.position);
                        Debug.Log("Heading to Storage Building. Distance to Building: " + lowestDistance);  
                    }
                    else{
                        int layerMask = 1 << 8;
                        RaycastHit hitDrop;
                        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hitDrop, Mathf.Infinity, layerMask)){
                            Vector3 newHitDrop = hitDrop.point;
                            Transform newDrop = Instantiate(rightHand.GetChild(0), newHitDrop, rightHand.GetChild(0).transform.rotation);
                            newDrop.localScale = new Vector3(20, 20, 120);
                            newDrop.name = "WoodenLog";
                            Destroy(rightHand.GetChild(0).gameObject);
                        }
                        agent.speed = 4;
                        Debug.Log("There are no available storage nearby. Unit has dropped the log annd coroutine will break");
                        break;
                    }
                }
            }

            //When unit Arrives to a storage, Log will be transfered to storage
            if(task == TaskList.MovingLogToStorage && target != null){
                distance = Vector3.Distance(target.transform.position, transform.position);
                agent.speed = 2;
                if(distance <= 3.5f){
                    agent.ResetPath();
                    Debug.Log("Target Name: " + target.name);
                    StartCoroutine(FaceTarget(150, target.transform.position));

                    int currentStorageAmountleft = 0;
                    string storageResource = "null";
                    if(target.name == "WoodenLogStorage"){
                        currentStorageAmountleft = target.gameObject.GetComponent<StorageInfo>().maxStorageAmount - target.gameObject.GetComponent<StorageInfo>().currentStorageAmount;
                        storageResource = target.gameObject.GetComponent<StorageInfo>().currentResource;
                    }
                    else if(target.name == "NormalStorage"){
                        currentStorageAmountleft = target.gameObject.GetComponent<NormalStorageInfo>().maxStorageAmount - target.gameObject.GetComponent<NormalStorageInfo>().currentStorageAmount;
                        storageResource = target.gameObject.GetComponent<NormalStorageInfo>().currentResource;
                    }
                    
                    if(currentStorageAmountleft > 0 && storageResource == "WoodenLog" || currentStorageAmountleft > 0 && storageResource == "null"){
                        yield return new WaitForSeconds(.5f);

                        if(target.name == "WoodenLogStorage"){
                            target.GetComponent<StorageInfo>().AddResource(rightHand.GetChild(0).gameObject);
                            target.GetComponent<StorageInfo>().currentStorageAmount += 1;  
                        }
                        else if(target.name == "NormalStorage"){
                            target.GetComponent<NormalStorageInfo>().AddResource(rightHand.GetChild(0).gameObject);
                        }

                        Destroy(rightHand.GetChild(0).gameObject);
                        agent.speed = 4;
                        task = TaskList.LookingForLog;
                    }
                    else{
                        task = TaskList.LookingForStorage;
                        Debug.Log("Storage is full. Looking for a second storage");
                    }
                }
            }
        }
    }

    public IEnumerator MillWorker(GameObject target){
        if(cloneEndPoint != null){
            Destroy(cloneEndPoint);
        }
        if(cloneEndPoints.Count > 0){
            for(int i = 0; i < cloneEndPoints.Count; i++){
                Destroy(cloneEndPoints[i].gameObject);
                cloneEndPoints.Remove(cloneEndPoints[i].gameObject);                                
            }
        } 
        task = TaskList.Idle;
        GameObject lumberMill = target;
        Transform workbench = null;
        Transform woodenLogStorage = null;
        Transform timberStorage = null;
        float progress = 0;
        float distance = 0;
        float lowestDistance = Mathf.Infinity;
        Debug.Log("MillWorker has been executed");

        if(target == null || target.GetComponent<LumberMill>().lumberMillisOccupied == true){ 
            int layer = 1 << 17;
            Collider[] findLumberMill = Physics.OverlapSphere(transform.position, 50, layer, QueryTriggerInteraction.Collide);
            if(findLumberMill.Length > 0){
                for(int i = 0; i < findLumberMill.Length; i++){
                    if(findLumberMill[i].gameObject.GetComponent<LumberMill>().lumberMillisOccupied == false){
                        distance = Vector3.Distance(findLumberMill[i].gameObject.transform.position, transform.position);
                        if(distance < lowestDistance){
                            lowestDistance = distance;
                            target = findLumberMill[i].gameObject;
                        }
                    }
                }
            }
            else{
                UpdateJob(job, JobList.Unemployed);        
                Debug.Log("There no Lumber Mills nearby. Unit stays as unemployed");
            }
        }  

        if(target != null && target.GetComponent<LumberMill>().lumberMillisOccupied == false){
            Debug.Log("Copying lumber mills data");
            lumberMill = target;
            UpdateJob(job, JobList.MillWorker);           
            lumberMill.GetComponent<LumberMill>().AddWorker(gameObject);
            woodenLogStorage = lumberMill.GetComponent<LumberMill>().woodenLogStorage;
            timberStorage = lumberMill.GetComponent<LumberMill>().timberStorage;
            
            if(lumberMill.GetComponent<LumberMill>().currentWorkersAmount == 0){
                workbench = lumberMill.GetComponent<LumberMill>().workbenchA;
            }
            if(lumberMill.GetComponent<LumberMill>().currentWorkersAmount == 1){
                workbench = lumberMill.GetComponent<LumberMill>().workbenchB;
            }

            agent.SetDestination(target.transform.position);
            task = TaskList.HeadingToLumberMill;
            Debug.Log("Heading to Lumber Mill. Distance to Lumber Mill " + lowestDistance);              
        }

        while(job == JobList.MillWorker){
            yield return new WaitForSeconds(.1f);

            if(task == TaskList.HeadingToLumberMill){
                Debug.Log("Heading to lumber mill");
                distance = Vector3.Distance(lumberMill.transform.position, transform.position);
                if(distance <= 4){
                    agent.ResetPath();
                    task = TaskList.CheckStorage;                    
                }  
            }

            if(task == TaskList.CheckStorage){
                Debug.Log("Checking storage");
                if(lumberMill.GetComponent<LumberMill>().currentWoodenLogAmount == 0){
                    task = TaskList.PickingUpWoodenLogs;
                }
                else if(lumberMill.GetComponent<LumberMill>().currentTimberAmount >= 70){
                    task = TaskList.EmptyingTimberStorage;
                }
                else{
                    task = TaskList.MovingWoodenLogToStorage;
                }
            }

            //Unit finds the closest storage with woodenlogs and walks over to it
            if(task == TaskList.PickingUpWoodenLogs){
                Debug.Log("Picking up woodenlogs");
                int layer = 1 << 16;
                target = null;
                lowestDistance = Mathf.Infinity;
                Collider[] findStorage = Physics.OverlapSphere(transform.position, 50, layer, QueryTriggerInteraction.Collide);
                if(findStorage.Length > 0){
                    for(int i = 0; i < findStorage.Length; i++){

                        Debug.Log("Name of found storage: " + findStorage[i].name);
                        int currentStorageAmount = 0;
                        string storageResource = "null";
                        if(findStorage[i].name == "WoodenLogStorage"){
                            currentStorageAmount = findStorage[i].gameObject.GetComponent<StorageInfo>().currentStorageAmount;
                            storageResource = findStorage[i].gameObject.GetComponent<StorageInfo>().currentResource;
                        }
                        else if(findStorage[i].name == "NormalStorage"){
                            currentStorageAmount = findStorage[i].gameObject.GetComponent<NormalStorageInfo>().currentStorageAmount;
                            storageResource = findStorage[i].gameObject.GetComponent<NormalStorageInfo>().currentResource;
                        }

                        if(currentStorageAmount > 0 && storageResource == "WoodenLog"){
                            distance = Vector3.Distance(findStorage[i].gameObject.transform.position, transform.position);
                            if(distance < lowestDistance){
                                lowestDistance = distance;
                                target = findStorage[i].gameObject;
                            }
                        }
                    }
                }

                if(target != null){
                    task = TaskList.HeadingTowardsStorage;
                    Animator();
                    agent.SetDestination(target.transform.position);
                    Debug.Log("Heading to Storage Building. Distance to Building: " + lowestDistance);  
                }

                else{
                    if(lumberMill.GetComponent<LumberMill>().currentWoodenLogAmount > 0){
                        task = TaskList.MovingTowardsMillsWoodenLogStorage;
                    }
                    else{
                        task = TaskList.WaitingForResources;    
                    }
                }  
            }

            if(task == TaskList.HeadingTowardsStorage){
                Debug.Log("Heading towards storage");
                distance = Vector3.Distance(target.transform.position, transform.position);
                if(distance <= 2){
                    agent.ResetPath();
                    Animator();
                    CheckWeapon();
                    Vector3 lookAtPos = new Vector3(target.transform.position.x, target.transform.position.y + .15f, target.transform.position.z);
                    StartCoroutine(FaceTarget(300, lookAtPos));
                    yield return new WaitForSeconds(.5f);

                    GameObject resourceObject = null;
                    if(target.name == "WoodenLogStorage"){
                        resourceObject = target.GetComponent<StorageInfo>().resourceObject;
                        target.GetComponent<StorageInfo>().TakeResource();  
                    }
                    else if(target.name == "NormalStorage"){
                        resourceObject = target.GetComponent<NormalStorageInfo>().resourceObject;
                        target.GetComponent<NormalStorageInfo>().TakeResource();
                    }

                    GameObject cloneLog = Instantiate(resourceObject, transform.position, transform.rotation);
                    cloneLog.transform.SetParent(rightHand);
                    cloneLog.name = "WoodenLog";
                    cloneLog.layer = 0;
                    cloneLog.transform.localPosition = new Vector3(0, 0, 0);
                    cloneLog.transform.localRotation = Quaternion.Euler(0, 0, 0);

                    agent.speed = 2;
                    agent.SetDestination(woodenLogStorage.position);
                    task = TaskList.MovingLogToLumberMill; 
                } 
            }

            if(task == TaskList.MovingLogToLumberMill){
                Debug.Log("Moving log to lumber mill");
                Animator();
                distance = Vector3.Distance(woodenLogStorage.position, transform.position);
                if(distance <= 2){
                    agent.ResetPath();
                    yield return new WaitForSeconds(.5f);

                    lumberMill.GetComponent<LumberMill>().AddResource(rightHand.GetChild(0).gameObject);
                    Destroy(rightHand.GetChild(0).gameObject);
                    agent.speed = 4;

                    if(lumberMill.GetComponent<LumberMill>().currentWoodenLogAmount == 6){
                        task = TaskList.CheckWorkbench;
                    }
                    else{
                        task = TaskList.PickingUpWoodenLogs;
                    }
                }
            }

            if(task == TaskList.CheckWorkbench){
                Debug.Log("Checking workbench");
                if(workbench.transform.childCount > 0){  
                    task = TaskList.HeadingTowardsWorkbench;
                    agent.SetDestination(workbench.transform.position);
                }
                else if(woodenLogStorage.transform.childCount > 0){
                    task = TaskList.HeadingTowardsWoodenLogStorage;
                    agent.SetDestination(woodenLogStorage.transform.position);
                }
                else{
                    task = TaskList.PickingUpWoodenLogs;
                }
            }

            if(task == TaskList.HeadingTowardsWoodenLogStorage){
                Debug.Log("Heading towards woodenlog storage");
                distance = Vector3.Distance(woodenLogStorage.transform.position, transform.position);
                if(distance <= 2){
                    agent.ResetPath();
                    Vector3 lookAtPos = new Vector3(woodenLogStorage.transform.position.x, woodenLogStorage.transform.position.y + .15f, woodenLogStorage.transform.position.z);
                    StartCoroutine(FaceTarget(300, lookAtPos));
                    yield return new WaitForSeconds(.5f);

                    lumberMill.GetComponent<LumberMill>().TakeResourceWoodenLog();
                    GameObject woodenLog = lumberMill.GetComponent<LumberMill>().woodenLogObject;
                    GameObject cloneLog = Instantiate(woodenLog, transform.position, transform.rotation);
                    cloneLog.layer = 0;
                    cloneLog.name = "WoodenLog";
                    cloneLog.transform.SetParent(rightHand);
                    cloneLog.transform.localPosition = new Vector3(0, 0, 0);
                    cloneLog.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    agent.speed = 2;

                    task = TaskList.HeadingTowardsWorkbench;
                    agent.SetDestination(workbench.transform.position);
                }
            }

            if(task == TaskList.HeadingTowardsWorkbench){
                Debug.Log("Heading towards workbench");
                if(target != workbench.gameObject){
                    target = workbench.gameObject;
                }

                distance = Vector3.Distance(workbench.transform.position, transform.position);
                if(distance <= 1){
                    agent.ResetPath();
                    if(rightHand.childCount > 0){
                        if(rightHand.GetChild(0).name == "WoodenLog"){
                            Vector3 lookAtPos = new Vector3(workbench.transform.position.x, workbench.transform.position.y + .15f, workbench.transform.position.z);
                            StartCoroutine(FaceTarget(300, lookAtPos));
                            yield return new WaitForSeconds(.5f);

                            lumberMill.GetComponent<LumberMill>().AddResourceWorkbench(workbench);
                            Destroy(rightHand.GetChild(0).gameObject);
                            agent.speed = 4f;
                            progress = 0;
                        }
                    }

                    if(workbench.childCount > 0){
                        if(workbench.GetChild(0).name == "Timber"){
                            task = TaskList.TakeTimberWorkbench;
                        }

                        if(workbench.GetChild(0).name == "WoodenLog"){
                            task = TaskList.ProducingTimber;  
                        }
                    }                                         
                }
            }

            if(task == TaskList.ProducingTimber){
                Debug.Log("Producing timber");
                CheckWeapon();
                Animator();
                yield return new WaitForSeconds(1f);
                Vector3 lookAtPos = new Vector3(workbench.transform.position.x, workbench.transform.position.y + .15f, workbench.transform.position.z);
                StartCoroutine(FaceTarget(300, lookAtPos));

                progress += 10;

                if(progress >= 100){
                    lumberMill.GetComponent<LumberMill>().ProduceTimber(workbench);

                    if(lumberMill.GetComponent<LumberMill>().currentTimberAmount >= 75){
                        task = TaskList.EmptyingTimberStorage;
                    }
                    else{
                        task = TaskList.TakeTimberWorkbench;
                    }
                }                
            }

            if(task == TaskList.TakeTimberWorkbench){
                if(lumberMill.GetComponent<LumberMill>().currentTimberAmount >= 75){
                    task = TaskList.EmptyingTimberStorage;
                }
                else{
                    Vector3 lookAtPos = new Vector3(workbench.transform.position.x, workbench.transform.position.y + .15f, workbench.transform.position.z);
                    StartCoroutine(FaceTarget(300, lookAtPos));
                    yield return new WaitForSeconds(0.5f);

                    CheckWeapon();
                    lumberMill.GetComponent<LumberMill>().TakeResourceWorkbench(workbench);
                    GameObject timberObject = lumberMill.GetComponent<LumberMill>().timberObject;
                    GameObject newTimber = Instantiate(timberObject, transform.position, transform.rotation);
                    newTimber.name = "Timber";
                    newTimber.transform.SetParent(rightHand);
                    newTimber.transform.localPosition = new Vector3(0, 0, 0);
                    newTimber.transform.localRotation = Quaternion.Euler(0, 90, 0);
                    var timberAmount = workbench.childCount - 1;
                    Destroy(workbench.GetChild(timberAmount));

                    agent.speed = 3f;
                    agent.SetDestination(timberStorage.position);
                    task = TaskList.MovingTimberToStorage;
                    Animator();                  
                }
               
            }

            if(task == TaskList.MovingTimberToStorage){
                Debug.Log("Moving timber to storage");
                distance = Vector3.Distance(timberStorage.position, transform.position);
                if(distance <= 2){
                    agent.ResetPath();
                    Vector3 lookAtPos = new Vector3(timberStorage.transform.position.x, timberStorage.transform.position.y + .15f, timberStorage.transform.position.z);
                    StartCoroutine(FaceTarget(300, lookAtPos));
                    yield return new WaitForSeconds(.5f);

                    lumberMill.GetComponent<LumberMill>().AddResource(rightHand.GetChild(0).gameObject);
                    Destroy(rightHand.GetChild(0).gameObject);
                    agent.speed = 4;

                    if(lumberMill.GetComponent<LumberMill>().currentTimberAmount >= 75){
                        task = TaskList.EmptyingTimberStorage;
                        agent.SetDestination(timberStorage.position);
                    }
                    else{
                        task = TaskList.CheckWorkbench;
                    }
                }
            }

            if(task == TaskList.EmptyingTimberStorage){
                distance = Vector3.Distance(timberStorage.position, transform.position);
                if(distance <= 1){
                    Debug.Log("Emptying Timber Storage");
                    int layer = 1 << 16;
                    target = null;
                    lowestDistance = Mathf.Infinity;
                    Collider[] findStorage = Physics.OverlapSphere(transform.position, 50, layer, QueryTriggerInteraction.Collide);
                    if(findStorage.Length > 0){
                        for(int i = 0; i < findStorage.Length; i++){

                            Debug.Log("Name of found storage: " + findStorage[i].name);
                            int currentStorageAmountleft = 0;
                            string storageResource = "null";

                            if(findStorage[i].name == "NormalStorage"){
                                currentStorageAmountleft = findStorage[i].gameObject.GetComponent<NormalStorageInfo>().maxStorageAmount - findStorage[i].gameObject.GetComponent<NormalStorageInfo>().currentStorageAmount;
                                storageResource = findStorage[i].gameObject.GetComponent<NormalStorageInfo>().currentResource;
                            }

                            if(currentStorageAmountleft > 0 && storageResource == "Timber" || currentStorageAmountleft > 0 && storageResource == "null"){
                                distance = Vector3.Distance(findStorage[i].gameObject.transform.position, transform.position);
                                if(distance < lowestDistance){
                                    lowestDistance = distance;
                                    target = findStorage[i].gameObject;
                                }
                            }
                        }
                    }
                    if(target != null){
                        yield return new WaitForSeconds(0.5f);

                        lumberMill.GetComponent<LumberMill>().TakeResourceTimber();
                        GameObject timberObject = lumberMill.GetComponent<LumberMill>().timberObject;
                        GameObject newTimber = Instantiate(timberObject, transform.position, transform.rotation);
                        newTimber.name = "Timber";
                        newTimber.transform.SetParent(rightHand);
                        newTimber.transform.localPosition = new Vector3(0, 0, 0);
                        newTimber.transform.localRotation = Quaternion.Euler(0, 90, 0);

                        task = TaskList.MovingTimberToStorage;
                        Animator();
                        agent.speed = 3;
                        agent.SetDestination(target.transform.position);
                        Debug.Log("Heading to Storage Building. Distance to Building: " + lowestDistance);  
                    } 
                    else{
                        if(lumberMill.GetComponent<LumberMill>().currentWoodenLogAmount > 0){
                            task = TaskList.MovingTowardsMillsWoodenLogStorage;
                        }
                        else{
                            task = TaskList.WaitingForResources;    
                        }
                    }                                       
                }
            }

            if(task == TaskList.MovingTimberToStorage){
                distance = Vector3.Distance(target.transform.position, transform.position);
                if(distance <= 2){
                    agent.ResetPath();
                    Vector3 lookAtPos = new Vector3(target.transform.position.x, target.transform.position.y + .15f, target.transform.position.z);
                    StartCoroutine(FaceTarget(300, lookAtPos));
                    yield return new WaitForSeconds(.5f);

                    target.GetComponent<NormalStorageInfo>().AddResource(rightHand.GetChild(0).gameObject);
                    Destroy(rightHand.GetChild(0).gameObject);
                    agent.speed = 4;
                    
                    if(lumberMill.GetComponent<LumberMill>().currentTimberAmount > 0){
                        agent.SetDestination(timberStorage.transform.position);
                        task = TaskList.EmptyingTimberStorage;
                    }
                    else if(lumberMill.GetComponent<LumberMill>().currentWoodenLogAmount < 6){
                        task = TaskList.PickingUpWoodenLogs;
                    }
                     
                }
            }

            if(task == TaskList.WaitingForResources){
                Debug.Log("Waiting for resources");
                yield return new WaitForSeconds(2f);

                int layer = 1 << 16;
                target = null;
                lowestDistance = Mathf.Infinity;
                Collider[] findStorage = Physics.OverlapSphere(transform.position, 50, layer, QueryTriggerInteraction.Collide);
                if(findStorage.Length > 0){
                    for(int i = 0; i < findStorage.Length; i++){

                        Debug.Log("Name of found storage: " + findStorage[i].name);
                        int currentStorageAmount = 0;
                        string storageResource = "null";
                        if(findStorage[i].name == "WoodenLogStorage"){
                            currentStorageAmount = findStorage[i].gameObject.GetComponent<StorageInfo>().currentStorageAmount;
                            storageResource = findStorage[i].gameObject.GetComponent<StorageInfo>().currentResource;
                        }
                        else if(findStorage[i].name == "NormalStorage"){
                            currentStorageAmount = findStorage[i].gameObject.GetComponent<NormalStorageInfo>().currentStorageAmount;
                            storageResource = findStorage[i].gameObject.GetComponent<NormalStorageInfo>().currentResource;
                        }

                        if(currentStorageAmount > 0 && storageResource == "WoodenLog"){
                            distance = Vector3.Distance(findStorage[i].gameObject.transform.position, transform.position);
                            if(distance < lowestDistance){
                                lowestDistance = distance;
                                target = findStorage[i].gameObject;
                            }
                        }
                    }
                }

                if(target != null){
                    task = TaskList.HeadingTowardsStorage;
                    Animator();
                    agent.SetDestination(target.transform.position);
                    Debug.Log("Heading to Storage Building. Distance to Building: " + lowestDistance);  
                }

            }
        }

    }

    //Checks which weapon is equipped and chooses the relevant animation for the unit
    public void Animator(){
        var rightHandObject = objectInfo.rightHand;
        var leftHandObject = objectInfo.leftHand;
        if(task == TaskList.Moving){
            if(rightHandObject != null){
                if(rightHandObject.tag == "Spear"){
                    m_Animator["MovingwSpear"].layer = 2;
                    m_Animator.Play("MovingwSpear");
                    m_Animator["MovingwSpear"].weight = .3f;     
                }
                if(rightHandObject.tag == "Sword" || rightHandObject.tag == "Axe"){
                    m_Animator["MovingwSwordorAxeRH"].layer = 2;
                    m_Animator.Play("MovingwSwordorAxeRH");
                    m_Animator["MovingwSwordorAxeRH"].weight = .3f;
                }
            }
            if(rightHandObject == null){

            }
            if(leftHandObject != null){
                if(leftHandObject.tag == "Shield"){
                    m_Animator["IdlewSpear"].layer = 2;
                    m_Animator.Play("IdlewSpear");
                    m_Animator["IdlewSpear"].weight = .3f;
                }
            }
            if(leftHandObject == null){

            } 
        }
        if(task == TaskList.Idle || task == TaskList.HeadingToLog || task == TaskList.HeadingToTree || task == TaskList.HeadingTowardsStorage){
            m_Animator.Play("Idle");
            m_Animator["IdleLeftHand"].layer = 1; 
            m_Animator.Play("IdleLeftHand");
            m_Animator["IdleLeftHand"].weight = .3f;
            if(rightHandObject != null){
                if(rightHandObject.tag == "Spear"){   
                    m_Animator["IdlewSpear"].layer = 2;
                    m_Animator.Play("IdlewSpear");
                    m_Animator["IdlewSpear"].weight = .3f;
                }
                else if(rightHandObject.tag == "Sword" || rightHandObject.tag == "Axe"){
                    m_Animator["IdlewSwordorAxeRH"].layer = 2;
                    m_Animator.Play("IdlewSwordorAxeRH");
                    m_Animator["IdlewSwordorAxeRH"].weight = .3f;
                }
            }
            if(rightHandObject == null){
                m_Animator["IdleRightHand"].layer = 2;
                m_Animator.Play("IdleRightHand");
                m_Animator["IdleRightHand"].weight = .3f;
            }
            if(leftHandObject != null){
                if(leftHandObject.tag == "Shield"){
                    m_Animator["IdleLeftHand"].layer = 1;
                    m_Animator.Play("IdleLeftHand");
                    m_Animator["IdleLeftHand"].weight = .3f;                    
                }
            }
            if(leftHandObject == null){
                m_Animator["IdleLeftHand"].layer = 1;
                m_Animator.Play("IdleLeftHand");
                m_Animator["IdleLeftHand"].weight = .3f;                
            }    
        }
        if(task == TaskList.Attacking){
            if(rightHandObject != null){
                if(rightHandObject.tag == "Spear"){
                    if(leftHandObject == null){
                        m_Animator["AttackwSpear2Hands"].layer = 2;
                        m_Animator.Play("AttackwSpear2Hands");
                        m_Animator["AttackwSpear2Hands"].weight = .3f;
                    }
                    else{
                        m_Animator["AttackwSpear"].layer = 2;
                        m_Animator.Play("AttackwSpear");
                        m_Animator["AttackwSpear"].weight = .3f;
                    }
                }
                else if(rightHandObject.tag == "Sword"){
                    m_Animator["AttackwSwordRH"].layer = 2;
                    m_Animator.Play("AttackwSwordRH");
                    m_Animator["AttackwSwordRH"].weight = .3f;
                }
                else if(rightHandObject.tag == "Axe"){
                    m_Animator["AttackwAxeRH"].layer = 2;
                    m_Animator.Play("AttackwAxeRH");
                    m_Animator["AttackwAxeRH"].weight = .3f;
                }                
            }
            if(rightHandObject == null){

            }
            if(leftHandObject != null){

            }
            if(leftHandObject == null){

            } 
        }

        if(task == TaskList.PreparingForattack){
            if(rightHandObject != null){

            }
            if(rightHandObject == null){

            }
            if(leftHandObject != null){

            }
            if(leftHandObject == null){

            }
        }

        if(task == TaskList.ReadyForAttack){
            if(rightHandObject != null){

            }
            if(rightHandObject == null){

            }
            if(leftHandObject != null){

            }
            if(leftHandObject == null){

            }            
        }

        if(task == TaskList.Blocking){
            if(rightHandObject != null){

            }
            if(rightHandObject == null){

            }
            if(leftHandObject != null){
                if(leftHandObject.tag == "Shield"){
                    m_Animator.Play("BlockwShield");
                }
            }
            if(leftHandObject == null){

            }              
        }

        if(task == TaskList.CuttingTree || task == TaskList.ProducingTimber){
            if(rightHandObject != null){
                if(rightHandObject.tag == "Axe"){
                    m_Animator["CuttingTreeHorisontal"].layer = 2;
                    m_Animator.Play("CuttingTreeHorisontal");
                    m_Animator["CuttingTreeHorisontal"].weight = .3f;
                }             
            }
            if(rightHandObject == null){

            }  
        }

        if(task == TaskList.MovingLogToStorage || task == TaskList.MovingLogToLumberMill || task == TaskList.MovingTimberToStorage || task == TaskList.MovingTimberToStorage){    
            m_Animator["MovingLog"].layer = 2;
            m_Animator.Play("MovingLog");
            m_Animator["MovingLog"].weight = .3f; 
        }
    }
}
