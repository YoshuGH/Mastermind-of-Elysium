using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Variables Publicas
    [Header("Prefabs")]
    public GameObject selectionField;
    public float selectionFieldScale = 3.5f;

    //Variables Privadas
    [Header("Managers References")]
    [SerializeField] private GameManager gm;

    [Header("Other")]
    [SerializeField] private Color selectingOutlineColor = Color.magenta;

    private GameObject selecField;
    private List<Node> nodesNearbySelectedNode;
    private int idleSelectedNodeIndex = 0, selectingNodeSelectedNodeIndex = 0;
    private Node idleSelectedNode, selectingNodeSelectedNode;
    private bool selectingNode = false, idle = true;

    // Start is called before the first frame update
    void Start()
    {
        // Inicializacion de listas
        nodesNearbySelectedNode = new List<Node>();

        // Activar el outline de seleccion del nodo, como al principio solo hay un nodo, se activa ese nada mas
        Invoke("OutlineAtStart", 0.11f);
    }

    // Update is called once per frame
    void Update()
    {
        // Si no esta haciendo nada, es decir, si no esta seleccionando
        #region Idle State
        if (idle)
        {
            // Al presionar la flecha itera hacia la derecha (es decir suma 1 al iterador),
            // sobre la lista de nodos que el player posee
            if(Input.GetKeyDown("right"))
            {
                if (gm.PlayerNodes.Count >= 1 && (idleSelectedNodeIndex + 1) < gm.PlayerNodes.Count)
                {
                    ChangeSelectedNode(gm.PlayerNodes[idleSelectedNodeIndex + 1], 0);
                }
                else if(gm.PlayerNodes.Count >= 1 && (idleSelectedNodeIndex + 1) >= gm.PlayerNodes.Count)
                {
                    ChangeSelectedNode(gm.PlayerNodes[0], 0);
                }
            }

            // Al presionar la flecha itera hacia la izquierda (es decir resta 1 al iterador),
            // sobre la lista de nodos que el player posee
            if (Input.GetKeyDown("left"))
            {
                if (gm.PlayerNodes.Count >= 1 && (idleSelectedNodeIndex - 1) >= 0)
                {
                    ChangeSelectedNode(gm.PlayerNodes[idleSelectedNodeIndex - 1], 0);
                }
                else if (gm.PlayerNodes.Count >= 1 && (idleSelectedNodeIndex - 1) < 0)
                {
                    ChangeSelectedNode(gm.PlayerNodes[gm.PlayerNodes.Count -1], 0);
                }
            }

            // Al presionar espacio entra en modo de seleccion de nodos
            if (Input.GetKeyDown("space"))
            {
                selectingNode = true;
                idle = false;
                selecField = Instantiate(selectionField, idleSelectedNode.GetComponentInParent<Transform>().localPosition, idleSelectedNode.GetComponentInParent<Transform>().localRotation);
                selecField.transform.localScale = new Vector3(selectionFieldScale, selectionFieldScale, selectionFieldScale);

                idleSelectedNode.GetComponentInParent<Outline>().OutlineColor = selectingOutlineColor;

                selectingNodeSelectedNode = idleSelectedNode.Neighbors[0];
                selectingNodeSelectedNode.GetComponentInParent<Outline>().enabled = true;

                /*Collider[] colliderTempNodes = Physics.OverlapSphere(idleSelectedNode.GetComponentInParent<Transform>().localPosition,
                    selectionFieldScale/2, LayerMask.GetMask("Nodes"));

                foreach(Collider colliderNodes in colliderTempNodes)
                {
                    if(colliderNodes.GetComponentInParent<Node>() != idleSelectedNode)
                    {
                        nodesNearbySelectedNode.Add(colliderNodes.GetComponentInParent<Node>());
                    }
                }

                idleSelectedNode.GetComponentInParent<Outline>().OutlineColor = selectingOutlineColor;

                if (nodesNearbySelectedNode.Count > 0)
                {
                    selectingNodeSelectedNode = nodesNearbySelectedNode[0];
                    selectingNodeSelectedNode.GetComponentInParent<Outline>().enabled = true;
                }
                else { Debug.LogWarning("No nodes reachable"); }*/
            }
        }
        #endregion

        // Si esta seleccionando un nodo
        #region Selecting Node State
        else if (selectingNode)
        {
            // Al presionar la flecha itera hacia la derecha (es decir suma 1 al iterador),
            // sobre la lista de nodos que haya detectado a su alrededor
            if (Input.GetKeyDown("right"))
            {          

                if ( idleSelectedNode.Neighbors.Count >= 1 && (selectingNodeSelectedNodeIndex + 1) < idleSelectedNode.Neighbors.Count 
                    )
                {
                    ChangeSelectedNode(idleSelectedNode.Neighbors[selectingNodeSelectedNodeIndex + 1], 1);
                }
                else if (idleSelectedNode.Neighbors.Count >= 1 && (selectingNodeSelectedNodeIndex + 1) >= idleSelectedNode.Neighbors.Count)
                {
                    ChangeSelectedNode(idleSelectedNode.Neighbors[0], 1);
                }
            }

            // Al presionar la flecha itera hacia la izquierda (es decir resta 1 al iterador),
            // sobre la lista de nodos que haya detectado a su alrededor
            if (Input.GetKeyDown("left"))
            {

                if (idleSelectedNode.Neighbors.Count >= 1 && (selectingNodeSelectedNodeIndex - 1) >= 0)
                {
                    ChangeSelectedNode(idleSelectedNode.Neighbors[selectingNodeSelectedNodeIndex - 1], 1);
                }
                else if (idleSelectedNode.Neighbors.Count >= 1 && (selectingNodeSelectedNodeIndex - 1) < 0)
                {
                    ChangeSelectedNode(idleSelectedNode.Neighbors[idleSelectedNode.Neighbors.Count - 1], 1);
                }
            }

            if(Input.GetKeyDown("enter"))
            {
                // Al seleccionar ese nodo se ejecuta el codigo
                if(selectingNodeSelectedNode.teamInControl != 1)
                {
                    selectingNodeSelectedNode.teamInControl = 1;
                    gm.AddPlayerNode(selectingNodeSelectedNode);
                }

                // Cancelar la seleccion
                idle = true;
                selectingNode = false;
                selectingNodeSelectedNode.GetComponentInParent<Outline>().enabled = false;
                idleSelectedNode.GetComponentInParent<Outline>().OutlineColor = Color.yellow;
                Destroy(selecField);
                selecField = null;
                nodesNearbySelectedNode.Clear();
                

                idleSelectedNode.GetComponentInParent<Outline>().enabled = false;
                idleSelectedNode = selectingNodeSelectedNode;
                idleSelectedNodeIndex = gm.PlayerNodes.IndexOf(selectingNodeSelectedNode);
                idleSelectedNode.GetComponentInParent<Outline>().enabled = true;

                selectingNodeSelectedNode = null;
            }

            // Si vuelve a presionar espacio estando en seleccion, este cancela la seleccion
            if (Input.GetKeyDown("space"))
            {
                idle = true;
                selectingNode = false;
                selectingNodeSelectedNode.GetComponentInParent<Outline>().enabled = false;
                idleSelectedNode.GetComponentInParent<Outline>().OutlineColor = Color.yellow;
                Destroy(selecField);
                selecField = null;
                nodesNearbySelectedNode.Clear();
                selectingNodeSelectedNode = null;
            }
        }
        #endregion
    }

    // Funcion que activa el Outline.cs del el primer y unico nodo al momento de iniciar el juego
    void OutlineAtStart()
    {
        idleSelectedNode = gm.PlayerNodes[idleSelectedNodeIndex];
        idleSelectedNode.GetComponentInParent<Outline>().enabled = true;
    }

    // Funcion para cambiar de nodo
    // States number:
    //   0: idle
    //   1: selecting node
    void ChangeSelectedNode( Node _nextNode, int _state)
    {
        switch(_state)
        {
            case 0:
                idleSelectedNode.GetComponentInParent<Outline>().enabled = false;
                idleSelectedNode = _nextNode;
                idleSelectedNodeIndex = gm.PlayerNodes.IndexOf(_nextNode);
                idleSelectedNode.GetComponentInParent<Outline>().enabled = true;
                break;
            case 1:
                selectingNodeSelectedNode.GetComponentInParent<Outline>().enabled = false;
                selectingNodeSelectedNode = _nextNode;
                selectingNodeSelectedNodeIndex = idleSelectedNode.Neighbors.IndexOf(_nextNode);
                selectingNodeSelectedNode.GetComponentInParent<Outline>().enabled = true;
                break;
            default:
                Debug.LogError("State non existing");
                break;
        }
    }
}
