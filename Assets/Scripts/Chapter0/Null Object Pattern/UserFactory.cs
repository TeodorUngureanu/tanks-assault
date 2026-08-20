using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserFactory {

	public static AbstractUser getUser(string name) {
		if (name != null && name != "") {
			return new RealUser (name);
		}
		return new NullUser ();
	}

}
