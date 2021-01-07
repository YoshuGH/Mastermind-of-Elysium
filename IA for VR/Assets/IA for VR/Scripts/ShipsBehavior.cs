using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipsBehavior : MonoBehaviour
{

    //Internos
    private int IndexActual = 0; //Index para mover en puntos
    private Vector3 PuntoA; //Punto A para Lerp
    private Vector3 PuntoB; //Punto B para Lerp
    private float t; //Factor tiempo de Lerp
    private float factorT; //Factor de moviento
    private bool oneTime = false, lastPoint = false;


    public float speed = 2f;
    public List<Transform> puntos;
    public SpawnManager spawn;

    // Start is called before the first frame update
    void Start()
    {
        puntos = new List<Transform>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(puntos.Count > 0)
        {
            if(!oneTime)
            {
                t = 1f; //Esto ayuda al primer calculo
                CalcularValores();
                oneTime = true;
            }
            Lerp();
        }
    }

    void Lerp()
    {
        t += factorT * Time.deltaTime;
        if (t >= 1f) //ya llegamos?
        {
            IndexActual++;
            if (IndexActual == puntos.Count - 1) //Ya es el ultimo tramo?
            {
                IndexActual = 0;
                lastPoint = true;
            }
            if(puntos.Count > 0)
            {
                CalcularValores();
            }
        }
        
        if(puntos.Count > 0)
        {
            transform.position = Vector3.Lerp(PuntoA, PuntoB, t);
            if(lastPoint)
            {
                puntos.Clear();
                lastPoint = false;
            }
        }
        
    }


    void CalcularValores()
    {
        PuntoA = puntos[IndexActual].position;
        PuntoB = puntos[IndexActual + 1].position;
        t = 1.0f - t; //Solo para continuar con el movimiento y no se vea brusco una pausa
                      //FactorT siempre sera diferente entre puntos, pero al final esto ayudara que el moviento siempre sea la misma.
        factorT = 1.0f / Vector3.Distance(PuntoA, PuntoB) * speed;
    }
}
