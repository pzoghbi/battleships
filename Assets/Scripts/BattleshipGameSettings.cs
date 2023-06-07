using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battleship Settings", menuName = "Battleships/Game Settings")]
public class BattleshipGameSettings : ScriptableObject
{
    public List<BattleshipData> ships;
}
