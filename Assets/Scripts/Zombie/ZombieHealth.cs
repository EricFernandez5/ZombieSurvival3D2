using System;
using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    public int maxHP = 60;
    public int currentHP;
    public Action onDied;

    void OnEnable()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        Debug.Log($"[ZombieHealth] {name} recibe {amount} de daño. Vida restante: {currentHP}");

        if (currentHP <= 0)
            Die();
    }

    void Die()
    {
        Debug.Log($"[ZombieHealth] {name} muere.");

        // Avisar a quien esté suscrito
        onDied?.Invoke();

        // ➜ SUMAR PUNTOS AL JUGADOR
        if (PointsManager.Instance != null)
        {
            PointsManager.Instance.AddPoints(10); // 10 puntos por zombi
        }
        else
        {
            Debug.LogWarning("No hay PointsManager en la escena, no se pueden sumar puntos.");
        }

        Destroy(gameObject);
    }
}
