using UnityEngine;
using System.Collections;

public class RotateWithMouse : MonoBehaviour {
	
	public float sensitivityX = 15.0f;
	public float sensitivityY = 15.0f;
	private Transform cameraTm;
	
	public bool down = false;
	
	// Use this for initialization
	void Start ()
	{
		cameraTm = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () {
		if( Input.GetMouseButtonDown( 0 ) )
			down = true;
		else if( Input.GetMouseButtonUp( 0 ) )
			down = false;
		
		if( down )
		{
			float rotationX = Input.GetAxis("Mouse X") * sensitivityX;
			float rotationY = Input.GetAxis("Mouse Y") * sensitivityY;
			transform.RotateAroundLocal( cameraTm.up, -Mathf.Deg2Rad * rotationX );
			transform.RotateAroundLocal( cameraTm.right, Mathf.Deg2Rad * rotationY );
		}
	}
}