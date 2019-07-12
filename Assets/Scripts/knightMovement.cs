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
        if (value == 3)
            extraJump = true;

        movementValue = value;

        if (dirs[direction].GetComponent<checkAvailability>().touchingEnemy && movementValue > 1)
        {
            // this code captures pieces like normal chess, which is boring

            //targetTile = dirs[direction].GetComponent<checkAvailability>().NearestTile();
            //Destroy(dirs[direction].GetComponent<checkAvailability>().enemyPiece.gameObject);
            //movementValue = 1;

            //if (GetComponent<Outline>() != null)
            //    Destroy(GetComponent<Outline>());
            //if (!tc.coroutineIsRunning)
            //    tc.StartCoroutine(tc.SwitchTurns());

            // THIS code captures pieces like dumb bad chess, which is fun as shit

            cc.playerPiece = this;
            cc.enemyPiece = dirs[direction].GetComponent<checkAvailability>().enemyPiece.GetComponent<basePiece>();
            isClashing = true;
            cc.EnterClash();
            source.PlayOneShot(clashClip);
        }
        else
        {
            if (value > 2 || !extraJump)
            {
                targetTile = dirs[direction].GetComponent<checkAvailability>().NearestTile();
                movementValue--;
                jumping = true;

                if (value > 2)
                    Debug.Log("making first of 2 jumps");
                if (value > 1 && !extraJump)
                    Debug.Log("making single jump");
            }

            if (value == 2 && extraJump)
            {
                Debug.Log("making second jump");

                List<Transform> tileOptions = new List<Transform>();
                for (int i = 0; i < dirs.Length; i++)
                {
                    if (dirs[i].GetComponent<checkAvailability>().available)
                    {
                        tileOptions.Add(dirs[i].GetComponent<checkAvailability>().NearestTile());
                    }
                }

                int choice = Random.Range(0, tileOptions.Count);
                targetTile = tileOptions[choice];
                Debug.Log("jumping to " + targetTile);

                movementValue--;
                jumping = true;
                extraJump = false;
            }

            if (movementValue <= 0)
            {
                if (GetComponent<Outline>() != null)
                    Destroy(GetComponent<Outline>());
                jumping = false;
                if (!wc.hasCheckedForVictory)
                    wc.CheckForVictory();

                extraJump = false;
            }
        }
    }
}
