using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatPanelManager : MonoBehaviour
{
    Text patk;
    GameObject primary;

    void Start()
    {
        patk = GameObject.Find("PhysicalAttackDisplay").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        primary = GameObject.FindGameObjectWithTag("Player").GetComponent<InputManager>().selectedObject;
        if (primary != null)
        {
            patk.text = "PATK: " + primary.GetComponent<ObjectInfo>().health;

        }
        //patkDisplay.text = "PATK: " + patk;
    }
}
