using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{
    [SerializeField]private List<Node> playerNodes;
    //private List<Ships> currentShips;
    public Material material;
    public bool isIA;
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

    
    
}
