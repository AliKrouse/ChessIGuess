using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pawn : basePiece
{
    public GameObject queen;

    public float endRowYPos;

    private Vector2 moveForward, captureLeft, captureRight;

    bool hasBeenMoved;

    protected override void Start()
    {
        base.Start();

        moveForward = allowedDirections[0];
        captureRight = allowedDirections[1];
        captureLeft = allowedDirections[2];
    }

    protected override void Update ()
    {
        base.Update();

        if (Input.GetButton("Fire1") && !hasBeenFlung && isSelected)
        {
            SetAllowedDirections();
            hasBeenMoved = true;
        }
	}

    void SetAllowedDirections()
    {
        if (capturableOnRight() == null && capturableOnLeft() == null)
        {
            allowedDirections = new Vector2[1];
            allowedDirections[0] = moveForward;
        }
        if (capturableOnRight() != null && capturableOnLeft() == null)
        {
            allowedDirections = new Vector2[2];
            allowedDirections[0] = moveForward;
            allowedDirections[1] = captureRight; ;
        }
        if (capturableOnLeft() != null && capturableOnRight() == null)
        {
            allowedDirections = new Vector2[2];
            allowedDirections[0] = moveForward;
            allowedDirections[1] = captureLeft;
        }
        if (capturableOnRight() != null && capturableOnLeft() != null)
        {
            allowedDirections = new Vector2[3];
            allowedDirections[0] = moveForward;
            allowedDirections[1] = captureRight;
            allowedDirections[2] = captureLeft;
        }
    }

    GameObject capturableOnRight()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, captureRight, 1, checkForLayer);
        if (hit.collider != null)
        {
            Debug.DrawRay(transform.position, captureRight * 1, Color.blue);
            return hit.collider.gameObject;
        }
        else
        {
            Debug.DrawRay(transform.position, captureRight * 1, Color.red);
            return null;
        }
    }

    GameObject capturableOnLeft()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, captureLeft, 1, checkForLayer);
        if (hit.collider != null)
        {
            Debug.DrawRay(transform.position, captureLeft * 1, Color.blue);
            return hit.collider.gameObject;
        }
        else
        {
            Debug.DrawRay(transform.position, captureLeft * 1, Color.red);
            return null;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isSelected)
        {
            if (collision.gameObject == capturableOnRight() || collision.gameObject == capturableOnLeft())
                Destroy(collision.gameObject);
        }
    }

    public void BecomeQueen()
    {
        if (endRowYPos - transform.position.y < 0.04)
        {
            queen.transform.position = transform.position;
            queen.GetComponent<SpriteRenderer>().enabled = true;
            queen.GetComponent<CircleCollider2D>().enabled = true;
            queen.GetComponent<basePiece>().isActivated = true;
            Destroy(this.gameObject);
        }
        else if (hasBeenMoved)
            forceMultiplier = 5;
    }

    public override bool KingIsInCheck()
    {
        bool canSeeKing = false;
        if (capturableOnLeft() != null)
        {
            if (capturableOnLeft().name == "king")
                canSeeKing = true;
        }
        if (capturableOnRight() != null)
        {
            if (capturableOnRight().name == "king")
                canSeeKing = true;
        }
        return canSeeKing;
    }
}
