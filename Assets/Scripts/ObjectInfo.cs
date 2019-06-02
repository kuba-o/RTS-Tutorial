using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectInfo : MonoBehaviour
{
    public TaskList task;
    public ResourceManager resourceManager;
    GameObject targetNode;
    public float velocity;


    public enum HeldResources {Stone};
    public NodeManager.ResourceTypes heldResourceType;
    public bool isSelected = false;
    public string objectName;
    public int heldResource;
    public int maxHeldResource;
    public bool isGathering = false;

    public GameObject[] drops;

    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
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
                Debug.Log("du[a");
                ReturnResources();

            }
            else
            {
                task = TaskList.Idle;
            }
        }
        if (heldResource >= maxHeldResource && (task == TaskList.Gathering || task == TaskList.Delivering))
        {
            ReturnResources();
        }

        if (Input.GetMouseButtonDown(1) && isSelected)
        {
            RightClick();
        }

        if (Input.GetMouseButtonDown(0) && isSelected)
        {
            isSelected = false;
        }
    }

    public void ReturnResources()
    {
        drops = GameObject.FindGameObjectsWithTag("Drops");
        agent.destination = GetClosestDropOff(drops).transform.position;
        //drop off point
        task = TaskList.Delivering;
    }

    GameObject GetClosestDropOff(GameObject[] dropOffs)
    {
        GameObject closestDrop = null;
        float closestDistance = Mathf.Infinity;
        Vector3 position = transform.position;

        foreach(GameObject targetDrop in dropOffs)
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
                agent.destination = hit.point;
                Debug.Log("Walking");
                task = TaskList.Moving;
            } else if (hit.collider.tag == "Resource")
            {
                targetNode = hit.collider.gameObject; 
                agent.destination = hit.collider.gameObject.transform.position;
                Debug.Log("harvesting");
                task = TaskList.Gathering;
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        GameObject hitObject = other.gameObject;

        if (hitObject.tag == "Resource" && task == TaskList.Gathering)
        {
            hitObject.GetComponent<NodeManager>().gatherers++;
            heldResourceType = hitObject.GetComponent<NodeManager>().resourceType;
            isGathering = true;
        } else if (hitObject.tag == "Drops" && task == TaskList.Delivering) {
            Debug.Log("puk");
            Debug.Log("puk");
            Debug.Log("puk");
            if (resourceManager.stone >= resourceManager.maxStone)
            {
                task = TaskList.Idle;
                
            } else
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
}
