using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPawnMovement : checkAvailability
{
    public override void Update()
    {
        if (!touchingAlly && !touchingEdge && !touchingEnemy)
            available = true;
        else
            available = false;

        arrow.SetActive(available);
    }
}
