﻿// Copyright (C) 2021-2023 Steffen Itterheim
// Refer to included LICENSE file for terms and conditions.

using CodeSmile.ProTiler.Tile;
using UnityEngine;

namespace CodeSmile.Tests.Editor.ProTiler.Tile
{
	public class Tile3DTestCaseSource
	{
		internal const Tile3DFlags AllDirectionsMask = Tile3DFlags.DirectionNorth | Tile3DFlags.DirectionWest |
		                                              Tile3DFlags.DirectionSouth | Tile3DFlags.DirectionEast;

		public static readonly object[] ValidIndexes =
		{
			new object[] { 0 },
			new object[] { 1 },
			new object[] { ushort.MaxValue },
		};

		public static readonly object[] EmptyIndexes =
		{
			new object[] { ushort.MinValue },
			new object[] { 0 },
		};
		public static readonly object[] NonEmptyIndexes =
		{
			new object[] { 1 },
			new object[] { ushort.MaxValue },
		};

		public static readonly object[] ValidIndexesWithFlags =
		{
			new object[] { 0, Tile3DFlags.None },
			new object[] { 1, AllDirectionsMask },
			new object[] { 2, Tile3DFlags.FlipBoth },
			new object[] { ushort.MinValue, Tile3DFlags.FlipBoth | AllDirectionsMask },
			new object[] { ushort.MaxValue, Tile3DFlags.FlipBoth | AllDirectionsMask },
		};

		public static readonly object[] DirectionFlags =
		{
			new object[] { Tile3DFlags.DirectionNorth },
			new object[] { Tile3DFlags.DirectionEast },
			new object[] { Tile3DFlags.DirectionSouth },
			new object[] { Tile3DFlags.DirectionWest },
			new object[] { AllDirectionsMask },
		};

		public static readonly object[] NonEqualTilePairs =
		{
			new object[]
			{
				new Tile3D(0, Tile3DFlags.DirectionSouth),
				new Tile3D(1, Tile3DFlags.DirectionSouth),
			},
			new object[]
			{
				new Tile3D(13, Tile3DFlags.DirectionSouth),
				new Tile3D(17, Tile3DFlags.DirectionSouth),
			},
			new object[]
			{
				new Tile3D(1, Tile3DFlags.DirectionWest),
				new Tile3D(1, Tile3DFlags.DirectionSouth),
			},
			new object[]
			{
				new Tile3D(0, Tile3DFlags.FlipHorizontal),
				new Tile3D(0, Tile3DFlags.FlipHorizontal | Tile3DFlags.FlipVertical),
			},
		};

		public static readonly object[] EqualTilePairs =
		{
			new object[]
			{
				new Tile3D(),
				new Tile3D(),
			},
			new object[]
			{
				new Tile3D(7, Tile3DFlags.DirectionWest | Tile3DFlags.FlipHorizontal),
				new Tile3D(7, Tile3DFlags.DirectionWest | Tile3DFlags.FlipHorizontal),
			},
			new object[]
			{
				new Tile3D(0, Tile3DFlags.None),
				new Tile3D(0, Tile3DFlags.None),
			},
		};

		public static readonly object[] NonEqualTileCoordPairs =
		{
			new object[]
			{
				new Tile3DCoord(new Vector3Int(1, 2, 3), new Tile3D()),
				new Tile3DCoord(new Vector3Int(3, 2, 1), new Tile3D()),
			},
			new object[]
			{
				new Tile3DCoord(new Vector3Int(1, 2, 3), new Tile3D(1)),
				new Tile3DCoord(new Vector3Int(1, 2, 3), new Tile3D(2)),
			},
		};

		public static readonly object[] EqualTileCoordPairs =
		{
			new object[]
			{
				new Tile3DCoord(),
				new Tile3DCoord(),
			},
			new object[]
			{
				new Tile3DCoord(new Vector3Int(1, 1, 1), new Tile3D()),
				new Tile3DCoord(new Vector3Int(1, 1, 1), new Tile3D()),
			},
			new object[]
			{
				new Tile3DCoord(new Vector3Int(1, 2, 3), new Tile3D(4)),
				new Tile3DCoord(new Vector3Int(1, 2, 3), new Tile3D(4)),
			},
		};
	}
}
