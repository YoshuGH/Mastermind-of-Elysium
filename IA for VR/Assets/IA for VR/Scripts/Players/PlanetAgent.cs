using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class PlanetAgent : Agent
{
    /*
     *   Observaciones:
    `      Caminos
            Estado del objetivo
     * */
    [SerializeField]private BattleGround battleGround;
    private Player player;
    public bool isNodeFree;
    public List<Node> nodesInControl;
    private bool state;
    public List<Node> nodeWaypoints;
    public Node targetNode, currentNode;

    private void Start() {
        player = GetComponent<Player>();   
    }

    public override void OnEpisodeBegin()
    {
        nodesInControl = player.PlayerNodes;
        currentNode = nodesInControl[0];
        nodeWaypoints = currentNode.Neighbors;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(currentNode);
        sensor.AddObservation(nodeWaypoints.Count);
        sensor.AddObservation(isNodeFree);
        sensor.AddObservation(targetNode);
        sensor.AddObservation(nodesInControl.Count);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        Node prevCurrentNode = currentNode;
        Node prevTargetNode = targetNode;
        int prevNodesQty = nodesInControl.Count;

        currentNode = nodesInControl[(int)vectorAction[0]];
        targetNode = nodeWaypoints[(int)vectorAction[1]];

        //Cambio de nodo?
        if(isTheSameNode(currentNode, prevCurrentNode)){

        }
        //Cambio de destino?
        if(isTheSameNode(targetNode, prevTargetNode))

        /// ----- Rewards ---- ///

        //Si pierde un nodo(futuro)

        //Si no hace nada hay tabla
        //Verifica si el nodo en el que esta y el nodo objetivo es exactamente el mismo que el step pasado
        if(isTheSameNode(prevCurrentNode, currentNode) &&  isTheSameNode(prevTargetNode, targetNode)){
            AddReward(-0.001f);
        }

        //Conquisto algo mi pana
        if(nodesInControl.Count > prevNodesQty){
            AddReward((nodesInControl.Count - prevNodesQty) * 0.01f);
        }

        


    }

    private bool isTheSameNode(Node node1, Node node2){
        if(node1 == node2){
            return true;
        } else {
            return false;
        }
    }
}
