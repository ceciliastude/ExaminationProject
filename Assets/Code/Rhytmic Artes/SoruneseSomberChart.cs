using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum NoteType
{
    Button,
    CircleSlider,
    BalanceSlider,
    InputCombo,
    Special
}

[System.Serializable]
public class NoteData
{
    public float beat;
    public NoteType type;

    public float approachBeats = 2f;

    public Vector2 spawnPosition = new Vector2(0f, 0f); // for UI
    public Vector3 worldPosition = Vector3.zero; // for 3D movement

    public int buttonPrefabIndex = 0;
    public bool isFinalSpin;
}

public class SoruneseSomberChart : MonoBehaviour
{
    public GameObject target3DModel;

    [Header("Chart Data")]
    public RectTransform canvasTransform;
    public RectTransform[] buttonPrefabs;
    public List<NoteData> chart = new List<NoteData>();

    private int nextIndex = 0;
    private Conductor conductor;
    private BeatMovementController mover;
    private ButtonSpawner spawner;

    void Start()
    {
        spawner = FindObjectOfType<ButtonSpawner>();
        conductor = FindObjectOfType<Conductor>();
        mover = target3DModel.GetComponent<BeatMovementController>();
    }

    void Update()
    {
        if (conductor == null || nextIndex >= chart.Count) return;

        var note = chart[nextIndex];

        if (conductor.songPositionInBeats >= note.beat)
        {
            SpawnFromChart(note);
            nextIndex++;
        }
    }

    void SpawnFromChart(NoteData note)
    {
        float targetTime = note.beat * conductor.secPerBeat;

        switch (note.type)
        {
            case NoteType.Button:
                if (spawner != null)
                    spawner.SpawnButton(note.buttonPrefabIndex, note.approachBeats, note.spawnPosition);
                break;

            case NoteType.CircleSlider:
                if (spawner != null)
                    spawner.SpawnCircleSlider(targetTime, note.isFinalSpin);
                break;
            case NoteType.BalanceSlider:
                if (spawner != null)
                    spawner.SpawnBalanceSlider(targetTime);
                break;

            case NoteType.InputCombo:
                    if (spawner != null)
                    spawner.SpawnInputCombo(targetTime);
                break;
            
            case NoteType.Special:
                    if (spawner != null)
                    spawner.SpawnSpecial(note.approachBeats, note.spawnPosition);
                break;
            
        }

        //Debug.Log($"Handled {note.type} at beat {note.beat}");
    }

}