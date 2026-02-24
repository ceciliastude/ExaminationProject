using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class IntroManager : MonoBehaviour
{

    [Header("Game Objects")]
    public CanvasGroup[] beginningUI;
    public RectTransform header;
    public RectTransform opponentScore;
    public RectTransform playerNext;
    public CinemachineVirtualCamera startCamera;

    
    
    [Header("Camera Settings")]
    public float cameraTime;
    public Vector3 newPos;
    private Vector3 initialPos;

    [Header("Header Settings")]
    public float tweenDuration;
    public float topPosY, newPosY;

    [Header("Standings Settings")]

    public float opponentTweenDuration;
    public float playerTweenDuration;
    public float opponentShownPos, opponentHiddenPos;
    public float playerShownPos, playerHiddenPos;


    void Start()
    {
        header.DOScaleY(newPosY, tweenDuration).SetUpdate(true);
        initialPos = startCamera.transform.position;
        StartCoroutine(Move(initialPos, newPos, cameraTime));
        FadeIn(0); 
    }

    
    public void FadeIn(int index)
    {
        if (!IsValid(index)) return;

        beginningUI[index].DOKill();
        beginningUI[index].DOFade(1f, 0.5f).SetUpdate(true);
    }

    public void FadeOut(int index)
    {
        if (!IsValid(index)) return;

        beginningUI[index].DOKill();
        beginningUI[index].DOFade(0f, 0.5f).SetUpdate(true);
    }

    private bool IsValid(int index)
    {
        if (index < 0 || index >= beginningUI.Length)
        {
            Debug.LogWarning("Invalid CanvasGroup index!");
            return false;
        }
        return true;
    }

    public void ShowStartStandings()
    {
        opponentScore.DOKill();
        playerNext.DOKill();

        Sequence seq = DOTween.Sequence().SetUpdate(true);

        seq.Append(
            opponentScore.DOAnchorPosY(opponentShownPos, opponentTweenDuration)
        );

        // Player starts 0.5 seconds after opponent starts
        seq.Insert(
            0.5f,
            playerNext.DOAnchorPosY(playerShownPos, playerTweenDuration)
        );
    }



    private IEnumerator Move(Vector3 startPos, Vector3 endPos, float time)
    {
        for(float t = 0; t < 1; t += Time.deltaTime / time)
        {
            startCamera.transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }
    }

}
