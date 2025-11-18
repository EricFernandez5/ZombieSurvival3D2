using UnityEngine;
using TMPro;

public class ContadorTiempo : MonoBehaviour
{
    [Header("Referencia UI")]
    public TextMeshProUGUI textoTiempo;

    [Header("Comportamiento")]
    [SerializeField] bool iniciarAlComenzar = true;

    float tiempoTranscurrido;
    bool enMarcha = false;
    public float GetTiempo() => tiempoTranscurrido;


    void Awake()
    {
        if (textoTiempo == null)
            textoTiempo = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        // Cargar tiempo desde GameDataPersist (si lo hay)
        if (GameDataPersist.Instance != null)
        {
            tiempoTranscurrido = GameDataPersist.Instance.tiempoJugador;
        }

        ActualizarTexto(tiempoTranscurrido);

        if (iniciarAlComenzar)
        {
            Time.timeScale = 1f;     // por si la escena entra pausada
            enMarcha = true;
        }
    }

    void Update()
    {
        if (!enMarcha) return;

        tiempoTranscurrido += Time.deltaTime;
        ActualizarTexto(tiempoTranscurrido);
    }

    void ActualizarTexto(float t)
    {
        int minutos = Mathf.FloorToInt(t / 60f);
        int segundos = Mathf.FloorToInt(t % 60f);
        if (textoTiempo != null)
            textoTiempo.text = $"Tiempo: {minutos:00}:{segundos:00}";
    }

    public void IniciarContador()
    {
        tiempoTranscurrido = 0f;
        enMarcha = true;
    }

    public void DetenerContador()
    {
        enMarcha = false;
    }
}
