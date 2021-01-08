/*------------------------------------------------------------------------------------------
 * Copyright (C) 2018 Version R Inc
 * 
 * PoloSound.cs
 * 
 * Author : mwkang@ver-r.com
 *----------------------------------------------------------------------------------------*/

 using UnityEngine;
using System.Text;
using System.Collections;
using System.Collections.Generic;


// 사운드를 간단하게 출력하고자 할때 사용한다.
public class PoloSound : MonoBehaviour
{
    public static PoloSound Instance = null;

    void Awake()
    {
        Instance = this;
        SfxVolume = 1;
        bgmVolume = 1;
    }

    //--------------------------------------------------------------------------------

    // 루핑되는 사운드를 처리하기 위한 정보 클래스 
    protected class LoopSoundInfo
	{
		public string key;				// 랜덤하기 생성된 키 
		public AudioClip clip;			// 루핑 재생중인 사운드 클립
		public AudioSource audioSrc;	// 해당 클립을 재생중인 AudioSource
			
		public void Clear()
		{
			audioSrc.Stop();
			audioSrc.clip = null;
			audioSrc = null;
			key = null;
			clip = null;
		}
	}

	protected const int AUDIO_POOL_SIZE = 5;											// 한번에 재생할 수 있는 클립의 갯수 더 사용하면 늘어나긴 한다. 초기값이라고 보면 된다.
	protected List<AudioSource> audioPool = new List<AudioSource>(AUDIO_POOL_SIZE);
	protected List<LoopSoundInfo> loopSounds = new List<LoopSoundInfo>();				// 재생중인 loop 사운드 정보를 담아두는 컨테이너 
	protected List<LoopSoundInfo> delayedSounds = new List<LoopSoundInfo>();			// 일정시간 지연 후 재생되야하는 사운드를 담아두는 컨테이너 (이펙트 연출등이 나오고 특정 시간 후에 사운드가 재생되어야할 경우 코드를 간편하게 작성하기 위해서 이 루틴을 이용한다.)
	protected bool isMute = false;														// 음소거 동작중일때 
		
	// 음소거 설정 
	public bool Mute	
	{
		get 
		{ 
			return isMute; 
		}
			
		set 
		{ 
			isMute = value; 
				
			for(int i=0; i<audioPool.Count; i++)
			{
				audioPool[i].mute = isMute;
			}
		}
	}

    private float bgmVolume;
    private float sfxVolume;

	public float BgmVolume
    {
        get { return bgmVolume; }
        set
        {
            bgmVolume = value;
            for (int idx = 0; idx < loopSounds.Count; idx++)
                loopSounds[idx].audioSrc.volume = bgmVolume;
        }
    }
	public float SfxVolume { get { return sfxVolume; } set { sfxVolume = value; } }
		
	void Start()
	{
		// 풀을 생성하고 오디오 소스를 풀의 갯수만큼 준비한다.
		//audioPool.Clear();
		for(int i=0; i<AUDIO_POOL_SIZE; i++)
		{
			AudioSource newOne = gameObject.AddComponent<AudioSource>();
			newOne.mute = isMute;
			audioPool.Add (newOne);
		}
	}

	void Update()
	{
		// 재생이 끝난 DELAYED 사운드를 종료한다.
		ReleaseUnusedDelayedSounds();
	}

	// 풀에서 놀고 있는 AudioSource를 하나 꺼내옴
	protected AudioSource GetUnusedAudioSource()
	{
		for(int i=0; i<audioPool.Count; i++)
		{
			if(audioPool[i].isPlaying == false)
			{
				return audioPool[i];
			}
		}

		AudioSource ret = gameObject.AddComponent<AudioSource>();
		ret.mute = isMute;
		audioPool.Add(ret);
		return ret;
	}

    // 한번만 재생하는 클립 실행 
    public AudioSource PlayOnce(AudioClip clip, float vol = 1f)
    {
        AudioSource src = GetUnusedAudioSource();
        src.volume = SfxVolume == 1 ? vol : 0;
        src.PlayOneShot(clip);
        return src;
    }

    //!< 클립 중지.
    public void StopAudioSource (AudioSource src)
    {
        src.Stop();
    }

