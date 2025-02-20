using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RubyController : MonoBehaviour
{
    public float moveSpeed = 4.0f;
    public int maxHealth = 5;
    public int health { get { return currentHealth;}}
    public float timeInvicible = 2.0f;
    public GameObject projectilePrefab;
    public ParticleSystem collEffectPrefab;
    public AudioClip throwClip;
    public AudioClip hitClip;
    public GameObject AndroidPanel;
    private bool isInvicible;
    private float inInvincibleTimer;
    private int currentHealth;
    private Rigidbody2D rb2d;
    private Vector2 position;
    private Animator animator;
    private Vector2 lookDirection = new Vector2(1, 0);
    private AudioSource audioSource;
    private PlayerMove moves;

    private void Start()
    {
#if (UNITY_ANDROID)
        AndroidPanel.SetActive(true);
#else
        AndroidPanel.SetActive(false);
#endif
        rb2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        position = rb2d.position;
        animator = GetComponent<Animator>();
        audioSource= GetComponent<AudioSource>();
        moves = GetComponent<PlayerMove>();
    }

    private void Update()
    {
#if (!UNITY_ANDROID)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        // GetAxisLaw를 사용하면 -1,1값이 넘어온다
        //float vertical = Input.GetAxisRaw("Vertical");
        //Debug.Log($"V:{vertical}");
        //Debug.Log($"H:{horizontal}");
        Vector2 move = new Vector2(horizontal, vertical);
#else
        Vector2 move = moves.MoveInput.normalized;
#endif
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
        
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Launch();
            PlaySound(throwClip);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Talk();
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
            //Instantiate(collEffectPrefab, rb2d.position + Vector2.up * 0.2f, Quaternion.identity);
            PlaySound(hitClip);
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        //Debug.Log(currentHealth + "/" + maxHealth);
        Debug.Log($"{currentHealth}/{maxHealth}");
        UIHealhtBar.instance.SetValue(currentHealth/(float)maxHealth);
    }
    public void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rb2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
    }
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
    public void Talk()
    {
            RaycastHit2D hit = Physics2D.Raycast(rb2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                //NPC jambi = hit.collider.GetComponent<NPC>();  이 두줄을 아래의 한줄로 변경 가능
                //if (jambi != null)
                hit.collider.TryGetComponent<NPC>(out var jambi);
                {
                    jambi.DisplayDialog();
                }
            }
    }
}
