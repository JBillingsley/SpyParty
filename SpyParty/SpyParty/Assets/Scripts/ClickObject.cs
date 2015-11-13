using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof (Collider))]
public class ClickObject : MonoBehaviour {
    private Vector3 initialPosition;
    private bool highlighted = false;

    public GameObject TextPanel;

    void Start() {
        GetComponent<Renderer>().material.color = Color.white;
        initialPosition = transform.position;
    }

	void OnMouseDown() {
        if(GetComponent<Renderer>().material.color.Equals(Color.white)) {
            GetComponent<Renderer>().material.color = Color.black;
        } else {
            GetComponent<Renderer>().material.color = Color.white;
        }
    }

    void OnMouseOver() {
        highlighted = true;
    }

    void Update() {
        if(highlighted) {
            transform.position = new Vector3(initialPosition.x, initialPosition.y, initialPosition.z - 1f);
            TextPanel.SetActive(true);
            highlighted = false;
        } else {
            transform.position = initialPosition;
            TextPanel.SetActive(false);
        }
    }
}
