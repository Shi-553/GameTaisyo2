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
}
