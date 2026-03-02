using UnityEngine;

public class ComboInputHandler : MonoBehaviour
{
    private InputCombo combo;
    private KeyCode expectedKey = KeyCode.None;
    private int myIndex;
    private bool cleared = false;
    public string childName = "Pressed"; 
    private Transform childTransform; 

    public void Initialize(InputCombo comboRef, int index)
    {
        childTransform = transform.Find(childName);
        combo = comboRef;
        myIndex = index;

        // Assign expected key based on prefab name
        string prefabName = gameObject.name.ToLower();

        if (prefabName.Contains("space")) expectedKey = KeyCode.Space;
        else if (prefabName.Contains("w")) expectedKey = KeyCode.W;
        else if (prefabName.Contains("a")) expectedKey = KeyCode.A;
        else if (prefabName.Contains("s")) expectedKey = KeyCode.S;
        else if (prefabName.Contains("d")) expectedKey = KeyCode.D;

        UpdateVisual(false);
    }

    private void Update()
    {
        if (cleared) return;

        // Only the input that is currently active in the sequence can respond
        if (combo.currentInputIndex != myIndex) return;

        if (!Input.anyKeyDown) return;

        if (!combo.TryConsumeFrameInput()) return;

        if (Input.GetKeyDown(expectedKey))
        {
            Debug.Log($"[{myIndex}] Correct input: {expectedKey}");
            cleared = true;

            UpdateVisual(true); 

            combo.OnPress(true);
            return;
        }

        Debug.Log($"[{myIndex}] Incorrect input (pressed other key)");
        combo.OnPress(false);
    }

    private void UpdateVisual(bool pressed)
    {
        if (childTransform != null)
        {
            childTransform.gameObject.SetActive(pressed);
        }
        
    }
}
