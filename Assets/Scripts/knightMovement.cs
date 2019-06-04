using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using cakeslice;

public class knightMovement : basePiece
{
    public bool extraJump;

    public override void SetMovement(int value)
    {
        //base.SetMovement(value);

        if (value == 3)
            extraJump = true;

        movementValue = value;

        if (dirs[direction].GetComponent<checkAvailability>().available)
        {
            if (value > 2 || !extraJump)
            {
                targetTile = dirs[direction].GetComponent<checkAvailability>().NearestTile();
                movementValue--;
            }

            if (value == 2 && extraJump)
            {
                Debug.Log("jump 2");

                List<Transform> tileOptions = new List<Transform>();
                for (int i = 0; i < dirs.Length; i++)
                {
                    if (dirs[i].GetComponent<checkAvailability>().available)
                    {
                        tileOptions.Add(dirs[i].GetComponent<checkAvailability>().NearestTile());
                        Debug.DrawLine(transform.position, dirs[i].GetComponent<checkAvailability>().NearestTile().position, Color.red, 5);
                    }
                }

                int choice = Random.Range(0, tileOptions.Count);
                targetTile = tileOptions[choice];
                Debug.Log(targetTile);

                movementValue--;
            }

            if (movementValue <= 0 || (movementValue == 1 && extraJump))
            {
                if (GetComponent<Outline>() != null)
                    Destroy(GetComponent<Outline>());
                if (!tc.coroutineIsRunning)
                    tc.StartCoroutine(tc.SwitchTurns());

                extraJump = false;
            }
        }
        else
        {
            movementValue = 0;

            if (GetComponent<Outline>() != null)
                Destroy(GetComponent<Outline>());
            if (!tc.coroutineIsRunning)
                tc.StartCoroutine(tc.SwitchTurns());

            extraJump = false;
        }
    }
}
