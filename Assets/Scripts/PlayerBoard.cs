using System.Collections.Generic;
using UnityEngine;

public class PlayerBoard : MonoBehaviour
{
    [SerializeField] private Transform gridRoot;
    private BoardData boardDataRef;
    private List<GameObject> ships = new List<GameObject>();

    private void Awake()
    {
    }

    internal void LoadBoardData(PlayerBoardData pbData)
    {
        boardDataRef = pbData;

        ships.ForEach(ship => Destroy(ship.gameObject)); // todo 

        pbData.ships.ForEach(ship =>
        {
            ship.shipParts.ForEach(part =>
            {
                //boardDataRef.grid[(byte) part.gridPosition.x, (byte) part.gridPosition.y] =
                //    part.isHit
                //        ? (int) BoardData.BoardTileType.Hit
                //        : (int) BoardData.BoardTileType.Ship;
            });

            // todo 
            var battleship = Instantiate(ship.prefab, gridRoot);
            battleship.battleshipData = ship;
            ships.Add(battleship.gameObject);
        });
    }
}