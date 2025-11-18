using UnityEngine;
using TMPro;

public class PointsManager : MonoBehaviour
{
    public static PointsManager Instance; // Singleton sencillo

    [Header("UI")]
    public TMP_Text puntosTexto;  // Aquí arrastraremos el TextMeshPro de la UI

    private int puntos = 0;

    // Propiedad pública para poder leer los puntos desde otros scripts
    public int Puntos
    {
        get { return puntos; }
    }

    private void Awake()
    {
        // Configurar el singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        // Si hay datos guardados, los usamos
        if (GameDataPersist.Instance != null)
        {
            puntos = GameDataPersist.Instance.puntosJugador;
        }

        ActualizarUI();
    }

    public void AddPoints(int amount)
    {
        puntos += amount;
        ActualizarUI();
    }

    private void ActualizarUI()
    {
        if (puntosTexto != null)
        {
            puntosTexto.text = "Puntos: " + puntos;
        }
        else
        {
            Debug.LogWarning("PointsManager: no hay texto asignado para mostrar los puntos.");
        }
    }
}
