using System.Collections.Generic;
using UnityEngine;
using static BoardData;

public class BoardTile : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosionParticles;
    internal Vector2Int gridPosition;

    private int tileType = (int) BoardTileType.Empty;
    internal int TileType
    {
        get => tileType;
        set
        {
            tileType = value;
            UpdateMaterial();
        }
    }

    internal bool interactable = false;

    [Header("Materials")]
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Material hitMaterial;
    [SerializeField] private Material missMaterial;
    [SerializeField] private Material emptyMaterial;
    [SerializeField] private Material currentMaterial;
    [SerializeField] private Material flagMaterial;

    private MeshRenderer meshRenderer;
    private static Dictionary<int, Material> materials = new Dictionary<int, Material>();

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        materials.TryAdd((int) BoardTileType.Normal, normalMaterial);
        materials.TryAdd((int) BoardTileType.Empty, emptyMaterial);
        materials.TryAdd((int) BoardTileType.Miss, missMaterial);
        materials.TryAdd((int) BoardTileType.Hit, hitMaterial);
        materials.TryAdd((int) BoardTileType.Flag, flagMaterial);
    }

    private void Start()
    {
        UpdateMaterial();
        SetTilePositionFromGridPosition();
    }

    private void SetTilePositionFromGridPosition()
    {
        var localOffset = new Vector3(0.5f, 0, 0.5f);
        transform.localPosition = new Vector3(gridPosition.x, 0, gridPosition.y) + localOffset;
    }

    private void OnMouseOver()
    {
        if (!interactable 
            || !IsClickableTileType()
            || !BattleManager.instance.AllowInput
            ) return;

        meshRenderer.material = highlightMaterial;
    }

    private void OnMouseExit()
    {
        if (!interactable) return;

        meshRenderer.material = currentMaterial;
    }

    private void OnMouseUp()
    {
        if (!interactable) return;

        PropagateClick(gridPosition);
    }

    internal void UpdateMaterial()
    {
        currentMaterial = materials[tileType];
        meshRenderer.material = currentMaterial;
    }

    private void PropagateClick(Vector2Int gridPosition)
    {
        if (IsClickableTileType())
        {
            BattleManager.instance.ProcessTileSelection(gridPosition);
        }
    }

    private bool IsClickableTileType()
    {
        switch((BoardTileType) tileType)
        {
            case BoardTileType.Empty:
                return true;

            case BoardTileType.Hit:
            case BoardTileType.Miss:
            case BoardTileType.Flag:
            case BoardTileType.Ship:
            default:
                return false;
        }
    }

    internal void PlayExplosionParticles()
    {
        explosionParticles.Play();
    }
}