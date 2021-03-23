using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBlock : TransformBlock {
    Vector3 firstRotate;
    void Start()
    {
        firstRotate = transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localEulerAngles=(firstRotate + GetCurrentAnimeValue());
    }
    private void OnDrawGizmos() {
        if (!UnityEditor.EditorApplication.isPlaying) {
            var child = transform.GetChild(0);
            transform.localEulerAngles += endValue;
            Gizmos.DrawMesh(child.GetComponent<MeshFilter>().sharedMesh, child.position, child.rotation, child.lossyScale);
            transform.localEulerAngles -= endValue;
        }
    }
}
