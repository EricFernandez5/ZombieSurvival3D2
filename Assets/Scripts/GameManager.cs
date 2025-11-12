using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] GameObject gameOverPanel;       // Panel negro a pantalla completa (desactivado por defecto)
    [SerializeField] TextMeshProUGUI gameOverTime;   // Texto debajo de "GAME OVER"
    [SerializeField] ContadorTiempo contador;        // Script del contador de tiempo

    [Header("Audio")]
    [SerializeField] AudioSource musicaNivel;        // Arrastra aquí el objeto con la música del nivel

    void Awake()
    {
        // Buscar contador si no está asignado
        if (!contador)
            contador = FindObjectOfType<ContadorTiempo>();

        // Ocultar panel de Game Over al inicio
        if (gameOverPanel)
            gameOverPanel.SetActive(false);

        // Asegurar que la escena empieza con el tiempo normal
        Time.timeScale = 1f;

        // Opcional: por si el cursor se quedó raro de antes
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnPlayerDied()
    {
        // 1) Parar el contador y obtener el tiempo sobrevivido
        float t = 0f;
        if (contador != null)
        {
            contador.DetenerContador();

            // 🔴 IMPORTANTE: en tu ContadorTiempo tienes que tener este método:
            // public float GetTiempo() => tiempoTranscurrido;
            t = contador.GetTiempo();
        }

        // 2) Parar la música del nivel
        if (musicaNivel != null)
        {
            musicaNivel.Stop();
        }

        // 3) Formatear el tiempo en mm:ss
        int m = Mathf.FloorToInt(t / 60f);
        int s = Mathf.FloorToInt(t % 60f);

        // 4) Mostrar panel de Game Over
        if (gameOverPanel)
            gameOverPanel.SetActive(true);

        // Escribir el tiempo debajo de GAME OVER
        if (gameOverTime)
            gameOverTime.text = $"Temps: {m:00}:{s:00}";

        // 5) Mostrar y liberar el cursor para poder pulsar el botón
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 6) Pausar el juego
        Time.timeScale = 0f;
    }
}
