using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Salud")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI (HUD)")]
    [SerializeField] Image barraFill;
    [SerializeField] TextMeshProUGUI textoBarra;

    [Header("Game Over / Vidas")]
    [SerializeField] GameManager gameManager;

    void Awake()
    {
        currentHealth = maxHealth;
        ActualizarUI();

        if (!gameManager)
            gameManager = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        // Si hay datos guardados de otro nivel, los aplicamos
        if (GameDataPersist.Instance != null)
        {
            SetHealth(GameDataPersist.Instance.vidaJugador);
        }
    }

    public void TakeDamage(int amount)
    {
        if (currentHealth <= 0) return;

        currentHealth = Mathf.Max(0, currentHealth - amount);

        ActualizarUI();

        if (currentHealth == 0)
        {
            if (gameManager)
                gameManager.OnPlayerDied();
        }
    }

    // Curar vida (para la poción)
    public void Curar(int amount)
    {
        if (currentHealth <= 0) return;

        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        ActualizarUI();
    }

    // Restaurar vida completa al hacer respawn
    public void RestaurarVidaCompleta()
    {
        currentHealth = maxHealth;
        ActualizarUI();
    }

    // NUEVO: para poder ponerle una vida concreta (por ejemplo la guardada)
    public void SetHealth(int amount)
    {
        currentHealth = Mathf.Clamp(amount, 0, maxHealth);
        ActualizarUI();
    }

    void ActualizarUI()
    {
        if (barraFill != null)
            barraFill.fillAmount = currentHealth / (float)maxHealth;

        if (textoBarra != null)
            textoBarra.text = $"{currentHealth}/{maxHealth}";
    }
}
