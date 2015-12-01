using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum AIStates { WANDER, IDLE, PERSUE };

public abstract class AICharacters : MonoBehaviour {
    public abstract void chooseNewGoal();
    public abstract bool characterBlockConditions();
}

public class NPC : AICharacters {
    public GameObject currentSquare;
    public GameObject board;
    public float YOffsetFromBoard;
    public GameObject textPanel;
    public List<GameObject> persueRoom;
    public GameObject goalPoint;
    public AIStates state;
    public GameObject conversationIcon;
    private float animationTime = 0.6f;
    private List<PathNode> pathToGoal;
    private List<PathNode> openSet = new List<PathNode>(); // set of nodes to be evaluated
    private List<PathNode> closedSet = new List<PathNode>(); // nodes along the best path


    // Use this for initialization

    void Start() {
        textPanel.SetActive(false);
        updateLocation();
        if(board == null) {
            board = GameObject.Find("Board");
        }
        if(goalPoint == null) {
            // Debug.Log("goal being set");

            chooseNewGoal();
            //  Debug.Log(string.Format("the goalpoint name is {0}", goalPoint));
            findPath();
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
                if(goalPoint == null) {
                    selectRandomState();
                    chooseNewGoal();
                    return;
                }
                if(currentSquare.Equals(goalPoint)) {
                    // you done! 
                    // choose a new state and return
                    //     Debug.Log("we have moved into the goal square so we are calculating a new square to wander to");
                    selectRandomState();
                    chooseNewGoal();
                    cleanPathNodes();
                    findPath();
                    //      Debug.Log(goalPoint);
                    return;
                }
                // sets the current square to not holding this character anymore in anticipation for the move
                // then choose a square that advances the ai towards the target square

                currentSquare = findNextMove();
                updateLocation();
                break;
            case AIStates.PERSUE:
             //   Debug.Log("You GETTIN CHASED");
                foreach(GameObject square in persueRoom) {
                    if(squareContainsPlayer(square)) {
                        // we do a persuit 
                        goalPoint = square;
                        cleanPathNodes();
                        findPath();
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
                state = AIStates.WANDER;
                goalPoint = null;
                break;
            default:
                break;
        }
    }

    private GameObject findNextMove() {
        GameObject potentialSquare = new GameObject();
        potentialSquare.tag = "Finish";
        /*
        if(pathToGoal == null) {
            Debug.Log("finding a new path");
            findPath();
        }
        */
        //NOTE I added the || statement to check if there is a character next to this object
        if(pathToGoal.Count <= 0 || characterBlockConditions()) {
            //  goalPoint = board.GetComponentsInChildren<ClickObject>()[Random.Range(0, board.GetComponentsInChildren<ClickObject>().Length)].gameObject;
            cleanPathNodes();
            //   Debug.Log("finding a new path");
            findPath();
            return currentSquare;
        }
        //  Debug.Log(pathToGoal);
        //   Debug.Log(pathToGoal.Count);
        potentialSquare = pathToGoal[0].thisSquare;
        pathToGoal.RemoveAt(0);
        /*
        potentialSquare = pathToGoal[0].gameObject;
        pathToGoal.RemoveAt(0);
        */
        currentSquare.GetComponent<ClickObject>().setCharacter(null);
        return potentialSquare;

    }

    public override bool characterBlockConditions() {
        if(pathToGoal[0].thisSquare.GetComponent<ClickObject>().getCharacter() == null || pathToGoal[0].thisSquare.GetComponent<ClickObject>().getCharacter().GetComponent<Player>() == null) {
            return false;
        } else {
            Debug.Log("character block conditions met by " + this.name);
            return true;
        }
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
        updateConvoIcon();
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

    public override void chooseNewGoal() {
        goalPoint = board.GetComponentsInChildren<ClickObject>()[Random.Range(0, board.GetComponentsInChildren<ClickObject>().Length)].gameObject;
    }

    public void findPath() {
        openSet = new List<PathNode>();
        closedSet = new List<PathNode>();
        pathToGoal = new List<PathNode>();
        GameObject container = new GameObject();
        container.name = "containerPath";
        container.tag = "Finish";
        PathNode start = container.AddComponent<PathNode>();
        start.createPathNode(currentSquare, goalPoint);
        start.calcWeights();
        //  Debug.Log(string.Format("we created a new start node {0} with weight {1}", start.thisSquare.name, start.fValue));

        openSet = openSet.OrderBy(x => x.fValue).ToList();
        openSet.Add(start);
        //pathToGoal = start.findNextPath();
        // while the list is not empty
        while(openSet.Count != 0) {
            PathNode current = openSet[0];
            //     Debug.Log(string.Format("we are now on node {0}", current.thisSquare.name));
            //Debug.Log(string.Format("path being calculated from {0} to {1}", current.thisSquare.name, goalPoint.name));
            openSet.Remove(openSet[0]);

            // Debug.Log(current.fValue);
            current.calcWeights();
            if(current.checkIfGoal()) {
                //Debug.Log(string.Format("we have found goal between {0} and {1}", current.thisSquare.name, goalPoint.name));
                pathToGoal = new List<PathNode>();
                //        Debug.Log(string.Format("we found a path from {0} to {1}", currentSquare.name, goalPoint.name));
                PathNode point = current;
                while(point.parent != null) {
                    //          Debug.Log(string.Format("{0} is being added to the path to take", point.name));
                    pathToGoal.Insert(0, point);
                    point = point.parent;
                }
                // we have our path and are done
                closedSet.RemoveRange(0, closedSet.Count);
                openSet.RemoveRange(0, openSet.Count);
                //       Debug.Log("we are done and moving on to NPC 95");
                return;
            } else {
                handleNeighborSetting(current);
            }
        }
        Debug.LogError("we have dropped out of the loop");
        cleanPathNodes();
    }

    void handleNeighborSetting(PathNode current) {
        //     Debug.Log("we are adding neighbors to the set");
        //      Debug.Log(string.Format("the current node is {0}", current.name));
        foreach(GameObject target in current.thisSquare.GetComponent<ClickObject>().neighborCubes) {
            if(target.GetComponent<ClickObject>().getThisPath() == null) {
                //      Debug.Log(string.Format("pathnode is null for {0}", target.name));
                GameObject container = new GameObject();
                container.tag = "Finish";
                container.name = target.name + "Path" + name;
                PathNode targetNode = container.AddComponent<PathNode>();
                //PathNode targetNode = target.GetComponent<PathNode>();
                //          Debug.Log(targetNode);
                targetNode.createPathNode(target, current, goalPoint);
                targetNode.calcWeights();
                //           Debug.Log(string.Format("adding {0} to open set", targetNode.thisSquare.name));
                target.GetComponent<ClickObject>().setThisPath(targetNode);
                openSet.Add(targetNode);
            } else {
                if(closedSet.Contains(target.GetComponent<ClickObject>().getThisPath())) {
                    //             Debug.Log(string.Format("closed set already contains node {0}", target.name));
                    // then we is good
                }
            }
        }
        //      Debug.Log(string.Format("we are adding {0} to the closedset", current.thisSquare.name));
        closedSet.Add(current);
    }

    private void cleanPathNodes() {
        foreach(GameObject target in GameObject.FindGameObjectsWithTag("Finish")) {
            //        Debug.Log(string.Format("Cleaning up target {0}", target.name));
          //  if(target.name.Contains("Path" + name)) {
                if(target.GetComponent<PathNode>() != null) {
                    target.GetComponent<PathNode>().thisSquare.GetComponent<ClickObject>().setThisPath(null);
                }
                Destroy(target);
          //  }
        }
    }

    private void updateConvoIcon() {
        foreach(GameObject neighbor in currentSquare.GetComponent<ClickObject>().neighborCubes) {
            if(neighbor.GetComponent<ClickObject>().getCharacter() != null && neighbor.GetComponent<ClickObject>().getCharacter().GetComponent<Player>() != null) {
                conversationIcon.SetActive(true);
                return;
            }
        }
        conversationIcon.SetActive(false);
    }
}
