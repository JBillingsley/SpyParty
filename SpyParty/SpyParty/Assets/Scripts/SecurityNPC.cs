using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SecurityNPC : NPC {
    public List<ClickObject> goalPoints;
    public bool randomWander = false;
    private int currentGoalPointIndex = 0;

    public override void chooseNewGoal() {
        if(randomWander) {
            randomGoalPoint();
        } else {
            orderedGoalPoint();
        }
        state = AIStates.WANDER;
    }

    public override bool characterBlockConditions() {
        return false;
    }

    void randomGoalPoint() {
        ClickObject nextGoal = currentSquare.GetComponent<ClickObject>();
        while(nextGoal.Equals(currentSquare.GetComponent<ClickObject>())) {
            nextGoal = goalPoints[Random.Range(0, goalPoints.Count)];
        }
        goalPoint = nextGoal.gameObject;
    }

    void orderedGoalPoint() {
        if(currentGoalPointIndex < goalPoints.Count) {
            goalPoint = goalPoints[currentGoalPointIndex].gameObject;
        } else {
            currentGoalPointIndex = 0;
            goalPoint = goalPoints[currentGoalPointIndex].gameObject;
        }
        currentGoalPointIndex += 1;
    }
}
