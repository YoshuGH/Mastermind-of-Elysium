using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGround : MonoBehaviour
{
    [SerializeField]private GameObject planet;
    [SerializeField]private GameObject planetWithResources;
    private Transform battleGroundTransform;
    private int nodeQty;
    private List<Node> nodes;
    private List<Vector3> transformNodes;
    private string layerMaskNode = "Nodes";
    [SerializeField]private float minDistanceBetweenNodes, maxDistanceBetweenNodes;

    public float bgWidth, bgLength, bgHeight;
    

    // Start is called before the first frame update
    void Start()
    {
        battleGroundTransform = this.transform;
        nodeQty = GenerateRandomNodeQty(3);
        nodes = new List<Node>();
        transformNodes = new List<Vector3>();
        InitBattlegroundGraph();
    }

    public int GenerateRandomNodeQty(int _battlegroundSize)
    {
        switch(_battlegroundSize)
        {
            case 0: //Chico 
                return Random.Range(7, 16);
            case 1: //Mediano
                return Random.Range(12, 21);
            case 2: //Grande
                return Random.Range(17, 26);
            case 3: //Muy grande
                return Random.Range(22, 31);
            default: //Mediano
                return Random.Range(12, 21);
        }
    }

    private void InitBattlegroundGraph()
    {
        Vector3 spawnNodePoint = RandomUniformPointInSphere(new Vector3(0f, bgHeight/2, 0f));
        Node tempNode;
        int currentNodes = nodes.Count;

        for(int i = 0; i < nodeQty; i++){
            if(i == 0){ //Primer nodo
                tempNode = Instantiate(planet, spawnNodePoint, Quaternion.identity, battleGroundTransform).GetComponent<Node>();
                nodes.Add(tempNode); 
            }

            //Verifico si puedo agregar mas nodos
            if(currentNodes >= nodeQty)
                break;

            int neighboursQty = Random.Range(2, 4);

            for(int j = 1; j <= neighboursQty; j++){
                bool canSpawn;

                if(currentNodes < nodeQty){
                    do{
                        spawnNodePoint = RandomUniformPointInSphere(nodes[i].transform.position);
                        canSpawn = CanSpawnANode(spawnNodePoint);
                    }while(!canSpawn);
                    
                    tempNode = Instantiate(planet, spawnNodePoint, Quaternion.identity, battleGroundTransform).GetComponent<Node>();

                    nodes.Add(tempNode);
                    currentNodes = nodes.Count;
                    nodes[i].AddNeighbor(nodes[currentNodes - 1]);
                    nodes[currentNodes -1].AddNeighbor(nodes[i]);
                }
            }
        }

        NormalizeBattleGroundGraph();
    }

    private void NormalizeBattleGroundGraph(){
        foreach(Node node in nodes){
            if(node.Neighbors.Count < 2){
                Collider[] hitColliders = Physics.OverlapSphere(node.transform.position, maxDistanceBetweenNodes);
                Debug.Log("El nodo en la posicion " + node.transform.position + "tiene solo una conexion pero puede conectarse a " + hitColliders);
            }
        }
    }

    private void OnDrawGizmos(){
        foreach(Node node in nodes){
            foreach(Node subNode in node.Neighbors){
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(node.transform.position, subNode.transform.position);
            }
        }
    }

    private bool CanSpawnANode(Vector3 _pos){
        Collider[] hitColliders = Physics.OverlapSphere(_pos, minDistanceBetweenNodes);

        if(hitColliders.Length > 0  ||  _pos.y <= 0.5f)
            return false;
        else 
            return true;
    }

    private Vector3 RandomUniformPointInSphere(Vector3 _pos){
            return (Random.insideUnitSphere * Mathf.Sqrt(Random.Range(0f, 1f))) * maxDistanceBetweenNodes + _pos;
    }
}
