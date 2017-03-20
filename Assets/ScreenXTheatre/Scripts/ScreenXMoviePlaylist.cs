using UnityEngine;
using System.Collections;

public class ScreenXMoviePlaylist : MonoBehaviour {
	[System.Serializable]
	public class Item {
		public string uri;
		public bool isStreamingAsset;
		public bool isScreenX;
	}

	public Item[] items;

	public Item GetFirstItem(ref int iterator) {
		if (items.Length == 0) {
			return null;
		}
		iterator = 0;
		return items[iterator];
	}

	public Item GetNextItem(ref int iterator) {
		if (iterator >= items.Length - 1) {
			return null;
		}
		iterator++;
		return items[iterator];
	}
}
