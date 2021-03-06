﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turnController : MonoBehaviour
{
    public camScript boom;
    public GameObject wTurn, bTurn;
    public GameObject[] wPieces, bPieces;
    public bool whiteTurn;

    private dieRoller dr;
    private winController wc;

    public bool coroutineIsRunning;
    private bool movingPiece;

    private pawnPiece[] pawns;
    
	void Start ()
    {
        whiteTurn = true;
        wPieces = GameObject.FindGameObjectsWithTag("White");
        bPieces = GameObject.FindGameObjectsWithTag("Black");

        dr = FindObjectOfType<dieRoller>();
        wc = GetComponent<winController>();

        pawns = FindObjectsOfType<pawnPiece>();
    }
	
	void Update ()
    {
        if (dr.isActive)
            movingPiece = true;

        if (movingPiece)
        {
            foreach (GameObject g in bPieces)
                if (g != null)
                    g.GetComponent<basePiece>().canBeClicked = false;
            foreach (GameObject g in wPieces)
                if (g != null)
                    g.GetComponent<basePiece>().canBeClicked = false;
        }
        else
        {
            if (whiteTurn)
            {
                wPieces = GameObject.FindGameObjectsWithTag("White");

                foreach (GameObject g in bPieces)
                    if (g != null)
                        g.GetComponent<basePiece>().canBeClicked = false;
                foreach (GameObject g in wPieces)
                    if (g != null)
                        g.GetComponent<basePiece>().canBeClicked = true;

                wTurn.SetActive(true);
                bTurn.SetActive(false);

                boom.rot = 0;
            }
            if (!whiteTurn)
            {
                bPieces = GameObject.FindGameObjectsWithTag("Black");

                foreach (GameObject g in wPieces)
                    if (g != null)
                        g.GetComponent<basePiece>().canBeClicked = false;
                foreach (GameObject g in bPieces)
                    if (g != null)
                        g.GetComponent<basePiece>().canBeClicked = true;

                bTurn.SetActive(true);
                wTurn.SetActive(false);

                boom.rot = 180;
            }
        }
	}

    public IEnumerator SwitchTurns()
    {
        //Debug.Log("switching");
        coroutineIsRunning = true;
        yield return new WaitForSeconds(1);

        whiteTurn = !whiteTurn;
        movingPiece = false;

        coroutineIsRunning = false;
        wc.hasCheckedForVictory = false;

        foreach (pawnPiece p in pawns)
            if (p != null)
                p.CheckPosition();
    }
}
