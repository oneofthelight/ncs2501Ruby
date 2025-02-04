using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RubyController : MonoBehaviour
{
    public float moveSpeed = 4.0f;
    void Start()
    {
   
    }

    void Update()
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
        transform.position = position;
    }
}
