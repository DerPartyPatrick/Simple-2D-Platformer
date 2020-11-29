using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaceController : MonoBehaviour
{
    public bool isGrounded;
    public bool startPosition; 
    public LayerMask groundLayers;
    public int stompDelay;
    public int stompSpeed;
    public int groundTime;
    public int upSpeed;
    public float offset; 

    private float width;
    private float height;
    private Rigidbody2D rb;
    private float startY;
    private bool isStomping;
    private bool stopped;
    private bool wait; 

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
        isGrounded = false;
        isStomping = false;
        startPosition = true;
        stopped = false;
        wait = true; 
        width = GetComponent<Renderer>().bounds.size.x;
        height = GetComponent<Renderer>().bounds.size.y;
        startY = transform.position.y;

        StartCoroutine(Wait()); 
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapArea(
            new Vector2(transform.position.x - width / 2, transform.position.y - height / 2),
            new Vector2(transform.position.x + width / 2, transform.position.y - (height / 2 + 0.001f)),
            groundLayers);

        startPosition = transform.position.y >= startY;

        if((isGrounded || startPosition) && !stopped)
        {
            rb.velocity = new Vector2(0, 0);
            stopped = true; 
        }

        if(!isGrounded && !startPosition)
        {
            stopped = false; 
        }

        if (!isStomping && !wait)
        {
            isStomping = true; 
            StartCoroutine(Stomp());
        }
    }

    IEnumerator Stomp()
    {
        yield return new WaitForSeconds(stompDelay); 
        rb.velocity = new Vector2(0, -stompSpeed);
        yield return new WaitUntil(() => isGrounded);
        yield return new WaitForSeconds(groundTime);
        rb.velocity = new Vector2(0, upSpeed);
        yield return new WaitUntil(() => startPosition);
        isStomping = false; 
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(offset);
        wait = false; 
    }
}
