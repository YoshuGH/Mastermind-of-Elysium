using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public GameObject shipPrefab;
    public Transform spawnPoint;
    public List<Transform> planets;

    public List<GameObject> ships;

    // Start is called before the first frame update
    void Start()
    {
        ships = new List<GameObject>();
        planets = new List<Transform>();
        InvokeRepeating("Spawn", 1f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Spawn()
    {
        
        GameObject ship = Instantiate(shipPrefab, spawnPoint.position, spawnPoint.rotation);
        ships.Add(ship);
    }

    
}
