﻿/**********************************************************************************
*
* Original code based on AutoDeconstruct generator created by Jason Bock
* and published in 'Writing Code to Generate Code in C#' article located at
* https://www.codemag.com/Article/2305061/Writing-Code-to-Generate-Code-in-C#
* AutoDestruct code was retrieved from https://github.com/JasonBock/AutoDeconstruct
*
***********************************************************************************/ 

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// Source: https://github.com/Sergio0694/PolySharp/blob/615f6ccb3e59644e0e16752571c0db81ce780c5d/src/PolySharp.SourceGenerators/Helpers/EquatableArray%7BT%7D.cs

// Keep the code paired with the above source.
#pragma warning disable IDE0007
#pragma warning disable IDE0009
#pragma warning disable IDE0021
#pragma warning disable IDE0022
#pragma warning disable IDE0023
#pragma warning disable IDE0024

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MavFiFoundation.SourceGenerators;

/// <summary>
/// Extensions for <see cref="EquatableArray{T}"/>.
/// </summary>
public static class EquatableArray
{
	/// <summary>
	/// Creates an <see cref="EquatableArray{T}"/> instance from a given <see cref="ImmutableArray{T}"/>.
	/// </summary>
	/// <typeparam name="T">The type of items in the input array.</typeparam>
	/// <param name="array">The input <see cref="ImmutableArray{T}"/> instance.</param>
	/// <returns>An <see cref="EquatableArray{T}"/> instance from a given <see cref="ImmutableArray{T}"/>.</returns>
	public static EquatableArray<T> AsEquatableArray<T>(this ImmutableArray<T> array)
		 where T : IEquatable<T>
	{
		return new(array);
	}
}

