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

    public CanvasGroup InfoPanel;
    public Text nameDisplay;

    public int health;
    public int maxHealth;
    public Slider HB;
    public Text healthDisplay;

    public int energy;
    public int maxEnergy;
    public Slider EB;
    public Text energyDisplay;

    public int patk;
    public int pdef;
    public int eatk;
    public int edef;
    public Text patkDisplay;
    public Text pdefDisplay;
    public Text eatkDisplay;
    public Text edefDisplay;

    public int kills;
    public Text killDisplay;

    public RankList rank;
    public Text rankDisplay;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isSelected)
        {
            Debug.Log("A");
        }
        selectionIndicator.SetActive(isSelected);
        if (maxEnergy <= 0)
        {
            EB.gameObject.SetActive(false);
        }
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        iconCam.SetActive(isSelected);
        HB.maxValue = maxHealth;
        HB.value = health;
        EB.maxValue = maxEnergy;
        EB.value = energy;

        nameDisplay.text = objectName;

        healthDisplay.text = "HP: " + health;
        energyDisplay.text = "EP: " + energy;
        patkDisplay.text = "PATK: " + patk;
        pdefDisplay.text = "PDEF: " + pdef;
        eatkDisplay.text = "EATK: " + eatk;
        edefDisplay.text = "EDEF: " + edef;
        rankDisplay.text = "Rank: " + rank.ToString();
        killDisplay.text = "Kills: " + kills;


        if (isSelected)
        {
            InfoPanel.alpha = 1;
            InfoPanel.blocksRaycasts = true;
            InfoPanel.interactable = true;
        } else
        {
            InfoPanel.alpha = 0;
            InfoPanel.blocksRaycasts = false;
            InfoPanel.interactable = false;
        }
    }

}
