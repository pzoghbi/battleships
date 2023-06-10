using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battleship Game Settings", menuName = "Battleships/Game Settings")]
public class BattleshipGameSettingsSO : ScriptableObject
{
    public List<BattleshipDataSO> battleshipsBlueprintData;
}
