using System;
using System.Collections.Generic;
using UnityEngine;

//①と②セットで書くのがUnity上の定石
//① 1つの会話データを表すクラス（インスペクターに表示するため [System.Serializable] が必須）
//以下の順でInspector上でセットになる
[Serializable]
public class SingleTalk
{
    [Header("名前・セリフ・画像")]
    public string name;

    [TextArea(2, 5)] // インスペクター上で複数行入力できるようにする属性（最小2行、最大5行）
    public string message;

    public Sprite faceImage;
}

//② 会話データのリストを保持するScriptableObject
[CreateAssetMenu(fileName = "NewTalkData", menuName = "ScriptableObjects/TalkData", order = 1)] //初期設定決める
public class TalkData : ScriptableObject
{
    public List<SingleTalk> talks = new List<SingleTalk>();
}