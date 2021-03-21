using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Eses : MonoBehaviour
{

    // Ha a játékos leesik a platformokról, akkor a játék újratölti a pályát.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Az adott pályanév megkeresése és betöltése
        }
    }
}
