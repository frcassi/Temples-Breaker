using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Collectible[] collectibles;

    float xRange = 23;
    float zRangeTop = 23;
    float zRangeBottom = -5;

    float noSpawnRadius = 1.0f;

    float spawnInterval = 15.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(KeepSpawningCollectibles());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator KeepSpawningCollectibles()
    {
        while (true)
        {
            // Try to spawn a collectible in a random position until it doesn't overlap anything
            while (!SpawnCollectible()) { }

            // Wait for spawnInterval seconds before spawning a new collectible
            yield return new WaitForSeconds(spawnInterval);
        }
        
    }

    // Spawn a collectible at a random position, if the position is empty
    // Returns true if a collectible has been spawnwed, false if spawning fails because
    // it would collide with another object
    bool SpawnCollectible()
    {
        // Choose random collectible to spawn
        int index = Random.Range(0, collectibles.Length);
        Collectible objToSpawn = collectibles[index];

        // Calculate random spawning position
        float xSpawnPos = Random.Range(-xRange, xRange);
        float zSpawnPos = Random.Range(zRangeBottom, zRangeTop);
        Vector3 spawnPosition = new Vector3(xSpawnPos, 1, zSpawnPos);

        // Check if spawning at spawnPosition would collide with other objects
        int layerMask = ~LayerMask.GetMask("Plane"); // Do not count collisions with ground plane
        Collider[] res = new Collider[1];
        int colls = Physics.OverlapSphereNonAlloc(spawnPosition, noSpawnRadius, res, layerMask, QueryTriggerInteraction.Collide);

        if (colls == 0) // No collisions
        {
            Instantiate(objToSpawn, spawnPosition, objToSpawn.transform.rotation);
            return true;
        } else // Collision with something
        {
          return false;
        }
    }
}
