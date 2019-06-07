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

    public ObjectInfo primary;

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
        GameObject selectedTmp = GameObject.FindGameObjectWithTag("Player").GetComponent<InputManager>().selectedObject;

        if (selectedTmp != null)
        {
            primary = selectedTmp.GetComponent<ObjectInfo>(); 
        } else
        {
            primary = null;
        }

        if (primary != null)
        {
            if (primary.maxEnergy <= 0)
            {
                EB.gameObject.SetActive(false);
            }

            unitName.text = primary.GetComponent<ObjectInfo>().objectName;
            health.text = "HP: " + primary.GetComponent<ObjectInfo>().health;
            energy.text = "EP: " + primary.GetComponent<ObjectInfo>().energy;

            HB.maxValue = primary.maxHealth;
            HB.value = primary.health;
            EB.maxValue = primary.maxEnergy;
            EB.value = primary.energy;


            patk.text = "PATK: " + primary.GetComponent<ObjectInfo>().patk;
            pdef.text = "PDEF: " + primary.GetComponent<ObjectInfo>().pdef;
            eatk.text = "PDEF: " + primary.GetComponent<ObjectInfo>().eatk;
            edef.text = "EDEF: " + primary.GetComponent<ObjectInfo>().edef;
            rank.text = "Rank: " + primary.GetComponent<ObjectInfo>().rank;
            kills.text = "Kills: " + primary.GetComponent<ObjectInfo>().kills;
            iconCam.enabled = true;


            InfoPanel.alpha = 1;
            InfoPanel.blocksRaycasts = true;
            InfoPanel.interactable = true;


        }
        else
        {
            InfoPanel.alpha = 0;
            InfoPanel.blocksRaycasts = false;
            InfoPanel.interactable = false;
            iconCam.enabled = false;
        }
    }
}
