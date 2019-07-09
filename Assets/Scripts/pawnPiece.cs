using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pawnPiece : basePiece
{
    public GameObject queen;

    public void CheckPosition()
    {
        if (Mathf.Abs(transform.position.z) > 2)
        {
            Instantiate(queen, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
