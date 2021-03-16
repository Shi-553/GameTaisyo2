using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalingBlock : TransformBlock {
    Vector3 firstScale;
    void Start()
    {
        firstScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = (firstScale + GetCurrentAnimeValue());
    }

    private void OnDrawGizmos() {
        if (!UnityEditor.EditorApplication.isPlaying) {
            var child = transform.GetChild(0);
            transform.localScale += endValue;
            Gizmos.DrawMesh(child.GetComponent<MeshFilter>().sharedMesh, child.position, child.rotation, child.lossyScale);
            transform.localScale -= endValue;
        }
    }
}
