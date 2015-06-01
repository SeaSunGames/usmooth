/*!lic_info

The MIT License (MIT)

Copyright (c) 2015 SeaSunOpenSource

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

*/

﻿using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ConsoleHandler : Attribute
{
    public ConsoleHandler(string cmd)
    {
        Command = cmd;
    }

    public string Command;
}

public class UsvConsoleCmds 
{
	public static UsvConsoleCmds Instance;

    [ConsoleHandler("showmesh")]
    public bool ShowMesh(string[] args)
    {
        return SetMeshVisible(args[1], true);
    }

    [ConsoleHandler("hidemesh")]
    public bool HideMesh(string[] args)
    {
        return SetMeshVisible(args[1], false);
    }

    private bool SetMeshVisible(string strInstID, bool visible)
    {
        int instID = 0;
        if (!int.TryParse(strInstID, out instID))
            return false;

        MeshRenderer[] meshRenderers = UnityEngine.Object.FindObjectsOfType(typeof(MeshRenderer)) as MeshRenderer[];
        foreach (MeshRenderer mr in meshRenderers)
        {
            if (mr.gameObject.GetInstanceID() == instID)
            {
                mr.enabled = visible;
                return true;
            }
        }

        return false;
    }

    [ConsoleHandler("testlogs")]
    public bool PrintTestLogs(string[] args)
    {
        Debug.Log("A typical line of logging.");
        Debug.Log("Another line.");
        Debug.LogWarning("An ordinary warning.");
        Debug.LogError("An ordinary error.");

        try
        {
            throw new ApplicationException("A user-thrown exception.");
        }
        catch (ApplicationException ex)
        {
            Debug.LogException(ex);
        }

        return true;
    }

    [ConsoleHandler("toggle")]
    public bool ToggleSwitch(string[] args)
    {
        try
        {
            GameInterface.Instance.ToggleSwitch(args[1], int.Parse(args[2]) != 0);
        }
        catch (Exception ex)
        {
            Log.Exception(ex);
            throw;
        }
        return true;
    }

    [ConsoleHandler("slide")]
    public bool SlideChanged(string[] args)
    {
        try
        {
            GameInterface.Instance.ChangePercentage(args[1], double.Parse(args[2]));
        }
        catch (Exception ex)
        {
            Log.Exception(ex);
            throw;
        }
        return true;
    }

    [ConsoleHandler("query_effect_list")]
    public bool QueryEffectListTriggered(string[] args)
    {
        try
        {
            if (args.Length != 1)
            {
                Log.Error("Command 'query_effect_list' parameter count mismatched. (%d expected, %d got)", 1, args.Length);
                return false;
            }

            UsEffectNotifier.Instance.PostEvent_QueryEffectList();
        }
        catch (Exception ex)
        {
            Log.Exception(ex);
            throw;
        }
        return true;
    }

    [ConsoleHandler("run_effect_stress")]
    public bool EffectStressTestTriggered(string[] args)
    {
        try
        {
            if (args.Length != 3)
            {
                Log.Error("Command 'effect_stress' parameter count mismatched. (%d expected, %d got)", 3, args.Length);
                return false;
            }

            string effectName = args[1];
            int effectCount = int.Parse(args[2]);

            UsEffectNotifier.Instance.PostEvent_RunEffectStressTest(effectName, effectCount);
        }
        catch (Exception ex)
        {
            Log.Exception(ex);
            throw;
        }
        return true;
    }
}
