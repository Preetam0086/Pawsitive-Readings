using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    // Call this from the Button OnClick()
    public void LoadMainScene()
    {
        SceneManager.LoadScene("Pawsitive_ReadingsMain Scene");
    }

    // Optional: quit button for later
    public void QuitGame()
    {
        Application.Quit();
    }
}
