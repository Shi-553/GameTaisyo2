using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hummer : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        var o = other.GetComponent<IOperatedHummerObject>();
        if (o != null)
        {
            o.Hit();
        }
    }

}
