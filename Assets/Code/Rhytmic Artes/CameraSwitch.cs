using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitch : MonoBehaviour
{
    [System.Serializable]
    public class CameraTarget
    {
        public float startBeat;
        public float endBeat;
        public CinemachineVirtualCamera activeCamera;
        public bool isRotating;
    }

    public List<CinemachineVirtualCamera> cameras = new List<CinemachineVirtualCamera>();
    public List<CameraTarget> cameraQueue = new List<CameraTarget>();

    private Conductor conductor;
    private int currentTargetIndex = 0;

    void Start()
    {
        conductor = FindObjectOfType<Conductor>();

        foreach (var cam in cameras)
        {
            cam.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (conductor == null || currentTargetIndex >= cameraQueue.Count)
            return;

        var target = cameraQueue[currentTargetIndex];
        float beat = conductor.songPositionInBeats;

        // Activate the camera if within its beat range
        if (beat >= target.startBeat && beat < target.endBeat)
        {
            ActivateCamera(target.activeCamera);
        }
        else if (beat >= target.endBeat)
        {
            currentTargetIndex++;
        }

        
        foreach (var cam in cameras)
{
    CamRotation camRot = cam.GetComponent<CamRotation>();
    if (camRot == null)
        continue; 

    if (cam.gameObject.activeSelf && target.isRotating)
        camRot.ActivateCamera();
    else
        camRot.DeactivateCamera();
        

}

    }

    private void ActivateCamera(CinemachineVirtualCamera cam)
    {
        foreach (var c in cameras)
        {
            c.gameObject.SetActive(false);
        }

        cam.gameObject.SetActive(true);

        CameraZoom camZoom = cam.GetComponent<CameraZoom>();
        if (camZoom != null)
        {
            camZoom.ActivateCamera();
        }
    }

    public void AddCamera(float startBeat, float endBeat, CinemachineVirtualCamera camera, bool isRotating = false)
    {
        cameraQueue.Add(new CameraTarget
        {
            startBeat = startBeat,
            endBeat = endBeat,
            activeCamera = camera,
            isRotating = isRotating
        });
    }
}
