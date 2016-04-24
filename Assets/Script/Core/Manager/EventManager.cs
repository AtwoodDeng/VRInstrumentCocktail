using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Events
{
	public delegate void EventHandler(Message msg);

	public static event EventHandler BeginGame;
	public static void FireBeginGame(Message msg){if ( BeginGame != null ) BeginGame(msg) ; }

}
	
public class Message : EventArgs
{
	public Message(object _this){m_sender = _this;}
	EventDefine m_eventName;
	object m_sender;
	Dictionary<string,object> m_dict = new Dictionary<string,object>();

	public EventDefine eventName {get{return m_eventName;}}
	public object sender{get{return m_sender;}}

	public void SetEventName(EventDefine name){
		m_eventName = name;
	}
	public void SetSender(object sender){
		m_sender = sender;
	}
	public void AddMessage(string key, object val)
	{
		m_dict.Add(key, val);
	}
	public object GetMessage(string key)
	{
		object res;
		m_dict.TryGetValue(key , out res);
		return res;
	}
	public bool ContainMessage(string key)
	{
		return m_dict.ContainsKey(key);
	}
}