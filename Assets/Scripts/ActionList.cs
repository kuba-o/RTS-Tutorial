using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActionList : MonoBehaviour
{

    public void Move(ref NavMeshAgent agent, ref NavMeshObstacle obstacle, ref RaycastHit hit, ref TaskList task)
    {
        obstacle.enabled = false;
        agent.SetDestination(hit.point);
        agent.enabled = true;

        Debug.Log("Walking");
        task = TaskList.Moving;
    }

    public void Harvest(ref NavMeshAgent agent, ref RaycastHit hit, ref TaskList task, ref GameObject targetNode)
    {
        agent.SetDestination(hit.collider.gameObject.transform.position);
        Debug.Log("harvesting");
        task = TaskList.Gathering;
        targetNode = hit.collider.gameObject;
    }
}
