/*------------------------------------------------------------------------------------------
 * Copyright (C) 2018 Version R Inc
 * 
 * PoloSFX.cs
 * 
 * Author : mwkang@ver-r.com
 *----------------------------------------------------------------------------------------*/
using UnityEngine;
using System.Collections;

public class PoloSFX : SimpleSingleton<PoloSFX>
{
    public AudioClip button;
	public AudioClip bgm_Home;
	public AudioClip bgm_Agit;
	public AudioClip bgm_Department;
	public AudioClip bgm_GrandFather;
	public AudioClip curtain_Open;
	public AudioClip curtain_Close;
	public AudioClip cardSlice;
    public AudioClip hackerUp;
	public AudioClip miss;

	private string playingBGM;

	protected override void Awake()
	{
		base.Awake();
		playingBGM = "";
	}

    public void PlayButton()
	{
		PoloSound.Instance.PlayOnce(button, 0.5f);
	}

	public void PlayAgitBGM()
    {
		if(playingBGM != "")
        {
			PoloSound.Instance.StopLoop(playingBGM);

		}
		playingBGM = PoloSound.Instance.PlayLoop(bgm_Agit, 1f);
	}

	public void PlayDepartmentBGM()
	{
		if (playingBGM != "")
		{
			PoloSound.Instance.StopLoop(playingBGM);

		}
		playingBGM = PoloSound.Instance.PlayLoop(bgm_Department, 1f);
	}

	public void PlayGrandFatherBGM()
	{
		if (playingBGM != "")
		{
			PoloSound.Instance.StopLoop(playingBGM);

		}
		playingBGM = PoloSound.Instance.PlayLoop(bgm_GrandFather, 0.8f);
	}

	public void PlayHomeBGM()
	{
		if (playingBGM != "")
		{
			PoloSound.Instance.StopLoop(playingBGM);

		}
		playingBGM = PoloSound.Instance.PlayLoop(bgm_Home, 1f);
	}


	public void Play_CurtainOpen()
	{
		PoloSound.Instance.PlayOnce(curtain_Open);
	}
	public void Play_CurtainClose()
	{
		PoloSound.Instance.PlayOnce(curtain_Close);
	}

	public void Play_CardSlice()
	{
		PoloSound.Instance.PlayOnce(cardSlice);
	}

	public void Play_HackerUp()
    {
		PoloSound.Instance.PlayOnce(hackerUp);

	}

	public void Play_Miss()
	{
		PoloSound.Instance.PlayOnce(miss);

	}

}
