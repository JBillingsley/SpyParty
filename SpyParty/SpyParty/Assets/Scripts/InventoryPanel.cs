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
        gameObject.SetActive(false);
    }

    public void toggleInventory() {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
