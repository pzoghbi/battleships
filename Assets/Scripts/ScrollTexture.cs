using UnityEngine;

public class ScrollTexture : MonoBehaviour
{
    [SerializeField] private Vector2 scrollOffset = new Vector2(0.001f, 0.001f);
    private new Renderer renderer;
    private Material material;
    private const string _detailAlbedoMapName = "_DetailAlbedoMap";

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        material = renderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        material.mainTextureOffset += scrollOffset;
        material.SetTextureOffset(_detailAlbedoMapName, material.mainTextureOffset);
    }
}
