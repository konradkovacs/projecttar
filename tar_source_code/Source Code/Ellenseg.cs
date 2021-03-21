using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ellenseg : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody2D rb;
    protected AudioSource death;

    // Az alkalmazás vagy a fájl meghívásakor lefutó kód
    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        death = GetComponent<AudioSource>();
    }

    // Az ellenségre ráugrottak (a játékos)
    public void JumpedOn()
    {
        anim.SetTrigger("Death");
        death.Play();
    }

    // Eltűnik az ellenség, "meghal"
    private void Death()
    {
        Destroy(this.gameObject);
    }
}
