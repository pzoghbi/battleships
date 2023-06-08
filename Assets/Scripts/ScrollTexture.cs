using UnityEngine;

public class ScrollTexture : MonoBehaviour
{
    private new Renderer renderer;
    private Material material;
    [SerializeField] private Vector2 scrollOffset = new Vector2(0.001f, 0.001f);

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
        material.SetTextureOffset("_DetailAlbedoMap", material.mainTextureOffset);
    }
}
