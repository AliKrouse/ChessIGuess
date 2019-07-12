using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPawnCapture : checkAvailability
{	
	public override void Update ()
    {
        if (touchingEnemy)
            available = true;
        else
            available = false;

        arrow.SetActive(available);

        if (!allyPiece)
            touchingAlly = false;
        if (!enemyPiece)
            touchingEnemy = false;
    }
}
