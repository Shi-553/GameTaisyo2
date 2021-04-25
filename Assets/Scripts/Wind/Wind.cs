using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Wind 
{
    public Vector3 dir;
    public float value;

    public Wind(Vector3 dir, float value) {
        this.dir = dir;
        this.value = value;
    }
}
