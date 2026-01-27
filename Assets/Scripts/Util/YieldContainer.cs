using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class YieldContainer
{
    private static readonly Dictionary<float, WaitForSeconds> _waitForSecondsDict
        = new Dictionary<float, WaitForSeconds>();

    public static WaitForSeconds WaitForSeconds(float seconds)
    {
        if(!_waitForSecondsDict.ContainsKey(seconds))
            _waitForSecondsDict.Add(seconds, new WaitForSeconds(seconds));
        
        return  _waitForSecondsDict[seconds];
    }
}