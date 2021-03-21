using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KameraIranyito : MonoBehaviour
{
    public Transform jatekos;

    // Minden egyes képkockával lefutó kód
    private void Update()
    {
        transform.position = new Vector3(jatekos.position.x, jatekos.position.y, transform.position.z);
    }
}
