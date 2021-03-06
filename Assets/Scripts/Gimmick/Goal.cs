﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour, IOperatedPlayerObject
{
    public void Show() {
        gameObject.SetActive(true);
    }
    void IOperatedPlayerObject.Hit() {
        StageManager.Instance.ClearStage(Player.PlayerCore.Instance.Status);
    }

    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 1.5f));
    }

}