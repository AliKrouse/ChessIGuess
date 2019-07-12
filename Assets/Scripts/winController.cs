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
    public GameObject reset;

    private turnController tc;

    public bool hasCheckedForVictory;

    private AudioSource source;
    public AudioClip victoryClip;
    
	void Start ()
    {
        whitePieces = 16;
        blackPieces = 16;

        tc = GetComponent<turnController>();

        source = GetComponent<AudioSource>();
	}

    public void CheckForVictory()
    {
        Debug.Log("checking victory");

        hasCheckedForVictory = true;

        if (blackPieces <= 0 || blackKing == null)
        {
            winImage.sprite = whiteWin;
            StartCoroutine(EndGame());

            Debug.Log("white player wins");
        }
        else if (whitePieces <= 0 || whiteKing == null)
        {
            winImage.sprite = blackWin;
            StartCoroutine(EndGame());

            Debug.Log("black player wins");
        }
        else
        {
            if (!tc.coroutineIsRunning)
                tc.StartCoroutine(tc.SwitchTurns());

            Debug.Log("game continues");
        }
    }

    private IEnumerator EndGame()
    {
        gameObject.GetComponent<turnController>().enabled = false;
        gameObject.GetComponent<clashController>().enabled = false;

        winImage.gameObject.SetActive(true);
        source.PlayOneShot(victoryClip);
        yield return new WaitForSeconds(3);
        reset.SetActive(true);
    }
}
