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
    [SerializeField] private BattleGround battleGround;
    [SerializeField] private GameManager gameManager;
    private Player player;
    private int oldPlayerNodeCount;
    private bool haveNodeToConquist = false;
    private bool isNodeFree;

    private void Awake()
    {
        player = GetComponent<Player>();
        battleGround = GetComponentInParent<BattleGround>();
        gameManager = battleGround.GetComponentInChildren<GameManager>();
    }

    public override void OnEpisodeBegin()
    {
        battleGround.MapExists = false;
        player.Idle_SelectedNodeIndex = 0;
        player.Selecting_SelectedNodeIndex = 0;
        gameManager.RestartEverything();
        
    }

    private void FixedUpdate()
    {
        //print(gameManager.MapExists);
        if (battleGround.MapExists)
        {
            RequestDecision();
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        isNodeFree = false;
        haveNodeToConquist = false;

        //Si esta en idle
        if (player.Idle)
        {
            // Que sepa la posicion del nodo de su lista de nodos en el que esta
            sensor.AddObservation(player.Idle_SelectedNodeIndex);
            // Que sepa en que estado se encuentra
            sensor.AddObservation(player.State);
            // Que sepa de que equipo es el nodo que esta seleccionando
            sensor.AddObservation(player.Idle_SelectedNode.teamInControl);
            // Que sepa si hay nodos que no son de su equipo, tomando de referenci el nodo en el que esta
            foreach(Node node in player.Idle_SelectedNode.Neighbors)
            {
                if(node.teamInControl != player.TeamId)
                {
                    haveNodeToConquist = true;
                }
            }
            sensor.AddObservation(haveNodeToConquist);
            // Que sepa cuantos nodos le faltan de manera global
            sensor.AddObservation(battleGround.NodeQty - player.PlayerNodes.Count);
            //print(haveNodeToConquist);
        }
        else if(player.SelectingNode)
        {
            // Que sepa la posicion del nodo de si lista de nodos a los que se puede mover
            sensor.AddObservation(player.Selecting_SelectedNodeIndex);
            // Que sepa en que estado se encuentra
            sensor.AddObservation(player.State);
            // Que sepa de que equipo es el nodo que esta seleccionando
            sensor.AddObservation(player.Selecting_SelectedNode.teamInControl);
            // Que sepa cuantos nodos no son de su equipo, tomando de referenci el nodo en el que esta
            if(player.Selecting_SelectedNode.teamInControl != player.TeamId)
            {
                isNodeFree = true;
            }
            sensor.AddObservation(isNodeFree);
            // Que sepa cuantos nodos le faltan de manera global
            sensor.AddObservation(battleGround.NodeQty - player.PlayerNodes.Count);
            //print(haveNodeToConquist);
        }
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        // Se le quita recompensa por tiempo para que la IA no se quede sin hacer nada
        if(battleGround.MapExists)
        {
            this.AddReward(-0.0001f * Time.deltaTime);
        }

        // Si la IA ya conquisto todos los nodos
        if (battleGround.NodeQty == player.PlayerNodes.Count)
        {
            // Hace que la IA deje de ejecutar acciones al no existir el mapa
            battleGround.MapExists = false;
            // La recompensa por haber terminado de manera exitosa
            SetReward(5.0f);
            // Acaba el episodio y comienza todo de nuevo
            EndEpisode();
        }

        // Si no esta haciendo nada, es decir, si no esta seleccionando
        #region Idle State
        if (player.Idle)
        {
            // Al presionar la flecha itera hacia la derecha (es decir suma 1 al iterador),
            // sobre la lista de nodos que el player posee
            if (Mathf.FloorToInt(vectorAction[0]) == 1 && battleGround.MapExists)
            {
                //print("Move right Idle" + player.Idle_SelectedNodeIndex);
                player.MoveRight(player.Idle_SelectedNodeIndex, player.PlayerNodes, 0);

                if(haveNodeToConquist && player.PlayerNodes.Count > 1)
                {
                    AddReward(0.0001f);
                }
                
            }
            // Al presionar la flecha itera hacia la izquierda (es decir resta 1 al iterador),
            // sobre la lista de nodos que el player posee
            if (Mathf.FloorToInt(vectorAction[0]) == 2 && battleGround.MapExists)
            {
                //print("Move left Idle" + player.Idle_SelectedNodeIndex);
                player.MoveLeft(player.Idle_SelectedNodeIndex, player.PlayerNodes, 0);

                if (haveNodeToConquist && player.PlayerNodes.Count > 1)
                {
                    AddReward(0.0001f);
                }
            }

            // Al presionar espacio entra en modo de seleccion de nodos
            if (Mathf.FloorToInt(vectorAction[0]) == 3 && battleGround.MapExists)
            {
                // Se le recompensa por entrar en seleccion en un nodo que tiene nodos por conquitar
                if (haveNodeToConquist)
                {
                    AddReward(0.0005f);
                }

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
            if (Mathf.FloorToInt(vectorAction[0]) == 1 && battleGround.MapExists)
            {
                player.MoveRight(player.Selecting_SelectedNodeIndex, player.Idle_SelectedNode.Neighbors, 1);
            }

            // Al presionar la flecha itera hacia la izquierda (es decir resta 1 al iterador),
            // sobre la lista de nodos que haya detectado a su alrededor
            if (Mathf.FloorToInt(vectorAction[0]) == 2 && battleGround.MapExists)
            {
                player.MoveLeft(player.Selecting_SelectedNodeIndex, player.Idle_SelectedNode.Neighbors, 1);
            }

            // En esta accion es cuando selecciona un nodo
            if (Mathf.FloorToInt(vectorAction[0]) == 4 && battleGround.MapExists)
            {
                // Por seleccionar un nodo se le recompensa
                AddReward(1f);
                
                // Si selecciona un nodo que no es de su equipo y se puede conquistar se le recompensa aun mas
                if (player.Selecting_SelectedNode.CanConquisting && player.Selecting_SelectedNode.teamInControl != player.TeamId)
                {
                    AddReward(0.8f);
                }
                // Si selecciona un nodo que no es de su equipo pero esta en proceso de conquista se le castiga, para que no abuse de esto
                else if (!player.Selecting_SelectedNode.CanConquisting && player.Selecting_SelectedNode.teamInControl != player.TeamId)
                {
                    AddReward(-1.1f);
                }
                // Si selecciona un nodo de su mismo equipo se le castiga
                else if(player.Selecting_SelectedNode.teamInControl == player.TeamId)
                {
                    AddReward(-1.3f);
                }
                
                player.SelectNode();
            }

            // Si vuelve a presionar espacio estando en seleccion, este cancela la seleccion
            if (Mathf.FloorToInt(vectorAction[0]) == 3 && battleGround.MapExists)
            {
                AddReward(-0.3f);
                player.ExitSelectMode();
            }
        }
        #endregion

        if(oldPlayerNodeCount < player.PlayerNodes.Count)
        {
            AddReward(0.5f);
            oldPlayerNodeCount = player.PlayerNodes.Count;
        }
        else if(oldPlayerNodeCount > player.PlayerNodes.Count)
        {
            AddReward(-0.3f);
            oldPlayerNodeCount = player.PlayerNodes.Count;
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
            if (Input.GetKey(KeyCode.LeftArrow))
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
}