    // 루핑되는 사운드를 재생한다. 리턴되는 String은 루핑되는 사운드를 멈출때 필요하다.
    public string PlayLoop(AudioClip clip, float vol = 1f, float fadeTime = 0f)
	{
		if( clip == null )
		{
			Debug.LogError ("Sound : PlayLoop : clip not found");
			return string.Empty;
		}

		AudioSource audioSrc = GetUnusedAudioSource();
		audioSrc.volume = BgmVolume == 1 ? vol : 0;
        audioSrc.clip = clip; 
		audioSrc.loop = true;
		audioSrc.Play();
			
		// key 생성 
		StringBuilder strKey = new StringBuilder();
		strKey.Append(clip.name);
		strKey.Append(Time.time);
			
		LoopSoundInfo info = new LoopSoundInfo();
		info.key = strKey.ToString();
		info.clip = clip;
		info.audioSrc = audioSrc;
			
		loopSounds.Add (info);

		if( fadeTime > Mathf.Epsilon )
		{
			audioSrc.volume = 0f;
			StartCoroutine(_FadeInLoop (info, fadeTime));
		}

		return info.key;
	}

	// 루핑되는 사운드는 주로 BGM인 경우가 많으므로 코루틴을 이용해서 
	IEnumerator _FadeInLoop(LoopSoundInfo info, float fadeTime)
	{
		float curTime = 0f;
		float curVol = info.audioSrc.volume;
			
		while( true )
		{
            if (info == null || info.audioSrc == null)
                break;

			info.audioSrc.volume = Mathf.Lerp(0f, curVol, curTime / fadeTime);
				
			yield return 0;
				
			curTime += Time.deltaTime;
			if( curTime >= fadeTime )
			{
				info.audioSrc.volume = BgmVolume;
				break;
			}
		}
	}

	public void StopLoop(string key, float fadeTime = 0f)
	{
		if( fadeTime > Mathf.Epsilon )
		{
			StartCoroutine(_FadeOutLoop (key, fadeTime));
			return;
		}

		for(int i=0; i<loopSounds.Count; i++)
		{
			if( loopSounds[i].key == key )
			{
				loopSounds[i].Clear();
				loopSounds.RemoveAt(i);
				return;
			}
		}
	}

	public bool IsPlayLoop(string key)
	{
		for(int i=0; i<loopSounds.Count; i++)
		{
			if( loopSounds[i].key == key )
			{
				return true;
			}
		}
			
		return false;
	}

	protected IEnumerator _FadeOutLoop(string key, float fadeTime)
	{
		LoopSoundInfo info = null;

		for(int i=0; i<loopSounds.Count; i++)
		{
			if( loopSounds[i].key == key )
			{
				info = loopSounds[i];
				break;
			}
		}

		if( info == null )
		{
			//Log.Error ("_FadeOutLoop : info == null");
			yield break;
		}

		float curTime = 0f;
		float curVol = info.audioSrc.volume;

		while( true )
        {
            if (info == null || info.audioSrc)
                break;

            info.audioSrc.volume = Mathf.Lerp(curVol, 0f, curTime / fadeTime);

			yield return 0;

			curTime += Time.deltaTime;
			if( curTime >= fadeTime )
            {
                info.audioSrc.volume = 0f;
				break;
			}
		}

		StopLoop (key);
	}

	public void StopAllLoopSounds()
	{
		for(int i=0; i<loopSounds.Count; i++)
		{
			loopSounds[i].Clear();
		}

		loopSounds.Clear ();
	}

	public void PlayDelayed(AudioClip clip, float delay, float vol = 1f)
	{
		AudioSource audioSrc = GetUnusedAudioSource();
		audioSrc.volume = vol;
		audioSrc.clip = clip; 
		audioSrc.loop = false;
		audioSrc.PlayDelayed(delay);
			
		LoopSoundInfo info = new LoopSoundInfo();
		info.clip = clip;
		info.audioSrc = audioSrc;

		delayedSounds.Add (info);
	}

	public void ClearDelayedSounds()
	{
		for(int i=0; i<loopSounds.Count; i++)
		{
			loopSounds[i].Clear();
		}

		loopSounds.Clear();
	}

	protected void ReleaseUnusedDelayedSounds()
	{
		for(int i=0; i<delayedSounds.Count; i++)
		{
			if( delayedSounds[i].audioSrc.isPlaying == false )
			{
				delayedSounds[i].Clear();
				delayedSounds.RemoveAt(i);
				ReleaseUnusedDelayedSounds();
				return;
			}
		}		
	}
}

