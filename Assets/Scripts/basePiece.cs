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
    public Vector3[] directions;

    public GameObject arrows;

    public int movementValue;

	void Start ()
    {
        allies.AddRange(GameObject.FindGameObjectsWithTag(this.tag));
        if (allies.Contains(this.gameObject))
            allies.Remove(this.gameObject);

        arrows = transform.GetChild(0).gameObject;
	}
	
	void Update ()
    {
		
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
}
