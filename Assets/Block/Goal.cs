using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Goal : MonoBehaviour, IOperatedPlayerObject
{
    //Inspectorでキャラクターとゴールオブジェクトの指定を出来る様にします。
    [SerializeField] GameObject chara;

    void IOperatedPlayerObject.Hit(){
        chara.SetActive(false);
        Debug.Log("GOAL");
    }

}