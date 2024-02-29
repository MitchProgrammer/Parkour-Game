using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab of the bullet
    public Transform bulletSpawnPoint; // Point where the bullet will spawn
    public float fireRate = 0.5f; // Rate of fire (bullets per second)
    private float nextFireTime; // Time when the next bullet can be fired

    void Update()
    {
        // Check if it's time to fire
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            // Set the time for the next bullet
            nextFireTime = Time.time + 1f / fireRate;

            // Fire the bullet
            Fire();
        }
    }

    void Fire()
    {
        // Instantiate a new bullet at the bullet spawn point
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        // Set the bullet's velocity
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.velocity = bullet.transform.forward * 10f; // Adjust the speed as needed
        }
    }
}
