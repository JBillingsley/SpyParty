using UnityEngine;
using System.Collections;

[RequireComponent (typeof (AudioSource)) ]

public class MovieController : MonoBehaviour {


	public MovieTexture movie;

	// Use this for initialization
	void Start () {

		movie.Play ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
