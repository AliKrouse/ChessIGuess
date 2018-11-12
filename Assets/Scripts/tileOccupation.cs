using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tileOccupation : MonoBehaviour
{
    public bool isOccupied;
    public float MaxDistance;

    private GameObject[] whitePieces = new GameObject[24];
    private GameObject[] blackPieces = new GameObject[24];
    private GameObject[] allPieces = new GameObject[48];

    private void Start()
    {
        whitePieces = GameObject.FindGameObjectsWithTag("White");
        blackPieces = GameObject.FindGameObjectsWithTag("Black");
        for (int i = 0; i < 24; i++)
            allPieces[i] = whitePieces[i];
        for (int i = 24; i < 48; i++)
            allPieces[i] = blackPieces[i - 24];
    }

    private void Update()
    {
        if (NearestPiece() != null)
            Debug.DrawLine(transform.position, NearestPiece().transform.position, Color.yellow);
    }

    public GameObject NearestPiece()
    {
        float lastDistance = 100;
        int currentLowest = 0;
        for (int i = 0; i < allPieces.Length; i++)
        {
            if (allPieces[i] != null && allPieces[i].GetComponent<basePiece>().isActivated)
            {
                float distance = Vector2.Distance(transform.position, allPieces[i].transform.position);
                if (distance < lastDistance)
                {
                    currentLowest = i;
                    lastDistance = distance;
                }
            }
        }
        if (Vector2.Distance(allPieces[currentLowest].transform.position, transform.position) < MaxDistance)
        {
            isOccupied = true;
            return allPieces[currentLowest];
        }
        else
        {
            isOccupied = false;
            return null;
        }
    }
}
