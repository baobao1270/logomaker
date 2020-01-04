/* This program is free software. It comes without any warranty, to
 * the extent permitted by applicable law. You can redistribute it
 * and/or modify it under the terms of the Do What The Fuck You Want
 * To Public License, Version 2, as published by Joseph Chirs. See
 * http://www.wtfpl.net/ for more details. */

using CommandLine;

namespace LogoMaker
{
    class CommandLineOptions
    {
        [Option('t', "text", Required = true, HelpText = "Text in the logo, suggest less than 2 alphabet.")]
        public string Text { get; set; }

        [Option('S', "size", Default = 300, Required = false, HelpText = "Size of PNG file, the unit is px, width and height are equal.")]
        public int Size { get; set; }

        [Option('C', "background-color", Default = "#66ccff", Required = false, HelpText = "Background color, CSS sytle.")]
        public string BackgroundColor { get; set; }

        [Option('s', "text-size", Default = 0, Required = false, HelpText = "Text size, based on 120px, you can add or minus it. Unit is px.")]
        public int TextSize { get; set; }

        [Option('c', "text-color", Default = "white", Required = false, HelpText = "Text color, CSS sytle.")]
        public string TextColor { get; set; }

        [Option('f', "font-family", Default = "Microsoft YaHei", Required = false, HelpText = "Text size, CSS sytle.")]
        public string FontFamily { get; set; }

        [Option('o', "output-file", Required = true, HelpText = "Output File name. Only support extensions below: jpg/jpeg, png, gif, bmp, tif/tiff, ico, svg.")]
        public string FileName { get; set; }

        [Option('W', "windows-native-encoder", Default = false, Required = false, HelpText = "Use the windows native ico encoder instead of high-quality ico encoder. The windows native encoder have more compatibility but only support size <= 48x48 and 16-bit color.")]
        public bool UseWindowsNativeIconEncoder { get; set; }
    }
}
