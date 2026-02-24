using UnityEngine;
using Cinemachine;

public class CamRotation : MonoBehaviour
{
    public float orbitSpeed;
    public float initialX;
    private bool isActive;

    CinemachineOrbitalTransposer orbital;
    

    void Awake()
    {
        orbital = GetComponent<CinemachineVirtualCamera>()
            .GetCinemachineComponent<CinemachineOrbitalTransposer>();
        
        initialX = orbital.m_XAxis.Value;
    }

    void LateUpdate()
    {
        if (isActive)
        {
        orbital.m_XAxis.Value = (orbital.m_XAxis.Value + orbitSpeed * Time.deltaTime) % 360f;
        if (orbital.m_XAxis.Value < 0f) orbital.m_XAxis.Value += 360f;
        }

    }
    public void ActivateCamera()
    {
        isActive = true;
    }

    public void DeactivateCamera()
    {
        if (orbital == null) return;
        isActive = false;
        orbital.m_XAxis.Value = initialX; 
    }
}
