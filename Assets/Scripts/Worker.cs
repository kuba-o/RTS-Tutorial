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
    public int heldResource;
    public int maxHeldResource;
    public GameObject[] drops;
    public float velocity;

    // Start is called before the first frame update
    void Start()
    {
        resourceManager = GameObject.FindGameObjectWithTag("Player").GetComponent<ResourceManager>();
        task = TaskList.Idle;
        actionList = GetComponent<ActionList>();
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(GatherTick());
    }

    // Update is called once per frame
    void Update()
    {
        velocity = agent.velocity.magnitude / agent.speed;
        if (targetNode == null)
        {
            if (heldResource != 0)
            {
                ReturnResources();
            }
            else
            {
                //task = TaskList.Idle;
            }
        }

        if (heldResource >= maxHeldResource && (task == TaskList.Gathering || task == TaskList.Delivering))
        {
            ReturnResources();
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
                actionList.Move(ref agent, ref hit, ref task);
                //agent.destination = hit.point;
                //Debug.Log("Walking");
                //task = TaskList.Moving;


            }
            else if (hit.collider.tag == "Resource")
            {
                actionList.Harvest(ref agent, ref hit, ref task, ref targetNode);
                /*
                agent.destination = hit.collider.gameObject.transform.position;
                Debug.Log("harvesting");
                task = TaskList.Gathering;
                targetNode = hit.collider.gameObject;
                */
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        GameObject hitObject = other.gameObject;

        if (hitObject.tag == "Resource" && task == TaskList.Gathering)
        {
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<NavMeshObstacle>().enabled = true;


            hitObject.GetComponent<NodeManager>().gatherers++;
            resourceType = hitObject.GetComponent<NodeManager>().resourceType;
            isGathering = true;

        }
        else if (hitObject.tag == "Drops" && task == TaskList.Delivering)
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
    }

    public void OnTriggerExit(Collider other)
    {
        GameObject hitObject = other.gameObject;

        if (hitObject.tag == "Resource")
        {
            hitObject.GetComponent<NodeManager>().gatherers--;
            isGathering = false;
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

    public void ReturnResources()
    {
        GetComponent<NavMeshObstacle>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = true;
        drops = GameObject.FindGameObjectsWithTag("Drops");
        agent.destination = GetClosestDropOff(drops).transform.position;
        //drop off point
        task = TaskList.Delivering;
    }
}
