using UnityEngine;

public class CheckpointZone : MonoBehaviour
{
    [Header("Referencia al popup")]
    public CheckpointPopup popup;

    [Header("Mostrar solo una vez")]
    public bool onlyOnce = true;

    private bool alreadyShown = false;

    private void OnTriggerEnter(Collider other)
    {
        if (onlyOnce && alreadyShown)
            return;

        if (other.CompareTag("Player"))
        {
            // Mostrar texto "CHECKPOINT"
            if (popup != null)
            {
                popup.Show("CHECKPOINT");
            }
            else
            {
                Debug.LogWarning("[CheckpointZone] No hay CheckpointPopup asignado.");
            }

            // NUEVO: guardar este checkpoint en el GameManager
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SetCheckpoint(transform);
            }

            alreadyShown = true;
        }
    }
}
