using UnityEngine;
using UnityEngine.SceneManagement;

public class UiMainMenu : MonoBehaviour
{
    [Header("Scenes")]
    [SerializeField] private string level1ExteriorScene = "Level1_Exterior";
    [SerializeField] private string level2InteriorScene = "Level2_Interior";

    [Header("UI Panels")]
    [SerializeField] private GameObject mainMenuPanel;   // Panel con Play / Opcions / Sortir
    [SerializeField] private GameObject optionsPanel;    // Panel de Opciones

    private void Start()
    {
        // Asegurar estado correcto del menú al iniciar
        Time.timeScale = 1f;

        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (optionsPanel != null) optionsPanel.SetActive(false);
    }

    // Botón PLAY del menú principal
    public void Play()
    {
        SceneManager.LoadScene(level1ExteriorScene);
    }

    // Botón OPCIONS del menú principal → abrir panel de opciones sin ocultar menú
    public void Options()
    {
        if (optionsPanel != null) optionsPanel.SetActive(true);
    }

    // Botón TORNAR dentro del panel de opciones → cerrar panel
    public void CloseOptions()
    {
        if (optionsPanel != null) optionsPanel.SetActive(false);
    }

    // Botón dentro de Opciones → ir al Nivel 1 Exterior
    public void LoadLevel1FromOptions()
    {
        SceneManager.LoadScene(level1ExteriorScene);
    }

    // Botón dentro de Opciones → ir al Nivel 2 Exterior
    public void LoadLevel2FromOptions()
    {
        SceneManager.LoadScene(level2InteriorScene);
    }

    // Botón SALIR del menú principal
    public void Quit()
    {
        Time.timeScale = 1f;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
