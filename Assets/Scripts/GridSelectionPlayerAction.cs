using UnityEngine;

public class GridSelectionPlayerAction : PlayerAction
{
    PlayerActionType type = PlayerActionType.GridSelection;
    internal Vector2Int gridPosition;

    public GridSelectionPlayerAction(Vector2Int gridPosition)
    {
        this.gridPosition = gridPosition;
    }

    public override void Execute()
    {
        BattleManager.instance.ProcessTileSelection(gridPosition);
    }
}