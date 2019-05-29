using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class directionArrows : MonoBehaviour
{
    public int direction;
    private basePiece bp;
    
	void Start ()
    {
        bp = transform.parent.transform.parent.gameObject.GetComponent<basePiece>();
	}

    private void OnMouseDown()
    {
        bp.direction = direction;
    }
}
