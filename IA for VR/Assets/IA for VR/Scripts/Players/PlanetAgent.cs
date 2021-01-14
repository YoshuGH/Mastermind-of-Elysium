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
    public bool isNodeFree;
    public List<Node> nodesInControl;
    public List<Node> nodeWaypoints;
    public Node targetNode, currentNode;

    public override void OnEpisodeBegin()
    {
        nodesInControl = null;//Agregar el accesor
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

        currentNode = nodesInControl[(int)vectorAction[0]];
        targetNode = nodeWaypoints[(int)vectorAction[1]];


        

        //Si pierde un nodo(futuro)

        //Si no hace nada hay tabla
        //Verifica si el nodo en el que esta y el nodo objetivo es exactamente el mismo que el step pasado
        if(isTheSameNode(prevCurrentNode, currentNode) &&  isTheSameNode(prevTargetNode, targetNode)){
            AddReward(-0.001f);
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
