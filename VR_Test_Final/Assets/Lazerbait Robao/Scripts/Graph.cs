using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct NodePlanet
{
    public float x;
    public float y;
    public float z;
    public int type;

    public NodePlanet(float _x, float _y, float _z, int  _type)
    {
        x = _x;
        y = _y;
        z = _z;
        type = _type;
    }
}

public class Graph : MonoBehaviour
{  
    public int nodeQty;
   public int[,] mAdjacency;
   public NodePlanet[] nodeCoords;
    private bool initialize = false;
    public int NodeQty
    {
        get { return nodeQty; }

        set { nodeQty = value; }
    }

    void Start()
    {
        nodeCoords = new NodePlanet[nodeQty];

        initialize = true;
    }

    void Update()
    {

    }

      private void OnDisable()
    {
        initialize = false;
    }

     public void AddEdge(int startNode, int finalNode)
    {
        mAdjacency[startNode, finalNode] = 1;
        mAdjacency[finalNode, startNode] = 1;
    }

    public void AddCoords(int nodeIdx, float _x, float _y, float _z, int _type)
    {
        nodeCoords[nodeIdx] = new NodePlanet(_x, _y, _z, _type);
    }

    public int getAdyacency(int n, int m)
    {
        return mAdjacency[n, m];
    }

     private void OnDrawGizmos()
    {
        //Verifica si el juego esta corriendo o se trabaja en el editor
        if (initialize)
        {
            //Recorre todos los nodos y dibuja una esfera verde en su posicion
            foreach (NodePlanet node in nodeCoords)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(new Vector3(node.x, node.y, node.z), 0.5f);
            }

            int n = 0;
            int m = 0;

            for (n = 0; n < nodeQty; n++)
            {
                for (m = 0; m < nodeQty; m++)
                {
                    //Busca si hay una arista que conecte los nodos n y m
                    if (mAdjacency[n, m] != 0 && mAdjacency[n, m] < int.MaxValue)
                    {
                        if (mAdjacency[n, m] == mAdjacency[m, n])
                        {
                            //Dibuja una linea de color rojo entre ellos
                            Gizmos.color = Color.red;
                            Gizmos.DrawLine(new Vector3(nodeCoords[n].x, nodeCoords[n].y, nodeCoords[n].z),
                                            new Vector3(nodeCoords[m].x, nodeCoords[m].y, nodeCoords[m].z));
                        }
                        else
                        {
                            //Dibuja una linea de color amarillo entre ellos
                            Gizmos.color = Color.yellow;
                            Gizmos.DrawLine(new Vector3(nodeCoords[n].x, nodeCoords[n].y, nodeCoords[n].z),
                                            new Vector3(nodeCoords[m].x, nodeCoords[m].y, nodeCoords[m].z));
                        }
                    }
                }
            }
        }
    }
    
}
