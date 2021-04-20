using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BeltConbeyor : MonoBehaviour
{
    public void Activation() {
        var colliders = Physics.OverlapBox(transform.position, transform.localScale / 2, transform.rotation);
        var objects = colliders.Where(c => !c.CompareTag("Mebiusu")&& !c.CompareTag("Player"));
        foreach (var obj in objects) {
            var trans = obj.transform;
            var pos = trans.localPosition;
            pos.z *= -1;
            var angle=trans.localEulerAngles;
            angle.x +=180;
            trans.localEulerAngles = angle;
            trans.localPosition = pos;
        }
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
