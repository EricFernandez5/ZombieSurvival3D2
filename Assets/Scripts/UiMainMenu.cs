using UnityEngine;
using UnityEngine.SceneManagement;

public class UiMainMenu : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string levelExteriorScene = "Level1_Exterior";
    [SerializeField] private string optionsScene = "OptionsMenu";

    public void Play()
    {
        // Empieza en el nivel exterior (requisito: nivel aire libre con Terrain)
        SceneManager.LoadScene(levelExteriorScene);
    }

    public void Options()
    {
        SceneManager.LoadScene(optionsScene);
    }

    public void Quit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
