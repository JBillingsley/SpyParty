using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriggerActivate : MonoBehaviour {
    public List<GameObject> thingsToActivate;
    public List<GameObject> thingsToDeactivate;

	void OnTriggerEnter(Collider target) {
        foreach(GameObject item in thingsToDeactivate) {
            item.SetActive(false);
        }

       foreach(GameObject item in thingsToActivate) {
            item.SetActive(true);
        }
    }
}
