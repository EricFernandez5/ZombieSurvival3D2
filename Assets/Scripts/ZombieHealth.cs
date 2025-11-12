using System;
using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    public int maxHP = 60;
    public int currentHP;
    public Action onDied;

    void OnEnable() { currentHP = maxHP; }

    public void TakeDamage(int amount) {
        currentHP -= amount;
        if (currentHP <= 0) Die();
    }

    void Die() {
        onDied?.Invoke();
        Destroy(gameObject);
    }
}
