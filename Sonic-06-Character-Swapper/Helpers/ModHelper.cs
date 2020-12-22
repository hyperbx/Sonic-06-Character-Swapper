// Sonic '06 Character Swapper is licensed under the MIT License:
/* 
 * MIT License
 * 
 * Copyright (c) 2020 HyperBE32
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System;
using System.IO;

namespace S2006CharSwapMarathon.Helpers
{
    public class ModHelper
    {
        /// <summary>
        /// Writes a pre-made mod configuration.
        /// </summary>
        /// <param name="filePath">Path to write to.</param>
        /// <param name="seed">Seed to use for versioning.</param>
        public static void WriteConfig(string filePath, string seed)
        {
            // Write INI to path.
            File.WriteAllText
            (
                filePath,

                "[Details]\n" +
                $"Title=\"Character Swap ({seed})\"\n" +
                $"Version=\"{seed}\"\n" +
                $"Date=\"{DateTime.Now:dd/MM/yyyy}\"\n" +
                "Author=\"Sonic '06 Character Swapper\"\n" +
                "Platform=\"All Systems\"\n\n" +
                "" +
                "[Filesystem]\n" +
                "Merge=\"False\""
            );
        }
    }
}
