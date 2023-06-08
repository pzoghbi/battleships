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
        if (!interactable) return;

        if (tileType != (int) BoardTileType.Hit
            && tileType != (int) BoardTileType.Miss
            && tileType != (int) BoardTileType.Flag
        )
        {
            meshRenderer.material = highlightMaterial;
        }
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
        if (tileType == (int) BoardTileType.Empty)
        {
            BattleManager.instance.ProcessTileSelection(gridPosition);
            PlayVFX();
        }
    }

    internal void PlayVFX()
    {
        explosionParticles.Play();
    }
}