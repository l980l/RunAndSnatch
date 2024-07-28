using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialWarpAbility : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private Rigidbody2D rb;
    [SerializeField] private ParticleSystem spatialWarpPS;
    private WaitForSeconds waitForSeconds;

    private void Start()
    {
        waitForSeconds = new WaitForSeconds(0.1f);
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void Teleport()
    {
        SoundManager.Instance.PlaySFX(SFX.SpatialWarpSFX, Camera.main.transform.position);

        StartCoroutine(TeleportCoroutine());
    }

    private IEnumerator TeleportCoroutine()
    {
        playerMovement.onMovingSkill = true;
        rb.MovePosition(GameManager.Instance.GetMapGenerator().RandomPos(false, true));

        yield return waitForSeconds;

        playerMovement.onMovingSkill = false;
        spatialWarpPS.Play();
    }
}
