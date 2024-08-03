using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSlot : MonoBehaviour
{
    public Team team;
    public GameObject icon;


    // make sure things are in the right position
    public void UpdateSlot()
    {
        // Get the thing that should be in this slot -> this slot's index, but put into the positionmanager's combatant list
        CombatantController combatant = PositionManager.instance.combatants[transform.GetSiblingIndex()];
        if (combatant != null)
        {
            // move it to in this slot
            combatant.transform.position = gameObject.transform.position;
        }

    }

    //set icon colour on validate
}
