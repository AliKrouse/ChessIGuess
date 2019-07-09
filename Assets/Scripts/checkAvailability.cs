using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkAvailability : MonoBehaviour
{
    public string alliedTag, enemyTag;
    protected GameObject arrow;

    protected bool touchingAlly, touchingEdge;
    public bool available;
    public bool touchingEnemy;

    private GameObject[] tiles;

    public GameObject enemyPiece;
    
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
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Edge"))
            touchingEdge = true;
        if (other.CompareTag(alliedTag))
            touchingAlly = true;
        if (other.CompareTag(enemyTag))
        {
            touchingEnemy = true;
            enemyPiece = other.gameObject;
            //Debug.Log(name + " is touching " + other.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Edge"))
            touchingEdge = false;
        if (other.CompareTag(alliedTag))
            touchingAlly = false;
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
