using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Player : MonoBehaviour {
    public GameObject currentSquare;
    private bool selected = false;

	// Use this for initialization
	void Start () {
        updateLocation();
        updateCurrentSquare();
	}

    public void moving(GameObject newSquare) {
        currentSquare.GetComponent<ClickObject>().clearSquares();
        currentSquare = newSquare;
        updateLocation();
        updateCurrentSquare();
    }

    void updateLocation() {
        transform.position = new Vector3(currentSquare.transform.position.x, currentSquare.transform.position.y, currentSquare.transform.position.z - 1f);
    }

    void updateCurrentSquare() {
        currentSquare.GetComponent<ClickObject>().holdingPlayer();
    }
	
	// Update is called once per frame
	void Update () {
	    
	}
}
