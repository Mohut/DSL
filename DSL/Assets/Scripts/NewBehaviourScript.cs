using UnityEngine;
using UnityEngine.UIElements;

public class NewBehaviourScript : MonoBehaviour
{
    public UIDocument uiDoc; // assign it in inspector
    private VisualElement root;
 
    private void Awake()
    {
        root = uiDoc.rootVisualElement;
 
        // using Button here but you can use any visual element with //text
        Button btn = root.Q<Button>("Learn");
        string btnText = btn.text;
 
        float btnWidth = btn.resolvedStyle.width;
 
        IStyle style = btn.style;
        style.fontSize = new   StyleLength (btnWidth / btnText.Length);
 
        btn.style.fontSize = style.fontSize;
 
    }
}
