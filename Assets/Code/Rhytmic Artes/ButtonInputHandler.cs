using System.Collections.Generic;
using UnityEngine;

public class ButtonInputHandler : MonoBehaviour
{
    private static List<ButtonInputHandler> activeHandlers = new List<ButtonInputHandler>();
    private static bool inputConsumedThisFrame = false;

    private RhythmButton buttonpref;
    private KeyCode expectedKey = KeyCode.None;
    private int myIndex;
    private float spawnTime;
    private bool cleared = false;

    public KeyCode ExpectedKey => expectedKey;
    public float SpawnTime => spawnTime;
    public bool Cleared => cleared;

    public void Initialize(RhythmButton buttonRef, int index)
    {
        buttonpref = buttonRef;
        myIndex = index;
        spawnTime = Time.time;

        string prefabName = gameObject.name.ToLower();
        if (prefabName.Contains("w")) expectedKey = KeyCode.W;
        else if (prefabName.Contains("a")) expectedKey = KeyCode.A;
        else if (prefabName.Contains("s")) expectedKey = KeyCode.S;
        else if (prefabName.Contains("d")) expectedKey = KeyCode.D;
    }

    private void OnEnable()
    {
        activeHandlers.Add(this);
    }

    private void OnDisable()
    {
        activeHandlers.Remove(this);
    }

    private void LateUpdate()
    {
        // reset the input flag at the end of the frame
        inputConsumedThisFrame = false;
    }

    private void Update()
    {
        // Only earliest uncleared listens
        ButtonInputHandler earliest = GetEarliestUncleared();
        if (earliest != this) return;

        if (inputConsumedThisFrame) return;

        if (Input.anyKeyDown)
        {
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    inputConsumedThisFrame = true;
                    HandleInput(key);
                    break;
                }
            }
        }
    }

    private void HandleInput(KeyCode pressedKey)
    {
        if (cleared) return;

        if (pressedKey == expectedKey)
        {
            Debug.Log($"[{myIndex}] Correct input: {pressedKey}");
            cleared = true;
            buttonpref.OnPressed(true);
        }
        else
        {
            Debug.Log($"[{myIndex}] Incorrect input: {pressedKey}");
            cleared = true;
            buttonpref.OnPressed(false);
        }

        activeHandlers.Remove(this);
    }

    private static ButtonInputHandler GetEarliestUncleared()
    {
        ButtonInputHandler earliest = null;
        float oldestTime = float.MaxValue;

        foreach (var handler in activeHandlers)
        {
            if (handler.cleared) continue;

            if (handler.spawnTime < oldestTime)
            {
                oldestTime = handler.spawnTime;
                earliest = handler;
            }
        }

        return earliest;
    }
}
