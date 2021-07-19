using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActionList : MonoBehaviour{

    public void Move(NavMeshAgent agent, RaycastHit hit, TaskList task){
        agent.destination = hit.point;
        Debug.Log("Moving");
        task = TaskList.Moving;
    }

    public void Harvest(NavMeshAgent agent, RaycastHit hit, TaskList task, GameObject targetNode){
        agent.destination = hit.collider.gameObject.transform.position;
        Debug.Log("Harvesting");
        task = TaskList.Gathering;
        targetNode = hit.collider.gameObject;
    }

    public void Delivery(NavMeshAgent agent, RaycastHit hit, TaskList task, GameObject targetNode){
        agent.destination = hit.collider.gameObject.transform.position;
        Debug.Log("Delivering");
        task = TaskList.Delivering;
        targetNode = hit.collider.gameObject;
    }
}
