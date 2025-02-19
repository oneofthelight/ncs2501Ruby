using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 4.0f;
    public bool vertical;
    public float changeTime = 3.0f;
    public int needFix = 3;
    public GameObject dialogBox;
    public AudioClip fixedClip;
    public AudioClip completedClip;
    private float timer;
    private int direction = 1;
    private Vector2 position;
    private Rigidbody2D rigidbody2D;
    private Animator animator;
    private bool broken;
    private int fixedCount;
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        timer = changeTime;
        position = rigidbody2D.position;
        animator = GetComponent<Animator>();
        broken = true;
    }
    void Update()
    {
        if(!broken)
        {
            return;
        }
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
    public void Fix()
    {
        fixedCount++;
        if(fixedCount >= needFix)
        {
            broken = false;
            rigidbody2D.simulated = false;
            animator.SetTrigger("Fixed");
            // 루비에게 fixed 알리기
            RubyController ruby = GameObject.FindWithTag("RUBY").GetComponent<RubyController>();
            ruby.PlaySound(fixedClip);
            // jambi에게 fixed 알리기
            NPC jambi = GameObject.FindWithTag("JAMBI").GetComponent<NPC>();
            if(jambi.NoticeRobotFixed())
            {
                ruby.PlaySound(completedClip);
            }
        }
    }
}
