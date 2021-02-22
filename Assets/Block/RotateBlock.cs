using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBlock : MonoBehaviour
{
    [SerializeField] TransformBlock TransformBlock;
    Vector3 firstRotate;
    void Start()
    {
        firstRotate = transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(firstRotate + TransformBlock.GetCurrentAnimeValue());
    }
}
