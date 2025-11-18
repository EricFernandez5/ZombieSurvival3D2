using UnityEngine;

public class GameDataPersist : MonoBehaviour
{
    public static GameDataPersist Instance;

    [Header("Datos que se guardan entre niveles")]
    public int vidaJugador = 100;
    public int puntosJugador = 0;
    public float tiempoJugador = 0f;
    public bool tienePistola = false;
    public bool tienePocion = false;

    private void Awake()
    {
        // Singleton + no destruir al cambiar de escena
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
