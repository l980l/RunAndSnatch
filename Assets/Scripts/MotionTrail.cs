using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class MotionTrail : MonoBehaviour
{
    [SerializeField] private float ScanTerm;
    [SerializeField] private float TrailLifeTime;

    private int TrailPoolCount;
    private SpriteRenderer spriteRenderer;
    private List<GameObject> Trails;
    private List<float> TrailStartTimes;
    private float FlownTime;
    private bool isMotionTrail;
    private int TrailIndex;


    private void Awake()
    {
        TrailPoolCount = Mathf.FloorToInt(TrailLifeTime / ScanTerm) + 2;    // 오브젝트 풀의 크기는 딱 맞게 만든다면 TrailLifeTime / ScanTerm 이다. 다만, 0으로 딱 떨어지지 않는 경우 1개가 더 필요하고, 넉넉히 2개 더해준다.
        spriteRenderer = GetComponent<SpriteRenderer>();
        Trails = new List<GameObject>();
        TrailStartTimes = new List<float>();
        for (int i = 0; i < TrailPoolCount; i++)
        {
            GameObject Trail = new GameObject();
            SpriteRenderer SR = Trail.AddComponent<SpriteRenderer>();
            Trail.SetActive(false);
            Trails.Add(Trail);
            TrailStartTimes.Add(0f);
        }
    }

    private void Update()
    {
        if(isMotionTrail)
        {
            FlownTime += Time.deltaTime;
            if (FlownTime > ScanTerm)
            {
                FlownTime = 0;
                Present();
            }
        }

        SetTrailAlphaAndActive();
    }
   
    private void Present()
    {
        Trails[TrailIndex].SetActive(true);
        Trails[TrailIndex].transform.position = transform.position;
        Trails[TrailIndex].GetComponent<SpriteRenderer>().sprite = spriteRenderer.sprite;
        Trails[TrailIndex].GetComponent<SpriteRenderer>().flipX = spriteRenderer.flipX;
        TrailStartTimes[TrailIndex] = Time.realtimeSinceStartup;

        TrailIndex++;
        if (TrailIndex >= TrailPoolCount)
            TrailIndex = 0;
    }

    public void MotionTrailStart()
    {
        isMotionTrail = true;
        FlownTime = 0;
    }

    public void MotionTrailEnd()
    {
        isMotionTrail = false;
    }

    private void SetTrailAlphaAndActive()
    {
        // 알파값 조절 및 비활성화
        for (int i = 0; i < TrailPoolCount; i++)
        {
            if (Trails[i].active)
            {
                float AfterBeginTime = Time.realtimeSinceStartup - TrailStartTimes[i];
                if (AfterBeginTime > TrailLifeTime)
                {
                    AfterBeginTime = 0f;
                    Trails[i].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                    Trails[i].SetActive(false);
                }
                else
                {
                    float Alpha = (TrailLifeTime - AfterBeginTime) / TrailLifeTime;
                    Trails[i].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Alpha);
                }
            }
        }
    }
}
