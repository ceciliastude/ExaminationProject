using UnityEngine;
using System.Collections.Generic;

public class BeatMovementController : MonoBehaviour
{
    [System.Serializable]
    public class MovementTarget
    {
        public float startBeat;
        public float endBeat;
        public Vector3 startPosition;
        public Quaternion startRotation;
        public Vector3 targetPosition;
        public Quaternion targetRotation;
        public bool isRotating;
        public bool isFinalSpin;
    }

    public List<MovementTarget> movementQueue = new List<MovementTarget>();

    private Conductor conductor;
    private int currentTargetIndex = 0;

    [Range(0f, 20f)] public float beatSmoothing = 10f;
    private float smoothedBeat = 0f;

    void Start()
    {
        conductor = FindObjectOfType<Conductor>();
        if (movementQueue.Count > 0)
        {
            transform.position = movementQueue[0].startPosition;
            transform.rotation = movementQueue[0].startRotation;
        }
    }

    void LateUpdate() 
    {
        if (conductor == null || currentTargetIndex >= movementQueue.Count)
            return;

        var target = movementQueue[currentTargetIndex];


        float rawBeat = conductor.songPositionInBeats;
        smoothedBeat = Mathf.Lerp(smoothedBeat, rawBeat, Time.deltaTime * beatSmoothing);

        if (smoothedBeat < target.startBeat) return;

        float t = Mathf.InverseLerp(target.startBeat, target.endBeat, smoothedBeat);
        t = Mathf.Clamp01(t);

        // Position interpolation
        transform.position = Vector3.Lerp(target.startPosition, target.targetPosition, t);

        // Base rotation interpolation
        Quaternion baseRot = Quaternion.Slerp(target.startRotation, target.targetRotation, t);

        // Optional spinning effect
        if (target.isRotating)
        {
            float spinT = Mathf.InverseLerp(target.startBeat, target.endBeat, smoothedBeat);
            float spins = 7f;
            float totalSpin = spins * 360f;

            float lastSpinStart = (spins - 1) / spins;
            if (spinT >= lastSpinStart)
            {
                float lastSpinT = (spinT - lastSpinStart) / (1f / spins);
                lastSpinT = Mathf.Sin(lastSpinT * Mathf.PI * 0.5f);
                spinT = lastSpinStart + lastSpinT / spins;
            }

            float spinAngle = Mathf.Lerp(0f, totalSpin, spinT);
            transform.rotation = baseRot * Quaternion.Euler(0f, spinAngle, 0f);
        }
        else
        {
            transform.rotation = baseRot;
        }

        // Advance to the next target when finished
        if (t >= 1f)
        {
            currentTargetIndex++;
        }
    }

    public void AddMovement(float startBeat, float endBeat, Vector3 to, Vector3 toEuler, bool spin = false, bool finalSpin = false)
    {
        Vector3 fromPos = (movementQueue.Count > 0) 
            ? movementQueue[movementQueue.Count - 1].targetPosition 
            : transform.position;

        Quaternion fromRot = (movementQueue.Count > 0) 
            ? movementQueue[movementQueue.Count - 1].targetRotation 
            : transform.rotation;

        movementQueue.Add(new MovementTarget
        {
            startBeat = startBeat,
            endBeat = endBeat,
            startPosition = fromPos,
            targetPosition = to,
            startRotation = fromRot,
            targetRotation = Quaternion.Euler(toEuler),
            isRotating = spin,
            isFinalSpin = finalSpin
        });
    }
}
