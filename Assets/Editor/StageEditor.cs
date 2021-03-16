using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class StageEditor
{
    [MenuItem("Tools/SetBlock")]
    static void SetBlock() {
        Undo.RecordObjects(Selection.gameObjects.Select(o=>o.transform).ToArray(), "set");
        var mask = LayerMask.GetMask(new string[] { "Mebiusu" });


        foreach (var gameObject in Selection.gameObjects) {
            var transform = gameObject.transform;

            var forwerdRay = new Ray(transform.position - transform.forward, transform.forward);

            if (Physics.Raycast(forwerdRay, out var forwerdHit, Mathf.Infinity, mask)) {

                var forwardPoints = PointDistance.GetUpRight(forwerdHit, transform.up, transform.right);


                Vector3 upFV = forwardPoints.Up.normalized;


                transform.LookAt(transform.position - forwardPoints.Normal, upFV);
            }


             forwerdRay = new Ray(transform.position - transform.forward, transform.forward);



            if (Physics.Raycast(forwerdRay, out  forwerdHit, Mathf.Infinity, mask)) {

                var forwardPoints = PointDistance.GetUpRight(forwerdHit, transform.up, transform.right);



                transform.localPosition = forwerdHit.point+ forwardPoints.Normal *2;
            }

        }
    }
}
