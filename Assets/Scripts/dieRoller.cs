using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dieRoller : MonoBehaviour
{
    public Vector3 activePoint, inactivePoint;
    public bool isActive;
    public float speed;
    public GameObject[] wPieces, bPieces;

    void Start ()
    {
        wPieces = GameObject.FindGameObjectsWithTag("White");
        bPieces = GameObject.FindGameObjectsWithTag("Black");
    }
	
	void Update ()
    {
        if (isActive)
        {
            transform.position = Vector3.MoveTowards(transform.position, activePoint, Time.deltaTime * speed);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, inactivePoint, Time.deltaTime * speed);
        }
	}
}
