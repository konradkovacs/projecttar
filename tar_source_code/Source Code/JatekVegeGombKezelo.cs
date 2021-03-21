using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class JatekVegeGombKezelo : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Az ESC billentyű lenyomásával a főmenübe fog visszatérni a játékos
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
        // Az ENTER billentyű lenyomásával kilép az alkalmazásból
        else if (Input.GetKey(KeyCode.Return))
        {
            Application.Quit();
        }
        else
        {
            // Későbbi fejlesztésekhez
        }
    }
}
