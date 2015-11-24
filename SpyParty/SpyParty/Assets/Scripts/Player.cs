using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Player : MonoBehaviour {
    public GameObject startSquare;
    public GameObject currentSquare;
    public float YOffsetFromBoard;
    public delegate void PlayerTurn();
    public static event PlayerTurn turn;

    private static Player _instance;
    public static Player instance {
        get {
            if(_instance == null) {
                _instance = GameObject.Find("Player").GetComponent<Player>();
            }
            return _instance;
        }
    }
   // private bool selected = false;
    private float animationTime = 0.6f;

	// Use this for initialization
	void Start () {
        updateLocation();
        updateCurrentSquare();
	}

    public void moving(GameObject newSquare) {
        currentSquare.GetComponent<ClickObject>().clearSquares();
        currentSquare = newSquare;
        updateLocation();
       // updateCurrentSquare();
       // turn();
    }

    void updateLocation() {
        iTween.MoveTo(gameObject, iTween.Hash("position",new Vector3(currentSquare.transform.position.x, currentSquare.transform.position.y + YOffsetFromBoard, currentSquare.transform.position.z),
                                              "time", animationTime,
                                              "easetype",iTween.EaseType.easeInOutQuad,
                                              "oncomplete", "updateCurrentSquare"));
    }

    void updateCurrentSquare() {
        currentSquare.GetComponent<ClickObject>().holdingPlayer();
        finished();
    }

    // this is just a container for the turn function, it will call if the NPC has been initialized so the NPC can have a turn
    public void finished() {
        if(turn != null) {
            turn();
            currentSquare.GetComponent<ClickObject>().holdingPlayer();
        }
    }

    public void caught() {
        currentSquare.GetComponent<ClickObject>().clearSquares();
        currentSquare = startSquare;
        iTween.MoveTo(gameObject, iTween.Hash("position", new Vector3(currentSquare.transform.position.x, currentSquare.transform.position.y + YOffsetFromBoard, currentSquare.transform.position.z),
                                              "time", animationTime,
                                              "easetype", iTween.EaseType.easeInOutQuad));
        currentSquare.GetComponent<ClickObject>().holdingPlayer();
    }
}
