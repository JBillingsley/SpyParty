using UnityEngine;
using System.Collections;

public class BlackBoxPanel : MonoBehaviour {
    public GameObject currentTarget;

    private static BlackBoxPanel _instance;
    public static BlackBoxPanel instance {
        get {
            if(_instance == null) {
                _instance = GameObject.FindObjectOfType<BlackBoxPanel>();
            }
            return _instance;
        }
    }

    void Start() {
        gameObject.SetActive(false);
    }

    public void toggleActive() {
            gameObject.SetActive(!gameObject.activeSelf);
    }

    public void blackBoxOn() {
        if(InventoryPanel.instance.gameObject.activeSelf) {
            InventoryPanel.instance.gameObject.SetActive(false);
        }
        gameObject.SetActive(true);
    }

    public void activateTarget(GameObject target) {
        if(currentTarget != null) {
            currentTarget.SetActive(false);
        }
        currentTarget = target;
        target.SetActive(true);
    }
}
