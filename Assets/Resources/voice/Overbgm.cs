using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overbgm : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("GameBGM").GetComponent<AudioSource>().Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
