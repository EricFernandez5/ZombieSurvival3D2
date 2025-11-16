using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ZonaPassarNivel : MonoBehaviour
{
    [Header("Configuración de nivel")]
    [Tooltip("Nombre de la escena del siguiente nivel")]
    public string nombreEscenaSiguiente = "Level2_Interior";

    [Tooltip("Segundos que el jugador debe estar dentro de la zona")]
    public float tiempoNecesarioDentro = 5f;

    [Tooltip("Puntos necesarios para poder pasar de nivel")]
    public int puntosNecesarios = 200;

    [Header("UI Mensaje")]
    [Tooltip("Texto TMP donde se mostrará el mensaje de que faltan puntos")]
    public TMP_Text missatgeText;

    [Tooltip("Segundos que el mensaje estará visible")]
    public float duracionMensaje = 3f;

    private bool jugadorDentro = false;
    private float tiempoAcumulado = 0f;

    private Coroutine coroutineComprobarTiempo;
    private Coroutine coroutineMensaje;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = true;

            // Empezamos a contar el tiempo solo una vez
            if (coroutineComprobarTiempo == null)
            {
                coroutineComprobarTiempo = StartCoroutine(ComprobarTiempoDentro());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = false;
            tiempoAcumulado = 0f;

            // Paramos la corrutina de tiempo si el jugador se va
            if (coroutineComprobarTiempo != null)
            {
                StopCoroutine(coroutineComprobarTiempo);
                coroutineComprobarTiempo = null;
            }
        }
    }

    private IEnumerator ComprobarTiempoDentro()
    {
        tiempoAcumulado = 0f;

        while (jugadorDentro)
        {
            tiempoAcumulado += Time.deltaTime;

            if (tiempoAcumulado >= tiempoNecesarioDentro)
            {
                // Miramos cuántos puntos tiene el jugador
                int puntosActuales = 0;
                if (PointsManager.Instance != null)
                {
                    puntosActuales = PointsManager.Instance.Puntos;
                }
                else
                {
                    Debug.LogWarning("ZonaPassarNivel: No hay PointsManager en la escena.");
                }

                if (puntosActuales >= puntosNecesarios)
                {
                    // Tiene suficientes puntos -> cargar escena
                    SceneManager.LoadScene(nombreEscenaSiguiente);
                }
                else
                {
                    // No tiene suficientes puntos -> mostrar mensaje en catalán
                    int faltan = puntosNecesarios - puntosActuales;
                    MostrarMensaje("Et falten " + faltan + " punts per passar al següent nivell");

                    // Reiniciamos el contador, tendrá que volver a estar 5 segundos dentro
                    tiempoAcumulado = 0f;
                }

                // Esperamos un frame para evitar repetir la acción en el mismo momento
                yield return null;
            }

            yield return null;
        }

        coroutineComprobarTiempo = null;
    }

    private void MostrarMensaje(string texto)
    {
        if (missatgeText == null)
        {
            Debug.LogWarning("ZonaPassarNivel: No hay TextMeshPro asignado para el mensaje.");
            return;
        }

        missatgeText.gameObject.SetActive(true);
        missatgeText.text = texto;

        // Si ya había una corrutina de mensaje, la paramos para reiniciar el tiempo
        if (coroutineMensaje != null)
        {
            StopCoroutine(coroutineMensaje);
        }
        coroutineMensaje = StartCoroutine(EsconderMensaje());
    }

    private IEnumerator EsconderMensaje()
    {
        yield return new WaitForSeconds(duracionMensaje);

        if (missatgeText != null)
        {
            missatgeText.gameObject.SetActive(false);
        }

        coroutineMensaje = null;
    }
}
