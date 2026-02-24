using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceChangeAnim : MonoBehaviour
{
    public Material faceMaterial;
    public Texture defaultTexture;
    public Texture[] newTextures;
    public float blinkInterval;
    public float blinkDuration;


    private float timer;

    void Start()
    {
        faceMaterial.mainTexture = defaultTexture;
        timer = blinkInterval;
    }

    void Update()
    {

        timer -= Time.deltaTime;

        if (timer <= 0f){
            StartCoroutine(Blink());
            timer = blinkInterval;
        }
        
    }

    IEnumerator Blink()
    {
        faceMaterial.mainTexture = newTextures[0];
        yield return new WaitForSeconds(blinkDuration);
        faceMaterial.mainTexture = defaultTexture;
    }
    
    public IEnumerator StatusFaceChange(HitResult result)
    {
        
        if (result == HitResult.Miss)
        {
            faceMaterial.mainTexture = newTextures[1];
        }
        else if (result == HitResult.Perfect)
        {
           faceMaterial.mainTexture = newTextures[2]; 
        }

        yield return new WaitForSeconds(2f);
        faceMaterial.mainTexture = defaultTexture;
    }
}
