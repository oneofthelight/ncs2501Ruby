using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 4.0f;
    public bool vertical;
    public float changeTime = 3.0f;
    private float timer;
    private int direction = 1;
    private Vector2 position;
    private Rigidbody2D rigidbody2D;
    private Animator animator;
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        timer = changeTime;
        position = rigidbody2D.position;
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <0)
        {
            direction = -direction;
            timer = changeTime;
        }
        
        if (vertical)
        {
            position.y += moveSpeed * direction * Time.deltaTime;
            animator.SetFloat("MoveX", 0);
            animator.SetFloat("MoveY", direction);
        }
        else
        {
            animator.SetFloat("MoveX", direction);
            animator.SetFloat("MoveY", 0);
            position.x += moveSpeed * direction * Time.deltaTime;
        }
        rigidbody2D.MovePosition(position);
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController>();

        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }
}
