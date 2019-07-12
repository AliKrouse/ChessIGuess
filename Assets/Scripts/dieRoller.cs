using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InnerDriveStudios.DiceCreator;

public class dieRoller : MonoBehaviour
{
    public Vector3 activePoint, inactivePoint;
    public Vector3 rtcActive, rtcInactive;
    public bool isActive;
    public float speed;
    public float textSpeed;
    public GameObject[] wPieces, bPieces;

    public int die;
    public GameObject[] dice;
    private GameObject d;
    
    public float throwForce;
    private bool rolled;

    private GameObject button;

    public basePiece currentPiece;

    public Text result;

    private turnController tc;

    public int mod;
    public bool rollingPlayer, rollingEnemy;
    public string rollingColor;
    private clashController cc;
    public Text rollToCapture;

    void Start ()
    {
        wPieces = GameObject.FindGameObjectsWithTag("White");
        bPieces = GameObject.FindGameObjectsWithTag("Black");
        button = transform.GetChild(4).gameObject;

        tc = GameObject.FindGameObjectWithTag("GameController").GetComponent<turnController>();
        cc = GameObject.FindGameObjectWithTag("GameController").GetComponent<clashController>();
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

        if (rollingPlayer || rollingEnemy)
        {
            if (isActive)
                rollToCapture.gameObject.GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(rollToCapture.gameObject.GetComponent<RectTransform>().localPosition, rtcActive, Time.deltaTime * textSpeed);
            else
                rollToCapture.gameObject.GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(rollToCapture.gameObject.GetComponent<RectTransform>().localPosition, rtcInactive, Time.deltaTime * textSpeed);

            rollToCapture.text = rollingColor + "\nRoll to capture!";

            if (rollingColor == "WHITE")
            {
                rollToCapture.color = Color.white;
                rollToCapture.gameObject.GetComponent<Outline>().effectColor = Color.black;
            }
            if (rollingColor == "BLACK")
            {
                rollToCapture.color = Color.black;
                rollToCapture.gameObject.GetComponent<Outline>().effectColor = Color.white;
            }
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
        value += mod;
        mod = 0;
        yield return new WaitForSeconds(1f);

        Destroy(d.gameObject);
        result.text = value.ToString();

        if (rollingPlayer || rollingEnemy)
        {
            if (rollingColor == "WHITE")
                result.color = Color.white;
            if (rollingColor == "BLACK")
                result.color = Color.black;
        }
        else
        {
            if (tc.whiteTurn)
                result.color = Color.white;
            else
                result.color = Color.black;
        }

        result.gameObject.SetActive(true);

        yield return new WaitForSeconds(1);
        result.gameObject.SetActive(false);
        isActive = false;
        yield return new WaitForSeconds(1);

        if (rollingPlayer)
        {
            cc.playerRoll = value;
            cc.RollForEnemy();
            rollingPlayer = false;
            rolled = false;
            yield break;
        }
        if (rollingEnemy)
        {
            cc.enemyRoll = value;
            cc.Resolve();
            rollingEnemy = false;
            rolled = false;
            yield break;
        }
        else
        {
            currentPiece.SetMovement(value + 1);
            rolled = false;
        }
    }
}
