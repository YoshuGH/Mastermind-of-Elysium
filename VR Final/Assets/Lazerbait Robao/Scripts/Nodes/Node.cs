﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName= "NodeData", menuName = "Node/Nodes")]
public class Node :  MonoBehaviour
{
    private int typeResources;
    public int TypeResources { get { return typeResources; } }
    public int teamInControl = int.MaxValue;
    [SerializeField]private List<Node> neighbors;
    public List<Node> Neighbors { get { return neighbors; } }
    public void AddNeighbor(Node _node) => neighbors.Add(_node);
    public void DeleteNeighbor(int _index) => neighbors.RemoveAt(_index);
    

    

}