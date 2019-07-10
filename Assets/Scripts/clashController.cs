using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class clashController : MonoBehaviour
{
    public basePiece playerPiece, enemyPiece;
    public Transform playerSpace, enemySpace;
    public float speed;

    private dieRoller dr;
    //private turnController tc;
    private winController wc;

    public int playerRoll, enemyRoll;

	void Start ()
    {
        dr = FindObjectOfType<dieRoller>();
        //tc = FindObjectOfType<turnController>();
        wc = FindObjectOfType<winController>();
    }

    public void EnterClash()
    {
        dr.isActive = true;
        dr.rollingPlayer = true;
        dr.rollingColor = playerPiece.pieceColor;
        dr.die = 2;
        dr.mod = playerPiece.strengthMod;
    }

    public void RollForEnemy()
    {
        dr.isActive = true;
        dr.rollingEnemy = true;
        dr.rollingColor = enemyPiece.pieceColor;
        dr.die = 2;
        dr.mod = enemyPiece.strengthMod;
    }

    public void Resolve()
    {
        if (playerRoll >= enemyRoll)
        {
            Destroy(enemyPiece.gameObject);
            playerPiece.targetTile = enemyPiece.CurrentTile();
            playerPiece.isClashing = false;
            playerPiece.movementValue = 1;
        }
        else
        {
            Destroy(playerPiece.gameObject);
            enemyPiece.targetTile = playerPiece.CurrentTile();
            enemyPiece.movementValue = 1;

            if (playerPiece.GetComponent<Outline>() != null)
                Destroy(GetComponent<Outline>());
            //if (!tc.coroutineIsRunning)
            //    tc.StartCoroutine(tc.SwitchTurns());
            if (!wc.hasCheckedForVictory)
                wc.CheckForVictory();
        }
    }
}
