using UnityEngine;

public class PlayCharacterVFX : MonoBehaviour
{
    [SerializeField] private GameObject vfxPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform vfxContainer;

    public void PlayParticle()
    {
        if (vfxPrefab != null && spawnPoint != null && vfxContainer != null)
        {
            GameObject vfx = Instantiate(
                vfxPrefab,
                spawnPoint.position,
                spawnPoint.rotation,
                vfxContainer   // parent here
            );
            Destroy(vfx, 2f);
            Debug.Log("Played animation");
        }
    }
}