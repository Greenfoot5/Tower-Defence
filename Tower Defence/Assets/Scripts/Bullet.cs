﻿using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform _target;
    
    public float speed = 30f;
    [Tooltip("Only set if we're dealing AoE")]
    public float explosionRadius;
    public int damage = 50;
    
    [Tooltip("The effect spawned when the bullet hit's a target")]
    public GameObject impactEffect;

    public void Seek(Transform newTarget)
    {
        _target = newTarget;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_target == null)
        {
            Destroy(gameObject);
            return;
        }
        
        // Get the direction of the target, and the distance to move this frame
        var position = transform.position;
        var dir = ((Vector2)_target.position - (Vector2)position);
        var distanceThisFrame = speed * Time.deltaTime;
        
        // TODO - Make it based on target size
        const float targetSize = 0.25f;
        // Have we "hit" the target
        if (dir.magnitude <= targetSize)
        {
            HitTarget();
            return;
        }
        
        // Move the bullet towards the target
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        var toTarget = _target.position - position;
        Vector3.Normalize(toTarget);
        transform.up = toTarget;

    }
    
    // Called when we hit the target
    private void HitTarget()
    {
        // Spawn hit effect
        var position = transform;
        var effectIns = Instantiate(impactEffect, position.position, position.rotation);
        Destroy(effectIns, 2f);
        
        // If we have AoE effect or not
        if (explosionRadius > 0f)
        {
            Explode();
        }
        else
        {
            Damage(_target);
        }

        // Destroy so we only hit once
        Destroy(gameObject);
    }
    
    // Called when dealing damage to a single enemy
    void Damage(Transform enemy)
    {
        Enemy em = enemy.GetComponent<Enemy>();

        if (em != null)
        {
            em.TakeDamage(damage);
        }
    }
    
    // Called if we have an AoE effect
    void Explode()
    {
        // Get's all the enemies in the AoE and calls Damage on them
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D collider2d in colliders)
        {
            if (collider2d.CompareTag("Enemy"))
            {
                Damage(collider2d.transform);
            }
        }
    }
    
    // Allows us to visualise the bullet's AoE in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
