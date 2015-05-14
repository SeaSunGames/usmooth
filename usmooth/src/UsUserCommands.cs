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

﻿using UnityEngine;
using System.Collections;
using System;
using usmooth.common;
using System.Collections.Generic;
using System.Reflection;
	
public enum eUserCmdResult
{
	OK,
	Error,
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ConsoleHandler : Attribute
{
    public ConsoleHandler(string cmd)
    {
        Command = cmd;
    }

    public string Command;
}

public class UsUserCommands 
{
	public static UsUserCommands Instance = new UsUserCommands();

	public KeyValuePair<eUserCmdResult, string> Execute(string cmd) {
		//Debug.Log ("executing command: " + cmd);

		string[] ca = cmd.Split ();
		if (ca.Length == 0) {
			return new KeyValuePair<eUserCmdResult, string> (eUserCmdResult.Error, "empty command.");
		} else if (ca [0] == "showmesh") {
			int instID = 0;
			if (int.TryParse(ca[1], out instID)) {
				return ShowMesh(instID, true);
			}
		} else if (ca [0] == "hidemesh") {
			int instID = 0;
			if (int.TryParse(ca[1], out instID)) {
				return ShowMesh(instID, false);
			}
		}

		return new KeyValuePair<eUserCmdResult, string> (eUserCmdResult.Error, "unknown command.");
	}
	
	private KeyValuePair<eUserCmdResult, string> ShowMesh(int instID, bool visible) {
		MeshRenderer[] meshRenderers = UnityEngine.Object.FindObjectsOfType(typeof(MeshRenderer)) as MeshRenderer[];
		foreach (MeshRenderer mr in meshRenderers) {
			if (mr.gameObject.GetInstanceID() == instID) {
				mr.enabled = visible;
				return new KeyValuePair<eUserCmdResult, string> (eUserCmdResult.OK, "");
			}
		}

		return new KeyValuePair<eUserCmdResult, string> (eUserCmdResult.Error, "mesh not found. <ShowMesh>");
	}

    public void RegisterHandlers(UsvConsole console)
    {
        foreach (var method in typeof(UsUserCommands).GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            foreach (var attr in method.GetCustomAttributes(typeof(ConsoleHandler), false))
            {
                ConsoleHandler handler = attr as ConsoleHandler;
                if (handler != null)
                {
                    try
                    {
                        Delegate del = Delegate.CreateDelegate(typeof(UsvConsoleCmdHandler), this, method);
                        if (del != null)
                        {
                            console.RegisterHandler(handler.Command, (UsvConsoleCmdHandler)del);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogException(ex);                    	
                    }
                }
            }
        }
    }

    [ConsoleHandler("testlogs")]
    private bool PrintTestLogs(string[] args)
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

}
