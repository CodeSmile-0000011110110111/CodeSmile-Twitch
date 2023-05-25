﻿// Copyright (C) 2021-2023 Steffen Itterheim
// Refer to included LICENSE file for terms and conditions.

using CodeSmile.ProTiler.Assets;
using CodeSmile.ProTiler.Editor.Creation;
using CodeSmile.ProTiler.Rendering;
using NUnit.Framework;
using System.Linq;
using UnityEngine;
using GridCoord = UnityEngine.Vector3Int;

namespace CodeSmile.Tests.Editor.ProTiler.Rendering
{
	public class Tilemap3DRendererTests
	{
		[Test] public void TilemapRendererIsNotNull()
		{
			var renderer = CreateTilemapRenderer();

			Assert.That(renderer != null);
		}

		[Test] public void TilemapRendererHasDefaultCullingInstance()
		{
			var renderer = CreateTilemapRenderer();

			Assert.That(renderer.FrustumCulling is Tilemap3DFrustumCulling);
		}

		[Test] public void DefaultCullingReturnsFixedNumberOfTiles()
		{
			var renderer = CreateTilemapRenderer();
			var coords = renderer.FrustumCulling.GetVisibleCoords();

			Assert.That(coords.Count(), Is.EqualTo(100));
		}

		[Test] public void GetPrefabWithIndexZeroReturnsEmptyTile()
		{
			var renderer = CreateTilemapRenderer();

			Assert.That(renderer.TileAssetSet[0], Is.EqualTo(Tile3DAssetBaseSet.LoadEmptyTileAsset()));
		}

		[Test] public void GetPrefabWithIndexOneReturnsTile()
		{
			var renderer = CreateTilemapRenderer();

			var tileAsset = ScriptableObject.CreateInstance<Tile3DAsset>();
			renderer.TileAssetSet.Add(tileAsset);

			Assert.That(renderer.TileAssetSet[1], Is.EqualTo(tileAsset));
		}

		[Test] public void GetPrefabWithInvalidIndexReturnsMissingTile()
		{
			var renderer = CreateTilemapRenderer();

			Assert.That(renderer.TileAssetSet[1000], Is.EqualTo(Tile3DAssetBaseSet.LoadMissingTileAsset()));
		}

		private Tilemap3DRenderer CreateTilemapRenderer() =>
			Tilemap3DCreation.CreateRectangularTilemap3D().GetComponent<Tilemap3DRenderer>();

	}
}
