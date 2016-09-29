using UnityEngine;
using System.Collections;

public class CrazyPhysics : MonoBehaviour {

    private Rigidbody rb;
    private float speed;
    private float timer;

	// Use this for initialization
	void Start () {
        speed = 100;
        timer = 0;
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (timer <= 0)
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<NavCharScript>().assignRandomValues();
            timer = 4.0f;
        }
        else
        {
            timer -= Time.deltaTime;
        }
	}
}
