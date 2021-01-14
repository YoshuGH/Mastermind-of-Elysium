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
    public List<GameObject> OrbitShips { get { return orbitShips; } }
    public List<Node> Neighbors { get { return neighbors; } }
    public void AddNeighbor(Node _node) => neighbors.Add(_node);
    public void DeleteNeighbor(int _index) => neighbors.RemoveAt(_index);
}
