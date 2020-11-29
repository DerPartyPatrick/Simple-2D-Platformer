using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class PlayerController : MonoBehaviour
{
    public int speed;
    public int jumpForce;
    public int resetPoint; 

    public bool isGrounded;
    public LayerMask groundLayers;
    public Text text;
    public GameObject coinParent;

    private int goal;
    private int coins; 
    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private void Start()
    {
        goal = coinParent.transform.childCount; 
        isGrounded = false;
        coins = 0;  
        text.text = "Coins: 0/" + goal; 
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>(); 
    }

    private void Update()
    {
        if(transform.position.y < resetPoint)
        {
            Restart(); 
        }
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapArea(
            new Vector2(transform.position.x - 0.5f, transform.position.y - 0.5f),
            new Vector2(transform.position.x + 0.5f, transform.position.y - 1f),
            groundLayers); 

        if(Input.GetKey("a"))
        {
            if(isGrounded)
            {
                rb.velocity = new Vector2(-speed, 0);
                animator.Play("PlayerRun");
            }
            else
            {
                rb.velocity = new Vector2(-speed, rb.velocity.y);
            }

            sr.flipX = true; 
        } 
        else if(Input.GetKey("d"))
        {
            if(isGrounded)
            {
                rb.velocity = new Vector2(speed, 0);
                animator.Play("PlayerRun");
            }
            else
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
            }

            sr.flipX = false; 
        }
        else
        {
            if(isGrounded)
            {
                rb.velocity = new Vector2(0, 0);
                animator.Play("PlayerAnimation");
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }

        if(Input.GetKey("space") && isGrounded)
        {
            rb.AddForce(new Vector2(0, jumpForce)); 
        }

        if(!isGrounded)
        {
            if(rb.velocity.y > 0)
            {
                animator.Play("PlayerJump"); 
            }
            else
            {
                animator.Play("PlayerFall"); 
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Coin"))
        {
            collision.gameObject.SetActive(false);
            coins++;
            text.text = "Coins: " + coins + "/" + goal; 
        }

        if(collision.CompareTag("Saw") || collision.CompareTag("Mace") || collision.CompareTag("Spikes"))
        {
            Restart(); 
        }
    }

    private void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}
