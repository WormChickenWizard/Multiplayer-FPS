using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class IPGetter : MonoBehaviour {

	// Use this for initialization
	void Start () {
#if !UNITY_WEBGL
        Text t = GetComponent<Text>();
        t.text = GetLocalIPAddress();
#endif

#if UNITY_WEBGL
        Text t = GetComponent<Text>();
        t.text = "";
#endif
    }

#if !UNITY_WEBGL
    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }
#endif
}
