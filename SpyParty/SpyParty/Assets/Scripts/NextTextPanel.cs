using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NextTextPanel : MonoBehaviour {
    public List<GameObject> thingsToActivate;
    public List<GameObject> thingsToDeactivate;

	public void IGotClicked() {
        foreach(GameObject item in thingsToDeactivate) {
            item.SetActive(false);
        }

        foreach(GameObject item in thingsToActivate) {
            item.SetActive(true);
        }
    }
}
