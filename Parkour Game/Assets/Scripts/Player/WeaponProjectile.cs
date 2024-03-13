using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponProjectile : MonoBehaviour
{
    public float speed = 10f; // Speed of the projectile
    public float explosionRadius = 1f; // Radius of the explosion force
    public float explosionForce = 2f; // Force of the explosion
    public GameObject explosionPrefab; // Prefab of the explosion effect

    void Start()
    {
        // Set the initial velocity of the projectile
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Trigger explosion upon collision
        Explode();
    }

    void Explode()
    {
        // Instantiate the explosion prefab at the projectile's position
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // Detect nearby colliders within the explosion radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        // Apply explosion force to nearby rigidbodies
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponentInParent<Rigidbody>();
            if (rb != null)
            {
                // Calculate explosion force direction
                Vector3 direction = (rb.transform.position - transform.position).normalized;

                // Apply explosion force to the nearby Rigidbody
                rb.AddForce(direction * explosionForce, ForceMode.Impulse);
            }
        }

        // Destroy the projectile
        Destroy(gameObject);
    }
}
