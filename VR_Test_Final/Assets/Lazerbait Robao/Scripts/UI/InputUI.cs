using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputUI : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject uiIdlePNL;
    [SerializeField] private GameObject uiSelectingPNL;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveRightIdle()
    {
        player.MoveRight(player.Idle_SelectedNodeIndex, player.PlayerNodes, 0);
    }

    public void MoveLeftIdle()
    {
        player.MoveLeft(player.Idle_SelectedNodeIndex, player.PlayerNodes, 0);
    }

    public void EnterSelectMode()
    {
        player.EnterSelectMode();
        uiIdlePNL.SetActive(false);
        uiSelectingPNL.SetActive(true);
    }

    public void MoveRightSelecting()
    {
        player.MoveRight(player.Selecting_SelectedNodeIndex, player.Idle_SelectedNode.Neighbors, 1);
    }

    public void MoveLeftSelecting()
    {
        player.MoveLeft(player.Selecting_SelectedNodeIndex, player.Idle_SelectedNode.Neighbors, 1);
    }

    public void ExitSelectNode()
    {
        player.ExitSelectMode();
        uiIdlePNL.SetActive(true);
        uiSelectingPNL.SetActive(false);
    }

    public void SelectNode()
    {
        player.SelectNode();
        uiIdlePNL.SetActive(true);
        uiSelectingPNL.SetActive(false);
    }
}
