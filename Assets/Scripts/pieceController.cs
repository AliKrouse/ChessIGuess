using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pieceController : MonoBehaviour
{
    private GameObject[] pieces = new GameObject[16];
    public string lookForTag;
    private basePiece piece;
    private turnController turn;
    
	void Start ()
    {
        pieces = GameObject.FindGameObjectsWithTag(lookForTag);
        turn = FindObjectOfType<turnController>();
	}
	
	void Update ()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            piece = NearestPiece().GetComponent<basePiece>();
            piece.isSelected = true;
            turn.lastUsedPiece = piece.gameObject.name;
        }
	}

    private GameObject NearestPiece()
    {
        pieces = GameObject.FindGameObjectsWithTag(lookForTag);
        float lastDistance = 100;
        int currentLowest = 0;
        for (int i = 0; i < pieces.Length; i++)
        {
            if (pieces[i] != null && pieces[i].GetComponent<SpriteRenderer>().enabled)
            {
                float distance = Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), pieces[i].transform.position);
                if (distance < lastDistance)
                {
                    currentLowest = i;
                    lastDistance = distance;
                }
            }
        }
        return pieces[currentLowest];
    }
}
