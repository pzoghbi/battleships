using UnityEngine;

public class Battleship : MonoBehaviour
{
    internal Battleship prefab;
    private const int flipZRotation = -90;

    internal void SetBattleshipPosition(BattleshipData battleshipData)
    {
        var offsetPosition = 0.5f * new Vector3(1, 0, 1);
        var position = new Vector3(battleshipData.gridPosition.x, 0, battleshipData.gridPosition.y) + offsetPosition;
        var rotation = Quaternion.Euler(0, battleshipData.isFlipped ? flipZRotation : 0, 0);

        transform.SetLocalPositionAndRotation(position, rotation);
    }
}