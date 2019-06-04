using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InnerDriveStudios.DiceCreator;

public class dieRoller : MonoBehaviour
{
    public Vector3 activePoint, inactivePoint;
    public bool isActive;
    public float speed;
    public GameObject[] wPieces, bPieces;

    public int die;
    public GameObject[] dice;
    private GameObject d;
    
    public float throwForce;
    private bool rolled;

    private GameObject button;

    public basePiece currentPiece;

    public Text result, clash1, clash2;
    public GameObject vs;

    private turnController tc;

    void Start ()
    {
        wPieces = GameObject.FindGameObjectsWithTag("White");
        bPieces = GameObject.FindGameObjectsWithTag("Black");
        button = transform.GetChild(4).gameObject;

        tc = GameObject.FindGameObjectWithTag("GameController").GetComponent<turnController>();
    }
	
	void Update ()
    {
        if (isActive)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, activePoint, Time.deltaTime * speed);
            if (!rolled)
                button.SetActive(true);
        }
        else
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, inactivePoint, Time.deltaTime * speed);
            if (Vector3.Distance(transform.position, inactivePoint) < float.Epsilon)
                Destroy(d);

            rolled = false;
        }
	}

    public void RollDie()
    {
        rolled = true;
        button.SetActive(false);
        Vector3 dir = new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1));
        d = Instantiate(dice[die], button.transform.position, Random.rotation);
        d.transform.rotation = Random.rotation;
        d.GetComponent<Rigidbody>().AddForce(dir * throwForce);
        StartCoroutine(CheckForDieSide());
    }

    private IEnumerator CheckForDieSide()
    {
        yield return new WaitForSeconds(1.5f);
        int value = d.GetComponent<DieSides>().GetDieSideMatchInfo().closestMatch.values[0];
        yield return new WaitForSeconds(1f);

        Destroy(d.gameObject);
        result.text = value.ToString();
        if (tc.whiteTurn)
            result.color = Color.white;
        else
            result.color = Color.black;
        result.gameObject.SetActive(true);

        yield return new WaitForSeconds(1);
        result.gameObject.SetActive(false);
        isActive = false;
        yield return new WaitForSeconds(1);
        currentPiece.SetMovement(value + 1);
    }
}
