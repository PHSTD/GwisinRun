using UnityEngine;

public interface IDamageable
{
    void TakeDamage(int amount);
    void TakeDamage(int amount, Transform attacker);
}
