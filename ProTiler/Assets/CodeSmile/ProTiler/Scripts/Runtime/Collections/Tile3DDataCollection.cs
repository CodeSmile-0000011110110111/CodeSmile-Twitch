﻿// Copyright (C) 2021-2023 Steffen Itterheim
// Refer to included LICENSE file for terms and conditions.

using CodeSmile.ProTiler.Data;
using System;

namespace CodeSmile.ProTiler.Collections
{
	/// <summary>
	///     Represents a single, flat layer of tiles. Essentially a 2D array.
	/// </summary>
	[Serializable]
	public class Tile3DDataCollection
	{
		private int m_Width;
		private Tile3DData[] m_Tiles;
		internal Tile3DData this[int index]
		{
			get => m_Tiles[index];
			set => m_Tiles[index] = value;
		}

		internal Tile3DData this[int x, int y]
		{
			get => m_Tiles[Grid3DUtility.ToIndex(x, y, m_Width)];
			set => m_Tiles[Grid3DUtility.ToIndex(x, y, m_Width)] = value;
		}

		internal int Count
		{
			get
			{
				var count = 0;
				foreach (var tileData in m_Tiles)
				{
					if (tileData.PrefabSetIndex != 0)
						count++;
				}
				return count;
			}
		}

		internal int Capacity => m_Tiles.Length;

		internal Tile3DDataCollection(int width, int height)
		{
			m_Width = width;
			m_Tiles = new Tile3DData[width * height];
		}

		internal void SetTiles(Tile3DCoordData[] tileChangeData)
		{
			foreach (var changeData in tileChangeData)
				this[changeData.Coord.x, changeData.Coord.z] = changeData.TileData;
		}
	}
}
