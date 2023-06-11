using Newtonsoft.Json;
using System;
using UnityEngine;

[Serializable]
public class BattleshipPartData
{
    [NonSerialized] public BattleshipData battleshipData;
    [NonSerialized] public Vector2Int gridPosition;
    [JsonProperty(PropertyName = "gridPosition")]
    public SerializableVector2Int GridPosition
    {
        get { return new SerializableVector2Int(gridPosition.x, gridPosition.y); }
        set { gridPosition = new Vector2Int(value.x, value.y); }
    }

    public bool isHit = false;

    public static implicit operator bool(BattleshipPartData battleshipPartData)
    {
        return battleshipPartData != null;
    }
}