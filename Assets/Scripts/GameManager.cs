using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using Unity.Entities;

public class GameManager : MonoBehaviour
{
	public static GameManager gm;
	public static Transform camera;
	public static MenuController menu;
	public static TreesController trees;
	public static PlayerController player;

	public bool started;
	public int currentTreesAmount;

	private void Awake()
	{
		if (!gm)
		{
			gm = this;
		}
		else
		{
			return;
		}
	}

	private void Start()
    {
		StartCoroutine(GetCurrentTreesAmount("https://teamtrees.org/"));
	}

	private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
		{
			Cursor.lockState = CursorLockMode.None;
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);			
		}
    }

	private IEnumerator GetCurrentTreesAmount(string url)
	{
		using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
		{
			yield return webRequest.SendWebRequest();

			if (webRequest.isNetworkError)
			{
				Debug.LogWarning("Fetchin Error: " + webRequest.error);
			}
			else
			{
				var webText = webRequest.downloadHandler.text;
				var titleString = "id=\"totalTrees\"";
				var titleIndex = webText.IndexOf(titleString);
				var countString = "data-count=\"";
				var titleStart = webText.IndexOf(countString, titleIndex) + countString.Length;
				var titleLength = webText.IndexOf("\"", titleStart) - titleStart;

				currentTreesAmount = int.Parse(webText.Substring(titleStart, titleLength));

				if (menu)
				{
					menu.Fetch();
				}
			}
		}
	}
}