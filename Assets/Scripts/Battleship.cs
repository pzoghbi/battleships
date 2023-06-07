using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Battleship : MonoBehaviour
{
    internal BattleshipData battleshipData;
    private const int flipRotation = -90;

    private void Start()
    {
        var offsetPosition = 0.5f * new Vector3(1, 0, 1);
        var position = new Vector3(battleshipData.gridPosition.x, 0, battleshipData.gridPosition.y) + offsetPosition;
        var rotation = Quaternion.Euler(0, battleshipData.isFlipped ? flipRotation : 0, 0);

        transform.SetLocalPositionAndRotation(position, rotation);
    }
}