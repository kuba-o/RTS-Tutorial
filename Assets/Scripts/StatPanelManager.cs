using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatPanelManager : MonoBehaviour
{
    Text unitName;

    Text health;
    Text energy;
    Text patk;
    Text pdef;
    Text eatk;
    Text edef;
    Text rank;
    Text kills;
    Slider HB;
    Slider EB;

    Camera iconCam;
    CanvasGroup InfoPanel;

    public ObjectInfo objectInfo;
    public GameObject primary;

    void Start()
    {
        unitName = GameObject.Find("UnitName").GetComponent<Text>();
        health = GameObject.Find("HealthDisplay").GetComponent<Text>();
        energy = GameObject.Find("EnergyDisplay").GetComponent<Text>();
        patk = GameObject.Find("PhysicalAttackDisplay").GetComponent<Text>();
        pdef = GameObject.Find("PhysicaDefenceDisplay").GetComponent<Text>();
        eatk = GameObject.Find("EnergyAttackDamage").GetComponent<Text>();
        edef = GameObject.Find("EnergyDefenceDisplay").GetComponent<Text>();
        rank = GameObject.Find("RankDisplay").GetComponent<Text>();
        kills = GameObject.Find("KillsDisplay").GetComponent<Text>();
        HB = GameObject.Find("HealthBar").GetComponent<Slider>();
        EB = GameObject.Find("EnergyBar").GetComponent<Slider>();
        iconCam = gameObject.GetComponentInChildren<Camera>();
        InfoPanel = GameObject.Find("UnitPanel").GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        primary = GameObject.FindGameObjectWithTag("Player").GetComponent<InputManager>().primary;

        if (primary != null)
        {
            objectInfo = primary.GetComponent<ObjectInfo>(); 
        }

        if (primary != null)
        {
            if (objectInfo.maxEnergy <= 0)
            {
                EB.gameObject.SetActive(false);
            }

            unitName.text = objectInfo.GetComponent<ObjectInfo>().objectName;
            health.text = "HP: " + objectInfo.GetComponent<ObjectInfo>().health;
            energy.text = "EP: " + objectInfo.GetComponent<ObjectInfo>().energy;

            HB.maxValue = objectInfo.maxHealth;
            HB.value = objectInfo.health;
            EB.maxValue = objectInfo.maxEnergy;
            EB.value = objectInfo.energy;


            patk.text = "PATK: " + objectInfo.GetComponent<ObjectInfo>().patk;
            pdef.text = "PDEF: " + objectInfo.GetComponent<ObjectInfo>().pdef;
            eatk.text = "PDEF: " + objectInfo.GetComponent<ObjectInfo>().eatk;
            edef.text = "EDEF: " + objectInfo.GetComponent<ObjectInfo>().edef;
            rank.text = "Rank: " + objectInfo.GetComponent<ObjectInfo>().rank;
            kills.text = "Kills: " + objectInfo.GetComponent<ObjectInfo>().kills;
            primary.GetComponentInChildren<Camera>().enabled = true;
            //iconCam.enabled = true;


            InfoPanel.alpha = 1;
            InfoPanel.blocksRaycasts = true;
            InfoPanel.interactable = true;


        }
        else
        {
            InfoPanel.alpha = 0;
            InfoPanel.blocksRaycasts = false;
            InfoPanel.interactable = false;
            //primary.GetComponent<Camera>().enabled = false;
            //iconCam.enabled = false;
        }
    }
}
