using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PalyaValto : MonoBehaviour
{
    // Felügyelő változók
    // [SerializeField] elérhetővé teszi a privát változókat a Unity fejlesztői környezetében

    [SerializeField] private string sceneName; // Pálya betöltése, ami a Unity szerkesztőjében is elérhető

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(sceneName); // Az adott pályanév megkeresése és betöltése
        }
    }
}
