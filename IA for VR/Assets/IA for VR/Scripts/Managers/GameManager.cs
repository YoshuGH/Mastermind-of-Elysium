using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //  Teams Tag
    //  0: N/A Team
    //  1: Player Team

    // Variables Privadas
    [Header("Lista de jugadores")]
    [SerializeField] private List<Player> players;




    // Start is called before the first frame update
    void Start()
    {
        // Buscar los nodos del personaje
        Invoke("FindPlayerNodes", 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FindPlayerNodes(Player player)
    {
        GameObject[] playerNodesModels;
        playerNodesModels = GameObject.FindGameObjectsWithTag("Nodes");

        for (int i = 0; i < playerNodesModels.Length; i++)
        {
            Node tempNode = playerNodesModels[i].GetComponent<Node>();
            if (tempNode.teamInControl == player.TeamId)
            {
                AddPlayerNode(tempNode, player);
            }
        }
    }

    public void AddPlayerNode(Node node, Player player)
    {
        

        if (node.GetComponent<Renderer>() != null)
        {
            player.AddPlayerNode(node);
            Renderer rend = node.GetComponent<Renderer>();
            rend.material = player.material;
        }
        else if(node.HaveResources)
        {
            player.AddPlayerNode(node);
            Renderer rend = node.transform.GetChild(0).GetComponent<Renderer>();
            rend.material = player.material;
        }
        
    }

    public void FightForNode(Node _firstNode, Node _secondNode)
    {
        if(_firstNode.OrbitShips.Count > _secondNode.OrbitShips.Count)
        {
            // Mandar las naves al nodo
            foreach(GameObject ship in _firstNode.OrbitShips)
            {
                // Solo se agregan los nodos y la nave se mueve sola
                ship.GetComponent<ShipsBehavior>().Puntos.Add(_firstNode.transform.Find("SpawnPoint"));
                ship.GetComponent<ShipsBehavior>().Puntos.Add(_secondNode.transform.Find("SpawnPoint"));
            }
        }
    }
}
