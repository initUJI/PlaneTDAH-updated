﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishingPlaneGame : MonoBehaviour
{
    GameManagerScript gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
    }

    


}
