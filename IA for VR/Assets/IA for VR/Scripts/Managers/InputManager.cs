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
    //private List<Node> nodesNearbySelectedNode;
    private int idleSelectedNodeIndex = 0, selectingNodeSelectedNodeIndex = 0;
    private Node idleSelectedNode, selectingNodeSelectedNode;
    private bool selectingNode = false, idle = true;

    //Accesores
    public Node Idle_SelectedNode { set { idleSelectedNode = value; } get { return idleSelectedNode; } }
    public Node Selecting_SelectedNode { set { selectingNodeSelectedNode = value; } get { return selectingNodeSelectedNode; } }
    public int Idle_SelectedNodeIndex { set { selectingNodeSelectedNodeIndex = value; } get { return selectingNodeSelectedNodeIndex; } }
    public int Selecting_SelectedNodeIndex { set { selectingNodeSelectedNodeIndex = value; } get { return selectingNodeSelectedNodeIndex; } }

    /*
    // Start is called before the first frame update
    void Start()
    {
        // Activar el outline de seleccion del nodo, como al principio solo hay un nodo, se activa ese nada mas
        Invoke("OutlineAtStart", 0.11f);
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
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
    
    void Inputs()
    {
        // Si no esta haciendo nada, es decir, si no esta seleccionando
        #region Idle State
        if (idle)
        {
            // Al presionar la flecha itera hacia la derecha (es decir suma 1 al iterador),
            // sobre la lista de nodos que el player posee
            if (Input.GetKeyDown("right"))
            {
                MoveRight(idleSelectedNodeIndex, gm.PlayerNodes, 0);
            }
            // Al presionar la flecha itera hacia la izquierda (es decir resta 1 al iterador),
            // sobre la lista de nodos que el player posee
            if (Input.GetKeyDown("left"))
            {
                    MoveLeft(idleSelectedNodeIndex, gm.PlayerNodes, 0);
               }

            // Al presionar espacio entra en modo de seleccion de nodos
            if (Input.GetKeyDown("space"))
            {
                EnterSelectMode();

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

                MoveRight(selectingNodeSelectedNodeIndex, idleSelectedNode.Neighbors, 1);
            }

            // Al presionar la flecha itera hacia la izquierda (es decir resta 1 al iterador),
            // sobre la lista de nodos que haya detectado a su alrededor
            if (Input.GetKeyDown("left"))
            {
                MoveLeft(selectingNodeSelectedNodeIndex, idleSelectedNode.Neighbors, 1);
            }

            if (Input.GetKeyDown("enter"))
            {
                SelectNode();
            }

            // Si vuelve a presionar espacio estando en seleccion, este cancela la seleccion
            if (Input.GetKeyDown("space"))
            {
                ExitSelectMode();
            }
        }
        #endregion
    }*/
    /*
    public void MoveRight(int _iterator, List<Node> _listToIterate, int _state)
    {
        if (_listToIterate.Count >= 1 && (_iterator + 1) < _listToIterate.Count)
        {
            ChangeSelectedNode(_listToIterate[_iterator + 1], _state);
        }
        else if (_listToIterate.Count >= 1 && (_iterator + 1) >= _listToIterate.Count)
        {
            ChangeSelectedNode(_listToIterate[0], _state);
        }
    }

    public void MoveLeft( int _iterator, List<Node> _listToIterate, int _state)
    {
        if (_listToIterate.Count >= 1 && (_iterator - 1) >= 0)
        {
            ChangeSelectedNode(_listToIterate[_iterator - 1], _state);
        }
        else if (_listToIterate.Count >= 1 && (_iterator - 1) < 0)
        {
            ChangeSelectedNode(_listToIterate[_listToIterate.Count - 1], _state);
        }
    }

    public void EnterSelectMode()
    {
        selectingNode = true;
        idle = false;
        //selecField = Instantiate(selectionField, idleSelectedNode.GetComponentInParent<Transform>().localPosition, idleSelectedNode.GetComponentInParent<Transform>().localRotation);
        //selecField.transform.localScale = new Vector3(selectionFieldScale, selectionFieldScale, selectionFieldScale);

        idleSelectedNode.GetComponentInParent<Outline>().OutlineColor = selectingOutlineColor;

        selectingNodeSelectedNode = idleSelectedNode.Neighbors[0];
        selectingNodeSelectedNode.GetComponentInParent<Outline>().enabled = true;
    }

    public void ExitSelectMode()
    {
        idle = true;
        selectingNode = false;
        selectingNodeSelectedNode.GetComponentInParent<Outline>().enabled = false;
        idleSelectedNode.GetComponentInParent<Outline>().OutlineColor = Color.yellow;
        //Destroy(selecField);
        //selecField = null;
        nodesNearbySelectedNode.Clear();
        selectingNodeSelectedNode = null;
    }

    public void SelectNode()
    {
        // Al seleccionar ese nodo se ejecuta el codigo
        if (selectingNodeSelectedNode.teamInControl != 1)
        {
            selectingNodeSelectedNode.teamInControl = 1;
            gm.AddPlayerNode(selectingNodeSelectedNode);
        }

        // Cancelar la seleccion
        idle = true;
        selectingNode = false;
        selectingNodeSelectedNode.GetComponentInParent<Outline>().enabled = false;
        idleSelectedNode.GetComponentInParent<Outline>().OutlineColor = Color.yellow;
        //Destroy(selecField);
        //selecField = null;
        nodesNearbySelectedNode.Clear();


        idleSelectedNode.GetComponentInParent<Outline>().enabled = false;
        idleSelectedNode = selectingNodeSelectedNode;
        idleSelectedNodeIndex = gm.PlayerNodes.IndexOf(selectingNodeSelectedNode);
        idleSelectedNode.GetComponentInParent<Outline>().enabled = true;

        selectingNodeSelectedNode = null;
    }
    */
}
