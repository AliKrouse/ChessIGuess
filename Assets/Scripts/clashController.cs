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
    private winController wc;

    public int playerRoll, enemyRoll;

    public GameObject particles;

	void Start ()
    {
        dr = FindObjectOfType<dieRoller>();
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
            GameObject p = Instantiate(particles, enemyPiece.transform.position, Quaternion.identity);
            if (enemyPiece.pieceColor == "BLACK")
                p.GetComponent<ParticleSystem>().startColor = Color.black;
            if (enemyPiece.pieceColor == "WHITE")
                p.GetComponent<ParticleSystem>().startColor = Color.white;

            Destroy(enemyPiece.gameObject);
            playerPiece.targetTile = enemyPiece.CurrentTile();
            playerPiece.isClashing = false;
            playerPiece.movementValue = 1;
        }
        else
        {
            GameObject p = Instantiate(particles, playerPiece.transform.position, Quaternion.identity);
            if (playerPiece.pieceColor == "BLACK")
                p.GetComponent<ParticleSystem>().startColor = Color.black;
            if (playerPiece.pieceColor == "WHITE")
                p.GetComponent<ParticleSystem>().startColor = Color.white;

            Destroy(playerPiece.gameObject);
            enemyPiece.targetTile = playerPiece.CurrentTile();
            enemyPiece.movementValue = 1;
            enemyPiece.jumping = true;

            if (playerPiece.GetComponent<Outline>() != null)
                Destroy(GetComponent<Outline>());
            if (!wc.hasCheckedForVictory)
                wc.CheckForVictory();
        }
    }
}
