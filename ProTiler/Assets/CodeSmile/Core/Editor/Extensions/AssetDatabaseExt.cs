﻿// Copyright (C) 2021-2023 Steffen Itterheim
// Refer to included LICENSE file for terms and conditions.

using CodeSmile.Attributes;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CodeSmile.Editor.Extensions
{
	[FullCovered]
	public static class AssetDatabaseExt
	{
		public static T LoadAsset<T>() where T : Object => LoadAssets<T>().FirstOrDefault();

		public static T[] LoadAssets<T>() where T : Object
		{
			var assetPaths = FindAssetPaths<T>();
			var assetCount = assetPaths.Length;

			var loadedAssets = new T[assetCount];
			for (var i = 0; i < assetCount; i++)
				loadedAssets[i] = AssetDatabase.LoadAssetAtPath<T>(assetPaths[i]);

			return loadedAssets;
		}

		public static string[] FindAssets<T>() where T : Object => AssetDatabase.FindAssets($"t:{typeof(T)}");

		public static string[] FindAssetPaths<T>() where T : Object
		{
			var assetGuids = FindAssets<T>();
			var assetCount = assetGuids.Length;

			var assetPaths = new string[assetCount];
			for (var i = 0; i < assetCount; i++)
				assetPaths[i] = AssetDatabase.GUIDToAssetPath(assetGuids[i]);

			return assetPaths;
		}

		public static bool AssetExists<T>() where T : Object => FindAssets<T>().Length > 0;

		public static bool CreateDirectoryIfNotExists(string path)
		{
			path = PathUtility.TrimTrailingDirectorySeparatorChar(path);
			if (Directory.Exists(path))
				return false;

			Directory.CreateDirectory(path);
			AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
			return true;
		}

		public static T CreateScriptableObjectAssetAndDirectory<T>(string assetPath) where T : ScriptableObject
		{
			assetPath = Path.ChangeExtension(assetPath, ".asset");
			CreateDirectoryIfNotExists(Path.GetDirectoryName(assetPath));

			var instance = ScriptableObject.CreateInstance<T>();
			AssetDatabase.CreateAsset(instance, AssetDatabase.GenerateUniqueAssetPath(assetPath));
			return instance;
		}

		[ExcludeFromCodeCoverage]
		public static void ForceSaveAsset(Object obj)
		{
			EditorUtility.SetDirty(obj);
			AssetDatabase.SaveAssetIfDirty(obj);
		}
	}
}
