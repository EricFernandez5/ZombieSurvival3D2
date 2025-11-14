using UnityEngine;

public class ChestOpener : MonoBehaviour
{
    [Header("Referencia al texto")]
    public GameObject pressEText;     // Aquí arrastraremos el texto "Pulsa E para abrir"

    private bool playerInRange = false;  // ¿Está el jugador cerca del cofre?
    private bool isOpen = false;         // ¿El cofre ya se ha abierto?
    private Animation chestAnimation;    // Componente Animation del cofre

    void Start()
    {
        // Obtenemos el componente Animation del propio cofre
        chestAnimation = GetComponent<Animation>();

        // Aseguramos que el texto empiece oculto
        if (pressEText != null)
        {
            pressEText.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Si quien entra en el trigger tiene la tag Player y el cofre aún no está abierto
        if (other.CompareTag("Player") && !isOpen)
        {
            playerInRange = true;

            if (pressEText != null)
            {
                pressEText.SetActive(true); // Mostrar el texto
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Si el jugador sale del trigger
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (pressEText != null)
            {
                pressEText.SetActive(false); // Ocultar el texto
            }
        }
    }

    void Update()
    {
        // Si el jugador está cerca, el cofre no está abierto y se pulsa E
        if (playerInRange && !isOpen && Input.GetKeyDown(KeyCode.E))
        {
            isOpen = true; // Marcamos que ya se ha abierto

            if (pressEText != null)
            {
                pressEText.SetActive(false); // Ocultamos el texto al abrir
            }

            // Reproducimos la animación del cofre
            if (chestAnimation != null)
            {
                // Si el clip se llama exactamente "ChestAnim"
                chestAnimation.Play("ChestAnim");
                // Si solo tienes un clip, también valdría:
                // chestAnimation.Play();
            }
        }
    }
}
