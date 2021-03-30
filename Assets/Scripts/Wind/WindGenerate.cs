using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindGenerate : MonoBehaviour
{
    [SerializeField]
    Wind wind;
    
    private void Update()
    {

        if (Physics.BoxCast(transform.position, transform.localScale / 2, transform.forward, out RaycastHit hit))
        {
            IWindAffectable windaffectable = hit.transform.GetComponent<IWindAffectable>();
            if(windaffectable != null)
            {
                windaffectable.AffectWind(wind);
            }
        }
    }
}
