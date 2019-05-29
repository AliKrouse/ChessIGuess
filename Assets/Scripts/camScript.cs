using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camScript : MonoBehaviour
{
    public float rot;
    private float currentRot;
    public float speed;
    
	void Start ()
    {
		
	}
	
	void Update ()
    {
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, rot, 0), speed * Time.deltaTime);
    }
}
