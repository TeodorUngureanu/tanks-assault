using UnityEngine;
using System.Collections;

namespace StatePattern {
	
	public class ObserverManager : IObserver {

		private string name;
		private float observerState;
		private Transform obj;
		private ConcreteSubject subject;

		public ObserverManager(ConcreteSubject _subject, string _name, Transform _obj) {
			subject = _subject;
			name = _name;
			obj = _obj;
		}

		public void UpdateObserver() {
			observerState = subject.getSubjectState();
			EnemyManager.changedHealthStatus (obj, observerState);
		}

		public ConcreteSubject getSubject() { return subject; }
		public void setSubject(ConcreteSubject _subject) { subject = _subject; }

	}
}