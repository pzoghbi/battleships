using UnityEngine;

public class BattleshipPartData : ScriptableObject
{
    public Vector2 gridPosition;
    public BattleshipData battleshipData;
    public bool isHit = false;
}