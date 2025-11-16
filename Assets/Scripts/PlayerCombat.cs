using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    [Header("Referencias")]
    public PlayerInventory inventario;
    public Transform camara;
    public Image crosshair;

    [Header("Daños")]
    public int dañoPistola = 20;

    [Header("Rangos")]
    public float rangoPistola = 200f;

    [Header("Tiempos entre ataques")]
    public float tiempoEntreDisparos = 0.25f;

    private float proximoDisparo = 0f;

    void Start()
    {
        if (inventario == null)
            inventario = GetComponent<PlayerInventory>();

        if (camara == null && Camera.main != null)
            camara = Camera.main.transform;

        if (crosshair != null)
            crosshair.gameObject.SetActive(false);
    }

    void Update()
    {
        ActualizarCrosshair();

        if (Input.GetMouseButtonDown(0))
        {
            if (inventario == null) return;

            if (inventario.SlotSeleccionado == 1) // ahora 1 = pistola
                IntentarDisparar();
        }
    }

    void ActualizarCrosshair()
    {
        if (crosshair == null || inventario == null) return;

        // La mira solo aparece con la pistola
        if (inventario.SlotSeleccionado == 1)
        {
            if (!crosshair.gameObject.activeSelf)
                crosshair.gameObject.SetActive(true);
        }
        else
        {
            if (crosshair.gameObject.activeSelf)
                crosshair.gameObject.SetActive(false);
        }
    }

    void IntentarDisparar()
    {
        if (Time.time < proximoDisparo) return;
        proximoDisparo = Time.time + tiempoEntreDisparos;

        if (camara == null)
        {
            Debug.LogWarning("PlayerCombat: no hay cámara asignada.");
            return;
        }

        Camera camComp = camara.GetComponent<Camera>();
        if (camComp == null) camComp = Camera.main;

        Ray ray;

        if (camComp != null)
        {
            Vector3 centro = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
            ray = camComp.ScreenPointToRay(centro);
        }
        else
        {
            ray = new Ray(camara.position, camara.forward);
        }

        Debug.DrawRay(ray.origin, ray.direction * rangoPistola, Color.red, 0.5f);

        RaycastHit[] hits = Physics.RaycastAll(ray, rangoPistola);

        if (hits.Length == 0)
        {
            Debug.Log("Disparo: no golpeó nada.");
            return;
        }

        ZombieHealth objetivo = null;
        float distanciaMasCercana = Mathf.Infinity;

        foreach (RaycastHit h in hits)
        {
            ZombieHealth z = h.collider.GetComponent<ZombieHealth>();
            if (z == null)
                z = h.collider.GetComponentInParent<ZombieHealth>();

            if (z != null)
            {
                if (h.distance < distanciaMasCercana)
                {
                    distanciaMasCercana = h.distance;
                    objetivo = z;
                }
            }
        }

        if (objetivo != null)
        {
            objetivo.TakeDamage(dañoPistola);
            Debug.Log($"Disparo: zombi recibe {dañoPistola} de daño.");
        }
        else
        {
            Debug.Log("Disparo: no había zombies en la línea.");
        }
    }
}
