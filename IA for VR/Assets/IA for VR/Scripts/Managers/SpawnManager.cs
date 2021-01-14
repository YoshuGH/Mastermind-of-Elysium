using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Variables Publicas
    [Header("Prefabs")]
    public GameObject neutralShipsPref;
    public GameObject playerShipsPref;
    public int initQuantityShips;

    //Variables Privadas
    [Header("Managers")]
    [SerializeField] private GameManager gm;
    

    // Start is called before the first frame update
    void Start()
    {
        //ships = new List<GameObject>();
        Invoke("InitSpawnNeutralShips", 0.11f);
        InvokeRepeating("SpawnPlayerShips", 1f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitSpawnNeutralShips()
    {
        GameObject[] tempPlanets = GameObject.FindGameObjectsWithTag("Nodes");
        GameObject ship;

        foreach(GameObject node in tempPlanets)
        {
            
            Node tempNode = node.GetComponent<Node>();

            if(tempNode.teamInControl == 0)
            {
                Transform tempTransform = node.transform.Find("SpawnPoint");

                for(int i = 0; i < initQuantityShips; i++)
                {
                    ship = Instantiate(neutralShipsPref, tempTransform.position, tempTransform.rotation);
                    tempNode.OrbitShips.Add(ship);
                    ship.transform.SetParent(tempTransform);
                }
            }
        }
    }

    void SpawnPlayerShips()
    {
        GameObject ship;

        foreach (Player player in gm.Players)
        {
            foreach(Node node in player.PlayerNodes)
            {
                Transform spawnpointTransform = node.transform.Find("SpawnPoint");
                ship = Instantiate(playerShipsPref, spawnpointTransform.position, spawnpointTransform.rotation);
                ship.transform.SetParent(spawnpointTransform);
                node.OrbitShips.Add(ship);
            }
        }

        
    }

    
}
