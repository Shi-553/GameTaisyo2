using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialtext : MonoBehaviour
{
    public GameObject Intext;
    public GameObject Outtext;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Intext.SetActive(true);
        }
        if (other.CompareTag("Player"))
        {
            Outtext.SetActive(false);
        }
        if (other.CompareTag("Player"))
        {
            this.gameObject.SetActive(false);
        }

        
    }
}
