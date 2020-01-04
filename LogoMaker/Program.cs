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
        public static ImageFormat Svg { get; }
        public static Dictionary<string, ImageFormat> extensionMapping = new Dictionary<string, ImageFormat>()
        {
            { ".bmp" , ImageFormat.Bmp },
            { ".gif" , ImageFormat.Gif },
            { ".png" , ImageFormat.Png },
            { ".jpg" , ImageFormat.Jpeg },
            { ".jpeg", ImageFormat.Jpeg },
            { ".tif" , ImageFormat.Tiff },
            { ".tiff", ImageFormat.Tiff },
            { ".ico" , ImageFormat.Icon },
            { ".svg", Svg }
        };

        static int Main(string[] args)
        {
            ParserResult<CommandLineOptions> cliParseResult = Parser.Default.ParseArguments<CommandLineOptions>(args);
            if(cliParseResult.Tag == ParserResultType.NotParsed)
            {
                return -1;
            }

            CommandLineOptions options = ((Parsed<CommandLineOptions>)cliParseResult).Value;

            string fullFilePath = Path.GetFullPath(options.FileName);
            string extension = Path.GetExtension(options.FileName);
            if (!extensionMapping.TryGetValue(extension, out ImageFormat outputImageFormat))
            {
                Console.WriteLine(Resource.ExtensionNotSupportedNotice, extension);
                return -5;
            }

            string svgContent = string.Format(Resource.SvgBase,
                options.Size.ToString(),
                options.BackgroundColor,
                options.TextColor,
                (options.TextSize + (0.4 * options.Size)).ToString(),
                options.FontFamily,
                options.Text,
                (((double)options.Size / 2) + (options.TextSize + (0.4 * options.Size)) / 3).ToString());
            if(extension == ".svg")
            {
                File.WriteAllText(fullFilePath, svgContent);
                Console.WriteLine(Resource.FileSavedNotice, "SVG", fullFilePath);
                return 0;
            }
            SvgDocument svgDocument = SvgDocument.FromSvg<SvgDocument>(svgContent);
            Bitmap bitmap = svgDocument.Draw();
            if(extension == ".ico")
            {
                if(bitmap.Width > 256)
                {
                    Console.WriteLine(Resource.IconLargerThan256Notice);
                }

                if (bitmap.Width > 128)
                {
                    Console.WriteLine(Resource.IconLargerThan128Notice);
                }

                if (options.UseWindowsNativeIconEncoder)
                {
                    Console.WriteLine(Resource.IconUseWNIENotice);
                    using (FileStream iconWriteStream = new FileStream(fullFilePath, FileMode.OpenOrCreate))
                    {
                        Icon icon = Icon.FromHandle(bitmap.GetHicon());
                        icon.Save(iconWriteStream);
                    }
                }
                else
                {
                    Console.WriteLine(Resource.IconUseHQIENotice);
                    using (FileStream iconWriteStream = new FileStream(fullFilePath, FileMode.OpenOrCreate))
                    {
                        byte[] icoBytes = ConvertToIcon(bitmap, bitmap.Width);
                        if (icoBytes == null)
                        {

                            Console.WriteLine(Resource.IconHQIEFailedNotice);
                            return -21;
                        }
                        iconWriteStream.Write(icoBytes, 0, icoBytes.Length);
                    }
                }
                Console.WriteLine(Resource.FileSavedNotice, "Icon", fullFilePath);
                return 0;
            }
            bitmap.Save(fullFilePath, outputImageFormat);
            Console.WriteLine(Resource.FileSavedNoticeWithExtension, "Bitmap", extension, fullFilePath);
            return 0;
        }

        public static byte[] ConvertToIcon(Bitmap inputBitmap, int size)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                inputBitmap.Save(memoryStream, ImageFormat.Png);

                MemoryStream output = new MemoryStream();
                BinaryWriter iconWriter = new BinaryWriter(output);
                if (output != null && iconWriter != null)
                {
                    // 0-1 reserved, 0
                    iconWriter.Write((byte)0);
                    iconWriter.Write((byte)0);

                    // 2-3 image type, 1 = icon, 2 = cursor
                    iconWriter.Write((short)1);

                    // 4-5 number of images
                    iconWriter.Write((short)1);

                    // image entry 1
                    // 0 image width
                    iconWriter.Write((byte)size);
                    // 1 image height
                    iconWriter.Write((byte)size);

                    // 2 number of colors
                    iconWriter.Write((byte)0);

                    // 3 reserved
                    iconWriter.Write((byte)0);

                    // 4-5 color planes
                    iconWriter.Write((short)0);

                    // 6-7 bits per pixel
                    iconWriter.Write((short)32);

                    // 8-11 size of image data
                    iconWriter.Write((int)memoryStream.Length);

                    // 12-15 offset of image data
                    iconWriter.Write(6 + 16);

                    // write image data
                    // png data must contain the whole png data file
                    iconWriter.Write(memoryStream.ToArray());

                    iconWriter.Flush();

                    return output.ToArray();
                }
            }
            return null;
        }
    }
}
