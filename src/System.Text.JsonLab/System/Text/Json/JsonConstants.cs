﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


using System.Buffers;

namespace System.Text.JsonLab
{
    internal static class JsonConstants
    {
        #region Formatting constants

        public static readonly StandardFormat NumberFormat = new StandardFormat('D');
        public static readonly StandardFormat DateTimeFormat = new StandardFormat('O');
        public static readonly StandardFormat GuidFormat = new StandardFormat('D');

        #endregion Formatting constants

        #region Control characters

        public const byte OpenBrace = (byte)'{';
        public const byte CloseBrace = (byte)'}';
        public const byte OpenBracket = (byte)'[';
        public const byte CloseBracket = (byte)']';
        public const byte Space = (byte)' ';
        public const byte CarriageReturn = (byte)'\r';
        public const byte LineFeed = (byte)'\n';
        public const byte Tab = (byte)'\t';
        public const byte ListSeperator = (byte)',';
        public const byte KeyValueSeperator = (byte)':';
        public const byte Quote = (byte)'"';
        public const byte ReverseSolidus = (byte)'\\';
        public const byte Solidus = (byte)'/';

        #endregion Control characters

        #region Common values

        public static readonly byte[] TrueValue = { (byte)'t', (byte)'r', (byte)'u', (byte)'e' };
        public static readonly byte[] FalseValue = { (byte)'f', (byte)'a', (byte)'l', (byte)'s', (byte)'e' };
        public static readonly byte[] NullValue = { (byte)'n', (byte)'u', (byte)'l', (byte)'l' };

        public static readonly byte[] Delimiters = { ListSeperator, CloseBrace, CloseBracket, CarriageReturn, LineFeed, Space, Tab };

        public static readonly byte[] WhiteSpace = { Space, CarriageReturn, LineFeed, Tab };

        #endregion Common values
    }
}
