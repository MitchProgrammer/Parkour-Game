using Photon.Pun;
using UnityEngine;

public class WeaponProjectile : MonoBehaviour
{
    public float speed = 10f; // Speed of the projectile
    public float explosionRadius = 1f; // Radius of the explosion force
    public float explosionForce = 2f; // Force of the explosion
    public GameObject explosionPrefab; // Prefab of the explosion effect

    public bool online;
    private PhotonView view;

    void Start()
    {
        if (online) view = GetComponent<PhotonView>();

        // Set the initial velocity of the projectile
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Only the owner handles the collision
        if (!online || view.IsMine)
        {
            Explode();
        }
    }

    void Explode()
    {
        // Use RPC to trigger the explosion across all clients
        if (online)
        {
            view.RPC("HandleExplosion", RpcTarget.All);
        }
        else
        {
            HandleExplosion();
        }
    }

    [PunRPC]
    void HandleExplosion()
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

        // Safely destroy the projectile
        if (online && view.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
        else if (!online)
        {
            Destroy(gameObject);
        }
    }
}