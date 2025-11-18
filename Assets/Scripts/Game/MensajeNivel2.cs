using UnityEngine;
using TMPro;

public class MensajeNivel2 : MonoBehaviour
{
    public TextMeshProUGUI mensajeText;
    public float tiempoVisible = 6f;

    void Start()
    {
        if (mensajeText != null)
        {
            mensajeText.gameObject.SetActive(true);  // Mostrar el texto
            Invoke("OcultarMensaje", tiempoVisible); // Ocultar luego de unos seconds
        }
    }

    void OcultarMensaje()
    {
        mensajeText.gameObject.SetActive(false);
    }
}
