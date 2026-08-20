using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullUser : AbstractUser {

	public override string getName() {
		return "Unknown";
	}

	public override bool isNil() {
		return true;
	}
}
