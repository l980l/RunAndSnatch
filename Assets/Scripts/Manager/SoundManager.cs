using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using System;
using System.Collections;

public enum SFX
{
    BellSFX,
    BrightenSFX,
    ButtonSFX,
    CatHitSFX,
    ChronoTwistSFX,
    DeathSFX,
    DialogueSFX,
    EvilWizardAttackSFX,
    ExitFailSFX,
    ExitSuccessSFX,
    FerociousHowlSFX,
    FlyingEyeAttackSFX,
    GiftSFX,
    GoblinAttackSFX,
    HealingLargeSFX,
    HealingSmallSFX,
    PerchaseSFX,
    PhantomPassageSkillSFX,
    RegenUpSFX,
    SellSFX,
    ShadowVeilSFX,
    SkeletonAttackSFX,
    SkeletonShieldSFX,
    SortButtonSFX,
    SpatialWarpSFX,
    SpeedUpSFX,
    TimerSFX,
}

public class SoundManager : MonoBehaviour
{
    #region singleton
    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Init();
    }
    #endregion

    [Header("#BGM")]
    [SerializeField] private AudioClip[] bgmClip;
    [SerializeField] private GameObject BGMAudioPlayerPrefab;
    private AudioSource BGMAudioSource;

    [Header("#SFX")]
    [SerializeField] private AudioClip[] sfxClip;
    [SerializeField] private int channelCount;
    [SerializeField] private GameObject SFXAudioPlayerPrefab;
    private AudioSource[] SFXAudioSource;
    private int channelIndex;

    [Header("#LoopSFX")]
    [SerializeField] private int loopChannelCount;
    [SerializeField] private GameObject LoopSFXAudioPlayerPrefab;
    private AudioSource[] loopSFXAudioSource;
    private int loopChannelIndex;


    private void Init()
    {
        // BGM 플레이어 초기화
        BGMAudioSource = Instantiate(BGMAudioPlayerPrefab).GetComponent<AudioSource>();
        // 랜덤한 BGM 재생
        int bgmIndex = UnityEngine.Random.Range(0, bgmClip.Length);
        StartCoroutine(PlayBGM(bgmIndex));

        // SFX 플레이어 초기화
        SFXAudioSource = new AudioSource[channelCount];
        loopSFXAudioSource = new AudioSource[loopChannelCount];

        for (int i = 0; i < channelCount; i++)
        {
            SFXAudioSource[i] = Instantiate(SFXAudioPlayerPrefab).GetComponent<AudioSource>();
        }

        for (int i = 0; i < loopChannelCount; i++)
        {
            loopSFXAudioSource[i] = Instantiate(LoopSFXAudioPlayerPrefab).GetComponent<AudioSource>();
        }
    }

    private void OnDestroy()
    {
        // 씬이 언로드될 때 호출되는 메서드
        foreach (var clip in bgmClip)
        {
            if (clip.loadState == AudioDataLoadState.Loaded)
            {
                clip.UnloadAudioData();
            }
        }
    }

    private IEnumerator PlayBGM(int _bgmIndex)
    {
        // 오디오 데이터를 비동기적으로 로드
        bgmClip[_bgmIndex].LoadAudioData();
        // 오디오 데이터가 로드될 때까지 대기
        while (!bgmClip[_bgmIndex].loadState.Equals(AudioDataLoadState.Loaded))
        {
            yield return null;
        }
        // 오디오 데이터가 로드되면 재생
        BGMAudioSource.clip = bgmClip[_bgmIndex];
        BGMAudioSource.Play();
    }

    public void PlaySFX(SFX _SFX, Vector3 _Pos)
    {
        for(int i = 0; i<channelCount;++i)
        {
            int loopIndex = (i + channelIndex) % channelCount;

            if (SFXAudioSource[loopIndex].isPlaying)
                continue;

            channelIndex = loopIndex;
            SFXAudioSource[loopIndex].transform.position = _Pos;
            SFXAudioSource[loopIndex].clip = sfxClip[(int)_SFX];
            SFXAudioSource[loopIndex].Play();
            break;
        }
    }

    public int PlayLoopSFX(SFX _SFX, Vector3 _Pos)
    {
        for (int i = 0; i < loopChannelCount; ++i)
        {
            int loopIndex = (i + loopChannelIndex) % loopChannelCount;

            if (loopSFXAudioSource[loopIndex].isPlaying)
                continue;

            loopChannelIndex = loopIndex;
            loopSFXAudioSource[loopIndex].transform.position = _Pos;
            loopSFXAudioSource[loopIndex].clip = sfxClip[(int)_SFX];
            loopSFXAudioSource[loopIndex].Play();
            return loopIndex;
        }
        return -1;
    }

    public void StopLoopSFX(int Index)
    {
        loopSFXAudioSource[Index].Stop();
    }

    public void SetLoopSFXPos(int Index, Vector3 _Pos)
    {
        loopSFXAudioSource[Index].transform.position = _Pos;
    }
}
