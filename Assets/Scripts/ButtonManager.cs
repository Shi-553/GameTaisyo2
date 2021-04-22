using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    [SerializeField]
    Transform selectImage;
    int selectIndex = 0;
    bool pressed = false;


    void Update()
    {
        float v = Input.GetAxisRaw("Vertical");

        if(!pressed)
        {
            if (v > 0)
            {
                selectIndex -= 1;
                pressed = true;
            }
            if (v < 0)
            {
                selectIndex += 1;
                pressed = true;
            }
        }
        if(v==0)
        {
            pressed = false;
        }
        
        if(selectIndex < 0)
        {
            selectIndex = transform.childCount - 1;
        }
        if (selectIndex > transform.childCount - 1)
        {
            selectIndex = 0;
        }
        selectImage.position = transform.GetChild(selectIndex).position;


        if(Input.GetButtonDown("Submit"))
        {
            UnityEngine.EventSystems.BaseEventData data = new UnityEngine.EventSystems.BaseEventData(UnityEngine.EventSystems.EventSystem.current);
            transform.GetChild(selectIndex).GetComponent<Button>().OnSubmit(data);
        }
    }
}
