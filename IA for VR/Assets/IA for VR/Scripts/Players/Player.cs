using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{
    [SerializeField]private List<Node> playerNodes;
    //private List<Ships> currentShips;
    public Material material;
    //public bool isIA;
    [SerializeField]private int teamId;
    public int TeamId {  get {  return teamId; } }
    [SerializeField]private int maxQtyPerNode;
    private int maxShipsQty;
    public int MaxshipsQty { get { return maxShipsQty; } }
    private int resources;
    public int Resources { get { return resources; } }

    public void AddResources(int _resourcesQty) => resources += _resourcesQty;

    public void UpdateMaxShipQty() => maxShipsQty  = playerNodes.Count * maxQtyPerNode;

    public void AddPlayerNode (Node node) => playerNodes.Add(node);
    public List<Node> PlayerNodes { get { return playerNodes; } }
    public void RemoveNode(Node node) => playerNodes.Remove(node);

    // Variables del Input Manager
    private int idleSelectedNodeIndex = 0, selectingNodeSelectedNodeIndex = 0;
    private Node idleSelectedNode, selectingNodeSelectedNode;
    private bool selectingNode = false, idle = true;
    [Header("Other")]
    [SerializeField] private Color selectingOutlineColor = Color.magenta;

    //Accesores del Input Manager
    public Node Idle_SelectedNode { set { idleSelectedNode = value; } get { return idleSelectedNode; } }
    public Node Selecting_SelectedNode { set { selectingNodeSelectedNode = value; } get { return selectingNodeSelectedNode; } }
    public int Idle_SelectedNodeIndex { set { selectingNodeSelectedNodeIndex = value; } get { return selectingNodeSelectedNodeIndex; } }
    public int Selecting_SelectedNodeIndex { set { selectingNodeSelectedNodeIndex = value; } get { return selectingNodeSelectedNodeIndex; } }

    // Start is called before the first frame update
    void Start()
    {
        // Activar el outline de seleccion del nodo, como al principio solo hay un nodo, se activa ese nada mas
        Invoke("OutlineAtStart", 0.11f);
    }

    #region Input Manager

    // Funcion para cambiar de nodo
    // States number:
    //   0: idle
    //   1: selecting node
    void ChangeSelectedNode(Node _nextNode, int _state)
    {
        switch (_state)
        {
            case 0:
                idleSelectedNode.GetComponentInParent<Outline>().enabled = false;
                idleSelectedNode = _nextNode;
                idleSelectedNodeIndex = playerNodes.IndexOf(_nextNode);
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

    // Funcion que activa el Outline.cs del el primer y unico nodo al momento de iniciar el juego
    void OutlineAtStart()
    {
        idleSelectedNode = playerNodes[idleSelectedNodeIndex];
        idleSelectedNode.GetComponentInParent<Outline>().enabled = true;
    }

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

    public void MoveLeft(int _iterator, List<Node> _listToIterate, int _state)
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
        selectingNodeSelectedNode = null;
    }

    public void SelectNode()
    {
        // Al seleccionar ese nodo se ejecuta el codigo
        if (selectingNodeSelectedNode.teamInControl != 1)
        {
            selectingNodeSelectedNode.teamInControl = 1;
            AddPlayerNode(selectingNodeSelectedNode);
        }

        // Cancelar la seleccion
        idle = true;
        selectingNode = false;
        selectingNodeSelectedNode.GetComponentInParent<Outline>().enabled = false;
        idleSelectedNode.GetComponentInParent<Outline>().OutlineColor = Color.yellow;
        //Destroy(selecField);
        //selecField = null;


        idleSelectedNode.GetComponentInParent<Outline>().enabled = false;
        idleSelectedNode = selectingNodeSelectedNode;
        idleSelectedNodeIndex = playerNodes.IndexOf(selectingNodeSelectedNode);
        idleSelectedNode.GetComponentInParent<Outline>().enabled = true;

        selectingNodeSelectedNode = null;
    }

    #endregion
}
