using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    public GameObject selectionField;

    public Transform testPlanet;

    private bool selecting = false, clickOnce = false;
    private GameObject selecField;
    [SerializeField]
    private SpawnManager spawn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            if(!clickOnce)
            {
                selecting = true;
                clickOnce = true;
            }
            else
            {
                clickOnce = false;
                selecting = false;
            }
            
            if(selecting)
            {
                if(clickOnce)
                {
                    selecField = Instantiate(selectionField, this.transform.position, this.transform.rotation);
                    selecField.transform.localScale = new Vector3(6f, 6f, 6f);
                }

            }
            else
            {
                if(selecField != null)
                {
                    Destroy(selecField);
                }
            }
        }

        if(selecting && Input.GetKeyDown("1"))
        {
            spawn.planets.Add(this.transform);
            spawn.planets.Add(testPlanet);

            for(int i = 0; i < spawn.ships.Count; i++)
            {
                spawn.ships[i].GetComponent<ShipsBehavior>().puntos = spawn.planets;
            }

            clickOnce = false;
            selecting = false;

            if (selecField != null)
            {
                Destroy(selecField);
            }

            spawn.ships.Clear();
            //spawn.planets.Clear();
        }
    }
}
