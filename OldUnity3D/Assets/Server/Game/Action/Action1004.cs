﻿using System;
using UnityEngine;

public class Action1004 : GameAction
{
    private ActionResult actionResult;

    public Action1004()
        : base(1004)
    {
    }

    protected override void SendParameter(NetWriter writer, ActionParam actionParam)
    {
        string pwd = new DESAlgorithmNew().EncodePwd(GameSetting.Instance.Password, GameSetting.ClientPasswordKey);
        //default url param
        writer.writeInt32("MobileType", GameSetting.Instance.MobileType);
        writer.writeString("Pid", GameSetting.Instance.Pid);
        writer.writeString("Pwd", pwd);
        writer.writeString("DeviceID", GameSetting.Instance.DeviceID);
        writer.writeInt32("ScreenX", GameSetting.Instance.ScreenX);
        writer.writeInt32("ScreenY", GameSetting.Instance.ScreenY);
        writer.writeString("RetailID", GameSetting.Instance.RetailID);
        writer.writeInt32("GameType", GameSetting.Instance.GameType);
        writer.writeInt32("ServerID", GameSetting.Instance.ServerID);
        writer.writeString("RetailUser", "");
        writer.writeString("ClientAppVersion", GameSetting.Instance.ClientAppVersion);
    }

    protected override void DecodePackage(NetReader reader)
    {
        actionResult = new ActionResult();
        actionResult["SessionID"] = reader.readString();
        actionResult["UserID"] = reader.readString();
        actionResult["UserType"] = reader.getInt();
        actionResult["LoginTime"] = reader.readString();
        actionResult["GuideID"] = reader.getInt();
        actionResult["PassportId"] = reader.readString();
        actionResult["RefeshToken"] = reader.readString();
        actionResult["QihooUserID"] = reader.readString();
        actionResult["Scope"] = reader.readString();

        NetWriter.setUserID(ulong.Parse(actionResult["UserID"].ToString()));
        NetWriter.setSessionID(actionResult.Get<string>("SessionID"));
        Net.Instance.ReBuildHearbeat();
    }

    public override ActionResult GetResponseData()
    {
        return actionResult;
    }
}
