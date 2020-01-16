using UnityEditor;
using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.U2D;
using UnityEditor.U2D;
using System;

public class AssetProcessor : AssetPostprocessor
{
	private static TextureImporterPlatformSettings iosSetting;
	private static TextureImporterPlatformSettings androidSetting;

    public AssetProcessor()
	{
		//ios的设置
		iosSetting = new TextureImporterPlatformSettings();
		iosSetting.name = "iPhone";
		iosSetting.maxTextureSize = 2048;
		iosSetting.compressionQuality = 50;
		iosSetting.overridden = true;
		iosSetting.format = TextureImporterFormat.ASTC_RGBA_8x8;
		//Android的设置
		androidSetting = new TextureImporterPlatformSettings();
		androidSetting.name = "Android";
		androidSetting.maxTextureSize = 2048;
		androidSetting.compressionQuality = 50;
		androidSetting.overridden = true;
	}

	bool IsAtlasSourseFolder()
	{
		return (assetPath.IndexOf("Assets/AtlasSource/") == 0);
	}

	void OnPreprocessTexture()
	{
		if (IsAtlasSourseFolder()) {
			TextureImporter importer = assetImporter as TextureImporter;
			importer.mipmapEnabled = false;
			importer.alphaIsTransparency = true;
			importer.textureCompression = TextureImporterCompression.Compressed;
			importer.textureType = TextureImporterType.Sprite;
			importer.SetPlatformTextureSettings (iosSetting);
			importer.SetPlatformTextureSettings (androidSetting);
		}
	}
}