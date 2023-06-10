using System.Collections.Generic;
using UnityEngine;

public class PlayerBoard : Board
{
    private List<Battleship> battleships = new List<Battleship>();
    private Dictionary<BattleshipData, Battleship> placedBattleships = new Dictionary<BattleshipData, Battleship>();

    private new void Awake()
    {
        base.Awake();
        CreateBattleships();
    }

    private void CreateBattleships()
    {
        GameManager.instance.battleSettings.battleshipsBlueprintData.ForEach(battleshipData =>
        {
            var battleship = Instantiate(battleshipData.prefab, boardRoot);
            battleship.prefab = battleshipData.prefab;
            battleships.Add(battleship);
        });
    }

    internal void LoadBoardData(BoardData playerMovesBoardData, PlayerBattleshipsData playerBattleshipsData)
    {
        UpdateBattleshipsPosition(playerBattleshipsData);
        LoadBoardData(playerMovesBoardData);
    }

    private void UpdateBattleshipsPosition(PlayerBattleshipsData playerBattleshipsData)
    {
        // place battleship objects on the board
        placedBattleships.Clear();

        playerBattleshipsData.battleshipsData.ForEach(battleshipData =>
        {
            battleships.ForEach(battleship =>
            {
                if (!placedBattleships.ContainsKey(battleshipData)
                && !placedBattleships.ContainsValue(battleship))
                {
                    if (battleship.prefab == battleshipData.prefab)
                    {
                        battleship.SetBattleshipPosition(battleshipData);
                        placedBattleships.Add(battleshipData, battleship);
                    }
                }
            });
        });
    }
}