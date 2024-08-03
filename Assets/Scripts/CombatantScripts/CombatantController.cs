using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatantController : MonoBehaviour
{
    // controls the overall combat mechanics for all combatants
    // right now, it mostly just holds onto data for the other combatant components

    [SerializeField] public SO_Combatant CombatantData;
    public CombatantController mainTarget;

    // TODO: EVENTS
    // On spawn, die, take damage, attack, move, etc.

    // SETUP //
    public BattleManager battleManager;
    public MessageLog messageLog;
    public bool canAttack = false;

    private void OnValidate()
    {
        if (battleManager == null) battleManager = FindAnyObjectByType<BattleManager>();
        if (messageLog == null) messageLog = FindAnyObjectByType<MessageLog>();


        CombatantData = GetComponent<CombatantController>().CombatantData;

        // set stats
        MaxHealth = CombatantData.baseMaxHealth;
        MaxMana = CombatantData.baseMaxMana;
        ManaRegen = CombatantData.baseManaRegen;
        Strength = CombatantData.baseStrength;
        Defence = CombatantData.baseDefence;
        AtkSpeed = CombatantData.baseAtkSpeed;
    }

    public float MaxHealth;
    public float MaxMana;
    public float ManaRegen;
    public float Strength;
    public float Defence;
    public float AtkSpeed;

    private void Start()
    {

    }
}
