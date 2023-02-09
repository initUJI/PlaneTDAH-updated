using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game Data", menuName = "ScriptableObjects/Game_Settings", order = 1)]
public class GameSettings : ScriptableObject
{
    public int sensitivity = 0;
    public int micPower = 0;

}
