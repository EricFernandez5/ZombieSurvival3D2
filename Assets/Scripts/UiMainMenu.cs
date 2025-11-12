using UnityEngine;
using UnityEngine.SceneManagement;

public class UiMainMenu : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string levelExteriorScene = "Level1_Exterior";
    [SerializeField] private string optionsScene = "OptionsMenu";

    public void Play()
    {
        SceneManager.LoadScene(levelExteriorScene);
    }

    public void Options()
    {
        SceneManager.LoadScene(optionsScene);
    }

    public void Quit()
    {
        // Por si vienes de una escena con pausa activada
        Time.timeScale = 1f;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
