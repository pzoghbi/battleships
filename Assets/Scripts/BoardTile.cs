using System.Collections.Generic;
using UnityEngine;

public class BoardTile: MonoBehaviour
{
    internal int tileType;
    internal Vector2Int gridPosition;

    private MeshRenderer meshRenderer;
    private static Dictionary<int, Material> materials
        = new Dictionary<int, Material>();

    [Header("Materials")]
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Material hitMaterial;
    [SerializeField] private Material missMaterial;
    [SerializeField] private Material emptyMaterial;
    [SerializeField] private Material currentMaterial;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        materials.TryAdd((int) BoardData.BoardTileType.Empty, emptyMaterial);
        materials.TryAdd((int) BoardData.BoardTileType.Miss, missMaterial);
        materials.TryAdd((int) BoardData.BoardTileType.Hit, hitMaterial);
        materials.TryAdd((int) BoardData.BoardTileType.Normal, normalMaterial);
    }

    private void Start()
    {
        currentMaterial = normalMaterial;
    }

    private void OnMouseOver()
    {
        if (tileType != (int) BoardData.BoardTileType.Hit 
            && tileType != (int) BoardData.BoardTileType.Miss
        ) {
            meshRenderer.material = highlightMaterial;
        }
    }

    private void OnMouseExit()
    {
        meshRenderer.material = currentMaterial;
    }

    private void OnMouseUp()
    {
        PropagateClick(gridPosition);
    }

    internal void UpdateMaterial()
    {
        currentMaterial = materials[tileType];
        meshRenderer.material = currentMaterial;
    }

    private void PropagateClick(Vector2Int gridPosition)
    {
        BattleManager.instance.ApplyMove(gridPosition);
    }
}