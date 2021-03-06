﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkAvailability : MonoBehaviour
{
    public string alliedTag, enemyTag;
    protected GameObject arrow;

    public bool touchingAlly, touchingEdge;
    public bool available;
    public bool touchingEnemy;

    private GameObject[] tiles;

    public GameObject enemyPiece, allyPiece;
    
	void Start ()
    {
        arrow = transform.parent.parent.GetChild(0).GetChild(transform.GetSiblingIndex()).gameObject;

        tiles = GameObject.FindGameObjectsWithTag("Tile");
	}
	
	public virtual void Update ()
    {
        if (!touchingAlly && !touchingEdge)
            available = true;
        else
            available = false;

        arrow.SetActive(available);

        if (!allyPiece)
            touchingAlly = false;
        if (!enemyPiece)
            touchingEnemy = false;
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Edge"))
            touchingEdge = true;
        if (other.CompareTag(alliedTag))
        {
            touchingAlly = true;
            allyPiece = other.gameObject;
        }
        if (other.CompareTag(enemyTag))
        {
            touchingEnemy = true;
            enemyPiece = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Edge"))
            touchingEdge = false;
        if (other.CompareTag(alliedTag))
        {
            touchingAlly = false;
            allyPiece = null;
        }
        if (other.CompareTag(enemyTag))
        {
            touchingEnemy = false;
            enemyPiece = null;
        }
    }

    public Transform NearestTile()
    {
        int closestIndex = 0;
        float closestDistance = 100;

        for (int i = 0; i < tiles.Length; i++)
        {
            if (Vector3.Distance(transform.position, tiles[i].transform.position) < closestDistance)
            {
                closestDistance = Vector3.Distance(transform.position, tiles[i].transform.position);
                closestIndex = i;
            }
        }

        return tiles[closestIndex].transform;
    }
}
