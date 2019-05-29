using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dieButton : MonoBehaviour
{
    private dieRoller dr;
    private AudioSource source;
    public AudioClip shaking, throwing;
    
	void Start ()
    {
        dr = transform.parent.GetComponent<dieRoller>();
        source = GetComponent<AudioSource>();
	}

    private void OnMouseDown()
    {
        source.clip = shaking;
        source.loop = true;
        source.Play();
    }

    private void OnMouseUpAsButton()
    {
        source.loop = false;
        source.PlayOneShot(throwing);
        dr.RollDie();
    }
}
