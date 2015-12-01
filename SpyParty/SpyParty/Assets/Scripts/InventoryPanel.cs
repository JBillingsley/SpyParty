using UnityEngine;
using System.Collections;

public class InventoryPanel : MonoBehaviour {
    public GameObject currentActiveZoomTarget;
    public GameObject currentZoomText;

    // singleton declaration
    private static InventoryPanel _instance;
    public static InventoryPanel instance {
        get {
            if(_instance == null) {
                _instance = GameObject.FindObjectOfType<InventoryPanel>();
            }
            return _instance;
        }
    }

    void Start() {
       // gameObject.SetActive(false);
    }

    public void toggleInventory() {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void inventoryOn() {
        if(BlackBoxPanel.instance.gameObject.activeSelf) {
            BlackBoxPanel.instance.gameObject.SetActive(false);
        }
        gameObject.SetActive(true);
    }

    public void toggleGameObject(GameObject target) {
        target.SetActive(!target.activeSelf);
    }
}
