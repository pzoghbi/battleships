using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurveText : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    private const string titleText = "Battleships";
    private const float yOffsetAmount = .15f;
    private const float sizeOffsetPercent = 20;

    private void Awake()
    {
        DisplaceTextOnACurve();
    }

    private void DisplaceTextOnACurve()
    {
        textMesh = GetComponent<TextMeshProUGUI>();

        string finalText = "";
        for (byte i = 0; i < titleText.Length; i++)
        {
            char character = titleText[i];
            float angle = i * Mathf.PI / titleText.Length;
            float displaceCurve = Mathf.Sin(angle) * yOffsetAmount;
            float size = 100 + Mathf.Sin(angle) * sizeOffsetPercent; 
            string style = $"<voffset={displaceCurve}em><size={size}%>";
            finalText += style + character + "</size></voffset>";
        }

        textMesh.text = finalText;
    }
}
