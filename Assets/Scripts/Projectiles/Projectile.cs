using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float speed;
    private float reach;
    private float startPositionX;

    [SerializeField] private float gravity;
    [SerializeField] private float hitRadius;

    private Rigidbody2D rb2d;

    private bool isGravityOn;
    private bool hasHitGround;

    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private Transform hitPosition;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

        rb2d.gravityScale = 0f;
        rb2d.velocity = transform.right * speed;

        isGravityOn = false;
        
        startPositionX = transform.position.x;
    }

    private void Update()
    {
        if (!hasHitGround && isGravityOn)
        {
            var angle = Mathf.Atan2(rb2d.velocity.y, rb2d.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void FixedUpdate()
    {
        if (!hasHitGround)
        {
            var hitPlayer = Physics2D.OverlapCircle(hitPosition.position, hitRadius, whatIsPlayer);
            var hitGround = Physics2D.OverlapCircle(hitPosition.position, hitRadius, whatIsGround);

            if (hitPlayer)
            {
                Destroy(gameObject);
            }
            else if (hitGround)
            {
                hasHitGround = true;
                rb2d.gravityScale = 0f;
                rb2d.velocity = Vector2.zero;
            }
            
            if (Mathf.Abs(startPositionX - transform.position.x) >= reach && isGravityOn)
            {
                isGravityOn = true;
                rb2d.gravityScale = gravity;
            }
        }
    }

    public void FireProjectile(float speed, float reach, float damage)
    {
        this.speed = speed;
        this.reach = reach;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(hitPosition.position, hitRadius);
    }
}
