using UnityEngine;

public class ButtonController : MonoBehaviour
{
    private TilemapBuilder builder;
    private GUIStyle bigFonts;
    
    private void Start()
    {
        builder = FindFirstObjectByType<TilemapBuilder>();

    }

    private void OnGUI()
    {
        bigFonts = GUI.skin.button;
        bigFonts.fontSize = 24;

        if (GUI.Button(new Rect(10, 10, 160, 80), "Generate!", bigFonts))
            builder.GenerateGrid();
    }
}
