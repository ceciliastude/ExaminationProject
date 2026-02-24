using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class OutroManager : MonoBehaviour
{
  [Header("Game Objects")]
    public CanvasGroup[] endUI;
    public RectTransform header;
    public RectTransform firstPlace;
    public RectTransform secondPlace;
    public CinemachineVirtualCamera endCamera;

    
    
    [Header("Camera Settings")]
    public float cameraTime;
    public Vector3 newPos;
    private Vector3 initialPos;

    [Header("Header Settings")]
    public float tweenDuration;
    public float topPosY, newPosY;

    [Header("Standings Settings")]

    public float firstTweenDuration;
    public float secondTweenDuration;
    public float firstShownPos, firstHiddenPos;
    public float secondShownPos, secondHiddenPos;


    void Start()
    {
        initialPos = endCamera.transform.position;
    }

    
    public void FadeIn(int index)
    {
        if (!IsValid(index)) return;

        endUI[index].DOKill();
        endUI[index].DOFade(1f, 0.5f).SetUpdate(true);
    }

    public void FadeOut(int index)
    {
        if (!IsValid(index)) return;

        endUI[index].DOKill();
        endUI[index].DOFade(0f, 0.5f).SetUpdate(true);
    }

    private bool IsValid(int index)
    {
        if (index < 0 || index >= endUI.Length)
        {
            Debug.LogWarning("Invalid CanvasGroup index!");
            return false;
        }
        return true;
    }

    public void ShowEndStandings()
    {
        StartCoroutine(Move(initialPos, newPos, cameraTime));
        header.DOScaleY(newPosY, tweenDuration).SetUpdate(true);
        firstPlace.DOKill();
        secondPlace.DOKill();

        Sequence seq = DOTween.Sequence().SetUpdate(true);

        seq.Append(
            firstPlace.DOAnchorPosY(firstShownPos, firstTweenDuration)
        );

        seq.Insert(
            0.5f,
            secondPlace.DOAnchorPosY(secondShownPos, secondTweenDuration)
        );
    }



    private IEnumerator Move(Vector3 startPos, Vector3 endPos, float time)
    {
        for(float t = 0; t < 1; t += Time.deltaTime / time)
        {
            endCamera.transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }
    }

}
