using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalSwitch : MonoBehaviour, IOperatedHummerObject {
    [SerializeField] Goal goal;

    void IOperatedHummerObject.Hit() {
        goal.Show();

        gameObject.SetActive(false);
    }
}
