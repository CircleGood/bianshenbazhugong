﻿using System.Collections.Generic;
using GameRanking.Pack;
using UnityEngine;

public class TestGUI : MonoBehaviour
{
    private List<RankData> rankList;
    private ActionParam actionParam;
    //todo 启用自定的结构
    bool useCustomAction = false;

    // Use this for initialization
    void Start()
    {
        if (useCustomAction)
        {
            Net.Instance.HeadFormater = new CustomHeadFormater();
            Request1001Pack requestPack = new Request1001Pack() { PageIndex = 1, PageSize = 20 };
            actionParam = new ActionParam(requestPack);
        }
        else
        {
            actionParam = new ActionParam();
            actionParam["str"] = "1";
            actionParam["PageSize"] = "20";
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 500, 100));
        GUILayout.BeginHorizontal();
        // Now create any Controls you like, and they will be displayed with the custom Skin
        if (GUILayout.Button("Get ranking for Http"))
        {
            //NetWriter.SetUrl("http://127.0.0.1:8036/service.aspx");
            NetWriter.SetUrl("http://ph.scutgame.com/service.aspx");
            Net.Instance.Send((int)ActionType.RankSelect, OnRankingCallback, actionParam);
        }
        GUILayout.Space(20);
        // Any Controls created here will use the default Skin and not the custom Skin
        if (GUILayout.Button("Get ranking for Socket"))
        {
            NetWriter.SetUrl("127.0.0.1:9001");
            Net.Instance.Send((int)ActionType.RankSelect, OnRankingCallback, actionParam);
        }

        if (GUILayout.Button("Hello world!"))
        {
            NetWriter.SetUrl("127.0.0.1:9001");
            ActionParam action = new ActionParam();
            action["str"] = Time.time.ToString();
            Net.Instance.Send(100, OnHello, action);
        }
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
        OnRankGUI();
    }

    

    private void OnRankGUI()
    {
        if (rankList == null) return;

        GUILayout.BeginArea(new Rect(20, 100, 200, 200));
        GUILayout.BeginHorizontal();
        GUILayout.Label("UserName", GUILayout.Width(100));
        GUILayout.Label("Score", GUILayout.Width(100));
        GUILayout.EndHorizontal();

        foreach (var data in rankList)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(data.UserName, GUILayout.Width(100));
            GUILayout.Label(data.Score.ToString(), GUILayout.Width(100));
            GUILayout.EndHorizontal();
        }
        GUILayout.EndArea();
    }

    void OnHello(ActionResult actionResult)
    {
        Debug.LogError(actionResult["content"]);
    }

    void OnRankingCallback(ActionResult actionResult)
    {
        Response1001Pack pack = actionResult.GetValue<Response1001Pack>();
        if (pack == null)
        {
            return;
        }
        rankList = pack.Items;
    }
}