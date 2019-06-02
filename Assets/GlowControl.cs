using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowControl : MonoBehaviour
{
    Material mymat;
    // Start is called before the first frame update
    void Start()
    {
        mymat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        Color tmp = Color.red;

        if (tmp != Color.yellow)
        {
            tmp = Color.Lerp(Color.green, Color.yellow, Mathf.PingPong(Time.time, 1));
        } else
        {
            tmp =  Color.Lerp(Color.yellow, Color.green, Mathf.PingPong(Time.time, 1));
        }

        mymat.SetColor("_EmissionColor", tmp);
    }
}
