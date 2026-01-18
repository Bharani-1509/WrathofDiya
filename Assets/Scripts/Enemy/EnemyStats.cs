using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Stats")]
public class EnemyStats : ScriptableObject
{
    public int maxHealth = 100;
    public int attackDamage = 10;
    public float sightRange = 8f;
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
}
