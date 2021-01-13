using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //  Teams Tag
    //  0: N/A Team
    //  1: Player Team

    // Variables Privadas
    [Header("Debug")]
    [SerializeField] private List<Node> playerNodes;

    // Variables Publicas
    [Header("Materials")]
    public Material playerMat;

    // Accesores
    public List<Node> PlayerNodes { get { return playerNodes; } }

    // Start is called before the first frame update
    void Start()
    {
        // Inicializacion de Listas
        playerNodes = new List<Node>();

        // Buscar los nodos del personaje
        Invoke("FindPlayerNodes", 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FindPlayerNodes()
    {
        GameObject[] playerNodesModels;
        playerNodesModels = GameObject.FindGameObjectsWithTag("Nodes");

        for (int i = 0; i < playerNodesModels.Length; i++)
        {
            Node tempNode = playerNodesModels[i].GetComponent<Node>();
            if (tempNode.teamInControl == 1)
            {
                AddPlayerNode(tempNode);
            }
        }
    }

    public void AddPlayerNode(Node node)
    {
        

        if (node.GetComponent<Renderer>() != null)
        {
            playerNodes.Add(node);
            Renderer rend = node.GetComponent<Renderer>();
            rend.material = playerMat;
        }
        else if(node.HaveResources)
        {
            playerNodes.Add(node);
            Renderer rend = node.transform.GetChild(0).GetComponent<Renderer>();
            rend.material = playerMat;
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
