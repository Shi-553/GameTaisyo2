using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PushKeepSwitch : MonoBehaviour
{
    [SerializeField]
    List<GameObject> list;

    bool isActive=false;


    void Update() {

        var mebiusuLayer = LayerMask.GetMask(new string[] { "Mebiusu" });

        var colliders = Physics.OverlapBox(transform.position, transform.localScale / 2, transform.rotation);
        var objects = colliders.Where(c => c.CompareTag("Block") || c.CompareTag("Player"));

        if(isActive!=objects.Any()){

            foreach (var obj in list) {
                obj.SetActive(!objects.Any());
            }
            isActive = objects.Any();
        }
    }
}
