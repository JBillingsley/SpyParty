using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof (Collider))]
public class ClickObject : MonoBehaviour {
    private bool highlightable = false;
    private GameObject character;
    public List<GameObject> neighborCubes;
    public GameObject hiddenObjectPanel;
    private bool hiddenObjectDiscovered = false;
    private bool dangerSquare = false; // this means a player can be caught if they are on one of these squares
    //public GameObject TextPanel;

    // This is called when the currentSquare the player is in is this square
    public void holdingPlayer() {
        setCharacter(Player.instance.gameObject);
        foreach(GameObject neighbor in neighborCubes) {
            neighbor.GetComponent<ClickObject>().selectedSquare();
        }
        if(hiddenObjectPanel != null && !hiddenObjectDiscovered) {
            selectedSquare();
        }
    }

    // This is called when this square is a legal move, this flags this square as available
    void selectedSquare() {
        this.highlightable = true;
        GetComponent<Renderer>().material.color = Color.green;
    }

    public void clearSquares() {
        setCharacter(null);
        foreach(GameObject neighbor in neighborCubes) {
            neighbor.GetComponent<ClickObject>().notAvailable();
        }
        if(hiddenObjectDiscovered && highlightable) {
            notAvailable();
        }
    }

    void notAvailable() {
        highlightable = false;
        GetComponent<Renderer>().material.color = Color.white;
    }

    // when the cube is clicked
	void OnMouseDown() {
        // this is a hiddenobject tile clicked
        if(highlightable && hiddenObjectPanel != null && !hiddenObjectDiscovered && Player.instance.currentSquare.Equals(gameObject)) {
            // Hidden Object Objective
            if(ObjectiveManager.instance.currentObjective == 1) {
                ObjectiveManager.instance.objectiveComplete();
            }
            hiddenObjectPanel.SetActive(true);
            hiddenObjectDiscovered = true;
            notAvailable();
            Invoke("disableObjectPanel", 3f);
            // If we need to do other stuff do it here

            // this is normal character movement
        } else  if(highlightable && character == null) {
            // the player moves to this square
            GameObject.Find("Player").GetComponent<Player>().moving(this.gameObject);

            // this is conversation
        } else if(highlightable && character != null) {
            // player engages in conversation w/ character
            if(ObjectiveManager.instance.currentObjective == 0) {
                ObjectiveManager.instance.objectiveComplete();
            }
            NPC targetNPC = character.GetComponent<NPC>();
            if(targetNPC != null) {
                targetNPC.textPanel.SetActive(true);
                Invoke("disableTextPanel", 3f);
            }
            /*
             * tile holding npc is clicked
             * npc checks for a new piece of info to display
             * new convo thing that was chosen is displayed
             * wait some time longer than half a second before npc takes a turn
             * call Player.turn()
             */
        }
    }

    // This method is a hack please make this better later
    private void disableTextPanel() {
        if(character != null) {
            character.GetComponent<NPC>().textPanel.SetActive(false);
        }
        Player.instance.finished();
    }

    // this is the same hack which is even worse than having just 1 terrible function
    private void disableObjectPanel() {
        hiddenObjectPanel.SetActive(false);
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
