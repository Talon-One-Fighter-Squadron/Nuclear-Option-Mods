using System.Collections.Generic;
using System.IO;
using BepInEx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[BepInPlugin("com.yourname.mainmenureplacer", "MainMenu Replacer", "1.4.0")]
public class MainMenuReplacer : BaseUnityPlugin
{
	private string customImagePath;

	private Sprite customSprite;

	private void Awake()
	{
		string pluginPath = Paths.PluginPath;
		customImagePath = Path.Combine(pluginPath, "custom_menu.png");
		((BaseUnityPlugin)this).Logger.LogInfo((object)("[MainMenuReplacer] Looking for custom_menu.png at: " + customImagePath));
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name != "MainMenu")
		{
			return;
		}
		((BaseUnityPlugin)this).Logger.LogInfo((object)"[MainMenuReplacer] MainMenu scene loaded. Scanning for background UI...");
		if (customSprite == null)
		{
			if (!File.Exists(customImagePath))
			{
				((BaseUnityPlugin)this).Logger.LogWarning((object)"[MainMenuReplacer] custom_menu.png not found; cannot replace background.");
				return;
			}
			byte[] data = File.ReadAllBytes(customImagePath);
			Texture2D texture2D = new Texture2D(2, 2);
			if (!texture2D.LoadImage(data))
			{
				((BaseUnityPlugin)this).Logger.LogError((object)"[MainMenuReplacer] Failed to import custom_menu.png into Texture2D.");
				return;
			}
			Rect rect = new Rect(0f, 0f, texture2D.width, texture2D.height);
			Vector2 pivot = new Vector2(0.5f, 0.5f);
			customSprite = Sprite.Create(texture2D, rect, pivot);
			((BaseUnityPlugin)this).Logger.LogInfo((object)"[MainMenuReplacer] custom_menu.png loaded as Sprite successfully.");
		}
		Image[] array = Resources.FindObjectsOfTypeAll<Image>();
		((BaseUnityPlugin)this).Logger.LogInfo((object)$"[MainMenuReplacer] Found {array.Length} Image components overall.");
		List<Image> list = new List<Image>();
		Image[] array2 = array;
		foreach (Image image in array2)
		{
			if (!(image.gameObject.scene.name != "MainMenu"))
			{
				RectTransform component = image.GetComponent<RectTransform>();
				if (!(component == null) && IsFullScreen(component))
				{
					list.Add(image);
					((BaseUnityPlugin)this).Logger.LogInfo((object)$"[MainMenuReplacer] Full-screen Image: '{image.gameObject.name}', siblingIndex={component.GetSiblingIndex()}");
				}
			}
		}
		Image image2 = null;
		foreach (Image item in list)
		{
			if (item.gameObject.name.ToLower().Contains("background"))
			{
				image2 = item;
				((BaseUnityPlugin)this).Logger.LogInfo((object)("[MainMenuReplacer] Selecting by name: '" + item.gameObject.name + "' (contains \"background\")."));
				break;
			}
		}
		if (image2 == null && list.Count > 0)
		{
			int num = int.MaxValue;
			foreach (Image item2 in list)
			{
				int siblingIndex = item2.GetComponent<RectTransform>().GetSiblingIndex();
				if (siblingIndex < num)
				{
					num = siblingIndex;
					image2 = item2;
				}
			}
			if (image2 != null)
			{
				((BaseUnityPlugin)this).Logger.LogInfo((object)("[MainMenuReplacer] No \"background\" by name found; selecting lowest siblingIndex: '" + image2.gameObject.name + "'."));
			}
		}
		if (image2 != null)
		{
			image2.sprite = customSprite;
			image2.type = Image.Type.Simple;
			image2.preserveAspect = true;
			((BaseUnityPlugin)this).Logger.LogInfo((object)("[MainMenuReplacer] Replaced Image '" + image2.gameObject.name + "' with custom_menu.png."));
		}
		else
		{
			((BaseUnityPlugin)this).Logger.LogWarning((object)"[MainMenuReplacer] No full-screen Image found to replace. Check if the background uses RawImage or nonstandard anchors.");
		}
	}

	private void OnDestroy()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	private static bool IsFullScreen(RectTransform rt)
	{
		return rt.anchorMin == Vector2.zero && rt.anchorMax == Vector2.one;
	}
}
