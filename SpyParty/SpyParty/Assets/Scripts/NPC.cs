using UnityEngine;
using System.Collections;
    
    public enum AIStates { WANDER, PERSUE, IDLE};

public class NPC : MonoBehaviour {
    public GameObject currentSquare;
    public GameObject board;
    public float YOffsetFromBoard;
    public GameObject textPanel;
    private GameObject goalPoint;
    private AIStates state;
    private float animationTime = 0.6f;
    private int TurnsWandering = 0;

	// Use this for initialization
	void Start () {
        textPanel.SetActive(false);
        updateLocation();
        if(board == null) {
            board = GameObject.Find("Board");
        }
	    if(goalPoint == null) {
            state = AIStates.IDLE;
           // state = (AIStates)Random.Range(0, 3);
        }
        Player.turn += StateMachine;
	}

    // States: Wander, Persue, Idle
    void StateMachine() {
        //Debug.Log(string.Format("at time {0} the state is {1}", Time.time, state));
        switch(state) {
            case AIStates.WANDER:
                TurnsWandering += 1;
                // this means the ai needs a place to go, this happens on a new board or when the ai gets stuck looping for too long
                if(goalPoint == null || TurnsWandering > 5) {
                    // This line sucks but it chooses a random square on the board
                    goalPoint = board.GetComponentsInChildren<ClickObject>()[Random.Range(0, board.GetComponentsInChildren<ClickObject>().Length)].gameObject;
                    TurnsWandering = 0;
                } else if(currentSquare.Equals(goalPoint)) {
                    // you done! 
                    // choose a new state and return
                    selectRandomState();
                    return;
                }
                // sets the current square to not holding this character anymore in anticipation for the move
                // then choose a square that advances the ai towards the target square
                currentSquare.GetComponent<ClickObject>().setCharacter(null);
                currentSquare = findNextMove();
                updateLocation();
                break;
            case AIStates.PERSUE:
                TurnsWandering = 0;
                // this is not in use for now
                selectRandomState();
                break;
            case AIStates.IDLE:
                TurnsWandering = 0;
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

    // Update location performs the physical itween move to the next square, as well as setting the character of the square to this object
    void updateLocation() {
        iTween.MoveTo(gameObject, iTween.Hash("position", new Vector3(currentSquare.transform.position.x, currentSquare.transform.position.y + YOffsetFromBoard, currentSquare.transform.position.z), 
                                              "time", animationTime, 
                                              "easetype", iTween.EaseType.easeInOutQuad));
        currentSquare.GetComponent<ClickObject>().setCharacter(this.gameObject);
        //transform.position = new Vector3(currentSquare.transform.position.x, currentSquare.transform.position.y, currentSquare.transform.position.z - 1f);
    }

    AIStates selectRandomState() {
        return state = (AIStates)Random.Range(0, 3);
    }
}
