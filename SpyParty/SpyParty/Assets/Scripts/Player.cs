using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Player : MonoBehaviour {
    public GameObject currentSquare;
    public delegate void PlayerTurn();
    public static event PlayerTurn turn;
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
        turn();
    }

    void updateLocation() {
        transform.position = new Vector3(currentSquare.transform.position.x, currentSquare.transform.position.y, currentSquare.transform.position.z - 1f);
    }

    void updateCurrentSquare() {
        currentSquare.GetComponent<ClickObject>().holdingPlayer();
    }
}
