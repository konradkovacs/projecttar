using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FoMenu : MonoBehaviour
{
    // A gomb lenyomására betölti a megadott pályanevet, ami ebben az esetben az 1. pálya lesz
   public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene"); // Betölti a "SampleScene" nevezetű pályát.
    }

    public void LoadHighScores()
    {
        SceneManager.LoadScene("HighScoreList"); // Betölti a toplista menüt.
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // A toplistából a főmenüt fogja betölteni.
    }

    // A gomb lenyomására kifog lépni a programból.
    public void QuitGame()
    {
        Application.Quit(); // Kilép a programból.
    }
}
