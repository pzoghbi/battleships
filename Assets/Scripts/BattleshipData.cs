using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battleship", menuName = "Battleships/New battleship")]
[Serializable]
public class BattleshipData : ScriptableObject
{
    public Battleship prefab;
    [HideInInspector] public List<ShipPart> shipParts = new List<ShipPart>();
    [HideInInspector] public Vector2Int gridPosition;

    public byte gridWidth;
    public byte gridHeight;

    [HideInInspector] public bool isFlipped = false;

    public bool IsWrecked { 
        get {
            foreach(var part in shipParts)
            {
                if (!part.isHit) return false;
            }
            return true;
        }
    }
    public void Flip() {
        byte flip = gridWidth;
        gridWidth = gridHeight;
        gridHeight = flip;
        isFlipped = !isFlipped;
    }
}
