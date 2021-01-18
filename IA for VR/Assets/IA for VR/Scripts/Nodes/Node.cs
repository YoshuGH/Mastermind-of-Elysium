using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName= "NodeData", menuName = "Node/Nodes")]
public class Node :  MonoBehaviour
{
    [SerializeField]private bool haveResources;
    public bool HaveResources { get { return haveResources; } }
    public int teamInControl;
    [SerializeField]private List<Node> neighbors;
    [SerializeField] private List<GameObject> orbitShips;
    private bool canConquisting = true;
    public List<GameObject> OrbitShips { get { return orbitShips; } }
    public List<Node> Neighbors { get { return neighbors; } }
    public bool CanConquisting { set { canConquisting = value; } get { return canConquisting; } }
    public void AddNeighbor(Node _node) => neighbors.Add(_node);
    public void DeleteNeighbor(int _index) => neighbors.RemoveAt(_index);

    public IEnumerator CaptureTimeDown( int _playerId, Node _nodeToCapture, Player _player)
    {
        yield return new WaitForSeconds(3.0f);

        //Conquista el nodo
        Node tempNode = _nodeToCapture;
        _player.AddPlayerNode(tempNode);
        
        //Resetear Variable de acceso
        canConquisting = true;
    }
}
