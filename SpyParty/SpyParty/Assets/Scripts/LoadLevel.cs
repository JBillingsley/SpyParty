﻿using UnityEngine;
using System.Collections;

public class LoadLevel : MonoBehaviour {

	public void loadLevel(string level) {
        Application.LoadLevel(level);
    }
}
