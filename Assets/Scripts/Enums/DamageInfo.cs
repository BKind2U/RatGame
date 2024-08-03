using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class DamageInfo
    {
        public float Amount { get; set; }
        public CombatantController Victim { get; set; }      // person getting shot
        //public CharacterActions Source { get; set; }      // bullet that hit the person
        public CombatantController Instigator { get; set; }  // person that fired the bullet
        public DamageType DamageType { get; set; }
        public Team Team { get; set; }              // source team


        public DamageInfo(float amount, CombatantController victim, CombatantController instigator, DamageType damageType, Team team)
        {
            Amount = amount;
            Victim = victim;
            // Source = source;
            Instigator = instigator;
            DamageType = damageType;
            Team = team;
        }
    }

    public enum Team
    {
        None,
        Player,
        Enemy,
        Neutral
    }

    public enum TargetType
    {
    Closest,
    Farthest,
    Random
    }

    public enum DamageType
    {
        None,
        Physical,
        Fire,
        Lightning,
        Poison,
        Cold
    }