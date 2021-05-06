using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Item;

public class RepairItem : UseableItemBase {
    [SerializeField]
    int repairValue = 50;

    public override bool Use(GameObject player) {
        player.GetComponentInChildren<Player.PlayerHummer>(true).Repair(repairValue);

        return true;
    }
}
