using UnityEngine;
using System.Collections;

public class OVRIPAddressField : MonoBehaviour {
	private OVRIntField[] _byteField;

	void Awake() {
		_byteField = new OVRIntField[4];
	}

	void Start() {
		OVRIntField[] fields = gameObject.GetComponentsInChildren<OVRIntField>();
		foreach (OVRIntField field in fields) {
			if (field.gameObject.name.Equals("FirstByte")) {
				_byteField[0] = field;
			}
			else if (field.gameObject.name.Equals("SecondByte")) {
				_byteField[1] = field;
			}
			else if (field.gameObject.name.Equals("ThirdByte")) {
				_byteField[2] = field;
			}
			else if (field.gameObject.name.Equals("FourthByte")) {
				_byteField[3] = field;
			}
		}
	}

	public string address {
		get {
			return _byteField[0].currentValue.ToString() + "." +
				   _byteField[1].currentValue.ToString() + "." + 
				   _byteField[2].currentValue.ToString() + "." + 
				   _byteField[3].currentValue.ToString();
		}
		set {
			string[] bytes = value.Split(new string[] { "." }, System.StringSplitOptions.RemoveEmptyEntries);
			if (bytes.Length == 4) {
				for (int i = 0; i < 4; i++) {
					_byteField[i].currentValue = int.Parse(bytes[i]);
				}
			}
		}
	}
}
