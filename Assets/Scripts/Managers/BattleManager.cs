using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BattleManager : MonoBehaviour
{
    private static BattleManager _instance;
    public static BattleManager Instance { get { return _instance; } }

    public GameState _currentGameState = GameState.NotCombat;
    public enum GameState
    {
        Combat,
        NotCombat
    }

    // Singleton status

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }



    public List<CombatantController> playerTeam;
    public List<CombatantController> enemyTeam;
    public List<CombatantController> allCombatants;

    private void Start()
    {
        GetCombatants();

        OnCombatStart.AddListener(StartCombat);
    }

    private MessageLog messageLog;

    private void OnValidate()
    {
        if (messageLog == null) messageLog = FindAnyObjectByType<MessageLog>();
    }
    private void GetCombatants()
    {
        // clear all the lists
        playerTeam.Clear();
        enemyTeam.Clear();
        allCombatants.Clear();

        CombatantController[] allFighters = FindObjectsOfType<CombatantController>();

        foreach (CombatantController combatant in allFighters)
        {
            // sort into teams
            if (combatant.CombatantData.CombatantTeam == Team.Player) playerTeam.Add(combatant);
            if (combatant.CombatantData.CombatantTeam == Team.Enemy) enemyTeam.Add(combatant);
            allCombatants.Add(combatant);
        }
    }

    public void CheckConditions(CombatantController deadUnit)
    {
        // update teams
        GetCombatants();

        // check if the battle has been won or lost. Called when any combatant dies. 
        switch (deadUnit.CombatantData.CombatantTeam)
        {
            case Team.Player:
                playerTeam.Remove(deadUnit);
                if (playerTeam.Count == 0) Lose("All your units died!");
                break;
            case Team.Enemy:
                enemyTeam.Remove(deadUnit);
                if (enemyTeam.Count == 0) Win();
                break;
            }
    }

    [SerializeField] private GameObject loseScreen;
    [SerializeField] private TMPro.TextMeshProUGUI loseReason;
    [SerializeField] private GameObject winScreen;

    // MANAGING GAME STATE
    UnityEvent OnCombatStart = new UnityEvent();
    UnityEvent OnCombatEnd = new UnityEvent();
    UnityEvent OnCombatWin = new UnityEvent();
    UnityEvent OnCombatLose = new UnityEvent();

    public void StartCombat()
    {
        GetCombatants();
        messageLog.DisplayMessage("Combat Started!");

        //OnCombatStart.Invoke();
        // Everyone starts attacking!
        foreach (CombatantController combatant in allCombatants)
        {
            combatant.canAttack = true;
        }
    }

    public void EndCombat()
    {
        GetCombatants();
        messageLog.DisplayMessage("Combat Ended!");
        // Everyone stop attacking!
        foreach (CombatantController combatant in allCombatants)
        {
            combatant.canAttack = false;
        }
    }

    public void Lose(string reason)
    {
        loseReason.text = "You lost because: " + reason;
        loseScreen.SetActive(true);
        EndCombat();
    }

    public void Win()
    {
        winScreen.SetActive(true);
        EndCombat();
    }
}
