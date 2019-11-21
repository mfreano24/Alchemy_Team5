using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using UnityEngine;

public class DatabaseConnection : MonoBehaviour {

	public void Start() {
		if (PlayerPrefs.GetInt("Levels Created", -1) == -1) {
			PlayerPrefs.SetInt("Levels Created", 0);
		}
	}

	long generateHash(string s) {
		long hashValue = 0;
		foreach (char i in s) {
			hashValue = 37 * hashValue + (int)i;
		}
		// To ensure this doesn't go over the limit
		return hashValue % 2305843009213693951;
	}

	public string UploadMap(string auth, string level, string code, string floor, string walls, string spawners) {
		if (InternetConnectionFound()) {
			WebClient client = new WebClient();

			// You may have to change the link at some point, depending on the IPv4 address.
			// Especially when this has its own, dedicated website.
			// If you don't have this setup, just change 10.251.41.137 to "localhost"
			Uri uri = new Uri("https://alchemancer.000webhostapp.com/index.php");

			// Send inputs to the PHP File.
			NameValueCollection parameters = new NameValueCollection {
				{ "MODE", "Upload" },
				{"AUTHOR", auth },
				{ "LEVEL_NAME", code},
				{"CODE", code },
				{ "FLOOR", floor},
				{ "WALLS", walls},
				{"SPAWNERS", spawners}
			};

			// Pull the data from the database if available.
			try {
				return Encoding.UTF8.GetString(client.UploadValues(uri, parameters));
			} catch (Exception e) {
				return e.ToString();
			}
		} else {
			return "No connection found!";
		}
	}

	public string DownloadMaps() {
		if (InternetConnectionFound()) {
			WebClient client = new WebClient();

			// You may have to change the link at some point, depending on the IPv4 address.
			// Especially when this has its own, dedicated website.
			// If you don't have this setup, just change 10.251.41.137 to "localhost"
			Uri uri = new Uri("https://alchemancer.000webhostapp.com/index.php");

			// Send inputs to the PHP File.
			NameValueCollection parameters = new NameValueCollection {
				{ "MODE", "Download" },
			};

			// Pull the data from the database if available.
			try {
				return Encoding.UTF8.GetString(client.UploadValues(uri, parameters));
			} catch {
				return "Failed to download maps.";
			}
		} else {
			return "Failed to download maps.";
		}
	}

	public bool InternetConnectionFound() {
		try {
			// Try sending Googld a request.
			using (var client = new WebClient())
			using (client.OpenRead("http://clients3.google.com/generate_204")) { // If there's a better test, use that instead.
				// The request was received. There's wifi.
				return true;
			}
		} catch {
			// The request failed. There's no wifi.
			return false;
		}
	}
}
