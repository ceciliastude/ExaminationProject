using UnityEngine;

public class CircleSpinRotation : MonoBehaviour
{
    public bool isRotating = false;

    // Called from BeatMovementController
public void ApplyRotation(Quaternion baseRot, float currentBeat, float startBeat, float endBeat)
{
    if (!isRotating) return;

    float spinT = Mathf.InverseLerp(startBeat, endBeat, currentBeat);
    float spins = 5f;
    float totalSpin = spins * 360f;

    // Linear spins for all but the last spin
    if (spinT < (spins - 1) / spins)
    {
        // simple linear interpolation for earlier spins
        float angle = Mathf.Lerp(0f, totalSpin, spinT);
        transform.rotation = baseRot * Quaternion.Euler(0f, angle, 0f);
        return;
    }

    // Last spin: smooth ease-out
    float startLastSpin = (spins - 1) / spins;
    float t = (spinT - startLastSpin) * spins; // remap last spin to 0-1

    // Ease-out using Mathf.Sin
    t = Mathf.Sin(t * Mathf.PI * 0.5f); 

    // Total spin up to last spin + eased last spin
    float angleLastSpin = ((spins - 1) / spins) * totalSpin + t * (totalSpin / spins);
    transform.rotation = baseRot * Quaternion.Euler(0f, angleLastSpin, 0f);
}


}
