using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun: MonoBehaviour
{
    [SerializeField]
    float speed;

    [SerializeField]
    PlayerRenewal player;

    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        SunMove();
        Move();
    }

    void SunMove()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(gameObject.transform.position); 

        if (pos.x < 0f) 
            pos.x = 0f; 
        if (pos.x > 1f) 
            pos.x = 1f; 
        if (pos.y < 0f) 
            pos.y = 0f; 
        if (pos.y > 1f) 
            pos.y = 1f;

        gameObject.transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    void Move()
    {
        if (Mathf.Abs(player.GetComponent<Rigidbody2D>().velocity.x) > 1)
        {
            //Vector2 curPos = transform.position;
            Vector2 nextPos = new Vector2(player.H * player.ApplySpeed, 0) * (speed * 0.5f);
            //transform.position = curPos + nextPos;
            rigid.velocity = nextPos;
        }
        else
        {
            rigid.velocity = new Vector2(0, 0);
        }
    }
}
