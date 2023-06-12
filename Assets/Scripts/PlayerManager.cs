using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] internal bool autoPlay = false;
    internal PlayerData[] PlayersData => playersData;

    [SerializeField] private ReplayPlayer replayPlayer;
    private PlayerData[] playersData;

    internal void InitializePlayers(BattleshipGameSettingsSO gameSettings)
    {
        if (replayPlayer)
        {
            playersData = InitializePlayersDataFromReplay();
        }
        else
        {
            playersData = InitializePlayersData(
                GameManager.playerCount, gameSettings);
        }
    }

    private PlayerData[] InitializePlayersData(byte playerCount, BattleshipGameSettingsSO settings)
    {
        var playersData = new PlayerData[playerCount];

        for (byte playerIndex = 0; playerIndex < playerCount; playerIndex++)
            playersData[playerIndex] = new PlayerData(settings);

        return playersData;
    }

    private PlayerData[] InitializePlayersDataFromReplay() {
        return replayPlayer.replayData.staticData
                .Select(data => (PlayerData) data).ToArray();
    }
}