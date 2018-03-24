using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;

/// <summary>
/// 音の管理機能を提供します。
/// </summary>
public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// BGMのボリューム初期値
    /// </summary>
    public const float DefaultBgmVolume = 0.5f;

    /// <summary>
    /// SEのボリューム初期値
    /// </summary>
    public const float DefaultSeVolume = 0.08f;

    /// <summary>
    /// BGMのリソースパス
    /// </summary>
    private const string BgmPath = "Audios/Bgm";

    /// <summary>
    /// SEのリソースパス
    /// </summary>
    private const string SePath = "Audios/Se";

    /// <summary>
    /// SEの優先度初期値
    /// </summary>
    private const int DefaultSePriority = 250;

    /// <summary>
    /// BGM用のオーディオソース
    /// </summary>
    private AudioSource BgmSource { get; set; }

    /// <summary>
    /// SE用のオーディオソース
    /// </summary>
    private List<AudioSource> SeSources { get; set; }

    /// <summary>
    /// SE用のオーディオソース数
    /// </summary>
    private const int SeSourceCount = 10;

    /// <summary>
    /// BGMのオーディオクリップ
    /// </summary>
    private Dictionary<string, AudioClip> BgmDic { get; set; }

    /// <summary>
    /// SEのオーディオクリップ
    /// </summary>
    private Dictionary<string, AudioClip> SeDic { get; set; }

    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        //オーディオリスナーおよびオーディオソースをSE+1(BGMの分)作成
        for (int i = 0; i < SeSourceCount + 1; i++)
        {
            gameObject.AddComponent<AudioSource>();
        }

        //作成したオーディオソースを取得して各変数に設定、ボリュームも設定
        AudioSource[] audioSourceArray = GetComponents<AudioSource>();
        SeSources = new List<AudioSource>();

        for (int i = 0; i < audioSourceArray.Length; i++)
        {
            if (i == 0)
            {
                audioSourceArray[i].playOnAwake = true;
                audioSourceArray[i].loop = true;
                BgmSource = audioSourceArray[i];
                BgmSource.volume = DefaultBgmVolume;
            }
            else
            {
                SeSources.Add(audioSourceArray[i]);
                audioSourceArray[i].playOnAwake = true;
                audioSourceArray[i].volume = DefaultSeVolume;
                audioSourceArray[i].priority = DefaultSePriority;
            }

        }

        //リソースフォルダから全SE&BGMのファイルを読み込みセット
        BgmDic = Resources.LoadAll<AudioClip>(BgmPath).ToDictionary(r => r.name);
        SeDic = Resources.LoadAll<AudioClip>(SePath).ToDictionary(r => r.name);
    }

    /// <summary>
    /// 指定したファイル名のSEを流します。
    /// </summary>
    /// <param name="seName">SE名</param>
    public void PlaySE(string seName)
    {
        if (!SeDic.ContainsKey(seName))
        {
            throw new ArgumentException("ファイルが存在しません:" + seName);
        }

        foreach (AudioSource seSource in SeSources)
        {
            if (!seSource.isPlaying)
            {
                seSource.PlayOneShot(SeDic[seName] as AudioClip);
                return;
            }
        }
    }

    /// <summary>
    /// 指定したファイル名のBGMを流します。
    /// </summary>
    /// <param name="bgmName">BGM名</param>
    public void PlayBGM(string bgmName)
    {
        if (!BgmDic.ContainsKey(bgmName))
        {
            throw new ArgumentException("ファイルが存在しません:" + bgmName);
        }

        BgmSource.clip = BgmDic[bgmName] as AudioClip;
        BgmSource.Play();
    }

    /// <summary>
    /// BGMをすぐに止めます。
    /// </summary>
    public void StopBGM()
    {
        BgmSource.Stop();
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
    }

    /// <summary>
    /// BGMの音量を変えます。
    /// </summary>
    /// <param name="volume">音量</param>
    public void ChangeBgmVolume(float volume)
    {
        BgmSource.volume = volume;
    }

    /// <summary>
    /// SEの音量を変えます。
    /// </summary>
    /// <param name="volume">音量</param>
    public void ChangeSeVolume(float volume)
    {
        foreach (AudioSource source in SeSources)
        {
            source.volume = volume;
        }
    }
}