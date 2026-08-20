using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealUser : AbstractUser {

	public RealUser(string name) {
		this.name = name;		
	}

	public override string getName() {
		return name;
	}

	public override bool isNil() {
		return false;
	}

}