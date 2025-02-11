using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RubyController : MonoBehaviour
{
    public float moveSpeed = 4.0f;
    public int maxHealth = 5;
    public int health { get { return currentHealth;}}
    public float timeInvicible = 2.0f;
    private bool isInvicible;
    private float inInvincibleTimer;
    private int currentHealth;
    private Rigidbody2D rb2d;
    private Vector2 position;
    private Animator animator;
    private Vector2 lookDirection = new Vector2(1, 0);

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        position = rb2d.position;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        // GetAxisLaw를 사용하면 -1,1값이 넘어온다
        //float vertical = Input.GetAxisRaw("Vertical");
        //Debug.Log($"H:{horizontal}");
        //Debug.Log($"V:{vertical}");
        Vector2 move = new Vector2(horizontal, vertical);
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        //position.x += moveSpeed * horizontal * Time.deltaTime;
        //position.y += moveSpeed * vertical * Time.deltaTime;
        position = position + move * moveSpeed * Time.deltaTime;
        rb2d.MovePosition(position);

        if (isInvicible)
        {
            inInvincibleTimer -= Time.deltaTime;
            if (inInvincibleTimer < 0)
                isInvicible = false;
        }
        
    }
    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            animator.SetTrigger("Hit");
            if(isInvicible)
                return;
            isInvicible = true;
            inInvincibleTimer = timeInvicible;
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        //Debug.Log(currentHealth + "/" + maxHealth);
        Debug.Log($"{currentHealth}/{maxHealth}");
    }
}
