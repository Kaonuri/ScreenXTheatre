using UnityEngine;
using System.Collections;

public class VRUI {
	public enum Type {
		TouchpadTap,
		TouchpadHomeButton,
		GamepadButtonA,
		GamepadButtonB,
		GamepadButtonX,
		GamepadButtonY
	}
	
	public enum Action {
		Single,
		Double,
		LongPress,
		Pressed,
		Released,
		Left,
		Right,
		Up,
		Down
	}

	public static int HashValue(string value) {
		return value.GetHashCode();
	}
}
