using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private Button btnContinuar, btnOpciones, btnSalir;
    [SerializeField] private KeyCode toggleKey = KeyCode.Escape;
    private bool isPaused = false;

    void Awake()
    {
        if (pauseCanvas == null)
            pauseCanvas = GameObject.Find("Canvas_Pause");

        if (pauseCanvas != null)
        {
            if (btnContinuar == null) btnContinuar = pauseCanvas.transform.Find("PanelMenu/Btn_Continuar")?.GetComponent<Button>();
            if (btnOpciones  == null) btnOpciones  = pauseCanvas.transform.Find("PanelMenu/Btn_Opciones")?.GetComponent<Button>();
            if (btnSalir     == null) btnSalir     = pauseCanvas.transform.Find("PanelMenu/Btn_Salir")?.GetComponent<Button>();
        }
    }

    void Start()
    {
        SetPaused(false);
        if (btnContinuar) btnContinuar.onClick.AddListener(() => SetPaused(false));
        if (btnOpciones)  btnOpciones.onClick.AddListener(() => Debug.Log("Opciones (pendiente)"));
        if (btnSalir)     btnSalir.onClick.AddListener(() => Debug.Log("Salir (pendiente)"));
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
            SetPaused(!isPaused);
    }

    private void SetPaused(bool pause)
    {
        isPaused = pause;
        if (pauseCanvas) pauseCanvas.SetActive(isPaused);
        GameObject blurCam = GameObject.Find("BlurCamera");
        if (blurCam != null)
        blurCam.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
        Cursor.visible = isPaused;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
