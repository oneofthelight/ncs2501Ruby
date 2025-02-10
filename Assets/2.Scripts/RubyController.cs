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

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        // GetAxisLaw를 사용하면 -1,1값이 넘어온다
        //float vertical = Input.GetAxisRaw("Vertical");
        //Debug.Log($"H:{horizontal}");
        //Debug.Log($"V:{vertical}");
        Vector2 position = transform.position;
        position.x += moveSpeed * horizontal * Time.deltaTime;
        position.y += moveSpeed * vertical * Time.deltaTime;
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
