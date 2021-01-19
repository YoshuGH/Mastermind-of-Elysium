using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //  Teams Tag
    //  0: N/A Team
    //  1: Player Team

    // Variables Privadas
    [Header("Lista de jugadores")]
    [SerializeField] private List<Player> players;
    [SerializeField]private BattleGround battleGround;
    [SerializeField] private Material normalMat;

    // Variables publicas
    public bool spawnOnce = true;

    //Accesores
    public List<Player> Players { get { return players; } }
    
  


    private void Awake() {
        battleGround = GetComponentInParent<BattleGround>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RandomSpawnTeams()
    {
        if(spawnOnce)
        {
            for (int i = 0; i < players.Count; i++)
            {
                int randomPlanetIndex;

                //Esta libre
                do
                {
                    randomPlanetIndex = Random.Range(0, battleGround.NodeQty - 1);
                } while (battleGround.Nodes[randomPlanetIndex].teamInControl != 0);

                //Asignalo entonces
                battleGround.Nodes[randomPlanetIndex].teamInControl = players[i].TeamId;
                players[i].AddPlayerNode(battleGround.Nodes[randomPlanetIndex]);
                players[i].OutlineAtStart();
            }
            battleGround.MapExists = true;
            spawnOnce = false;
        }
    }

    public void RestartEverything()
    {
        //battleGround.RestartBG();

        foreach (Player player in players)
        {
            player.PlayerNodes.Clear();
        }

        foreach(Node node in battleGround.Nodes)
        {
            node.teamInControl = 0;

            if (node.GetComponent<Renderer>() != null)
            {
                Renderer rend = node.GetComponent<Renderer>();
                rend.material = normalMat;
            }
            else if (node.HaveResources)
            {
                Renderer rend = node.transform.GetChild(0).GetComponent<Renderer>();
                rend.material = normalMat;
            }

            node.GetComponent<Outline>().enabled = false;
        }

        Invoke("RandomSpawnTeams", 0.02f);
    }

    public void FightForNode(Node _firstNode, Node _secondNode)
    {
        if(_firstNode.OrbitShips.Count > _secondNode.OrbitShips.Count)
        {
            // Mandar las naves al nodo
            foreach(GameObject ship in _firstNode.OrbitShips)
            {
                // Solo se agregan los nodos y la nave se mueve sola
                ship.GetComponent<ShipsBehavior>().Puntos.Add(_firstNode.transform.Find("SpawnPoint"));
                ship.GetComponent<ShipsBehavior>().Puntos.Add(_secondNode.transform.Find("SpawnPoint"));
            }
        }
    }
}
