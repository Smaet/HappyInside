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
	public AudioClip tick;
	public AudioClip capture;
	public AudioClip recordStart;
	public AudioClip recordEnd;
	public AudioClip error;
    public AudioClip connect;
    public AudioClip disconnect;
    public AudioClip popTouch;
    public AudioClip popMessage;

	protected override void Awake()
	{
		base.Awake();
	}

    public void PlayButton()
	{
		PoloSound.Instance.PlayOnce(button);
	}

	public void PlayTick()
	{
        PoloSound.Instance.PlayOnce(tick);
	}

	public void PlayCapture()
	{
        PoloSound.Instance.PlayOnce(capture);
	}

	public void PlayRecordStart()
	{
        PoloSound.Instance.PlayOnce(recordStart);
	}

	public void PlayRecordEnd()
	{
        PoloSound.Instance.PlayOnce(recordEnd);
	}

	public void PlayError()
	{
        PoloSound.Instance.PlayOnce(error);
	}

	public void PlayConnect()
	{
        PoloSound.Instance.PlayOnce(connect, 0.5f);
	}

    public void PlayDisconnect()
    {
        PoloSound.Instance.PlayOnce(disconnect, 0.5f);
    }

    public void PlayPopTouch()
    {
        PoloSound.Instance.PlayOnce(popTouch);
    }

    public void PlayPopMessage()
    {
        PoloSound.Instance.PlayOnce(popMessage);
    }
}
