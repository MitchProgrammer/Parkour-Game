using Photon.Pun;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab of the bullet
    public GameObject onlineBulletPrefab;
    public Transform bulletSpawnPoint; // Point where the bullet will spawn
    public float fireRate = 0.5f; // Rate of fire (bullets per second)
    private float nextFireTime; // Time when the next bullet can be fired

    public bool online;
    public PhotonView view;

    public PlayerIdentity PI;

    private void Start()
    {
        if (online) view = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (!online)
        {
            // Local multiplayer logic
            if (PI.player == PlayerIdentity.Players.player1)
            {
                if (Input.GetKey(KeyCode.Q) && Time.time >= nextFireTime)
                {
                    nextFireTime = Time.time + 1f / fireRate;
                    Fire();
                }
            }
            else if (PI.player == PlayerIdentity.Players.player2)
            {
                if (Input.GetKey(KeyCode.RightShift) && Time.time >= nextFireTime)
                {
                    nextFireTime = Time.time + 1f / fireRate;
                    Fire();
                }
            }
        }
        else
        {
            // Online multiplayer logic
            if (view.IsMine && Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
            {
                nextFireTime = Time.time + 1f / fireRate;
                view.RPC("Fire", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    void Fire()
    {
        GameObject bullet;

        if (!online)
        {
            // Instantiate a new bullet locally
            bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        }
        else
        {
            // Instantiate a new bullet online
            bullet = PhotonNetwork.Instantiate(onlineBulletPrefab.name, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        }

        // Set the bullet's velocity
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.velocity = bulletSpawnPoint.forward * 10f; // Use bulletSpawnPoint.forward instead of bullet.transform.forward
        }
    }
}
