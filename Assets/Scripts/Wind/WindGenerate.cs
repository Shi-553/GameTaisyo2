using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindGenerate : MonoBehaviour
{
    [SerializeField]
    float power = 1;
    [SerializeField]
    LayerMask layer;
    private void Update()
    {

        var pos = transform.position;
        pos -= transform.up * transform.localScale.y*2;

        if (Physics.BoxCast(pos, transform.localScale / 2, transform.up, out RaycastHit hit,Quaternion.identity,Mathf.Infinity, layer))
        {
            IWindAffectable windaffectable = hit.transform.GetComponent<IWindAffectable>();
            if(windaffectable != null)
            {
                windaffectable.AffectWind(new Wind(transform.up, power));
            }
        }
    }
}
