﻿using System.Collections;
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
    [SerializeField] private BattleGround battleGround;
    [SerializeField] private GameManager gameManager;
    private Player player;
    private int oldPlayerNodeCount;
    //public bool isNodeFree;
    //public List<Node> nodesInControl;
    //public List<Node> nodeWaypoints;

    private void Awake()
    {
        player = GetComponent<Player>();
    }
    /*
public override void Initialize()
{
    player = GetComponent<Player>();
    battleGround = GetComponentInParent<BattleGround>();
    battleGround.Nodes = new List<Node>();
    //battleGround.NodeQty = battleGround.GenerateRandomNodeQty(2);
    oldPlayerNodeCount = player.PlayerNodes.Count;
    gameManager.ResetEverything();
}*/

    public override void OnEpisodeBegin()
    {
        gameManager.ResetEverything();
    }

    private void FixedUpdate()
    {
        //print(gameManager.MapExists);
        if (gameManager.MapExists)
        {
            RequestDecision();
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //Si esta en idle
        if(player.Idle)
        {
            // Que sepa la cantidad de nodos que tiene en su control
            sensor.AddObservation(player.PlayerNodes.Count);
            // Que sepa la posicion del nodo de su lista de nodos en el que esta
            sensor.AddObservation(player.Idle_SelectedNodeIndex);
            // Que sepa en que estado se encuentra
            sensor.AddObservation(player.State);
        }
        else if(player.SelectingNode)
        {
            // Que sepa la cantidad de nodos que se puede mover
            sensor.AddObservation(player.Idle_SelectedNode.Neighbors.Count);
            // Que sepa la posicion del nodo de si lista de nodos a los que se puede mover
            sensor.AddObservation(player.Selecting_SelectedNodeIndex);
            // Que sepa en que estado se encuentra
            sensor.AddObservation(player.State);
        }
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        // Si no esta haciendo nada, es decir, si no esta seleccionando
        #region Idle State
        if (player.Idle)
        {
            // Al presionar la flecha itera hacia la derecha (es decir suma 1 al iterador),
            // sobre la lista de nodos que el player posee
            if (Mathf.FloorToInt(vectorAction[0]) == 1)
            {
                player.MoveRight(player.Idle_SelectedNodeIndex, player.PlayerNodes, 0);
                print("Move right Idle" + player.Idle_SelectedNodeIndex);
            }
            // Al presionar la flecha itera hacia la izquierda (es decir resta 1 al iterador),
            // sobre la lista de nodos que el player posee
            if (Mathf.FloorToInt(vectorAction[0]) == 2)
            {
                player.MoveLeft(player.Idle_SelectedNodeIndex, player.PlayerNodes, 0);
                print("Move left Idle" + player.Idle_SelectedNodeIndex);
            }

            // Al presionar espacio entra en modo de seleccion de nodos
            if (Mathf.FloorToInt(vectorAction[0]) == 3)
            {
                player.EnterSelectMode();
            }
        }
        #endregion

        // Si esta seleccionando un nodo
        #region Selecting Node State
        else if (player.SelectingNode)
        {
            // Al presionar la flecha itera hacia la derecha (es decir suma 1 al iterador),
            // sobre la lista de nodos que haya detectado a su alrededor
            if (Mathf.FloorToInt(vectorAction[0]) == 1)
            {

                player.MoveRight(player.Selecting_SelectedNodeIndex, player.Idle_SelectedNode.Neighbors, 1);
                print("Move right Selecting" + player.Selecting_SelectedNodeIndex);
            }

            // Al presionar la flecha itera hacia la izquierda (es decir resta 1 al iterador),
            // sobre la lista de nodos que haya detectado a su alrededor
            if (Mathf.FloorToInt(vectorAction[0]) == 2)
            {
                player.MoveLeft(player.Selecting_SelectedNodeIndex, player.Idle_SelectedNode.Neighbors, 1);
                print("Move left Selecting" + player.Selecting_SelectedNodeIndex);
            }

            if (Mathf.FloorToInt(vectorAction[0]) == 4)
            {
                player.SelectNode();
            }

            // Si vuelve a presionar espacio estando en seleccion, este cancela la seleccion
            if (Mathf.FloorToInt(vectorAction[0]) == 3)
            {
                player.ExitSelectMode();
            }
        }
        #endregion

        if(oldPlayerNodeCount < player.PlayerNodes.Count)
        {
            AddReward(0.1f);
            oldPlayerNodeCount = player.PlayerNodes.Count;
        }
        else if(oldPlayerNodeCount > player.PlayerNodes.Count)
        {
            AddReward(-0.3f);
            oldPlayerNodeCount = player.PlayerNodes.Count;
        }

        if(battleGround.NodeQty == player.PlayerNodes.Count)
        {
            AddReward(1f);
            EndEpisode();
        }

    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = 0;

        // Si no esta haciendo nada, es decir, si no esta seleccionando
        #region Idle State
        if (player.Idle)
        {
            // Al presionar la flecha itera hacia la derecha (es decir suma 1 al iterador),
            // sobre la lista de nodos que el player posee
            if (Input.GetKey(KeyCode.RightArrow))
            {
                actionsOut[0] = 1;
            }
            // Al presionar la flecha itera hacia la izquierda (es decir resta 1 al iterador),
            // sobre la lista de nodos que el player posee
            if (Input.GetKey("left"))
            {
                actionsOut[0] = 2;
            }

            // Al presionar espacio entra en modo de seleccion de nodos
            if (Input.GetKey(KeyCode.Space))
            {
                actionsOut[0] = 3;

            }
        }
        #endregion

        // Si esta seleccionando un nodo
        #region Selecting Node State
        else if (player.SelectingNode)
        {
            // Al presionar la flecha itera hacia la derecha (es decir suma 1 al iterador),
            // sobre la lista de nodos que haya detectado a su alrededor
            if (Input.GetKey("right"))
            {

                actionsOut[0] = 1;
            }

            // Al presionar la flecha itera hacia la izquierda (es decir resta 1 al iterador),
            // sobre la lista de nodos que haya detectado a su alrededor
            if (Input.GetKey("left"))
            {
                actionsOut[0] = 2;
            }

            if (Input.GetKey("enter"))
            {
                actionsOut[0] = 4;
            }

            // Si vuelve a presionar espacio estando en seleccion, este cancela la seleccion
            if (Input.GetKey(KeyCode.Space))
            {
                actionsOut[0] = 3;
            }
        }
        #endregion
    }

    private bool isTheSameNode(Node node1, Node node2){
        if(node1 == node2){
            return true;
        } else {
            return false;
        }
    }
}
