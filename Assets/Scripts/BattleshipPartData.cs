using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class BattleshipPartData
{
    [NonSerialized] public BattleshipData battleshipData;
    public SerializableVector2Int gridPosition;
    public bool isHit = false;

    public static implicit operator bool(BattleshipPartData battleshipPartData)
    {
        return battleshipPartData != null;
    }
}