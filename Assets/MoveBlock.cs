using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlock : MonoBehaviour
{
    [SerializeField] TransformBlock TransformBlock;
    Vector3 firstPosition;
    private void Start()
    {
        firstPosition = transform.position;
    }

    private void Update()
    {
        transform.position=(firstPosition+TransformBlock.GetCurrentAnimeValue());
    }

}
