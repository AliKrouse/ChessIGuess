using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class directionArrows : MonoBehaviour
{
    public int direction;
    private basePiece bp;

    private dieRoller dr;
    
	void Start ()
    {
        bp = transform.parent.transform.parent.gameObject.GetComponent<basePiece>();

        dr = FindObjectOfType<dieRoller>();
	}

    private void OnMouseDown()
    {
        bp.direction = direction;
        dr.isActive = true;
        dr.die = bp.die;
        dr.currentPiece = bp;
        transform.parent.gameObject.SetActive(false);
    }
}
