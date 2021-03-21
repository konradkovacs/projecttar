using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EllensegSzocske : Ellenseg
{
    // Felügyelő változók
    // [SerializeField] elérhetővé teszi a privát változókat a Unity fejlesztői környezetében
    [SerializeField] private float leftLimit;
    [SerializeField] private float rightLimit;

    [SerializeField] private float jumpLength = 3f;
    [SerializeField] private float jumpHeight = 4f;
    [SerializeField] private LayerMask ground;
    
    private Collider2D coll;

    private bool lookingLeft = true;

    // Az alkalmazás vagy a fájl meghívásakor lefutó kód
    protected override void Start()
    {
        base.Start(); // Az "Ellenseg" fájlból örökölt tulajdonság
        coll = GetComponent<Collider2D>();
    }

    // Minden egyes képkockával lefutó kód
    private void Update()
    {
        // Ugrásról zuhanó animációra vált az ellenség
        if (anim.GetBool("Jumping")) // Ha az ugrás ("Jumping") változó igaz
        {
            if(rb.velocity.y < 0.1f)
            {
                anim.SetBool("Falling", true);
                anim.SetBool("Jumping", false);
            }
        }
        // Zuhanásról állóhelyzet animációra vált az ellenség
        if (coll.IsTouchingLayers(ground) && anim.GetBool("Falling"))
        {
            anim.SetBool("Falling", false);
        }
    }

    // A szöcske balra és jobbra mozgásáért felelős metódus
    private void Move()
    {
        if (lookingLeft == true)
        {
            // Tesztelni, hogy a bal irány limitnél (leftLimit) nagyobb-e az értéke az ellenségnek
            if (transform.position.x > leftLimit)
            {
                // Ha nem néz balra az entintás, akkor az értékét 1-re állítja, ami a bal oldalt jelenti
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);
                }
                // Ha az entitás a földön van, akkor ugorhat
                if (coll.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(-jumpLength, jumpHeight);
                    anim.SetBool("Jumping", true);
                }
            }
            // Ha ez nem igaz, akkor jobbra néz
            else
            {
                lookingLeft = false;
            }
        }

        else
        {
            // Tesztelni, hogy a jobb irány limitnél (rightLimit) nagyobb-e az értéke az ellenségnek
            if (transform.position.x < rightLimit)
            {
                // Ha nem néz jobbra az entintás, akkor az értékét -1-re állítja, ami a jobb oldalt jelenti
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);
                }
                // Ha az entitás a földön van, akkor ugorhat
                if (coll.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(jumpLength, jumpHeight);
                    anim.SetBool("Jumping", true);
                }
            }
            // Ha ez igaz, akkor balra néz
            else
            {
                lookingLeft = true;
            }
        }
    }

}
