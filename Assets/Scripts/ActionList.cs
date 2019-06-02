using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActionList : MonoBehaviour
{

    public void Move(ref NavMeshAgent agent, ref RaycastHit hit, ref TaskList task)
    {
        agent.destination = hit.point;
        Debug.Log("Walking");
        task = TaskList.Moving;
    }

    public void Harvest(ref NavMeshAgent agent, ref RaycastHit hit, ref TaskList task, ref GameObject targetNode)
    {
        agent.destination = hit.collider.gameObject.transform.position;
        Debug.Log("harvesting");
        task = TaskList.Gathering;
        targetNode = hit.collider.gameObject;
    }
}
