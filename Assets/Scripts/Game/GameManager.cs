using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;   // <-- AÑADIDO

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Over")]
    [SerializeField] GameObject gameOverPanel;       // Panel negro (desactivado al inicio)
    [SerializeField] TextMeshProUGUI gameOverTime;   // Texto debajo de "GAME OVER"
    [SerializeField] ContadorTiempo contador;        // Script del contador de tiempo

    [Header("Audio")]
    [SerializeField] AudioSource musicaNivel;        // Música de este nivel

    [Header("Jugador / Respawn")]
    [SerializeField] Transform player;               // Arrastra aquí el Player
    private Vector3 spawnInicialPos;
    private Quaternion spawnInicialRot;
    private Transform currentCheckpoint;

    [Header("Vidas")]
    [SerializeField] int maxLives = 3;
    [SerializeField] TextMeshProUGUI vidasTexto;     // Texto "Vidas" de la UI
    private int currentLives;

    private void Awake()
    {
        // Singleton sencillo
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Buscar contador si no está asignado
        if (!contador)
            contador = FindObjectOfType<ContadorTiempo>();

        // Ocultar panel de Game Over al inicio
        if (gameOverPanel)
            gameOverPanel.SetActive(false);

        // Tiempo normal
        Time.timeScale = 1f;

        // Cursor bloqueado, como en un FPS
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Buscar jugador por tag si no está asignado
        if (player == null)
        {
            GameObject pObj = GameObject.FindGameObjectWithTag("Player");
            if (pObj != null)
                player = pObj.transform;
        }

        // Guardar posición y rotación inicial del jugador
        if (player != null)
        {
            spawnInicialPos = player.position;
            spawnInicialRot = player.rotation;
        }

        // Vidas iniciales
        currentLives = maxLives;
        ActualizarVidasUI();
    }

    private void ActualizarVidasUI()
    {
        if (vidasTexto != null)
        {
            vidasTexto.text = "Vides: " + currentLives;
        }
    }

    // Llamado desde el CheckpointZone cuando entras en un checkpoint
    public void SetCheckpoint(Transform checkpointTransform)
    {
        currentCheckpoint = checkpointTransform;
    }

    // Llamado desde PlayerHealth cuando la vida llega a 0
    public void OnPlayerDied()
    {
        if (currentLives > 1)
        {
            // Todavía le quedan vidas → restar 1 y respawn
            currentLives--;
            ActualizarVidasUI();
            RespawnPlayer();
        }
        else
        {
            // Ya no quedan vidas → Game Over
            GameOver();
        }
    }

    private void RespawnPlayer()
    {
        if (player == null)
        {
            Debug.LogWarning("GameManager: no hay jugador asignado para respawn.");
            return;
        }

        // ============================
        // Saber en qué escena estamos
        // ============================
        string escenaActual = SceneManager.GetActiveScene().name;

        // Nombre EXACTO de tu escena de nivel 2
        const string NOMBRE_NIVEL_2 = "Level2_Interior";

        // Elegir posición de respawn: checkpoint si existe, si no, spawn inicial
        Vector3 destinoPos = spawnInicialPos;
        Quaternion destinoRot = spawnInicialRot;

        if (currentCheckpoint != null)
        {
            destinoPos = currentCheckpoint.position;
            destinoRot = currentCheckpoint.rotation;
        }

        // Teletransportar al jugador
        var cc = player.GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = false;
            player.position = destinoPos;
            player.rotation = destinoRot;
            cc.enabled = true;
        }
        else
        {
            player.position = destinoPos;
            player.rotation = destinoRot;
        }

        // Restaurar vida completa
        var ph = player.GetComponent<PlayerHealth>();
        if (ph != null)
        {
            ph.RestaurarVidaCompleta();
        }

        // ==========================================
        // INVENTARIO:
        // - En Level2_Interior -> NO se resetea
        // - En el resto de escenas -> SÍ se resetea
        // ==========================================
        var inv = player.GetComponent<PlayerInventory>();
        if (inv != null)
        {
            if (escenaActual == NOMBRE_NIVEL_2)
            {
                // Estamos en el nivel 2 → mantener items
                Debug.Log("Respawn en nivel 2 (Level2_Interior): se mantiene el inventario.");
            }
            else
            {
                // Nivel 1 u otro → perder items como antes
                inv.ResetInventory();
            }
        }

        // Asegurarnos de que el juego sigue corriendo
        Time.timeScale = 1f;

        // Reanudar música si estuviera parada
        if (musicaNivel != null && !musicaNivel.isPlaying)
        {
            musicaNivel.Play();
        }

        // Bloquear otra vez el cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // IMPORTANTE: NO tocamos PointsManager, así conserva los puntos.
    }

    private void GameOver()
    {
        // 1) Parar el contador y obtener tiempo sobrevivido
        float t = 0f;
        if (contador != null)
        {
            contador.DetenerContador();
            t = contador.GetTiempo();   // Asegúrate de tener este método en ContadorTiempo
        }

        // 2) Parar música
        if (musicaNivel != null)
        {
            musicaNivel.Stop();
        }

        // 3) Formatear tiempo mm:ss
        int m = Mathf.FloorToInt(t / 60f);
        int s = Mathf.FloorToInt(t % 60f);

        // 4) Mostrar panel de Game Over
        if (gameOverPanel)
            gameOverPanel.SetActive(true);

        if (gameOverTime)
            gameOverTime.text = $"Temps: {m:00}:{s:00}";

        // 5) Mostrar cursor para poder pulsar el botón
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 6) Pausar juego
        Time.timeScale = 0f;
    }
}
