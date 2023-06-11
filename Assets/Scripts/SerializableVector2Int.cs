using System;

[Serializable]
public struct SerializableVector2Int
{
    private int x;
    private int y;

    public SerializableVector2Int(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}