using System.Collections.Generic;
using UnityEngine;
using static BoardData;

public class BoardTile : MonoBehaviour
{
    // Exposed
    internal Vector2Int gridPosition;

    internal int TileType
    {
        get => tileType;
        set
        {
            tileType = value;
            UpdateMaterial();
        }
    }

    internal bool Interactable 
    { 
        get => interactable && IsClickableTileType() && GameManager.instance.AllowInput;
        set => interactable = value;
    }

    // Private
    [SerializeField] private ParticleSystem explosionParticles;
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
    private int tileType = (int) BoardTileType.Empty;
    private bool interactable = false;

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

    internal void ResetMaterial() 
    { 
        meshRenderer.material = currentMaterial;
    }

    internal void MarkSelected()
    {
        meshRenderer.material = highlightMaterial;
    }

    internal void UpdateMaterial()
    {
        currentMaterial = materials[tileType];
        meshRenderer.material = currentMaterial;
    }

    internal void PlayExplosionParticles()
    {
        explosionParticles.Play();
    }
}