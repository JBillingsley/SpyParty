using UnityEngine;
using System.Collections;

public class InventoryButton : MonoBehaviour {
    public GameObject zoomTarget;
    public GameObject zoomText;

    public void shiftZoomTarget() {
        if(InventoryPanel.instance.currentActiveZoomTarget != null) {
            InventoryPanel.instance.currentActiveZoomTarget.SetActive(false);
        }
        if(InventoryPanel.instance.currentZoomText != null) {
            InventoryPanel.instance.currentZoomText.SetActive(false);
        }
        InventoryPanel.instance.currentActiveZoomTarget = zoomTarget;
        InventoryPanel.instance.currentZoomText = zoomText;
        zoomTarget.SetActive(true);
        zoomText.SetActive(true);
    }
	
}
