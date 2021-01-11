using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Variables Publicas
    [Header("Prefabs")]
    public GameObject selectionField;

    //Variables Privadas
    [Header("Managers References")]
    [SerializeField] private GameManager gm;
    [SerializeField] private SpawnManager spawn;
    private GameObject selecField;
    private int idleSelectedNodeIndex = 0;
    private Node idleSelectedNode;
    private bool selectingNode = false, clickOnce = false, idle = true;

    // Start is called before the first frame update
    void Start()
    {
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
                    ChangeSelectedNode(gm.PlayerNodes[idleSelectedNodeIndex + 1]);
                }
                else if(gm.PlayerNodes.Count >= 1 && (idleSelectedNodeIndex + 1) >= gm.PlayerNodes.Count)
                {
                    ChangeSelectedNode(gm.PlayerNodes[0]);
                }
            }

            // Al presionar la flecha itera hacia la izquierda (es decir resta 1 al iterador),
            // sobre la lista de nodos que el player posee
            if (Input.GetKeyDown("left"))
            {
                print(idleSelectedNodeIndex);
                if (gm.PlayerNodes.Count >= 1 && (idleSelectedNodeIndex - 1) >= 0)
                {
                    ChangeSelectedNode(gm.PlayerNodes[idleSelectedNodeIndex - 1]);
                }
                else if (gm.PlayerNodes.Count >= 1 && (idleSelectedNodeIndex - 1) < 0)
                {
                    ChangeSelectedNode(gm.PlayerNodes[gm.PlayerNodes.Count -1]);
                }
            }

            // Al presionar espacio entra en modo de seleccion de nodos
            if (Input.GetKeyDown("space"))
            {
                selectingNode = true;
                //idle = false;
                
                selecField = Instantiate(selectionField, idleSelectedNode.GetComponentInParent<Transform>().localPosition, idleSelectedNode.GetComponentInParent<Transform>().localRotation);
                selecField.transform.localScale = new Vector3(6f, 6f, 6f);
                print("presione espacio");
            }
        }
        #endregion

        // Si esta seleccionando un nodo
        #region Selecting Node State
        if (selectingNode)
        {
            //Si vuelve a presionar espacio estando en seleccion, este cancela la seleccion
            if(Input.GetKeyDown("space"))
            {
                idle = true;
                selectingNode = false;
                Destroy(selecField);
            }
        }
        #endregion

        /*
        if (selectingNode && Input.GetKeyDown("1"))
        {
            spawn.planets.Add(this.transform);
            spawn.planets.Add(testPlanet);

            for (int i = 0; i < spawn.ships.Count; i++)
            {
                spawn.ships[i].GetComponent<ShipsBehavior>().puntos = spawn.planets;
            }

            clickOnce = false;
            selectingNode = false;

            if (selecField != null)
            {
                Destroy(selecField);
            }

            spawn.ships.Clear();
            //spawn.planets.Clear();
        }*/
    }

    // Funcion que activa el Outline.cs del el primer y unico nodo al momento de iniciar el juego
    void OutlineAtStart()
    {
        idleSelectedNode = gm.PlayerNodes[idleSelectedNodeIndex];
        idleSelectedNode.GetComponentInParent<Outline>().enabled = true;
    }

    // Funcion para cambiar de nodo
    void ChangeSelectedNode( Node _nextNode)
    {
        idleSelectedNode.GetComponentInParent<Outline>().enabled = false;
        idleSelectedNode = _nextNode;
        idleSelectedNodeIndex = gm.PlayerNodes.IndexOf(_nextNode);
        idleSelectedNode.GetComponentInParent<Outline>().enabled = true;
    }
}
