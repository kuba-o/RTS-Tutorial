using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public enum ResourceTypes {Stone}
    public ResourceTypes resourceType;

    public List<ObjectInfo> harvesters;

    public float harvestTime;
    public float availableResource;
    public int gatherers = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ResourceTick());
    }

    // Update is called once per frame  
    void Update()
    {
        if (availableResource <= 0)
        {
            foreach(ObjectInfo harvester in harvesters)
            {
                harvester.isGathering = false;
                harvester.ReturnResources();
            }
            Destroy(gameObject);
        }
    }

    public void ResourceGather()
    {
        if (gatherers != 0)
        {
            availableResource-=gatherers;
        }
    }

    IEnumerator ResourceTick()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            ResourceGather();
        }
    }
}
