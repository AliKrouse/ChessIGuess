using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    void Start ()
    {
        wPieces = GameObject.FindGameObjectsWithTag("White");
        bPieces = GameObject.FindGameObjectsWithTag("Black");
        button = transform.GetChild(4).gameObject;
    }
	
	void Update ()
    {
        if (isActive)
        {
            transform.position = Vector3.MoveTowards(transform.position, activePoint, Time.deltaTime * speed);
            if (!rolled)
                button.SetActive(true);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, inactivePoint, Time.deltaTime * speed);
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
        d.GetComponent<Rigidbody>().AddForce(dir * throwForce);
        StartCoroutine(CheckForDieSide());
    }

    private IEnumerator CheckForDieSide()
    {
        yield return new WaitForSeconds(1);
        int value = d.GetComponent<DieSides>().GetDieSideMatchInfo().closestMatch.values[0];
        Debug.Log(value);
        yield return new WaitForSeconds(2);
        isActive = false;
        yield return new WaitForSeconds(1);
        currentPiece.SetMovement();
        currentPiece.movementValue = value;
    }
}
