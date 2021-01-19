using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{
    [SerializeField]private List<Node> playerNodes;
    //private List<Ships> currentShips;
    public Material material;
    //public GameObject holderTimer;
    //bool spawn = false;
    //public bool isIA;
    [SerializeField]private int teamId;
    public int TeamId {  get {  return teamId; } }
    [SerializeField]private int maxQtyPerNode;
    private int maxShipsQty;
    public int MaxshipsQty { get { return maxShipsQty; } }
    private int resources;
    public int Resources { get { return resources; } }
    private int state = 0;
    public int State { get { return state; } }
    public bool withOutline = true;

    public void AddResources(int _resourcesQty) => resources += _resourcesQty;

    public void UpdateMaxShipQty() => maxShipsQty  = playerNodes.Count * maxQtyPerNode;

    public void AddPlayerNode (Node node) {
        if (node.GetComponent<Renderer>() != null)
        {
            playerNodes.Add(node);
            node.teamInControl = teamId;
            Renderer rend = node.GetComponent<Renderer>();
            rend.material = material;
        }
        else if(node.HaveResources)
        {
            playerNodes.Add(node);
            node.teamInControl = teamId;
            Renderer rend = node.transform.GetChild(0).GetComponent<Renderer>();
            rend.material = material;
        }
    }
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
    public int Idle_SelectedNodeIndex { set { idleSelectedNodeIndex = value; } get { return idleSelectedNodeIndex; } }
    public int Selecting_SelectedNodeIndex { set { selectingNodeSelectedNodeIndex = value; } get { return selectingNodeSelectedNodeIndex; } }
    public bool Idle { get { return idle; } }
    public bool SelectingNode { get { return selectingNode; } }

    // Start is called before the first frame update
    void Start()
    {
        // Activar el outline de seleccion del nodo, como al principio solo hay un nodo, se activa ese nada mas
        //Invoke("OutlineAtStart", 0.15f);
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
                if(withOutline)
                {
                    idleSelectedNode.GetComponentInParent<Outline>().enabled = false;
                    idleSelectedNode = _nextNode;
                    idleSelectedNodeIndex = playerNodes.IndexOf(_nextNode);
                    idleSelectedNode.GetComponentInParent<Outline>().enabled = true;
                }
                else
                {
                    idleSelectedNode = _nextNode;
                    idleSelectedNodeIndex = playerNodes.IndexOf(_nextNode);
                }
                
                break;
            case 1:
                if(withOutline)
                {
                    selectingNodeSelectedNode.GetComponentInParent<Outline>().enabled = false;
                    selectingNodeSelectedNode = _nextNode;
                    selectingNodeSelectedNodeIndex = idleSelectedNode.Neighbors.IndexOf(_nextNode);
                    selectingNodeSelectedNode.GetComponentInParent<Outline>().enabled = true;
                }
                else
                {
                    selectingNodeSelectedNode = _nextNode;
                    selectingNodeSelectedNodeIndex = idleSelectedNode.Neighbors.IndexOf(_nextNode);
                }
                break;
            default:
                Debug.LogError("State non existing");
                break;
        }
    }

    // Funcion que activa el Outline.cs del el primer y unico nodo al momento de iniciar el juego
    public void OutlineAtStart()
    {
        idleSelectedNodeIndex = 0;
        idleSelectedNode = playerNodes[idleSelectedNodeIndex];
        if(withOutline)
        {
            idleSelectedNode.GetComponentInParent<Outline>().enabled = true;
        }
    }

    public void MoveRight(int _iterator, List<Node> _listToIterate, int _state)
    {
        //print(_iterator);
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
        //print(_iterator);
        if (_listToIterate.Count >= 1 && (_iterator - 1) >= 0)
        {
            ChangeSelectedNode(_listToIterate[_iterator - 1], _state);
        }
        else if (_listToIterate.Count >= 1 && (_iterator - 1) < 0)
        {
            ChangeSelectedNode(_listToIterate[_listToIterate.Count - 1], _state);
        }
    }

    private void MoveTo(int _iterator, List<Node> _listToIterate, int _state){
        if(_iterator < _listToIterate.Count && _iterator >= 0){
                ChangeSelectedNode(_listToIterate[_iterator], _state);
        }
    }

    public void EnterSelectMode()
    {
        selectingNode = true;
        idle = false;
        state = 1;
        //selecField = Instantiate(selectionField, idleSelectedNode.GetComponentInParent<Transform>().localPosition, idleSelectedNode.GetComponentInParent<Transform>().localRotation);
        //selecField.transform.localScale = new Vector3(selectionFieldScale, selectionFieldScale, selectionFieldScale);

        selectingNodeSelectedNode = idleSelectedNode.Neighbors[0];

        if(withOutline)
        {
            idleSelectedNode.GetComponentInParent<Outline>().OutlineColor = selectingOutlineColor;
            selectingNodeSelectedNode.GetComponentInParent<Outline>().enabled = true;
        }
    }

    public void ExitSelectMode()
    {
        idle = true;
        selectingNode = false;
        state = 0;
        if(withOutline)
        {
            selectingNodeSelectedNode.GetComponentInParent<Outline>().enabled = false;
            idleSelectedNode.GetComponentInParent<Outline>().OutlineColor = Color.yellow;
        }
     
        //Destroy(selecField);
        //selecField = null;
        selectingNodeSelectedNode = null;
        selectingNodeSelectedNodeIndex = 0;
    }

    public void SelectNode()
    {
        // Al seleccionar ese nodo y este sea del diferente del equipo del jugador se ejecuta
        if (selectingNodeSelectedNode.teamInControl != teamId)
        {
            //Si no se esta conquistando se manda a llamar a la corrutina del nodo para conquistarlo, este proceso tarda 3s
            if (selectingNodeSelectedNode.CanConquisting)
            {
                
                /*Instantiate(holderTimer, selectingNodeSelectedNode.transform.position + new Vector3(0,1,0), Quaternion.identity);
                StartCoroutine(Esperar());*/
                StartCoroutine(selectingNodeSelectedNode.CaptureTimeDown(teamId, selectingNodeSelectedNode, this));
                selectingNodeSelectedNode.GetComponentInParent<Transform>().Find("FightFX").gameObject.SetActive(true);
                selectingNodeSelectedNode.CanConquisting = false;
            }

            //Resetear las referencias visuales
            if(withOutline)
            {
                if (selectingNodeSelectedNode != null)
                {
                    selectingNodeSelectedNode.GetComponent<Outline>().enabled = false;
                }
                idleSelectedNode.GetComponent<Outline>().OutlineColor = Color.yellow;
            }

            //Resetear el Selecting Node
            selectingNodeSelectedNode = null;
            selectingNodeSelectedNodeIndex = 0;

            // Cancelar la seleccion
            idle = true;
            selectingNode = false;
            state = 0;
        }
        // Al seleccionar un nodo que sea de mi equipo, me transportare a el
        else if (selectingNodeSelectedNode.teamInControl == teamId)
        {
            // Resetear las referencias visuales
            if(withOutline)
            {
                if (selectingNodeSelectedNode != null)
                {
                    selectingNodeSelectedNode.GetComponent<Outline>().enabled = false;
                }
                idleSelectedNode.GetComponent<Outline>().OutlineColor = Color.yellow;
            }

            // Moverme hacia el nodo seleccionado
            if(withOutline)
            {
                idleSelectedNode.GetComponentInParent<Outline>().enabled = false;
                idleSelectedNode = selectingNodeSelectedNode;
                idleSelectedNodeIndex = playerNodes.IndexOf(selectingNodeSelectedNode);
                idleSelectedNode.GetComponentInParent<Outline>().enabled = true;
            }
            else
            {
                idleSelectedNode = selectingNodeSelectedNode;
                idleSelectedNodeIndex = playerNodes.IndexOf(selectingNodeSelectedNode);
            }
            

            //Resetear el Selecting Node
            selectingNodeSelectedNode = null;
            selectingNodeSelectedNodeIndex = 0;

            // Cancelar la seleccion
            idle = true;
            selectingNode = false;
            state = 0;
        }
    }

    #endregion

    /*IEnumerator Esperar()
    {
        yield return new WaitForSecondsRealtime(3.0f);
        Destroy(holderTimer.gameObject);
    }*/
}
