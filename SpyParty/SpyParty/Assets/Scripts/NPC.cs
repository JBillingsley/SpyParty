using UnityEngine;
using System.Collections;
using System.Collections.Generic;
    
    public enum AIStates { WANDER, IDLE, PERSUE };

public class NPC : MonoBehaviour {
    public GameObject currentSquare;
    public GameObject board;
    public float YOffsetFromBoard;
    public GameObject textPanel;
    public List<GameObject> persueRoom;
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

        // this flags all the squares as hot
        foreach(GameObject square in persueRoom) {
            square.GetComponent<ClickObject>().personalSquare();
        }
	}

    // States: Wander, Persue, Idle
    void StateMachine() {
        //Debug.Log(string.Format("at time {0} the state is {1}", Time.time, state));
        checkRoomInvasion();
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
                
                currentSquare = findNextMove();
                updateLocation();
                break;
            case AIStates.PERSUE:
                TurnsWandering = 0;
                foreach(GameObject square in persueRoom) {
                    if(squareContainsPlayer(square)) {
                        // we do a persuit 
                        goalPoint = square;
                        currentSquare = findNextMove();
                        updateLocation();
                        return;
                    } 
                }
                selectRandomState();
                // this is not in use for now
               // selectRandomState();
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
        currentSquare.GetComponent<ClickObject>().setCharacter(null);
        if(goalPoint != null) {
            float distance = 1000000f;
           GameObject potentialSquare = null;
            // this loop finds a square in the direction of the goal point
            foreach(GameObject square in currentSquare.GetComponent<ClickObject>().neighborCubes) {
                // the second half of this is a conditional hack so NPCs dont end up in the same square
                if(Mathf.Abs(Vector3.Distance(square.transform.position, goalPoint.transform.position)) < distance && squareDoesntContainNPC(square)) {
                    distance = Mathf.Abs(Vector3.Distance(square.transform.position, goalPoint.transform.position));
                    // if the square contains the player
                    if(squareContainsPlayer(square)) {
                        // if it is dangerous it can be chosen 
                        if(square.GetComponent<ClickObject>().isSquareDangerous()) {
                            potentialSquare = square;
                        }
                        // if it doesnt contain the player it can be chosen
                    } else {
                        potentialSquare = square;
                    }
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
        if(squareContainsPlayer(currentSquare)) {
            Debug.Log("CAUGHT");
            Player.instance.caught();
            selectRandomState();
        }
        currentSquare.GetComponent<ClickObject>().setCharacter(this.gameObject);
        //transform.position = new Vector3(currentSquare.transform.position.x, currentSquare.transform.position.y, currentSquare.transform.position.z - 1f);
    }

    AIStates selectRandomState() {
        return state = (AIStates)Random.Range(0, 2);
    }

    void checkRoomInvasion() {
        foreach(GameObject square in persueRoom) {
            if(squareContainsPlayer(square)) {
                state = AIStates.PERSUE;
            }
        }
    }

    bool squareContainsPlayer(GameObject square) {
        if(square.GetComponent<ClickObject>().getCharacter() != null && square.GetComponent<ClickObject>().getCharacter().Equals(Player.instance.gameObject)) {
            return true;
        } else {
            return false;
        }
    }

    bool squareDoesntContainNPC(GameObject square) {
        if((square.GetComponent<ClickObject>().getCharacter() == null || square.GetComponent<ClickObject>().getCharacter().GetComponent<NPC>() == null)) {
            return true;
        } else {
            return false;
        }
    }
}
