using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitacioJavito : MonoBehaviour
{
    // Ha a játékos a platform oldalán van, nem tud mozogni
   private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<JatekosIranyitas>().enabled = false;
        }
    }
}
