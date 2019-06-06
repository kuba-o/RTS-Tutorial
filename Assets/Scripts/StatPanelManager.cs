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
        //patkDisplay.text = "PATK: " + patk;
        patk.text = "PATK: " + primary.GetComponent<ObjectInfo>().health;
    }
}
