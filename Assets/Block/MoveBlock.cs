using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlock : MonoBehaviour
{
    [SerializeField] TransformBlock TransformBlock;
    Vector3 firstPosition;
    private void Start()
    {
        firstPosition = transform.localPosition;
    }

    private void Update()
    {
        transform.localPosition = (firstPosition+TransformBlock.GetCurrentAnimeValue());
    }

}
