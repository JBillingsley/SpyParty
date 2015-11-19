using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectiveManager : MonoBehaviour {
    public List<GameObject> objectives;
    public int currentObjective = 0;

    // singleton declaration
    private static ObjectiveManager _instance;
    public static ObjectiveManager instance {
        get {
            if(_instance == null) {
                _instance = GameObject.FindObjectOfType<ObjectiveManager>();
            }
            return _instance;
        }
    }

	// Use this for initialization
	void Start () {
        updateObjective();
	}

    public void objectiveComplete() {
        currentObjective += 1;
        updateObjective();
    }

    private void updateObjective() {
        for(int i = 0; i < objectives.Count; i++) {
            if(i != currentObjective) {
                objectives[i].SetActive(false);
            } else {
                objectives[i].SetActive(true);
            }
        }
    }
}
