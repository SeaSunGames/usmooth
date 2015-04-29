using System;
using System.IO;
using UnityEngine;

public class UsvLogging
{
    private UsNet _net;
    private int _assertCount = 0;
    private int _errorCount = 0;
    private int _exceptionCount = 0;

    public UsvLogging(UsNet net)
    {
        if (net == null)
        {
            Debug.Break();
            return;
        }
         
        _net = net;
    }

    public void OnLogReceived(string condition, string stackTrace, LogType type)
    {
        // do nothing if usmooth connection is not available (yet)
        if (_net == null)
            return;

        UsLogPacket pkt = new UsLogPacket();
        pkt.RealtimeSinceStartup = Time.realtimeSinceStartup;
        pkt.Time = Time.time;
        pkt.Content = condition;
        pkt.LogType = (UsLogType)type;

        switch (type)
        {
            case LogType.Assert:
                _assertCount++;
                break;
            case LogType.Error:
                _errorCount++;
                pkt.Callstack = stackTrace;
                break;
            case LogType.Exception:
                _exceptionCount++;
                pkt.Callstack = stackTrace;
                break;

            case LogType.Warning:
            case LogType.Log:
            default:
                break;
        }

        _net.SendCommand(pkt.CreatePacket());
    }
}