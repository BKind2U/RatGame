using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicAttack : MonoBehaviour
{
    // gives instructions for a combatant's Basic Attack
    // basic attacks occur on a cooldown which occurs in the CombatantController
    // gets attack info from SO


    [SerializeField] private SO_Attack _attackData;

    [SerializeField] private Image _atkBar;

    public DamageInfo damageInfo;

    private BattleManager _battleManager;
    [SerializeField] private CombatantController _parentController;
    private SO_Combatant _combatantData;
    private MessageLog _messageLog;

    private void OnValidate()
    {
        // get managers
        if (_messageLog == null) _messageLog = _parentController.messageLog;
        if (_battleManager == null) _battleManager = _parentController.battleManager;
        if (_combatantData == null) _combatantData = _parentController.CombatantData;
    }

    private void Start()
    {

        StartCoroutine(Attacking());
    }

    // call this to find a target using the provided method
    public CombatantController FindTarget(TargetType type)
    {
        // contains all the different targeting type functionality.
        // returns CombatantController

        // sets a target to attack based on targeting type
        switch (type)
        {
            case TargetType.Closest:
                return FindClosestEnemy(_battleManager.allCombatants, gameObject.transform.position);
            case TargetType.Farthest:
                return FindFarthestEnemy(_battleManager.allCombatants, gameObject.transform.position);
            case TargetType.Random:
                return FindRandomEnemy(_battleManager.allCombatants);
            default: return null;
        }


    }
    // targeting types
    CombatantController FindClosestEnemy(List<CombatantController> combatants, Vector3 referencePosition)
    {
        CombatantController closest = null;
        float minDistance = Mathf.Infinity;

        foreach (CombatantController obj in combatants)
        {
            // make sure the target is of the target team!
            if (obj != null && obj.CombatantData.CombatantTeam == _combatantData.CombatantFoeTeam)
            {
                float distance = Vector3.Distance(referencePosition, obj.transform.position);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = obj;
                }
            }
        }

        return closest;
    }

    CombatantController FindFarthestEnemy(List<CombatantController> combatants, Vector3 referencePosition)
    {
        CombatantController farthest = null;
        float minDistance = 0.5f;

        foreach (CombatantController obj in combatants)
        {
            // make sure the target is of the target team!
            if (obj != null && obj.CombatantData.CombatantTeam == _combatantData.CombatantFoeTeam)
            {
                float distance = Vector3.Distance(referencePosition, obj.transform.position);

                if (distance > minDistance)
                {
                    minDistance = distance;
                    farthest = obj;
                }
            }
        }

        return farthest;
    }

    CombatantController FindRandomEnemy(List<CombatantController> combatants)
    {
        CombatantController randomEnemy = null;


        SO_Combatant enemyData;

        while (randomEnemy == null)
        {
            int randomValue = Random.Range(0, combatants.Count);
            CombatantController pickedEnemy = combatants[randomValue];

            randomEnemy = combatants[randomValue];

            // make sure target is on the intended team
            enemyData = randomEnemy.CombatantData;
            if (enemyData.CombatantTeam == _combatantData.CombatantFoeTeam) randomEnemy = pickedEnemy;
        }

        return randomEnemy;
    }


    public IEnumerator Attacking()
    {
        while (true)
        {
            if (_parentController.canAttack)
            {
                // fill up the bar
                _atkBar.fillAmount += Time.deltaTime * _parentController.AtkSpeed;

                // when full, do attack and reset bar
                if (_atkBar.fillAmount == 1)
                {
                    _atkBar.fillAmount = 0;
                    PerformAttack(null); // can put null, since it'll find the target based on default target type
                }
            }
        yield return null;
        }
    }


    // call this to perform an attack against the target
    public void PerformAttack(CombatantController target)
    {
        // if this isn't a compelled attack, use the targeting type of the attached attack.
        if (target == null) target = FindTarget(_attackData.targetType);


        DamageInfo damageInfo = new DamageInfo(_parentController.CombatantData.baseStrength * _attackData.strModifier, target, _parentController, _attackData.damageType, _parentController.CombatantData.CombatantTeam);

        if (target == null)
        {
            Debug.Log(gameObject.name + " target not found");
            return;
        }

        _messageLog.DisplayMessage(_parentController.CombatantData.CharacterName + " attacks " + target.CombatantData.CharacterName + " using " + _attackData.attackName);

        Health targetHealth = target.GetComponent<Health>();

        targetHealth.Damage(damageInfo);
    }
}
