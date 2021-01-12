using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Variables Publicas
    public GameObject neutralShipsPref;
    public GameObject playerShipsPref;
    public int initQuantityShips;
    

    //public List<GameObject> ships;

    // Start is called before the first frame update
    void Start()
    {
        //ships = new List<GameObject>();
        Invoke("InitSpawnNeutralShips", 0.11f);
        //InvokeRepeating("Spawn", 1f, 5f);
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

    //  Type ships
    //    0: Neutral
    //    1: Player
   /* GameObject InstantiateShips( Transform _transform, int _quantity, int _type )
    {
        GameObject ship;
        for (int i = 0; i<_quantity; i++)
        {
            switch (_type)
            {
                case 0:
                    ship = Instantiate(neutralShipsPref, _transform.position, _transform.rotation);
                    break;
                case 1:
                    ship = Instantiate(playerShipsPref, _transform.position, _transform.rotation);
                    break;
            }
        }
        return ship;
    }*/

    
}
