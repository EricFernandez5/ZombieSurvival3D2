using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverButtons : MonoBehaviour
{
    // Llamado por el botón "Tornar al menú"
    public void GoToMainMenu()
    {
        // Volvemos a la velocidad normal del juego
        Time.timeScale = 1f;

        // MUY IMPORTANTE: el nombre de la escena tiene que ser EXACTAMENTE el de tu menú principal
        SceneManager.LoadScene("MainMenu");
    }
}
