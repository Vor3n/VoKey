using UnityEngine;
using System.Collections.Generic;
using System;

public class Invoker : MonoBehaviour {
	
	Queue<Action> work; 
	public static Invoker ST;
	
	public Invoker ()
	{
		work = new Queue<Action>();
		ST = this;
	}
	
	public void Add(Action a)
	{
		lock (work)
		{
			work.Enqueue(a);
		}
	}
	
	
	void FixedUpdate()
	{
		if (work.Count > 0)
		{
			lock (work)
			{
				foreach (var a in work)
				{
					a();
				}
				work.Clear();
			}
		}
	}
}