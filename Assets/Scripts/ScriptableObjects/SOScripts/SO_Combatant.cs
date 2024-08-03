using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Combatant")]
public class SO_Combatant : ScriptableObject
{
    public string CharacterName  = "BaseCombatant";
    public Team CombatantTeam;
    public Team CombatantFoeTeam;
    public float baseMaxHealth;
    public float baseMaxMana;
    public float baseManaRegen;
    public float baseStrength;
    public float baseDefence;
    public float baseAtkSpeed;

}
