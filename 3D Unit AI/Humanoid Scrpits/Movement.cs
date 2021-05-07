using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class Movement : MonoBehaviour{
    public float rotationSpeed;
    private ActionList AL; //List for movenment
    public TaskList task; //List for Tasks
    public Select select;
    public UnitUI unitUI;
    public UnitMovement unitMovement;
    public ObjectInfo objectInfo;
    private NodeManager Gathers; 
    private Quaternion targetRotation;
    public NavMeshAgent agent;
    public GameObject mainCamera; 
    public GameObject endPoint;
    public GameObject cloneEndPoint;
    public List<GameObject> cloneEndPoints = new List<GameObject>();
    public List<GameObject> target = new List<GameObject>();
    public Animation m_Animator;
    public Transform rightHand;
    public Transform leftHand;
    public GameObject targetNode;
    public GameObject[] drops;
    public bool standGround = false;
    public bool standGroundDefend = false;
    public bool followTarget;
    public bool goToNextEndPoint = false; 
    public bool patrolActive = false;
    public bool isGathering = false;
    public bool treeDestroyed = false;
    public bool pickingUp = false;
    public bool goingToTree = false;
    public bool onlyMove = false;
    public float wanderRadius = 2;
    public GameObject log;

    // Start is called before the first frame update
    void Start(){
        agent = GetComponent<NavMeshAgent>();
        AL = FindObjectOfType<ActionList>();
        m_Animator = gameObject.GetComponent<Animation>();
        m_Animator.Play("Idle");
        followTarget = false;
        StartCoroutine(Agro());
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
        if(task == TaskList.PickingUp){
            drops = GameObject.FindGameObjectsWithTag("Woodenlog");
            //If there arent anymore woodenlogs left in the world, swtich task to FindObject
            if (drops.Length <= 0) {
                task = TaskList.FindObject;
            }
            agent.destination = GetClosestDropOff(drops).transform.position;
            drops = null; 
            Debug.Log("Found the Woodenlog");
        }
        if(task == TaskList.Delivering){
            drops = GameObject.FindGameObjectsWithTag("Stockpile");
            agent.destination = GetClosestDropOff(drops).transform.position;
            drops = null;
            Debug.Log("Delivering");
        }
        if(task == TaskList.FindObject){   
            drops = GameObject.FindGameObjectsWithTag("Resource");
            agent.destination = GetClosestDropOff(drops).transform.position;
            drops = null;
            task = TaskList.Gathering;
            Debug.Log("Going to the closest tree");
            m_Animator.Play("Walking");
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
                        if (followTarget == false){
                            m_Animator.Play("Idle");
                        }
                        if (onlyMove == true){
                            onlyMove = false;
                            StartCoroutine(Agro());
                        }
                        if (patrolActive == true){
                            goToNextEndPoint = true;
                        }
                        if (cloneEndPoints.Count == 0){
                            Destroy(cloneEndPoint);
                        } 
                        if (cloneEndPoints.Count > 0 && patrolActive == false){
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
                        if (patrolActive == true){
                            patrolActive = false;
                        }
                        if (standGround == false){
                            StartCoroutine(Agro());
                        }
                        cloneEndPoints.Clear();
                        followTarget = false;
                        onlyMove = true;
                        isGathering = false;
                        AL.Move(agent, hit, task);
                        task = TaskList.Moving;         
                        Vector3 newHit = hit.point;
                        newHit.y += 0.1f;
                        cloneEndPoint = Instantiate(endPoint, newHit, Quaternion.identity); //Creates EndPoint for unit destination
                        Animator();
                    }
                    //Adds endpoints to a list which unit will walk to from 0 and up 
                    if (Input.GetKey(KeyCode.LeftShift)){
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
                    task = TaskList.Moving; 
                    Debug.Log("Unit moving towards enemy");
                    Animator();
                }
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

    GameObject GetClosestDropOff(GameObject[] dropOffs){
        GameObject closestDrop = null;
        float closestDistance = Mathf.Infinity;
        Vector3 position = transform.position;

        foreach (GameObject targetDrop in dropOffs){
            Vector3 direction = targetDrop.transform.position - position;
            float distance = direction.sqrMagnitude;
            if (distance < closestDistance){
                closestDistance = distance;
                closestDrop = targetDrop;
            }
        }
        return closestDrop;
    }

    public void OnTriggerEnter(Collider other){
        GameObject hitObject = other.gameObject;

        if (hitObject.tag == "Resource" && task == TaskList.FindObject){
            task = TaskList.Gathering;         
        }
        if (hitObject.tag == "Resource" && task == TaskList.Gathering){
            Debug.Log("Working on tree");
            isGathering = true;
            m_Animator.Play("Cutting");
        }
        if (hitObject.tag == "Occupied" && task == TaskList.Gathering){
            task = TaskList.FindObject;
        }
        if (hitObject.tag == "Stockpile" && task == TaskList.Delivering){
            Debug.Log("Object has been delivered");

            if (GameObject.FindGameObjectsWithTag("Woodenlog").Length == 0){
                Debug.Log("No more woodenlogs - Finding tree");
                task = TaskList.FindObject;
            }
            else{
                Debug.Log("Found a new Woodenlog");
                task = TaskList.PickingUp;
                pickingUp = true;
            }
        }
        //Spawns wooden log on the NPC
        if (other.gameObject.tag == "Woodenlog" && pickingUp == true){
            GameObject go = Instantiate(log, new Vector3(transform.position.x + 0.6f, transform.position.y + 0.6f, transform.position.z), Quaternion.Euler(90, 0, 0)) as GameObject;
            drops = GameObject.FindGameObjectsWithTag("Selectable");
            go.transform.parent = GetClosestDropOff(drops).transform; 
            drops = null;
            Debug.Log("Delivering");
            task = TaskList.Delivering;
            pickingUp = false;
        }
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

    IEnumerator FaceEnemy(float rotationSpeed, Transform target){
        targetRotation = Quaternion.LookRotation(target.position - transform.position);
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
        Debug.Log("Looking at enemy");  
    }
    
    //Follows target and attacks it
    IEnumerator FollowTarget(GameObject newTarget){
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
                    m_Animator.Play("Idle");
                    goToNextEndPoint = true;
                }
                StartCoroutine(Agro());                      
                break;
            }
            if (followTarget == false || onlyMove == true){
                break;
            }
            dist = Vector3.Distance(newTarget.transform.position, transform.position);             
            if (dist > 50){
                followTarget = false;
                task = TaskList.Idle;
                if(patrolActive == true){
                    m_Animator.Play("Idle");
                    goToNextEndPoint = true;
                }
                StartCoroutine(Agro());                
                break;
            }
            if (dist < 50){
                agent.SetDestination(newTarget.transform.position);
                if (dist < 10){
                    m_Animator.Play("DangerClose");
                    task = TaskList.Moving;      
                }
                if (dist < 4 && dist > 10){
                    m_Animator.Play("AttackMode");
                    task = TaskList.Moving;  
                }
                if (dist < 3){
                    agent.ResetPath();
                    StartCoroutine(FaceEnemy(rotationSpeed, newTarget.transform));
                    m_Animator.Play("AttackSpear1");
                    task = TaskList.Attacking;
                    yield return new WaitForSeconds(.5f);
                    newTarget.GetComponent<ObjectInfo>().TakeDamage(5, gameObject);
                }  
            }
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
                m_Animator.Play("Idle");
                if(patrolActive == true){
                    goToNextEndPoint = true;
                }
                break;
            }
            if(dist <= 5){
                agent.SetDestination(attacker.transform.position);
                if(dist <= 3){
                    agent.ResetPath();
                    StartCoroutine(FaceEnemy(rotationSpeed, attacker.transform));
                    m_Animator.Play("AttackSpear1");
                    task = TaskList.Attacking;
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

    public IEnumerator Agro(){
        target.Clear();
        int layer;
        if (objectInfo.team == 12){
            layer = 1 << 11;
        }
        else{
            layer = 1 << 12;
        }
        while (standGround == false && onlyMove == false){
            yield return new WaitForSeconds(5); 
            Collider[] agroRadius = Physics.OverlapSphere(transform.position, 10, layer, QueryTriggerInteraction.Collide);
            if (0 < agroRadius.Length){
                if (target.Count == 0){
                    target.Add(agroRadius[0].gameObject);
                    StartCoroutine(FollowTarget(target[0]));
                    Debug.Log("Agro: Enemy nearby. Attacking enemy"); 
                    yield return new WaitUntil(() => target[0] == null);
                    if(patrolActive == true){
                        m_Animator.Play("Idle");
                        goToNextEndPoint = true;
                    }  
                }
            }
            else{   
                Animator();           
                Debug.Log("No enemies neaby");               
            }
        }       
    }

    //Checks which weapon is equipped and chooses the correct animation for the unit
    public void Animator(){
        foreach (Transform child in rightHand){
            if(child.CompareTag("Spear")){
                m_Animator.Play("Relaxed");
            }
            else{
                m_Animator.Play("Walking");
            }
        }
    }
    public void NewTask(){
        task = TaskList.PickingUp;
        isGathering = false;
        pickingUp = true;
    }
}
