using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class JatekosIranyitas : MonoBehaviour
{
    // Start() metódus változók
    private Rigidbody2D rb; // A 2D fizikáért felelős komponens a Unityben
    private Animator anim; // 2D-s interfész ami az animációkért felelős
    private Collider2D collide;

    // Állapotokat jelző enumeráció
    private enum State {idle, running, jumping, falling, hurt}
    private State state = State.idle; // Az alapértelmezett állapot az idle, állóhelyzet

    // Felügyelő változók
    // [SerializeField] elérhetővé teszi a privát változókat a Unity fejlesztői környezetében
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpDensity = 10f;
    [SerializeField] private int coins = 0;
    [SerializeField] private int health = 100;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private float hurtDensity = 10f;
    [SerializeField] private AudioSource coin;
    [SerializeField] private AudioSource footstep;

    // Az alkalmazás vagy a fájl meghívásakor lefutó kód
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        collide = GetComponent<Collider2D>();
        healthText.text = health.ToString();
    }

    // Minden egyes képkockával lefutó kód
    private void Update()
    {
        if(state!= State.hurt)
        {
            InputManager();
        }

        // Metódus meghívása
        AnimationStatus();
        // Az állapotot frissíti a játék
        anim.SetInteger("state", (int)state);
    }

    // Érmét, életerő érintést kezelő metódus
    private void OnTriggerEnter2D(Collider2D collide)
    {
        if(collide.tag == "Collectable")
        {
            coin.Play(); // Az érme hangeffekt lejátszása
            Destroy(collide.gameObject); // Eltűnik az objektum (érme), ha a játékos megérinti
            coins++; // Növeli az érme számláló értékét +1-gyel
            coinText.text = coins.ToString(); // Kiírja az érmék (coins) változó értékét
        }
        if(collide.tag == "HealthPickup")
        {
            Destroy(collide.gameObject);
            health += 50;
            healthText.text = health.ToString();
        }
    }

    // Az ellenség érintkezésével lefutó metódus
    private void OnCollisionEnter2D(Collision2D item)
    {
        if(item.gameObject.tag == "Enemy")
        {
            Ellenseg grasshopper = item.gameObject.GetComponent<Ellenseg>();

            if (state == State.falling)
            {
                grasshopper.JumpedOn();
                Jump();
            }
            else
            {
                state = State.hurt;
                HealthManager();
                if (item.gameObject.transform.position.x > transform.position.x)
                {
                    // Az ellenfél jobbra van a játékostól, ezért sebződni fog és balra fog mozdulni a játékos
                    rb.velocity = new Vector2(-hurtDensity, rb.velocity.y);
                }
                else
                {
                    // Az ellenfél balra van a játékostól, ezért sebződni fog és jobbra fog mozdulni a játékos
                    rb.velocity = new Vector2(hurtDensity, rb.velocity.y);
                }
            }
        }
    }

    // Az életerőt szabályozza, frissíti a felhasználói felülelet, újratölti a pályát, ha a játékosnak elfogyott az életereje
    private void HealthManager()
    {
        health -= 50;
        healthText.text = health.ToString();
        // Ha a játékosnak elfogyott az életereje, akkor újratölti az adott pályát a játék
        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }


    // Bemenetek, mozgások kezelése
    private void InputManager()
    {
        float hDirection = Input.GetAxisRaw("Horizontal"); // Vízszintes irány

        if (hDirection < 0) // Ha negatív az érték, a karakter iránya balra lesz
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }

        else if (hDirection > 0) // Ha pozitív az érték, a karakter iránya jobbra lesz
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }

        // Ugrik a karakter, ha a földön van
        if (Input.GetButtonDown("Jump") && collide.IsTouchingLayers(ground))
        {
            Jump();
        }

        // Az ESC billentyű lenyomásával visszakerül a játékos a főmenübe
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    // A játékos ugrásáért felelős metódus
    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpDensity);
        state = State.jumping;
    }

    // Animációk, állapotok lekezelése
    private void AnimationStatus()
    {
        if (state == State.jumping)
        {
            if(rb.velocity.y < 0.1f)
            {
                state = State.falling;
            }
        }
        
        else if(state == State.falling)
        {
            if(collide.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }

        else if (state == State.hurt)
        {
            if(Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                state = State.idle;
            }
        }

        else if (Mathf.Abs(rb.velocity.x) > 2f) // Mindig pozitív értéket kap
        {
            // Mozgás
            state = State.running;
        }

        else
        {
            // Karaktert állóhelyzetbe visszaállítani
            state = State.idle;
        }
        
    }

    // A hangeffektek lejátszásáért felelős metódus
    private void Footstep()
    {
        footstep.Play();
    }
}
