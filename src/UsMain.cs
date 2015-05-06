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
using System;

public class UsMain : MonoBehaviour {

    public bool LogIntoFile = false;

	private ushort _serverPort = 5555;
	private long _currentTimeInMilliseconds = 0;
	private long _tickNetLast = 0;
	private long _tickNetInterval = 200;

	void Start () 
    {
		Application.runInBackground = true;

		UsNet.Instance = new UsNet(_serverPort);
        UsvStart.Instance = new UsvStart(new UsvStartParams() { net = UsNet.Instance, logIntoFile = LogIntoFile });
        
		UsMainHandlers.Instance.RegisterHandlers (UsNet.Instance.CmdExecutor);
        UsUserCommands.Instance.RegisterHandlers(UsvStart.Instance.Console);
	}
	
	void Update () {
		_currentTimeInMilliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

		if (_currentTimeInMilliseconds - _tickNetLast > _tickNetInterval)
		{
			if (UsNet.Instance != null) {
				UsNet.Instance.Update ();
			}

			_tickNetLast = _currentTimeInMilliseconds;
		}
	}

    void OnApplicationQuit()
    {
        UsvStart.Instance.Logging.Dispose();
        UsNet.Instance.Dispose();
    }
}
