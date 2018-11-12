using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class king : basePiece
{
    private GameObject check;

    protected override void Start()
    {
        base.Start();

        check = transform.GetChild(0).gameObject;
        check.SetActive(false);
    }

    public void ActivateCheck()
    {
        check.SetActive(true);
    }

    public void DeactivateCheck()
    {
        check.SetActive(false);
    }
}
