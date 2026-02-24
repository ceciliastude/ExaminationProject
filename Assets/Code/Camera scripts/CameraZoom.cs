using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{
   public Vector3 initialOffset;
   public Vector3 newOffset;
   private Vector3 velocity = Vector3.zero;
   private AnimationCurve zoomCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);


   CinemachineOrbitalTransposer orbital;

    void Awake()
    {
        orbital = GetComponent<CinemachineVirtualCamera>()
            .GetCinemachineComponent<CinemachineOrbitalTransposer>();
    }

    public void ActivateCamera()
    {
        StartCoroutine(ZoomSequence());
    }

    private IEnumerator ZoomSequence()
    {
        yield return StartCoroutine(SmoothZoom(newOffset, 1.2f));
        yield return new WaitForSecondsRealtime(2f);
        yield return StartCoroutine(SmoothZoom(initialOffset, 1.2f));

    }

        private IEnumerator SmoothZoom(Vector3 targetOffset, float duration)
        {
            Vector3 start = orbital.m_FollowOffset;
            float t = 0f;

            while (t < 1f)
            {
                t += Time.deltaTime / duration;
                float easedT = Mathf.SmoothStep(0f, 1f, t);
                orbital.m_FollowOffset = Vector3.Lerp(start, targetOffset, easedT);
                yield return null;
            }

            orbital.m_FollowOffset = targetOffset;
        }




}
