using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{

    public float stone;
    public float maxStone;

    public float iron;
    public float maxIron;

    public float power;
    public float maxPower;

    public float food;
    public float maxFood;

    public float population;
    public float maxPopulation;

    public Text stoneDisplay;
    public Text ironDisplay;
    public Text powerDisplay;
    public Text foodDisplay;
    public Text populationDisplay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        stoneDisplay.text = "" + stone + "/" + maxStone;
        ironDisplay.text = "" + iron + "/" + maxIron;
        powerDisplay.text = "" + power + "/" + maxPower;
        foodDisplay.text = "" + food + "/" + maxFood;
        populationDisplay.text = "" + population + "/" + maxPopulation;
    }
}
