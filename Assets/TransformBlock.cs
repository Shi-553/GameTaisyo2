using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformBlock : MonoBehaviour
{
    [SerializeField] Vector3 beginValue;
    [SerializeField] Vector3 endValue;
    [SerializeField] AnimationCurve curve;
    // Start is called before the first frame update

    Vector3 GetCurrentAnimeValue()
    {

        var time = curve.Evaluate(Time.time);

       return  beginValue* (1 - time) + endValue* time;

    }

}
