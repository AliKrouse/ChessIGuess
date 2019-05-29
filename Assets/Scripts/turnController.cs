﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turnController : MonoBehaviour
{
    public GameObject cameraBoom;
    public GameObject wTurn, bTurn;
    public GameObject[] wPieces, bPieces;
    public bool whiteTurn;
    
	void Start ()
    {
        whiteTurn = true;
        wPieces = GameObject.FindGameObjectsWithTag("White");
        bPieces = GameObject.FindGameObjectsWithTag("Black");
	}
	
	void Update ()
    {
        if (whiteTurn)
        {
            foreach (GameObject g in bPieces)
                g.GetComponent<Collider>().enabled = false;
            foreach (GameObject g in wPieces)
                g.GetComponent<Collider>().enabled = true;

            wTurn.SetActive(true);
            bTurn.SetActive(false);
        }
        if (!whiteTurn)
        {
            foreach (GameObject g in wPieces)
                g.GetComponent<Collider>().enabled = false;
            foreach (GameObject g in bPieces)
                g.GetComponent<Collider>().enabled = true;

            bTurn.SetActive(true);
            wTurn.SetActive(false);
        }
	}
}
