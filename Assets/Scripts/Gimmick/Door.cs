using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Transform m_Tranansform;

    // Start is called before the first frame update
    void Start()
    {
        m_Tranansform = gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    public void OpenDoor()
    {
        gameObject.SetActive(false);
    }

    public void CloseDoor()
    {
        gameObject.SetActive(true);
    }
}
