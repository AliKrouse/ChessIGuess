using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class turnController : MonoBehaviour
{
    public pieceController whiteController, blackController;
    private GameObject[] whitePieces = new GameObject[24];
    private GameObject[] blackPieces = new GameObject[24];
    private pawn[] allPawns = new pawn[48];
    
    public float numberOfPiecesReset;

    public bool turnIsOver;

    public GameObject bturn, wturn;

    public king bKing, wKing;
    public Text victoryText;

    public string currentScene, menuScene;

    public string lastUsedPiece;

    public GameObject smash;
    public GameObject resetButton;

    public static bool isWhiteTurn = true;
    
	void Start ()
    {
        whitePieces = GameObject.FindGameObjectsWithTag("White");
        blackPieces = GameObject.FindGameObjectsWithTag("Black");
        allPawns = FindObjectsOfType<pawn>();

        whiteController.enabled = true;
        blackController.enabled = false;
        wturn.SetActive(true);
        bturn.SetActive(false);
	}

    void Update()
    {
        if (turnIsOver)
        {
            ResetPieces();
        }
    }

    public void ResetPieces()
    {
        numberOfPiecesReset = 0;

        for (int i = 0; i < whitePieces.Length; i++)
        {
            if (whitePieces[i] != null && whitePieces[i].GetComponent<basePiece>().isActivated)
            {
                basePiece bp = whitePieces[i].GetComponent<basePiece>();
                if (!bp.IsOnNearestTile())
                {
                    bp.resetting = true;
                    bp.SetRenderLayer();
                }
                else
                {
                    bp.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    bp.resetting = false;
                    numberOfPiecesReset++;
                    if (whitePieces[i].GetComponent<pawn>() != null)
                        whitePieces[i].GetComponent<pawn>().BecomeQueen();
                }
            }
            else
            {
                numberOfPiecesReset++;
            }
        }
        for (int i = 0; i < blackPieces.Length; i++)
        {
            if (blackPieces[i] != null && blackPieces[i].GetComponent<basePiece>().isActivated)
            {
                basePiece bp = blackPieces[i].GetComponent<basePiece>();
                if (!bp.IsOnNearestTile())
                {
                    bp.resetting = true;
                    bp.SetRenderLayer();
                }
                else
                {
                    bp.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    bp.resetting = false;
                    numberOfPiecesReset++;
                    if (blackPieces[i].GetComponent<pawn>() != null)
                        blackPieces[i].GetComponent<pawn>().BecomeQueen();
                }
            }
            else
            {
                numberOfPiecesReset++;
            }
        }

        if (numberOfPiecesReset == 48)
            EndTurn();
    }

    public void EndTurn()
    {
        if (bKing == null || wKing == null)
        {
            victoryText.gameObject.SetActive(true);

            if (bKing == null)
            {
                if (lastUsedPiece == "queen")
                    smash.SetActive(true);
                victoryText.text = "WHITE PLAYER WINS!";
                victoryText.color = Color.white;
                victoryText.gameObject.GetComponent<Outline>().effectColor = Color.black;
            }
            if (wKing == null)
            {
                if (lastUsedPiece == "queen")
                    smash.SetActive(true);
                victoryText.text = "BLACK PLAYER WINS!";
                victoryText.color = Color.black;
                victoryText.gameObject.GetComponent<Outline>().effectColor = Color.white;
            }

            resetButton.SetActive(true);
        }
        else
        {
            whiteController.enabled = !whiteController.enabled;
            blackController.enabled = !blackController.enabled;
            wturn.SetActive(!wturn.activeSelf);
            bturn.SetActive(!bturn.activeSelf);

            isWhiteTurn = !isWhiteTurn;

            bool whiteCheck = false;
            bool blackCheck = false;

            foreach (GameObject g in whitePieces)
            {
                if (g != null)
                {
                    g.GetComponent<basePiece>().isSelected = false;
                    if (g.name != "king")
                    {
                        if (g.GetComponent<basePiece>().KingIsInCheck())
                            blackCheck = true;
                    }
                }
            }
            foreach (GameObject g in blackPieces)
            {
                if (g != null)
                {
                    g.GetComponent<basePiece>().isSelected = false;
                    if (g.name != "king")
                    {
                        if (g.GetComponent<basePiece>().KingIsInCheck())
                            whiteCheck = true;
                    }
                }
            }

            if (blackCheck)
                bKing.ActivateCheck();
            else
                bKing.DeactivateCheck();

            if (whiteCheck)
                wKing.ActivateCheck();
            else
                wKing.DeactivateCheck();

            turnIsOver = false;
        }
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
