using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class knight : basePiece
{
    private Animator anim;
    private int nonJumpingLayer;

    protected override void Start()
    {
        base.Start();

        anim = GetComponent<Animator>();

        nonJumpingLayer = gameObject.layer;
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetButtonUp("Fire1") && !hasBeenFlung && isSelected)
        {
            anim.enabled = true;
            gameObject.layer = 10;
        }
    }

    public void DisableAnimation()
    {
        anim.enabled = false;
        gameObject.layer = nonJumpingLayer;
    }
}
