using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
	public GameObject mainMenu;
	public GameObject loadingMenu;
	public Text treesAmountText;
	public Text FPSText;
	public InputField treesAmountField;
	public InputField treesSpacingField;
	public int treesAmount = 1;

	private float deltaTime;

	private void Awake()
	{
		GameManager.menu = this;

		if (mainMenu)
		{
			mainMenu.SetActive(true);
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.U))
		{
			treesAmountText.gameObject.SetActive(!treesAmountText.gameObject.activeInHierarchy);
		}

		if (Input.GetKeyDown(KeyCode.I))
		{
			FPSText.gameObject.SetActive(!FPSText.gameObject.activeInHierarchy);
		}

		if (FPSText.gameObject.activeInHierarchy)
		{
			deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
			FPSText.text = Mathf.RoundToInt(1f / deltaTime).ToString();
		}
	}

	public void Plant()
	{
		StartCoroutine(ManagePlanting());
	}

	private IEnumerator ManagePlanting()
	{
		loadingMenu.SetActive(true);
		mainMenu.SetActive(false);
		yield return new WaitForSeconds(.1f);

		if (GameManager.trees)
		{
			GameManager.trees.PlantTrees(treesAmount);
		}

		yield return new WaitForSeconds(.1f);
		loadingMenu.SetActive(false);
		treesAmountText.gameObject.SetActive(true);

		Cursor.lockState = CursorLockMode.Locked;
		GameManager.gm.started = true;
	}

	public void Fetch()
	{
		treesAmount = GameManager.gm.currentTreesAmount;
		RefreshTreesAmount();
	}

	public void Default()
	{
		GameManager.trees.spacing = int.Parse(treesSpacingField.text);
		RefreshTreesSpacing();
	}

	public void Donate()
	{
		Application.OpenURL("https://teamtrees.org/");
	}

	public void RefreshTreesAmount()
	{
		treesAmountField.text = treesAmount.ToString();
		treesAmountText.text = treesAmount.ToString("TREES PLANTED: " + 0);
	}

	public void RefreshTreesSpacing()
	{
		treesSpacingField.text = GameManager.trees.spacing.ToString();
	}

	public void UpdateTreesAmount()
	{
		treesAmount = int.Parse(treesAmountField.text);
		RefreshTreesAmount();
	}

	public void UpdateSpacing()
	{
		if (GameManager.trees){
			GameManager.trees.spacing = int.Parse(treesSpacingField.text);
		}
	}
}