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
            if (popup != null)
            {
                // Muestra "CHECKPOINT" ~2.5 s (ajusta en el popup)
                popup.Show("CHECKPOINT");
            }
            else
            {
                Debug.LogWarning("[CheckpointZone] No hay CheckpointPopup asignado.");
            }

            alreadyShown = true;
        }
    }
}
