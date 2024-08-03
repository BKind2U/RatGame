using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Attack")]
public class SO_Attack : ScriptableObject
{
    public string attackName = "Basic Attack";
    public float strModifier = 1f;
    public TargetType targetType;
    public DamageType damageType;
}
