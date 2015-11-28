using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathNode : MonoBehaviour {
    public GameObject thisSquare; // this is the square on the board that the node is representing
    public PathNode parent; // this is the node that discovered this node
    public List<ClickObject> neighbors; // these are the posible next nodes from this node
    public int weight = 1; // this is the weight of this node (which is 1 + all previous nodes)
    public GameObject goalSquare;
    public int hValue = 0;
    public int fValue = 0;
    
    public void createPathNode(GameObject square, GameObject goalSquare) {
        thisSquare = square;
        this.goalSquare = goalSquare;
        neighbors = findNeighbors();
        parent = null;
       // square.GetComponent<Renderer>().material.color = Color.red;
    }

    public void createPathNode(GameObject square, PathNode parent, GameObject goalSquare) {
        thisSquare = square;
        neighbors = findNeighbors();
        this.parent = parent;
        this.goalSquare = goalSquare;
        this.weight += parent.weight;
       // square.GetComponent<Renderer>().material.color = Color.red;
    }

    public void calcWeights() {
       // Debug.Log(string.Format("the dist between {0} and {1}", thisSquare.name, goalSquare));
        int startX = int.Parse(thisSquare.name.Substring(5, 1));
        int startY = int.Parse(thisSquare.name.Substring(7, 1));
        int endX = int.Parse(goalSquare.name.Substring(5, 1));
        int endY = int.Parse(goalSquare.name.Substring(7, 1));
        hValue = Mathf.Abs(endX - startX) + Mathf.Abs(endY - startY);
        fValue = hValue + weight;
        
    }

    public bool checkIfGoal() {
        if(thisSquare.name.Equals(goalSquare.name)) {
            // we are done and should return this square or the list of squares to create this path
          //  Debug.Log(string.Format("{0} and {1} have the same name", thisSquare.name, goalSquare.name));
            return true;
        } else {
            // we gotta search the paths for a goal
            return false;
         //   findNextPath();
        }
    }

    List<ClickObject> findNeighbors() {
     //   Debug.Log(string.Format("neighbors being found for {0}", gameObject.name));
        List<ClickObject> theseNeighbors = new List<ClickObject>();
        foreach(GameObject target in thisSquare.GetComponent<ClickObject>().neighborCubes) {
           // Debug.Log(string.Format("current target being set is {0}", target.name));
            theseNeighbors.Add(target.GetComponent<ClickObject>());
        }
        return theseNeighbors;
    }
}
