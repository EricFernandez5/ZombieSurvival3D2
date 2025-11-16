using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    [Header("Iconos de la UI")]
    public Image iconoPistola;
    public Image iconoPocion;

    [Header("Prefabs de objetos")]
    public GameObject prefabPistola;
    public GameObject prefabPocion;

    [Header("Punto donde se sujetan las armas")]
    public Transform manoJugador;

    // ===== SISTEMA DE LA POCIÓN =====
    [Header("Poción")]
    public float cooldownPocion = 20f;   // tiempo entre usos
    private float siguienteUsoPocion = 0f;
    public int vidaCurada = 20;          // cantidad de vida que cura

    private PlayerHealth playerHealth;

    private bool tienePistola = false;
    private bool tienePocion = false;

    private GameObject instanciaPistola;
    private GameObject instanciaPocion;

    // 0 = nada, 1 = pistola, 2 = poción
    private int slotSeleccionado = 0;
    public int SlotSeleccionado => slotSeleccionado;

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();

        if (iconoPistola != null)
        {
            iconoPistola.gameObject.SetActive(false);
            iconoPistola.enabled = false;
        }

        if (iconoPocion != null)
        {
            iconoPocion.gameObject.SetActive(false);
            iconoPocion.enabled = false;
        }

        OcultarTodo();
    }

    private void Update()
    {
        // Cambiar entre objetos
        if (Input.GetKeyDown(KeyCode.Alpha1) && tienePistola)
            SeleccionarSlot(1);

        if (Input.GetKeyDown(KeyCode.Alpha2) && tienePocion)
            SeleccionarSlot(2);

        // Usar poción (clic izquierdo) cuando está equipada
        if (slotSeleccionado == 2 && tienePocion)
        {
            if (Input.GetMouseButtonDown(0))
                UsarPocion();
        }
    }

    // ======================
    // ENTREGA DE OBJETOS
    // ======================

    public void DarPistola()
    {
        if (tienePistola) return;
        tienePistola = true;

        if (iconoPistola != null)
        {
            iconoPistola.gameObject.SetActive(true);
            iconoPistola.enabled = true;
        }

        if (slotSeleccionado == 0)
            SeleccionarSlot(1);
    }

    public void DarPocion()
    {
        if (tienePocion) return;
        tienePocion = true;

        if (iconoPocion != null)
        {
            iconoPocion.gameObject.SetActive(true);
            iconoPocion.enabled = true;
        }

        if (slotSeleccionado == 0)
            SeleccionarSlot(2);
    }

    // ======================
    // EQUIPAR
    // ======================

    private void SeleccionarSlot(int slot)
    {
        slotSeleccionado = slot;
        OcultarTodo();

        if (manoJugador == null)
        {
            Debug.LogWarning("PlayerInventory: manoJugador no asignado.");
            return;
        }

        switch (slot)
        {
            case 1: // pistola
                if (!tienePistola) return;

                if (instanciaPistola == null && prefabPistola != null)
                    instanciaPistola = Instantiate(prefabPistola, manoJugador);

                instanciaPistola.SetActive(true);
                instanciaPistola.transform.localPosition = Vector3.zero;
                instanciaPistola.transform.localRotation = Quaternion.identity;
                break;

            case 2: // poción
                if (!tienePocion) return;

                if (instanciaPocion == null && prefabPocion != null)
                    instanciaPocion = Instantiate(prefabPocion, manoJugador);

                instanciaPocion.SetActive(true);
                instanciaPocion.transform.localPosition = Vector3.zero;
                instanciaPocion.transform.localRotation = Quaternion.identity;
                break;
        }
    }

    private void OcultarTodo()
    {
        if (instanciaPistola != null) instanciaPistola.SetActive(false);
        if (instanciaPocion != null) instanciaPocion.SetActive(false);
    }

    // ======================
    // USAR POCIÓN
    // ======================

    private void UsarPocion()
    {
        if (playerHealth == null)
        {
            Debug.LogWarning("PlayerInventory: no se encontró PlayerHealth en el jugador");
            return;
        }

        // Comprobar cooldown
        if (Time.time < siguienteUsoPocion)
        {
            float falta = Mathf.Ceil(siguienteUsoPocion - Time.time);
            Debug.Log("La poción aún está en cooldown. Faltan " + falta + " segundos.");
            return;
        }

        // Curar al jugador
        playerHealth.Curar(vidaCurada);
        Debug.Log("Poción usada. +" + vidaCurada + " de vida.");

        // Aplicar cooldown
        siguienteUsoPocion = Time.time + cooldownPocion;
    }

    // ======================
    // REINICIAR INVENTARIO AL MORIR
    // ======================

    public void ResetInventory()
    {
        // No tenemos ningún objeto
        tienePistola = false;
        tienePocion = false;

        // Borrar instancias de la mano
        if (instanciaPistola != null)
        {
            Destroy(instanciaPistola);
            instanciaPistola = null;
        }

        if (instanciaPocion != null)
        {
            Destroy(instanciaPocion);
            instanciaPocion = null;
        }

        // Ocultar iconos de la UI
        if (iconoPistola != null)
        {
            iconoPistola.gameObject.SetActive(false);
            iconoPistola.enabled = false;
        }

        if (iconoPocion != null)
        {
            iconoPocion.gameObject.SetActive(false);
            iconoPocion.enabled = false;
        }

        // Ningún slot seleccionado
        slotSeleccionado = 0;
        OcultarTodo();

        // Reiniciar cooldown de la poción
        siguienteUsoPocion = 0f;
    }
}
