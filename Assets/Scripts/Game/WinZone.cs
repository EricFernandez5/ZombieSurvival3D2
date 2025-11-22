using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class WinZone : MonoBehaviour
{
[Header("UI de victoria")]
public GameObject panelVictoria;
public TextMeshProUGUI textoTiempoFinal;
public TextMeshProUGUI textoPuntosFinales;

[Header("Referencias al HUD actual")]
public TextMeshProUGUI textoTiempoHUD;
public TextMeshProUGUI textoPuntosHUD;

private bool yaHasGanado = false;

private void OnTriggerEnter(Collider other)
{
if (yaHasGanado) return;

if (other.CompareTag("Player"))
{
yaHasGanado = true;

// Activar panel de victoria
if (panelVictoria != null)
panelVictoria.SetActive(true);

// Copiar tiempo desde el HUD
if (textoTiempoFinal != null && textoTiempoHUD != null)
textoTiempoFinal.text = "Tiempo final: " + textoTiempoHUD.text;

// Copiar puntos desde el HUD
if (textoPuntosFinales != null && textoPuntosHUD != null)
textoPuntosFinales.text = "Puntos finals: " + textoPuntosHUD.text;

// Pausar juego
Time.timeScale = 0f;

// Mostrar ratón
Cursor.lockState = CursorLockMode.None;
Cursor.visible = true;
}
}

// MÉTODO PARA VOLVER AL MENÚ PRINCIPAL
public void VolverAlMenu()
{
Time.timeScale = 1f; // Importante para reanudar el tiempo
SceneManager.LoadScene("MainMenu"); // Asegúrate que el nombre coincide
}
}