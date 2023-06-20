﻿// Copyright (C) 2021-2023 Steffen Itterheim
// Refer to included LICENSE file for terms and conditions.

using CodeSmile.Extensions.NativeCollections;
using CodeSmile.ProTiler.CodeDesign.Model;
using CodeSmile.Serialization;
using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Serialization.Binary;
using ChunkSize = Unity.Mathematics.int3;

namespace CodeSmile.ProTiler.CodeDesign.Serialization
{
	public class LinearDataMapChunkBinaryAdapter<TData> : VersionedBinaryAdapterBase,
		IBinaryAdapter<LinearDataMapChunk<TData>>
		where TData : unmanaged
	{
		private readonly Allocator m_Allocator;

		private static unsafe void WriteChunkData(
			in BinarySerializationContext<LinearDataMapChunk<TData>> context, in UnsafeList<TData>.ReadOnly dataList)
		{
			var dataLength = dataList.Length;
			context.Writer->Add(dataLength);

			foreach (var data in dataList)
				context.SerializeValue(data);
		}

		private static unsafe UnsafeList<TData> ReadChunkData(
			in BinaryDeserializationContext<LinearDataMapChunk<TData>> context, Allocator allocator)
		{
			var dataLength = context.Reader->ReadNext<Int32>();

			var list = UnsafeListExt.NewWithLength<TData>(dataLength, allocator);
			for (var i = 0; i < dataLength; i++)
				list[i] = context.DeserializeValue<TData>();

			return list;
		}

		public LinearDataMapChunkBinaryAdapter(Byte adapterVersion, Allocator allocator)
			: base(adapterVersion) => m_Allocator = allocator;

		public unsafe void Serialize(in BinarySerializationContext<LinearDataMapChunk<TData>> context,
			LinearDataMapChunk<TData> chunk)
		{
			var writer = context.Writer;

			WriteAdapterVersion(writer);
			writer->Add(chunk.Size);
			WriteChunkData(context, chunk.Data);
		}

		public unsafe LinearDataMapChunk<TData> Deserialize(
			in BinaryDeserializationContext<LinearDataMapChunk<TData>> context)
		{
			var reader = context.Reader;

			var serializedVersion = ReadAdapterVersion(reader);
			if (serializedVersion == AdapterVersion)
			{
				var chunkSize = reader->ReadNext<ChunkSize>();
				var data = ReadChunkData(context, m_Allocator);

				return new LinearDataMapChunk<TData>(chunkSize, data);
			}

			throw new SerializationVersionException(GetVersionExceptionMessage(serializedVersion));
		}
	}
}
