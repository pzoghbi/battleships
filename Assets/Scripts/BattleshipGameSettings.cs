using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battleship Game Settings", menuName = "Battleships/Game Settings")]
public class BattleshipGameSettings : ScriptableObject
{
    public List<BattleshipDataSO> battleshipsBlueprintData;
}
