using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basePiece : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;

    protected Vector2 direction;
    public Vector2[] allowedDirections;
    protected Vector2 finalDirection;
    public float forceMultiplier;
    protected float force;

    private GameObject[] tiles = new GameObject[64];

    public bool isSelected;
    protected bool hasBeenFlung;
    public float returnSpeed;

    private turnController turn;
    public bool resetting;

    public arrow a;
    public float arrowSizeMultiplier;

    public LayerMask checkForLayer;
    protected List<GameObject> capturablePieces = new List<GameObject>();
    public float raycastDistance;
    
    public float maxMagnitude;

    public bool isActivated;
    
	protected virtual void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        tiles = GameObject.FindGameObjectsWithTag("Tile");

        turn = GameObject.FindGameObjectWithTag("GameController").GetComponent<turnController>();

        SetRenderLayer();

        a = GameObject.FindGameObjectWithTag("Arrow").GetComponent<arrow>();
	}

    protected virtual void Update ()
    {
        if (isActivated)
        {
            if (isSelected)
                DetectCapturablePieces();

            if (Input.GetButton("Fire1") && !hasBeenFlung && isSelected)
            {
                Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                Vector2 clampedMousePoint = Vector2.ClampMagnitude(mousePoint, maxMagnitude);
                direction = mousePoint.normalized * -1;
                ConstrainDirection();

                Debug.DrawRay(transform.position, mousePoint * 3, Color.red);
                Debug.DrawRay(transform.position, finalDirection * 3, Color.green);

                force = clampedMousePoint.magnitude * forceMultiplier;

                sr.sortingOrder = 101;
                a.sr.enabled = true;
                a.SetTransform(transform.position, finalDirection, force, arrowSizeMultiplier);

                Cursor.lockState = CursorLockMode.Confined;
            }
            if (Input.GetButtonUp("Fire1") && !hasBeenFlung && isSelected)
            {
                rb.AddForce(finalDirection * force);
                a.sr.enabled = false;

                Cursor.lockState = CursorLockMode.None;
            }


            if (rb.velocity != Vector2.zero)
            {
                if (isSelected)
                    hasBeenFlung = true;
                SetRenderLayer();
            }
            if (hasBeenFlung)
            {
                if (rb.velocity == Vector2.zero)
                {
                    turn.turnIsOver = true;
                    hasBeenFlung = false;
                    isSelected = false;
                    capturablePieces.Clear();
                }
            }

            if (resetting)
            {
                transform.position = Vector2.MoveTowards(transform.position, NearestTile().transform.position, Time.deltaTime * returnSpeed);
            }
        }
    }

    void ConstrainDirection()
    {
        float lastDistance = 10;
        int currentLowest = 0;
        for (int i = 0; i < allowedDirections.Length; i++)
        {
            float distance = Vector2.Distance(allowedDirections[i], direction);
            if (distance < lastDistance)
            {
                currentLowest = i;
                lastDistance = distance;
            }
        }
        finalDirection = allowedDirections[currentLowest];
    }

    GameObject NearestTile()
    {
        float lastDistance = 100;
        int currentLowest = 0;
        for (int i = 0; i < tiles.Length; i++)
        {
            tileOccupation t = tiles[i].GetComponent<tileOccupation>();
            if (!t.isOccupied || t.NearestPiece() == this.gameObject)
            {
                float distance = Vector2.Distance(transform.position, tiles[i].transform.position);
                if (distance < lastDistance)
                {
                    currentLowest = i;
                    lastDistance = distance;
                }
            }
        }
        return tiles[currentLowest];
    }

    public bool IsOnNearestTile()
    {
        float d = Vector2.Distance(transform.position, NearestTile().transform.position);
        if (d < float.Epsilon)
            return true;
        else
            return false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isSelected)
        {
            if (capturablePieces.Contains(collision.gameObject))
            {
                Destroy(collision.gameObject);
            }
        }
    }

    public void SetRenderLayer()
    {
        if (turnController.isWhiteTurn)
            sr.sortingOrder = Mathf.RoundToInt(transform.position.y * 10) * -1;
        if (!turnController.isWhiteTurn)
            sr.sortingOrder = Mathf.RoundToInt(transform.position.y * 10);
    }

    protected virtual void DetectCapturablePieces()
    {
        for (int i = 0; i < allowedDirections.Length; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, allowedDirections[i], 1, checkForLayer);
            if (hit.collider != null)
            {
                Debug.DrawRay(transform.position, allowedDirections[i] * 1, Color.blue);
                capturablePieces.Add(hit.collider.gameObject);
                if (hit.collider.gameObject.name == "king")
                    hit.collider.GetComponent<king>().ActivateCheck();
            }
            else
            {
                Debug.DrawRay(transform.position, allowedDirections[i] * 1, Color.red);
            }
        }
    }

    public virtual bool KingIsInCheck()
    {
        bool canSeeKing = false;
        foreach (Vector2 v in allowedDirections)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, v, raycastDistance, checkForLayer);
            if (hit.collider != null && hit.collider.gameObject.name == "king")
            {
                Debug.DrawRay(transform.position, v * 1, Color.blue);
                canSeeKing = true;
            }
            else
            {
                Debug.DrawRay(transform.position, v * 1, Color.red);
            }
        }
        return canSeeKing;
    }
}
