﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider))]
public class ClickObject : MonoBehaviour {
    private bool highlightable = false;
    private GameObject character;
    public List<GameObject> neighborCubes;
    public ClickObject requisiteItem;
    public GameObject hiddenObjectNotification;
    public bool requisiteItemFound = false;
    public string hiddenObjectText;
    public string requisiteNotMetText;
    public List<GameObject> inventoryItems;
    public bool hideable;
    private bool hiddenObjectDiscovered = false;
    private bool dangerSquare = false; // this means a player can be caught if they are on one of these squares
    private PathNode thisPath = null;
    //public GameObject TextPanel;


    void Start() {
        if(hiddenObjectNotification != null && !gameObject.name.Contains("Tagged")) {
            gameObject.name += "Tagged";
        }
    }
    public void setThisPath(PathNode path) {
        thisPath = path;
        //    Debug.Log(string.Format("path set to node {0}", path.name));
    }

    public PathNode getThisPath() {
        return thisPath;
    }
    // This is called when the currentSquare the player is in is this square
    public void holdingPlayer() {
        setCharacter(Player.instance.gameObject);
        selectedSquare();
       // Player.instance.updateIcon();
        foreach(GameObject neighbor in neighborCubes) {
            neighbor.GetComponent<ClickObject>().selectedSquare();
        }
        if(hiddenObjectNotification != null && !hiddenObjectDiscovered) {
           // selectedSquare();
        }
    }

    // This is called when this square is a legal move, this flags this square as available
    void selectedSquare() {
        this.highlightable = true;
        GetComponent<Renderer>().material.color = Color.green;
        if(character != null && character.GetComponent<NPC>() != null) {
            character.GetComponent<NPC>().conversationIcon.SetActive(true);
        }
    }

    public void clearSquares() {
        setCharacter(null);
        notAvailable();
        foreach(GameObject neighbor in neighborCubes) {
            neighbor.GetComponent<ClickObject>().notAvailable();
        }
        if(hiddenObjectDiscovered && highlightable) {
          //  notAvailable();
        }
    }

    void notAvailable() {
        highlightable = false;
        GetComponent<Renderer>().material.color = Color.white;
        if(character != null && character.GetComponent<NPC>() != null) {
            character.GetComponent<NPC>().conversationIcon.SetActive(false);
        }
    }

    // when the cube is clicked
    void OnMouseDown() {
       // Debug.Log(string.Format("this square highlightablity is {0} and the character is {1}", hideable, character.GetComponent<Player>()));
        if(!EventSystem.current.IsPointerOverGameObject()) {
            // this is a hiddenobject tile clicked
            if(highlightable && hiddenObjectNotification != null && !hiddenObjectDiscovered && Player.instance.currentSquare.Equals(gameObject)) {
                manageHiddenItem();

                // hidden character
                return;
            } else if(highlightable && Player.instance.currentSquare.Equals(gameObject) && hideable) {
                // gotta manage the character being hidden
                setCharacter(null);
                Player.instance.hideIcon.SetActive(false);
                Player.instance.finished();
               // Debug.Log("THE CHARACTER IS HIDING at time " + Time.time);
                Invoke("updatePlayerIcon", .7f);
                return;
            } else if(highlightable && (character == null || character.GetComponent<Player>() != null)) {
                // the player moves to this square
               // Debug.Log("moving");
                GameObject.Find("Player").GetComponent<Player>().moving(this.gameObject);
                return;
                // this is conversation
            } else if(highlightable && character != null) {
                // player engages in conversation w/ character
                /*
                if(ObjectiveManager.instance.currentObjective == 0) {
                    ObjectiveManager.instance.objectiveComplete();
                }
                */
                NPC targetNPC = character.GetComponent<NPC>();
                if(targetNPC != null) {
                    targetNPC.conversationIcon.SetActive(false);
                    targetNPC.textPanel.SetActive(true);
                    Invoke("disableTextPanel", 3f);
                }
                return;
                // character movement
            } 
          //  Player.instance.finished();
        }
    }

    private void updatePlayerIcon() {
        Player.instance.updateIcon();
    }

    private void manageHiddenItem() {
        // Hidden Object Objective
        /*
        if(ObjectiveManager.instance.currentObjective == 1) {
            ObjectiveManager.instance.objectiveComplete();
        }
        */
        Player.instance.examineIcon.SetActive(false);
        hiddenObjectNotification.SetActive(true);
        if(requisiteItem != null && !requisiteItem.requisiteItemFound) {
            hiddenObjectNotification.GetComponentInChildren<Text>().text = requisiteNotMetText;
        } else if((requisiteItem != null && requisiteItem.requisiteItemFound) || requisiteItem == null) {
            hiddenObjectNotification.GetComponentInChildren<Text>().text = hiddenObjectText;
            foreach(GameObject target in inventoryItems) {
                target.SetActive(true);
            }

            hiddenObjectDiscovered = true;
            requisiteItemFound = true;
        }

        
        notAvailable();
        Invoke("disableObjectNotification", 3f);
        // If we need to do other stuff do it here
    }

    // This method is a hack please make this better later
    private void disableTextPanel() {
        if(character != null) {
            character.GetComponent<NPC>().textPanel.SetActive(false);
        }
        Player.instance.finished();
    }

    // this is the same hack which is even worse than having just 1 terrible function
    private void disableObjectNotification() {
        hiddenObjectNotification.SetActive(false);
        Player.instance.finished();
    }

    public void setCharacter(GameObject character) {
        this.character = character;
    }

    public GameObject getCharacter() {
        return character;
    }

    public void personalSquare() {
        dangerSquare = true;
    }

    public bool isSquareDangerous() {
        return dangerSquare;
    }
}