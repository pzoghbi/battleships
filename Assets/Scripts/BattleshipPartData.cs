using System;
using UnityEngine;

[Serializable]
public class BattleshipPartData
{
    [NonSerialized] public BattleshipData battleshipData;
    [NonSerialized] public Vector2Int gridPosition;
    public SerializableVector2Int GridPosition => new SerializableVector2Int(gridPosition.x, gridPosition.y);
    public bool isHit = false;

    public static implicit operator bool(BattleshipPartData battleshipPartData)
    {
        return battleshipPartData != null;
    }
}