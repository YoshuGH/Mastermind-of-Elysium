using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGround : MonoBehaviour
{
    [SerializeField]private GameObject planet;
    private Transform battleGroundTransform;
    private int nodeQty;
    private List<Node> nodes;
    private string layerMaskNode = "Nodes";
   

    [Header("Tamaño")]
    public float bgWidth, bgLength, bgHeight;
    

    // Start is called before the first frame update
    void Start()
    {
        battleGroundTransform = this.transform;
        nodeQty = GenerateRandomNodeQty(3);
        nodes = new List<Node>();
        InitBattlegroundGraph();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        Vector3 spawnNodePoint = RandomUniformPointInSphere(new Vector3(0f, bgHeight/2, 0f), false);
        Node tempNode;
        int currentNodes = nodes.Count;

        for(int i = 0; i < nodeQty; i++){
            if(i == 0){
                tempNode = Instantiate(planet, spawnNodePoint, Quaternion.identity, battleGroundTransform).GetComponent<Node>();
                Debug.Log("Primer nodo instanciado en " + spawnNodePoint);
                nodes.Add(tempNode); 
            }

            if(currentNodes >= nodeQty)
                break;

            int neighboursQty = Random.Range(2, 4);

            for(int j = 1; j <= neighboursQty; j++){
                bool canSpawn;

                if(currentNodes < nodeQty){
                    do{
                        spawnNodePoint = RandomUniformPointInSphere(nodes[i].transform.position, true);
                        canSpawn = CanSpawnANode(spawnNodePoint);
                        Debug.Log("Posicion del nodo: " + nodes[i].transform.position + ", punto en el que spawnear el nuevo nodo: " + spawnNodePoint + ", la funcion dice que puede aparecer: " + canSpawn);
                    }while(!canSpawn);
                    
                    tempNode = Instantiate(planet, spawnNodePoint, Quaternion.identity, battleGroundTransform).GetComponent<Node>();
                    nodes.Add(tempNode);
                    currentNodes = nodes.Count;
                    nodes[i].AddNeighbor(nodes[currentNodes - 1]);
                    nodes[currentNodes -1].AddNeighbor(nodes[i]);
                }
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
        Collider[] hitColliders = Physics.OverlapSphere(_pos, 2.5f, layerMaskNode.GetHashCode());

        if(hitColliders.Length > 0 &&  _pos.y <= 0.5f){
            Debug.Log("Hay en medio " + hitColliders.Length + " nodos y la posicion en la que intenta aparecer es " + _pos);
            return false;
        } else {
            return true;
        }
    }

    private Vector3 RandomUniformPointInSphere(Vector3 _pos, bool excludeCenter){
        if(excludeCenter)
            return (Random.onUnitSphere * Mathf.Sqrt(Random.Range(0f, 1f))) * 3.5f + _pos;
        else 
            return (Random.insideUnitSphere * Mathf.Sqrt(Random.Range(0f, 1f))) * 0.5f + _pos;
    }
}
