﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof (Collider))]
public class ClickObject : MonoBehaviour {
    private bool highlightable = false;
    public List<GameObject> neighborCubes;
    public GameObject TextPanel;

    // This is called when the currentSquare the player is in is this square
    public void holdingPlayer() {
        foreach(GameObject neighbor in neighborCubes) {
            neighbor.GetComponent<ClickObject>().selectedSquare();
        }
    }

    // This is called when this square is a legal move, this flags this square as available
    void selectedSquare() {
        this.highlightable = true;
        GetComponent<Renderer>().material.color = Color.green;
    }

    public void clearSquares() {
        foreach(GameObject neighbor in neighborCubes) {
            neighbor.GetComponent<ClickObject>().notAvailable();
        }
    }

    void notAvailable() {
        highlightable = false;
        GetComponent<Renderer>().material.color = Color.white;
    }

    // when the cube is clicked
	void OnMouseDown() {
        // if it is a legal square
        if(highlightable) {
            // the player moves to this square
            GameObject.Find("Player").GetComponent<Player>().moving(this.gameObject);
        }
    }
}
