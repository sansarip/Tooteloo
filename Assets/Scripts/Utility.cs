using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility {
	
	public static string toString(char[] c) {
		string s = "";
		for (int i = 0; i < c.Length; i++) {
			if (c[i] == '\0') {
				break;
			}
			s += c[i];
		}
		return s;
	}
}
