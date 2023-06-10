using UnityEngine;

public class BattleshipPartData
{
    public Vector2Int gridPosition;
    public BattleshipData battleshipData;
    public bool isHit = false;

    public static implicit operator bool(BattleshipPartData battleshipPartData)
    {
        return battleshipPartData != null;
    }
}