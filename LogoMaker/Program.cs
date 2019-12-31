/* This program is free software. It comes without any warranty, to
 * the extent permitted by applicable law. You can redistribute it
 * and/or modify it under the terms of the Do What The Fuck You Want
 * To Public License, Version 2, as published by Joseph Chirs. See
 * http://www.wtfpl.net/ for more details. */

using CommandLine;
using Svg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace LogoMaker
{
    class Program
    {
        public static Dictionary<string, ImageFormat> extensionMapping = new Dictionary<string, ImageFormat>()
        {
            { ".bmp" , ImageFormat.Bmp },
            { ".emf" , ImageFormat.Emf },
            { ".wmf" , ImageFormat.Wmf },
            { ".gif" , ImageFormat.Gif },
            { ".png" , ImageFormat.Png },
            { ".jpg" , ImageFormat.Jpeg },
            { ".jpeg", ImageFormat.Jpeg },
            { ".tif" , ImageFormat.Tiff },
            { ".tiff", ImageFormat.Tiff },
            { ".ico" , ImageFormat.Icon },
        };

        static int Main(string[] args)
        {
            ParserResult<CommandLineOptions> cliParseResult = Parser.Default.ParseArguments<CommandLineOptions>(args);
            if(cliParseResult.Tag == ParserResultType.NotParsed)
            {
                return -1;
            }

            CommandLineOptions options = ((Parsed<CommandLineOptions>)cliParseResult).Value;

            string extension = Path.GetExtension(options.FileName);
            if (!extensionMapping.TryGetValue(extension, out ImageFormat outputImageFormat))
            {
                Console.WriteLine("ERROR: Extension {0} not supported.", extension);
                return -5;
            }            

            string svgContent = string.Format(Resource.SvgBase,
                options.Size.ToString(),
                options.BackgroundColor,
                options.TextColor,
                (options.TextSize + (0.4 * options.Size)).ToString(),
                options.FontFamily,
                options.Text,
                (((double)options.Size / 2) + (double)(options.TextSize + (0.4 * options.Size)) / 3).ToString());
            SvgDocument svgDocument = SvgDocument.FromSvg<SvgDocument>(svgContent);
            Bitmap bitmap = svgDocument.Draw();
            bitmap.Save(options.FileName, outputImageFormat);
            return 0;
        }
    }
}
