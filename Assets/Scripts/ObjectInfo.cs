using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ObjectInfo : MonoBehaviour
{
    public GameObject iconCam;
    public GameObject selectionIndicator;

    public bool isUnit;

    public bool isSelected = false;
    public string objectName;

    public int health;
    public int maxHealth;

    public int energy;
    public int maxEnergy;

    public int patk;
    public int pdef;
    public int eatk;
    public int edef;
    public int kills;
    public RankList rank;


    void Start()
    {

    }

    void Update()
    {

        if (health <= 0)
        {
            Destroy(gameObject);
        }

    }

}
