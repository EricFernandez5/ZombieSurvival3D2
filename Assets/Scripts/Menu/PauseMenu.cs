using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanvas;        // Canvas_Pause
    [SerializeField] private GameObject panelMenu;          // Canvas_Pause/PanelMenu
    [SerializeField] private GameObject panelOpcions;       // Canvas_Pause/PanelOpcions
    [SerializeField] private Button btnContinuar, btnOpciones, btnSalir;
    [SerializeField] private KeyCode toggleKey = KeyCode.Escape;
    private bool isPaused = false;

    void Awake()
    {
        if (pauseCanvas == null) pauseCanvas = GameObject.Find("Canvas_Pause");

        if (pauseCanvas != null)
        {
            if (panelMenu   == null) panelMenu   = pauseCanvas.transform.Find("PanelMenu")?.gameObject;
            if (panelOpcions== null) panelOpcions= pauseCanvas.transform.Find("PanelOpcions")?.gameObject;

            if (btnContinuar == null) btnContinuar = pauseCanvas.transform.Find("PanelMenu/Btn_Continuar")?.GetComponent<Button>();
            if (btnOpciones  == null) btnOpciones  = pauseCanvas.transform.Find("PanelMenu/Btn_Opcions")?.GetComponent<Button>();
            if (btnSalir     == null) btnSalir     = pauseCanvas.transform.Find("PanelMenu/Btn_Sortir")?.GetComponent<Button>();
        }
    }

    void Start()
    {
        SetPaused(false);

        if (btnContinuar) btnContinuar.onClick.AddListener(() => SetPaused(false));
        if (btnOpciones)  btnOpciones.onClick.AddListener(AbrirOpcions);
        if (btnSalir)     btnSalir.onClick.AddListener(VolverAlMenuPrincipal);
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            // Si estem en Opcions i es prem ESC, tornem al panell principal de pausa
            if (isPaused && panelOpcions != null && panelOpcions.activeSelf)
            {
                panelOpcions.SetActive(false);
                if (panelMenu) panelMenu.SetActive(true);
            }
            else
            {
                SetPaused(!isPaused);
            }
        }
    }

    private void AbrirOpcions()
    {
        if (!isPaused) SetPaused(true);
        if (panelMenu) panelMenu.SetActive(false);
        if (panelOpcions) panelOpcions.SetActive(true);
    }

    private void SetPaused(bool pause)
    {
        isPaused = pause;
        if (pauseCanvas) pauseCanvas.SetActive(isPaused);

        // Desenfoc si el tens
        GameObject blurCam = GameObject.Find("BlurCamera");
        if (blurCam != null) blurCam.SetActive(isPaused);

        Time.timeScale = isPaused ? 0f : 1f;
        Cursor.visible = isPaused;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;

        // Quan tornem a la pausa, mostra el menú principal
        if (isPaused)
        {
            if (panelMenu) panelMenu.SetActive(true);
            if (panelOpcions) panelOpcions.SetActive(false);
        }
    }

    private void VolverAlMenuPrincipal()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
