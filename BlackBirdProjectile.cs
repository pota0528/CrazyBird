using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBirdProjectile : Projectile
{
    public GameObject explosionEffect; // 폭발 파티클 프리팹
    public float explosionRadius = 2f; // 폭발 반경 설정
    public float explosionForce = 150f; // 폭발하는 힘

    void Explode()
    {
        // 폭발 이펙트 생성
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }
        
        // 폭발 반경 내 모든 객체에 힘을 가해준다.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D nearbyObject in colliders)
        {
            Rigidbody2D rb = nearbyObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 explosionDirection = (rb.position - (Vector2)transform.position).normalized;
                rb.AddForce(explosionDirection * explosionForce);
            }
        }
        
        // 새 파괴
        Destroy(gameObject, 2f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 기본적인 충돌 로직 실행
        base.OnCollisionEnter2D(collision);
        
        // 충돌 시 폭발하기
        Explode();
    }
}
