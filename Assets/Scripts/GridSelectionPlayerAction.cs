using System;
using UnityEngine;

[Serializable]
public class GridSelectionPlayerAction : PlayerAction
{
    PlayerActionType type = PlayerActionType.GridSelection;
    public Vector2Int gridPosition;

    public GridSelectionPlayerAction(Vector2Int gridPosition)
    {
        this.gridPosition = gridPosition;
    }

    public override void Execute()
    {
        GameManager.instance.ProcessTileSelection(gridPosition);
    }
}