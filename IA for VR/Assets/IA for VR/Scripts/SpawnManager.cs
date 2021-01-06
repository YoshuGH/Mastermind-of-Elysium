using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public GameObject shipPrefab;
    public Transform spawnPoint;
    public Transform[] planets;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Spawn", 1f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Spawn()
    {
        Debug.Log("Ship");
        GameObject ship = Instantiate(shipPrefab, spawnPoint.position, spawnPoint.rotation);
        ship.GetComponent<ShipsBehavior>().puntos = planets;
    }

    
}
