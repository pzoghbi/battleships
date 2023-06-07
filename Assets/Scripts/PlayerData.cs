using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerData: ScriptableObject
{
    internal PlayerBoardData playerBoardData;
    internal BattleBoardData battleBoardData;

    private void Awake()
    {
        playerBoardData = CreateInstance<PlayerBoardData>();
        battleBoardData = CreateInstance<BattleBoardData>();
        playerBoardData.PrintBoard();
    }
}