using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using cakeslice;

public class basePiece : MonoBehaviour
{
    public int die;
    public int strengthMod;
    private List<GameObject> allies = new List<GameObject>();

    public int direction;
    public Transform[] dirs;

    public GameObject arrows;

    public int movementValue;
    public float speed;

    public Transform targetTile;
    
    protected winController wc;
    protected clashController cc;

    public bool canBeClicked;

    private dieRoller dr;
    public string pieceColor;

    public bool isClashing;

    private AudioSource source;
    public AudioClip pieceClip;

    public bool jumping;

	void Start ()
    {
        allies.AddRange(GameObject.FindGameObjectsWithTag(this.tag));
        if (allies.Contains(this.gameObject))
            allies.Remove(this.gameObject);

        arrows = transform.GetChild(0).gameObject;

        dirs = new Transform[transform.GetChild(1).childCount];
        for (int i = 0; i < dirs.Length; i++)
        {
            dirs[i] = transform.GetChild(1).GetChild(i);
        }

        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        int closestIndex = 0;
        float closestDistance = 100;
        for (int i = 0; i < tiles.Length; i++)
        {
            if (Vector3.Distance(transform.position, tiles[i].transform.position) < closestDistance)
            {
                closestDistance = Vector3.Distance(transform.position, tiles[i].transform.position);
                closestIndex = i;
            }
        }
        targetTile = tiles[closestIndex].transform;
        
        wc = FindObjectOfType<winController>();
        cc = FindObjectOfType<clashController>();

        dr = FindObjectOfType<dieRoller>();

        source = GetComponent<AudioSource>();
    }

    public Transform CurrentTile()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        int closestIndex = 0;
        float closestDistance = 100;
        for (int i = 0; i < tiles.Length; i++)
        {
            if (Vector3.Distance(transform.position, tiles[i].transform.position) < closestDistance)
            {
                closestDistance = Vector3.Distance(transform.position, tiles[i].transform.position);
                closestIndex = i;
            }
        }
        return tiles[closestIndex].transform;
    }
	
	void Update ()
    {
        if (movementValue > 0 && !isClashing)
        {
            Vector3 moveTo = new Vector3(targetTile.position.x, 0.1241645f, targetTile.transform.position.z);
            Vector3 midPoint = transform.position + (moveTo - transform.position) / 2;
            Vector3 jumpPoint = new Vector3(midPoint.x, 0.44f, midPoint.z);

            if (jumping)
                transform.position = Vector3.MoveTowards(transform.position, jumpPoint, Time.deltaTime * speed);
            else
                transform.position = Vector3.MoveTowards(transform.position, moveTo, Time.deltaTime * speed);

            float jumpD = Vector3.Distance(transform.position, jumpPoint);
            if (jumpD < float.Epsilon)
                jumping = false;

            float d = Vector3.Distance(transform.position, moveTo);
            if (d < float.Epsilon)
            {
                source.PlayOneShot(pieceClip);
                SetMovement(movementValue);
            }
        }
	}

    private void OnMouseDown()
    {
        if (canBeClicked)
        {
            gameObject.AddComponent<Outline>();
            arrows.SetActive(true);

            foreach (GameObject g in allies)
            {
                if (g != null)
                {
                    if (g.GetComponent<Outline>() != null)
                        Destroy(g.GetComponent<Outline>());

                    g.GetComponent<basePiece>().arrows.SetActive(false);
                }
            }
        }
    }

    public virtual void SetMovement(int value)
    {
        movementValue = value;

        if (dirs[direction].GetComponent<checkAvailability>().available)
        {
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
            }
            else
            {
                targetTile = dirs[direction].GetComponent<checkAvailability>().NearestTile();
                movementValue--;
                jumping = true;

                if (movementValue <= 0)
                {
                    if (GetComponent<Outline>() != null)
                        Destroy(GetComponent<Outline>());
                    jumping = false;
                    if (!wc.hasCheckedForVictory)
                        wc.CheckForVictory();
                }
            }
        }
        else
        {
            movementValue = 0;

            if (GetComponent<Outline>() != null)
                Destroy(GetComponent<Outline>());
            jumping = false;
            if (!wc.hasCheckedForVictory)
                wc.CheckForVictory();
        }
    }
}
