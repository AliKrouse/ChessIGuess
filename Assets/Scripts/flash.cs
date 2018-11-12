using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class flash : MonoBehaviour
{
    public float flashTime;
    private Text t;
    private Outline o;

	void OnEnable ()
    {
        t = GetComponent<Text>();
        o = GetComponent<Outline>();
        StartCoroutine(flashImage());
	}

    private IEnumerator flashImage()
    {
        Debug.Log("running");
        while (true)
        {
            t.enabled = true;
            o.enabled = true;
            yield return new WaitForSeconds(flashTime);
            t.enabled = false;
            o.enabled = false;
            yield return new WaitForSeconds(flashTime);
        }
    }
}
