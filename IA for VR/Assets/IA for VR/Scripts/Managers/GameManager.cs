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
                AddPlayerNode(playerNodesModels[i]);
            }
        }
    }

    void AddPlayerNode(GameObject modelNode)
    {
        Node tempNode = modelNode.GetComponent<Node>();

        if (modelNode.GetComponent<Renderer>() != null)
        {
            playerNodes.Add(tempNode);
            Renderer rend = modelNode.GetComponent<Renderer>();
            rend.material = playerMat;
        }
        else if(tempNode.HaveResources)
        {
            playerNodes.Add(tempNode);
            Renderer rend = modelNode.transform.GetChild(0).GetComponent<Renderer>();
            rend.material = playerMat;
        }
        
    }

}
