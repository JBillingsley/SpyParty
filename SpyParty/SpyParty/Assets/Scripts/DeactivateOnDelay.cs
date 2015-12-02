using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DeactivateOnDelay : MonoBehaviour {
    public List<GameObject> thingsToActivate;
    private bool deactivated = false;
	// Use this for initialization
	void Update () {
        if(!deactivated) {
            deactivated = true;
            Invoke("deactivate", 4f);
        }
	}
	
	void deactivate() {
        deactivated = false;
        gameObject.SetActive(false);
        foreach(GameObject item in thingsToActivate) {
            item.SetActive(true);
        }
    }
}
