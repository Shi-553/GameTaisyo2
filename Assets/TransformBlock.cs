using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TransformBlock 
{
    [SerializeField] Vector3 beginValue;
    [SerializeField] Vector3 endValue;
    [SerializeField] AnimationCurve curve;
    // Start is called before the first frame update

    public Vector3 GetCurrentAnimeValue()
    {

        var time = curve.Evaluate(Time.time);

       return  beginValue* (1 - time) + endValue* time;

    }

}
