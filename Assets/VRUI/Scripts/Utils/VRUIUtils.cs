using UnityEngine;
using System.Collections;

public class VRUIUtils {
	public static string MultilineString(string text, int width, GUIStyle style) {
		string line = "";
		string result = "";
		for (int i = 0; i < text.Length; i++) {
			if (string.IsNullOrEmpty(line)) {
				line += text.Substring(i, 1);
			}
			else {
				if (style.CalcSize(new GUIContent(line + text.Substring(i, 1))).x >= width) {
					result += line + "\n";
					line = text.Substring(i, 1);
				}	
				else {
					line += text.Substring(i, 1);
				}
			}
		}
		if (string.IsNullOrEmpty(line) == false) {
			result += line;
		}
		return result;
	}
}
