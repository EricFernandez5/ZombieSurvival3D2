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

    [Header("Game Over")]
    [SerializeField] GameManager gameManager;

    void Awake()
    {
        currentHealth = maxHealth;
        ActualizarUI();

        if (!gameManager) 
            gameManager = FindObjectOfType<GameManager>();
    }

    public void TakeDamage(int amount)
    {
        if (currentHealth <= 0) return;

        currentHealth = Mathf.Max(0, currentHealth - amount);

        ActualizarUI();  // ← LA BARRA BAJA AQUÍ

        if (currentHealth == 0)
        {
            if (gameManager) 
                gameManager.OnPlayerDied();
        }
    }

    void ActualizarUI()
    {
        // BAJA LA BARRA PROPORCIONALMENTE
        barraFill.fillAmount = currentHealth / (float)maxHealth;

        // ACTUALIZA EL TEXTO (50/100, etc)
        textoBarra.text = $"{currentHealth}/{maxHealth}";
    }
}