/// <summary>
/// An immutable, equatable array. This is equivalent to <see cref="ImmutableArray{T}"/> but with value equality support.
/// </summary>
/// <typeparam name="T">The type of values in the array.</typeparam>
public readonly struct EquatableArray<T> : IEquatable<EquatableArray<T>>, IEnumerable<T>
	 where T : IEquatable<T>
{
	/// <summary>
	/// The underlying <typeparamref name="T"/> array.
	/// </summary>
	private readonly T[]? array;

	/// <summary>
	/// Creates a new <see cref="EquatableArray{T}"/> instance.
	/// </summary>
	/// <param name="array">The input <see cref="ImmutableArray{T}"/> to wrap.</param>
	public EquatableArray(ImmutableArray<T> array)
	{
		this.array = Unsafe.As<ImmutableArray<T>, T[]?>(ref array);
	}

	/// <summary>
	/// Gets a reference to an item at a specified position within the array.
	/// </summary>
	/// <param name="index">The index of the item to retrieve a reference to.</param>
	/// <returns>A reference to an item at a specified position within the array.</returns>
	public T this[int index]
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => AsImmutableArray()[index];
	}

	/// <summary>
	/// Gets a value indicating whether the current array is empty.
	/// </summary>
	public bool IsEmpty
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => AsImmutableArray().IsEmpty;
	}

	/// <summary>
	/// Gets a value indicating whether the current array is default or empty.
	/// </summary>
	public bool IsDefaultOrEmpty
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => AsImmutableArray().IsDefaultOrEmpty;
	}

	/// <summary>
	/// Gets the length of the current array.
	/// </summary>
	public int Length
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => AsImmutableArray().Length;
	}

	/// <sinheritdoc/>
	public bool Equals(EquatableArray<T> array)
	{
		return AsSpan().SequenceEqual(array.AsSpan());
	}

	/// <sinheritdoc/>
	public override bool Equals(object? obj)
	{
		return obj is EquatableArray<T> array && Equals(this, array);
	}

	/// <sinheritdoc/>
	public override int GetHashCode()
	{
		if (this.array is not T[] array)
		{
			return 0;
		}

		int hashCode = 0;

		foreach (T value in array)
		{
			hashCode = unchecked((hashCode * (int)0xA5555529) + value.GetHashCode());
		}

		return hashCode;
	}

	/// <summary>
	/// Gets an <see cref="ImmutableArray{T}"/> instance from the current <see cref="EquatableArray{T}"/>.
	/// </summary>
	/// <returns>The <see cref="ImmutableArray{T}"/> from the current <see cref="EquatableArray{T}"/>.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ImmutableArray<T> AsImmutableArray()
	{
		return Unsafe.As<T[]?, ImmutableArray<T>>(ref Unsafe.AsRef(in this.array));
	}

	/// <summary>
	/// Creates an <see cref="EquatableArray{T}"/> instance from a given <see cref="ImmutableArray{T}"/>.
	/// </summary>
	/// <param name="array">The input <see cref="ImmutableArray{T}"/> instance.</param>
	/// <returns>An <see cref="EquatableArray{T}"/> instance from a given <see cref="ImmutableArray{T}"/>.</returns>
	public static EquatableArray<T> FromImmutableArray(ImmutableArray<T> array)
	{
		return new(array);
	}

	/// <summary>
	/// Returns a <see cref="ReadOnlySpan{T}"/> wrapping the current items.
	/// </summary>
	/// <returns>A <see cref="ReadOnlySpan{T}"/> wrapping the current items.</returns>
	public ReadOnlySpan<T> AsSpan()
	{
		return AsImmutableArray().AsSpan();
	}

	/// <summary>
	/// Copies the contents of this <see cref="EquatableArray{T}"/> instance. to a mutable array.
	/// </summary>
	/// <returns>The newly instantiated array.</returns>
	public T[] ToArray()
	{
		return AsImmutableArray().ToArray();
	}

	/// <summary>
	/// Gets an <see cref="ImmutableArray{T}.Enumerator"/> value to traverse items in the current array.
	/// </summary>
	/// <returns>An <see cref="ImmutableArray{T}.Enumerator"/> value to traverse items in the current array.</returns>
	public ImmutableArray<T>.Enumerator GetEnumerator()
	{
		return AsImmutableArray().GetEnumerator();
	}

	/// <sinheritdoc/>
	IEnumerator<T> IEnumerable<T>.GetEnumerator()
	{
		return ((IEnumerable<T>)AsImmutableArray()).GetEnumerator();
	}

	/// <sinheritdoc/>
	IEnumerator IEnumerable.GetEnumerator()
	{
		return ((IEnumerable)AsImmutableArray()).GetEnumerator();
	}

	/// <summary>
	/// Implicitly converts an <see cref="ImmutableArray{T}"/> to <see cref="EquatableArray{T}"/>.
	/// </summary>
	/// <returns>An <see cref="EquatableArray{T}"/> instance from a given <see cref="ImmutableArray{T}"/>.</returns>
	public static implicit operator EquatableArray<T>(ImmutableArray<T> array)
	{
		return FromImmutableArray(array);
	}

	/// <summary>
	/// Implicitly converts an <see cref="EquatableArray{T}"/> to <see cref="ImmutableArray{T}"/>.
	/// </summary>
	/// <returns>An <see cref="ImmutableArray{T}"/> instance from a given <see cref="EquatableArray{T}"/>.</returns>
	public static implicit operator ImmutableArray<T>(EquatableArray<T> array)
	{
		return array.AsImmutableArray();
	}

	/// <summary>
	/// Checks whether two <see cref="EquatableArray{T}"/> values are the same.
	/// </summary>
	/// <param name="left">The first <see cref="EquatableArray{T}"/> value.</param>
	/// <param name="right">The second <see cref="EquatableArray{T}"/> value.</param>
	/// <returns>Whether <paramref name="left"/> and <paramref name="right"/> are equal.</returns>
	public static bool operator ==(EquatableArray<T> left, EquatableArray<T> right)
	{
		return left.Equals(right);
	}

	/// <summary>
	/// Checks whether two <see cref="EquatableArray{T}"/> values are not the same.
	/// </summary>
	/// <param name="left">The first <see cref="EquatableArray{T}"/> value.</param>
	/// <param name="right">The second <see cref="EquatableArray{T}"/> value.</param>
	/// <returns>Whether <paramref name="left"/> and <paramref name="right"/> are not equal.</returns>
	public static bool operator !=(EquatableArray<T> left, EquatableArray<T> right)
	{
		return !left.Equals(right);
	}
}