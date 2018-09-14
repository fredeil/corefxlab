// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Buffers.Text;
using System.Text;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace System.Text.Formatting.Benchmarks
{
    public class PerfSmokeTests
    {
        [Params(10, 1000, 100_000)]
        private int numbersToWrite;
        private static ArrayPool<byte> pool = ArrayPool<byte>.Shared;

        [Benchmark]
        private void InvariantFormatIntDec()
        {
            StringFormatter sb = new StringFormatter(numbersToWrite, pool);
            for (int i = 0; i < numbersToWrite; i++)
            {
                sb.Append(((int)(i % 10)));
            }
            var text = sb.ToString();
            if (text.Length != numbersToWrite)
            {
                throw new Exception("test failed");
            }
        }

        [Benchmark]
        private void InvariantFormatIntDecClr()
        {
            StringBuilder sb = new StringBuilder(numbersToWrite);
            for (int i = 0; i < numbersToWrite; i++)
            {
                sb.Append(((int)(i % 10)));
            }
            var text = sb.ToString();
            if (text.Length != numbersToWrite)
            {
                throw new Exception("test failed");
            }
        }

        [Benchmark]
        private void InvariantFormatIntHex()
        {
            StringFormatter sb = new StringFormatter(numbersToWrite, pool);
            StandardFormat format = new StandardFormat('X', StandardFormat.NoPrecision);

            for (int i = 0; i < numbersToWrite; i++)
            {
                sb.Append(((int)(i % 10)), format);
            }

            var text = sb.ToString();
            if (text.Length != numbersToWrite)
            {
                throw new Exception("test failed");
            }
        }

        [Benchmark]
        private void InvariantFormatIntHexClr()
        {
            StringBuilder sb = new StringBuilder(numbersToWrite);
            for (int i = 0; i < numbersToWrite; i++)
            {
                sb.Append(((int)(i % 10)).ToString("X"));
            }
            var text = sb.ToString();
            if (text.Length != numbersToWrite)
            {
                throw new Exception("test failed");
            }
        }

        [Benchmark]
        private void InvariantFormatStruct()
        {
            StringFormatter sb = new StringFormatter(numbersToWrite * 2, pool);
            for (int i = 0; i < numbersToWrite; i++)
            {
                sb.Append(new Age(i % 10));
            }
            var text = sb.ToString();
            if (text.Length != numbersToWrite * 2)
            {
                throw new Exception($"test failed [{text.Length} != {numbersToWrite * 2}]");
            }
        }

        [Benchmark]
        private void FormatGuid()
        {
            var guid = Guid.NewGuid();
            var guidsToWrite = numbersToWrite / 10;

            StringFormatter sb = new StringFormatter(guidsToWrite * 36, pool);
            for (int i = 0; i < guidsToWrite; i++)
            {
                sb.Append(guid);
            }
            var text = sb.ToString();
            if (text.Length != guidsToWrite * 36)
            {
                throw new Exception("test failed");
            }
        }

        [Benchmark]
        private void InvariantFormatStructClr()
        {
            StringBuilder sb = new StringBuilder(numbersToWrite * 2);
            for (int i = 0; i < numbersToWrite; i++)
            {
                sb.Append(new Age(i % 10));
            }
            var text = sb.ToString();
            if (text.Length != numbersToWrite * 2)
            {
                throw new Exception("test failed");
            }
        }

        [Benchmark]
        private void CustomCultureFormat()
        {
            StringFormatter sb = new StringFormatter(numbersToWrite * 3, pool);
            sb.SymbolTable = CreateCustomCulture();

            sb.Clear();
            for (int i = 0; i < numbersToWrite; i++)
            {
                var next = (i % 128) + 101;
                sb.Append(next);
            }

            var text = sb.ToString();
            if (text.Length != numbersToWrite * 3)
            {
                throw new Exception("test failed");
            }
        }

        [Benchmark]
        private void CustomCultureFormatClr()
        {
            StringBuilder sb = new StringBuilder(numbersToWrite * 3);
            var culture = new CultureInfo("th");

            for (int itteration = 0; itteration < itterationsCulture; itteration++)
            {
                sb.Clear();
                for (int i = 0; i < numbersToWrite; i++)
                {
                    sb.Append(((i % 128) + 100).ToString(culture));
                }
                var text = sb.ToString();
                if (text.Length != numbersToWrite * 3)
                {
                    throw new Exception("test failed");
                }
            }
        }

        [Benchmark]
        private void EncodeStringToUtf8()
        {
            string text = "Hello World!";
            int stringsToWrite = 2000;
            int size = stringsToWrite * text.Length + stringsToWrite;
            ArrayFormatter formatter = new ArrayFormatter(size, SymbolTable.InvariantUtf8, pool);

            formatter.Clear();
            for (int i = 0; i < stringsToWrite; i++)
            {
                formatter.Append(text);
                formatter.Append(1);
            }
        }

        [Benchmark]
        private void EncodeStringToUtf8Clr()
        {
            string text = "Hello World!";
            int stringsToWrite = 2000;
            int size = stringsToWrite * text.Length + stringsToWrite;
            StringBuilder formatter = new StringBuilder(size);

            formatter.Clear();
            for (int i = 0; i < stringsToWrite; i++)
            {
                formatter.Append(text);
                formatter.Append(1);
            }

            var bytes = Encoding.UTF8.GetBytes(formatter.ToString());
        }

        private static SymbolTable CreateCustomCulture()
        {
            var utf16digitsAndSymbols = new byte[17][];
            for (ushort digit = 0; digit < 10; digit++)
            {
                char digitChar = (char)(digit + 'A');
                var digitString = new string(digitChar, 1);
                utf16digitsAndSymbols[digit] = GetBytesUtf16(digitString);
            }

            utf16digitsAndSymbols[(ushort)SymbolTable.Symbol.DecimalSeparator] = Encoding.Unicode.GetBytes(".");
            utf16digitsAndSymbols[(ushort)SymbolTable.Symbol.GroupSeparator] = Encoding.Unicode.GetBytes(",");
            utf16digitsAndSymbols[(ushort)SymbolTable.Symbol.MinusSign] = Encoding.Unicode.GetBytes("_?");

            return new CustomUtf16SymbolTable(utf16digitsAndSymbols);
        }
    }
}
