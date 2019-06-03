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

    private Transform targetTile;

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
	}
	
	void Update ()
    {
        if (movementValue > 0)
        {
            float d = Vector2.Distance(transform.position, targetTile.position);
            if (d < float.Epsilon)
                SetMovement();

            transform.position = Vector2.MoveTowards(transform.position, targetTile.position, Time.deltaTime * speed);
        }
	}

    private void OnMouseDown()
    {
        gameObject.AddComponent<Outline>();
        arrows.SetActive(true);

        foreach (GameObject g in allies)
        {
            if (g.GetComponent<Outline>() != null)
                Destroy(g.GetComponent<Outline>());

            g.GetComponent<basePiece>().arrows.SetActive(false);
        }
    }

    private void SetMovement()
    {
        if (dirs[direction].GetComponent<checkAvailability>().available)
        {
            targetTile = dirs[direction].GetComponent<checkAvailability>().NearestTile();
            movementValue--;
        }
        else
            movementValue = 0;
    }
}
