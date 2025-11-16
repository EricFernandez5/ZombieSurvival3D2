using System.Collections;
using UnityEngine;

public class ChestOpener : MonoBehaviour
{
    [Header("Referencia al texto")]
    public GameObject pressEText;

    [Header("Inventario del jugador")]
    public PlayerInventory playerInventory;

    private bool playerInRange = false;
    private bool isOpen = false;
    private bool itemsGiven = false;

    private Animation chestAnimation;

    void Start()
    {
        chestAnimation = GetComponent<Animation>();

        if (pressEText != null)
            pressEText.SetActive(false);

        if (chestAnimation == null)
            Debug.LogWarning("ChestOpener: no se encontró componente Animation.");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isOpen)
        {
            playerInRange = true;
            if (pressEText != null)
                pressEText.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (pressEText != null)
                pressEText.SetActive(false);
        }
    }

    void Update()
    {
        if (playerInRange && !isOpen && Input.GetKeyDown(KeyCode.E))
        {
            AbrirCofre();
        }
    }

    private void AbrirCofre()
    {
        isOpen = true;

        if (pressEText != null)
            pressEText.SetActive(false);

        if (chestAnimation != null)
            chestAnimation.Play("ChestAnim");

        StartCoroutine(DarObjetosTrasEspera());
    }

    private IEnumerator DarObjetosTrasEspera()
    {
        yield return new WaitForSeconds(3f);
        DarObjetosAlJugador();
    }

    private void DarObjetosAlJugador()
    {
        if (itemsGiven) return;
        itemsGiven = true;

        if (playerInventory != null)
        {
            Debug.Log("Cofre: dando PISTOLA y POCIÓN al jugador");

            playerInventory.DarPistola();
            playerInventory.DarPocion();
        }
    }
}
