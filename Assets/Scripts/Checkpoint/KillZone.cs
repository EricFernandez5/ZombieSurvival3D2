using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Solo reaccionamos si lo que entra es el jugador
        if (!other.CompareTag("Player"))
            return;

        // Intentamos coger el componente PlayerHealth del jugador
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            // Le quitamos toda la vida de golpe:
            // Si tiene 100, le hacemos 100 de daño;
            // si tiene 60, le hacemos 60 de daño, etc.
            int vidaActual = playerHealth.currentHealth;

            if (vidaActual > 0)
            {
                playerHealth.TakeDamage(vidaActual);
            }
        }
        else
        {
            // Por si acaso no encontramos PlayerHealth, llamamos directo al GameManager
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnPlayerDied();
            }
            else
            {
                Debug.LogWarning("[KillZone] No se encontró PlayerHealth ni GameManager.Instance.");
            }
        }
    }
}
