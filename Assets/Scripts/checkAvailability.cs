using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkAvailability : MonoBehaviour
{
    public string alliedTag, enemyTag;
    private GameObject arrow;

    private bool touchingAlly, touchingEdge;
    public bool available;

    private GameObject[] tiles;
    
	void Start ()
    {
        arrow = transform.parent.parent.GetChild(0).GetChild(transform.GetSiblingIndex()).gameObject;

        tiles = GameObject.FindGameObjectsWithTag("Tile");
	}
	
	void Update ()
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
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Edge"))
            touchingEdge = false;
        if (other.CompareTag(alliedTag))
            touchingAlly = false;
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
