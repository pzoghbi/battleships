using UnityEngine;
using UnityEngine.UIElements;

public class RotateTowardsMouse : MonoBehaviour
{
    // Update is called once per frame
    private void Update()
    {
        UpdateRotation();
    }

    void UpdateRotation()
    {
        /* WIP */ 
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -Camera.main.transform.position.z;
        Vector3 position = Camera.main.ScreenToWorldPoint(mousePosition);
        position.y = 0;

        var angle = Vector2.SignedAngle(transform.root.position, position);

        transform.RotateAround(transform.root.position, transform.root.up, angle);
    }
}
