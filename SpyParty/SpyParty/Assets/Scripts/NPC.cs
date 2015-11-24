using UnityEngine;
using System.Collections;
    
    public enum AIStates { WANDER, PERSUE, IDLE};

public class NPC : MonoBehaviour {
    public GameObject currentSquare;
    public GameObject board;
    public float YOffsetFromBoard;
    private GameObject goalPoint;
    private AIStates state;
    private float animationTime = 0.6f;

	// Use this for initialization
	void Start () {
        updateLocation();
        if(board == null) {
            board = GameObject.Find("Board");
        }
	    if(goalPoint == null) {
            state = AIStates.IDLE;
           // state = (AIStates)Random.Range(0, 3);
            Debug.Log(state);
        }
        Player.turn += StateMachine;
	}

    // States: Wander, Persue, Idle
    void StateMachine() {
        switch(state) {
            case AIStates.WANDER:
                // this means the ai needs a place to go
                if(goalPoint == null) {
                    // This line sucks but it chooses a random square on the board
                    goalPoint = board.GetComponentsInChildren<ClickObject>()[Random.Range(0, board.GetComponentsInChildren<ClickObject>().Length)].gameObject;
                } else if(currentSquare.Equals(goalPoint)) {
                    // you done! 
                    // choose a new state and return
                    selectRandomState();
                    return;
                }
                // then chose a square that advances the ai towards the target square
                currentSquare = findNextMove();
                updateLocation();
                break;
            case AIStates.PERSUE:
                // this is not in use for now
                selectRandomState();
                break;
            case AIStates.IDLE:
                state = AIStates.WANDER;
                goalPoint = null;
                break;
            default:
                break;
        }
    }

    private GameObject findNextMove() {
        if(goalPoint != null) {
            float distance = 1000000f;
           GameObject potentialSquare = null;
            // this loop finds a square in the direction of the goal point
            foreach(GameObject square in currentSquare.GetComponent<ClickObject>().neighborCubes) {
                if(Mathf.Abs(Vector3.Distance(square.transform.position, goalPoint.transform.position)) < distance) {
                    distance = Mathf.Abs(Vector3.Distance(square.transform.position, goalPoint.transform.position));
                    potentialSquare = square;
                }
            }
            return potentialSquare;
        }
        return null;
    }

    void updateLocation() {
        iTween.MoveTo(gameObject, iTween.Hash("position", new Vector3(currentSquare.transform.position.x, currentSquare.transform.position.y + YOffsetFromBoard, currentSquare.transform.position.z), 
                                              "time", animationTime, 
                                              "easetype", iTween.EaseType.easeInOutQuad));
        //transform.position = new Vector3(currentSquare.transform.position.x, currentSquare.transform.position.y, currentSquare.transform.position.z - 1f);
    }

    AIStates selectRandomState() {
        return state = (AIStates)Random.Range(0, 3);
    }
}
