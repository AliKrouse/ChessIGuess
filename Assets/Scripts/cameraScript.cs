using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour
{
    private Camera cam;
    public float zoomedOutSize, zoomSpeed;
    private float followSpeed;
    public float followOutSpeed, followInSpeed;
    private Vector3 followPoint;
    public float rotationSpeed;
    public float maxMagnitude;

    private GameObject[] whitePieces = new GameObject[24];
    private GameObject[] blackPieces = new GameObject[24];
    private GameObject[] allPieces = new GameObject[48];
    private GameObject[] newQueens;

    void Start ()
    {
        cam = GetComponent<Camera>();

        whitePieces = GameObject.FindGameObjectsWithTag("White");
        blackPieces = GameObject.FindGameObjectsWithTag("Black");
        for (int i = 0; i < 24; i++)
            allPieces[i] = whitePieces[i];
        for (int i = 24; i < 48; i++)
            allPieces[i] = blackPieces[i - 24];
    }
	
	void Update ()
    {
        if (Input.GetButton("Fire1"))
        {
            if (cam.orthographicSize < zoomedOutSize)
                cam.orthographicSize += zoomSpeed;

            Vector2 fp2 = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) / 2;
            followPoint = Vector3.ClampMagnitude(new Vector3(fp2.x, fp2.y, -10), maxMagnitude);
            followSpeed = followOutSpeed;
        }
        else
        {
            if (cam.orthographicSize > 3)
                cam.orthographicSize -= zoomSpeed;

            followPoint = Vector3.ClampMagnitude(new Vector3(0, 0, -10), maxMagnitude);
            followSpeed = followInSpeed;
        }

        transform.position = Vector3.MoveTowards(transform.position, followPoint, Time.deltaTime * followSpeed);

        if (turnController.isWhiteTurn)
        {
            transform.eulerAngles = Vector3.MoveTowards(transform.eulerAngles, Vector3.zero, Time.deltaTime * rotationSpeed);
            foreach (GameObject g in allPieces)
                if (g != null)
                    g.transform.eulerAngles = Vector3.MoveTowards(g.transform.eulerAngles, Vector3.zero, Time.deltaTime * rotationSpeed);
            if (transform.eulerAngles.z != 0)
                foreach (GameObject g in allPieces)
                    if (g != null)
                        g.GetComponent<basePiece>().SetRenderLayer();
        }
        if (!turnController.isWhiteTurn)
        {
            transform.eulerAngles = Vector3.MoveTowards(transform.eulerAngles, new Vector3(0, 0, 180), Time.deltaTime * rotationSpeed);
            foreach (GameObject g in allPieces)
                if (g != null && g.GetComponent<SpriteRenderer>().enabled)
                    g.transform.eulerAngles = Vector3.MoveTowards(g.transform.eulerAngles, new Vector3(0, 0, 180), Time.deltaTime * rotationSpeed);
            if (transform.eulerAngles.z != 180)
                foreach (GameObject g in allPieces)
                    if (g != null)
                        g.GetComponent<basePiece>().SetRenderLayer();
        }
    }
}
