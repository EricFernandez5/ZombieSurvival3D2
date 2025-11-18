using UnityEngine;
using TMPro;
using System.Collections;

public class CheckpointPopup : MonoBehaviour
{
    [Header("Referencia al texto TMP")]
    public TextMeshProUGUI message;

    [Header("Duración visible (segundos)")]
    [Range(0.5f, 5f)]
    public float duration = 2.5f;

    private Coroutine currentRoutine;

    private void Awake()
    {
        if (message != null)
            message.gameObject.SetActive(false); // Oculto al inicio
    }

    /// <summary>
    /// Muestra el texto (por defecto "CHECKPOINT") y lo oculta pasado 'duration' segundos.
    /// </summary>
    public void Show(string text = "CHECKPOINT", float? forSeconds = null)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowRoutine(text, forSeconds ?? duration));
    }

    private IEnumerator ShowRoutine(string text, float seconds)
    {
        if (message == null)
        {
            Debug.LogWarning("[CheckpointPopup] Falta asignar el TextMeshProUGUI.");
            yield break;
        }

        message.text = text;
        message.gameObject.SetActive(true);

        yield return new WaitForSeconds(seconds);

        message.gameObject.SetActive(false);
        currentRoutine = null;
    }
}
