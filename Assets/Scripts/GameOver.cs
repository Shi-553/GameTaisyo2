using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField]
    ButtonManager buttonManager;
    [SerializeField]
    Transform relifeRetry;
    void Start()
    {
        if (StageManager.Instance.IsDeathHpUp()) {
            var root = buttonManager.transform;
            var childPos1 = root.GetChild(0).localPosition;
            childPos1.y -= 150;
            root.GetChild(0).localPosition= childPos1;
            var childPos2 = root.GetChild(1).localPosition;
            childPos2.y -= 130;
            root.GetChild(1).localPosition = childPos2;


            relifeRetry.gameObject.SetActive(true);
            relifeRetry.SetParent(root);
            relifeRetry.SetAsFirstSibling();

            buttonManager.ButtonReset();
        }
    }
}
