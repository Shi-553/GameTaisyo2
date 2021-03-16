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
}
