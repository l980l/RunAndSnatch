using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialWarpAbility : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private Rigidbody2D rb;
    [SerializeField] private ParticleSystem spatialWarpPS;

    private void Start()
    {
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
        rb.MovePosition(GameManager.Instance.GetMapGenerator().RandomPos(false));

        yield return new WaitForSeconds(0.1f);

        playerMovement.onMovingSkill = false;
        spatialWarpPS.Play();
    }
}
