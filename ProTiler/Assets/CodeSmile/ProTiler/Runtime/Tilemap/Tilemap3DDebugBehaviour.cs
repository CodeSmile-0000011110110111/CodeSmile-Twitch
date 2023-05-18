﻿// Copyright (C) 2021-2023 Steffen Itterheim
// Refer to included LICENSE file for terms and conditions.

using CodeSmile.Attributes;
using CodeSmile.Extensions;
using CodeSmile.ProTiler.Tile;
using CodeSmile.ProTiler.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using UnityEditor;
using UnityEngine;
using GridCoord = UnityEngine.Vector3Int;
using ChunkCoord = UnityEngine.Vector2Int;
using ChunkSize = UnityEngine.Vector2Int;

namespace CodeSmile.ProTiler.Tilemap
{
	[RequireComponent(typeof(Tilemap3DBehaviour))]
	[DisallowMultipleComponent]
	[ExecuteAlways]
	[ExcludeFromCodeCoverage]
	public class Tilemap3DDebugBehaviour : MonoBehaviour
	{
		[SerializeField] private ChunkCoord m_ActiveChunkCoord;
		[SerializeField] private Int32 m_ActiveLayerIndex;
		[SerializeField] private Boolean m_ClearMap;
		[SerializeField] private Boolean m_FillChunkLayer;
		[SerializeField] private Boolean m_FillChunkLayersFromOrigin;
		[SerializeField] [ReadOnlyField] private Int32 m_TileCount;

		public Tilemap3DBehaviour TilemapBehaviour => GetComponent<Tilemap3DBehaviour>();

		[Pure] private static Tile3DCoord[] GetIncrementingIndexChunkTileCoords(ChunkCoord chunkCoord,
			ChunkSize chunkSize, Int32 height)
		{
			var width = chunkSize.x;
			var length = chunkSize.y;
			var chunkOrigin = chunkCoord * chunkSize;
			var coordDataCount = width * length;
			var tileCoords = new Tile3DCoord[coordDataCount];
			for (var x = 0; x < width; x++)
			{
				for (var z = 0; z < length; z++)
				{
					var coord = new GridCoord(chunkOrigin.x + x, height, chunkOrigin.y + z);
					var tileIndex = Grid3DUtility.ToIndex2D(x, z, width);
					tileCoords[tileIndex] = new Tile3DCoord(coord, new Tile3D(tileIndex + 1));
				}
			}
			return tileCoords;
		}

		private void Update() => UpdateTileCount();

		private void OnEnable() => UpdateTileCount();

		[Pure] private void OnDrawGizmosSelected() => DrawActiveChunkTileIndexes();

		private void OnValidate()
		{
			m_ActiveLayerIndex = Mathf.Max(0, m_ActiveLayerIndex);

			if (m_ClearMap)
			{
				m_ClearMap = false;
				ClearMap();
			}

			if (m_FillChunkLayer)
			{
				m_FillChunkLayer = false;
				FillActiveLayerWithIncrementingTiles(m_ActiveChunkCoord, m_ActiveLayerIndex);
			}

			if (m_FillChunkLayersFromOrigin)
			{
				m_FillChunkLayersFromOrigin = false;
				FillActiveLayerFromOrigin();
			}

			UpdateTileCount();
		}

		private void ClearMap() => TilemapBehaviour.ClearTilemap();

		private void UpdateTileCount() => this.WaitForFramesElapsed(1, () =>
		{
			m_TileCount = TilemapBehaviour != null ? TilemapBehaviour.TileCount : -1;
		});

		[Pure] private void DrawActiveChunkTileIndexes()
		{
#if UNITY_EDITOR
			var normalStyle = new GUIStyleState { textColor = Color.yellow };
			var labelStyle = new GUIStyle { fontSize = 11, normal = normalStyle };

			var tilemapBehaviour = TilemapBehaviour;
			var chunkSize = tilemapBehaviour.ChunkSize;
			var cellSize = tilemapBehaviour.Grid.CellSize;
			var halfCellSize = cellSize * .5f;

			var layerCountInChunk = tilemapBehaviour.GetLayerCount(m_ActiveChunkCoord);
			for (var height = 0; height < layerCountInChunk; height++)
			{
				var coords =
					Tilemap3DUtility.GetAllChunkLayerCoords(m_ActiveChunkCoord, chunkSize, height);
				var tileCoords = tilemapBehaviour.GetTiles(coords);

				foreach (var tileCoord in tileCoords)
				{
					var coord = tileCoord.Coord;
					var pos = new Vector3(coord.x * cellSize.x, coord.y * cellSize.y, coord.z * cellSize.z) +
					          halfCellSize;

					Gizmos.DrawWireCube(pos, cellSize);
					if (height == 0)
						Handles.Label(pos, $"{tileCoord.Tile.Index}: {pos}", labelStyle);
				}
			}
#endif
		}

		[Pure] private void FillActiveLayerWithIncrementingTiles(Vector2Int chunkCoord, Int32 height)
		{
			var tileCoords = GetIncrementingIndexChunkTileCoords(chunkCoord, TilemapBehaviour.ChunkSize, height);
			TilemapBehaviour.SetTiles(tileCoords);
			UpdateTileCount();
			Debug.Log($"filled chunk {chunkCoord} layer {height} with {tileCoords.Length} tiles");
		}

		private void FillActiveLayerFromOrigin()
		{
			var allTileCoords = new List<Tile3DCoord>();

			for (var x = 0; x <= m_ActiveChunkCoord.x; x++)
			{
				for (var y = 0; y <= m_ActiveChunkCoord.y; y++)
				{
					for (var h = 0; h <= m_ActiveLayerIndex; h++)
					{
						var coord = new ChunkCoord(x, y);
						var tileCoords = GetIncrementingIndexChunkTileCoords(coord, TilemapBehaviour.ChunkSize, h);
						allTileCoords.AddRange(tileCoords);
					}
				}
			}

			TilemapBehaviour.SetTiles(allTileCoords);
			UpdateTileCount();
		}
	}
}
