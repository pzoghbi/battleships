using System;
using UnityEngine;

[Serializable]
public struct SerializableVector2Int
{
    public int x;
    public int y;

    public SerializableVector2Int(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static implicit operator Vector2Int(SerializableVector2Int sV2I)
    {
        return new Vector2Int(sV2I.x, sV2I.y);
    }

    public static implicit operator SerializableVector2Int(Vector2Int vector2Int)
    {
        return new SerializableVector2Int(vector2Int.x, vector2Int.y);
    }
}