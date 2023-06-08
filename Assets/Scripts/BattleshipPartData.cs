using UnityEngine;

public class BattleshipPartData : ScriptableObject
{
    public Vector2Int gridPosition;
    public BattleshipData battleshipData;
    public bool isHit = false;
}