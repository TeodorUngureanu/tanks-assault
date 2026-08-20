using UnityEngine;
using System.Collections;

public class ConcreteSubject : Subject {
	private float subjectState;

	public float getSubjectState() { return subjectState; }
	public void setSubjectState(float _subjectState) { subjectState = _subjectState; }
}