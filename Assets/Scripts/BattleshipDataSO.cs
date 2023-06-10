using System.Collections.Generic;
using System.Linq;
using static BoardData;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battleship", menuName = "Battleships/New battleship")]
public class BattleshipDataSO : ScriptableObject
{
    [SerializeField] internal Battleship prefab;
    [SerializeField] internal byte gridWidth;
    [SerializeField] internal byte gridHeight;
}
