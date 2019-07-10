using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class winController : MonoBehaviour
{
    public GameObject whiteKing, blackKing;
    public int whitePieces, blackPieces;

    public Image winImage;
    public Sprite whiteWin, blackWin;

    private turnController tc;

    public bool hasCheckedForVictory;
    
	void Start ()
    {
        whitePieces = 16;
        blackPieces = 16;

        tc = GetComponent<turnController>();
	}

    public void CheckForVictory()
    {
        hasCheckedForVictory = true;

        if (blackPieces <= 0 || blackKing == null)
        {
            winImage.sprite = whiteWin;
            EndGame();
        }
        else if (whitePieces <= 0 || whiteKing == null)
        {
            winImage.sprite = blackWin;
            EndGame();
        }
        else
        {
            if (!tc.coroutineIsRunning)
                tc.StartCoroutine(tc.SwitchTurns());
        }
    }

    void EndGame()
    {
        gameObject.GetComponent<turnController>().enabled = false;
        gameObject.GetComponent<clashController>().enabled = false;

        winImage.gameObject.SetActive(true);
    }
}
