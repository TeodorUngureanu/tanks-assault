using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Subject {

	private List<IObserver> observers = new List<IObserver>();

	public void Attach(IObserver observer) { observers.Add(observer); }
	public void Detach(IObserver observer) { observers.Remove(observer); }

	public void Notify() {
		for (int i = 0; i < observers.Count; i++) {
			observers[i].UpdateObserver();
		}
	}

}