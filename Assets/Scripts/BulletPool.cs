using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool
{
    GameObject prototype; // Bullet prefab
    List<GameObject> pool; // List of pooled bullets
    bool canGrow; // Whether the pool can grow dynamically
    int nextCheck = 0; // Next bullet to check in the pool

    public BulletPool(GameObject prototype, bool resizeable, int size)
    {
        this.prototype = prototype;
        this.canGrow = resizeable;
        this.pool = new List<GameObject>();

        // Preload the bullet pool with inactive bullets
        for (int i = 0; i < size; i++)
        {
            GameObject bullet = UnityEngine.Object.Instantiate(prototype);
            bullet.SetActive(false);
            pool.Add(bullet);
        }
    }

    public GameObject GetBullet()
    {
        int started = nextCheck;
        do
        {
            if (!pool[nextCheck].activeSelf) // Check if the bullet is inactive
            {
                GameObject bullet = pool[nextCheck];
                bullet.SetActive(true); // Activate the bullet
                nextCheck = (nextCheck + 1) % pool.Count;
                return bullet;
            }
            nextCheck = (nextCheck + 1) % pool.Count;
        } while (nextCheck != started); // Loop around the pool if needed

        // If no inactive bullet is available, grow the pool if allowed
        if (canGrow)
        {
            GameObject bullet = UnityEngine.Object.Instantiate(prototype);
            pool.Add(bullet);
            return bullet;
        }
        else
        {
            // Return null or throw an exception if the pool cannot grow
            return null;
        }
    }
}
