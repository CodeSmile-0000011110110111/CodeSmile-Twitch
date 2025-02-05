﻿// Copyright (C) 2021-2023 Steffen Itterheim
// Refer to included LICENSE file for terms and conditions.

using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Collections.LowLevel.Unsafe.NotBurstCompatible;
using Unity.Serialization.Binary;

namespace CodeSmile.Serialization
{
	/// <summary>
	/// Provides methods to serialize (save) objects and deserialize (load) them.
	/// </summary>
	public static class Serialize
	{
		/// <summary>
		/// Serializes the object to binary using the provided adapters.
		/// Adapters provide control over how serialization is processed.
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="adapters"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static unsafe Byte[] ToBinary<T>(T obj, IReadOnlyList<IBinaryAdapter> adapters = null)
		{
			var buffer = new UnsafeAppendBuffer(16, 8, Allocator.Temp);
			var parameters = new BinarySerializationParameters { UserDefinedAdapters = adapters?.ToList() };
			BinarySerialization.ToBinary(&buffer, obj, parameters);

			var bytes = buffer.ToBytesNBC();
			buffer.Dispose();

			return bytes;
		}

		/// <summary>
		/// Attemtps to deserialize a byte[] to the specified type using the provided adapters.
		/// </summary>
		/// <param name="serializedBytes"></param>
		/// <param name="adapters"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static unsafe T FromBinary<T>(Byte[] serializedBytes, IReadOnlyList<IBinaryAdapter> adapters = null)
		{
			fixed (Byte* ptr = serializedBytes)
			{
				var bufferReader = new UnsafeAppendBuffer.Reader(ptr, serializedBytes.Length);
				var parameters = new BinarySerializationParameters { UserDefinedAdapters = adapters?.ToList() };
				return BinarySerialization.FromBinary<T>(&bufferReader, parameters);
			}
		}
	}
}
