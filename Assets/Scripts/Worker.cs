using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Worker : MonoBehaviour
{
    public TaskList task;
    public ActionList actionList;
    public GameObject targetNode;
    public ResourceList resourceType;
    public ResourceManager resourceManager;
    public bool isGathering = false;
    private NavMeshAgent agent;
    private NavMeshObstacle obstacle;
    public int heldResource;
    public int maxHeldResource;
    public GameObject[] drops;
    public float harvestingDistance;
    public float distanceToTarget;

    void Start()
    {
        resourceManager = GameObject.FindGameObjectWithTag("Player").GetComponent<ResourceManager>();
        task = TaskList.Idle;
        actionList = GetComponent<ActionList>();
        agent = GetComponent<NavMeshAgent>();
        obstacle = GetComponent<NavMeshObstacle>();
        StartCoroutine(GatherTick());
    }

    void Update()
    {
        if (task == TaskList.Gathering)
        {
            distanceToTarget = Vector3.Distance(transform.position, targetNode.transform.position);
            if (distanceToTarget <= harvestingDistance)
            {
                Harvest();
            }
        }

        if (task == TaskList.Delivering)
        {
            distanceToTarget = Vector3.Distance(GetClosestDropOff(drops).transform.position, transform.position);
            if (distanceToTarget <= 7f)
            {
                StoreResources();
            }
        }

        
        if (targetNode == null)
        {
            if (heldResource != 0)
            {
                StartCoroutine(ReturnResources());
            }
            else
            {
                //task = TaskList.Idle;
            }
        }

        if (heldResource >= maxHeldResource && (task == TaskList.Gathering || task == TaskList.Harvesting))
        {
            StartCoroutine(ReturnResources());
        }

        if (Input.GetMouseButtonDown(1) && GetComponent<ObjectInfo>().isSelected)
        {
            RightClick();
        }

        if (Input.GetMouseButtonDown(0) && GetComponent<ObjectInfo>().isSelected)
        {
            GetComponent<ObjectInfo>().isSelected = false;
        }
    }

    GameObject GetClosestDropOff(GameObject[] dropOffs)
    {
        GameObject closestDrop = null;
        float closestDistance = Mathf.Infinity;
        Vector3 position = transform.position;

        foreach (GameObject targetDrop in dropOffs)
        {
            Vector3 direction = targetDrop.transform.position - position;
            float distance = direction.sqrMagnitude;

            if (distance < closestDistance)
            {
                closestDrop = targetDrop;
                closestDistance = distance;
            }
        }


        return closestDrop;
    }

    public void RightClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.collider.tag == "Ground")
            {
                actionList.Move(ref agent, ref obstacle, ref hit, ref task);
            }
            else if (hit.collider.tag == "Resource")
            {
                actionList.Harvest(ref agent, ref hit, ref task, ref targetNode);
            } else if (hit.collider.tag == "Drops")
            {
                StartCoroutine(ReturnResources());
            }
        }
    }

    public void Harvest()
    {
        isGathering = true;
        targetNode.GetComponent<NodeManager>().gatherers++;
        resourceType = targetNode.GetComponent<NodeManager>().resourceType;
        agent.destination = agent.transform.position;
        agent.enabled = false;
        obstacle.enabled = true;
        task = TaskList.Harvesting;
    }

    public void StoreResources()
    {
        if (resourceManager.stone >= resourceManager.maxStone)
        {
            task = TaskList.Idle;

        }
        else
        {
            resourceManager.stone += heldResource;
            heldResource = 0;
            task = TaskList.Gathering;
            agent.destination = targetNode.transform.position;
        }
    }

    IEnumerator GatherTick()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            if (isGathering)
            {
                heldResource++;
            }
        }
    }

    public IEnumerator ReturnResources()
    {
        bool returnResourcesRoutineShouldFire = true;
        if (returnResourcesRoutineShouldFire)
        {
            if (obstacle.enabled)
            {
                obstacle.enabled = false;
                yield return true;
            }

            Debug.Log("ASD");
            isGathering = false;
            targetNode.GetComponent<NodeManager>().gatherers--;
            agent.enabled = true;
            drops = GameObject.FindGameObjectsWithTag("Drops");

            agent.SetDestination(GetClosestDropOff(drops).transform.position);
            task = TaskList.Delivering;
            yield break;
        }

    }
}
