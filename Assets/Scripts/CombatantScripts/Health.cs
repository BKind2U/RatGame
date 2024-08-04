using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    // manages health, damage and dying for all combatants

    private CombatantController Combatant;
    private SO_Combatant CombatantData;
    private MessageLog _messageLog;
    private BattleManager _battleManager;

    public bool isAlive = true;

    private float _currentHealth;
    [SerializeField] private Image _healthBar;

    private void OnValidate()
    {
        if (Combatant == null) Combatant = GetComponent<CombatantController>();
        if (_messageLog == null) _messageLog = FindObjectOfType<MessageLog>();
        if (_battleManager == null) _battleManager = FindObjectOfType<BattleManager>();
    }

    private void Start()
    {
        CombatantData = Combatant.CombatantData;
        _currentHealth = Combatant.CombatantData.baseMaxHealth;
    }



    public void Damage(DamageInfo damageInfo)
    {
        damageInfo.Amount -= Combatant.Defence; // apply defence stat
        damageInfo.Amount = Mathf.Max(1, damageInfo.Amount); // ensures damage isn't negative or 0

        _currentHealth -= damageInfo.Amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, CombatantData.baseMaxHealth);
        _messageLog.DisplayMessage(CombatantData.CharacterName + " takes " + damageInfo.Amount.ToString() + " " + damageInfo.DamageType.ToString() + " damage");

        UpdateHealthBar();

        if (_currentHealth <= 0) Die();
    }

    private void UpdateHealthBar()
    {
        _healthBar.fillAmount = _currentHealth / CombatantData.baseMaxHealth;
    }

    private void Die()
    {
        _messageLog.DisplayMessage(CombatantData.CharacterName + " died!");
        _battleManager.CheckConditions(Combatant);
        isAlive = false;
        Destroy(gameObject);
    }
}
