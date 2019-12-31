using CommandLine;

namespace LogoMaker
{
    class CommandLineOptions
    {
        [Option('t', "text", Required = true, HelpText = "Text in the logo, suggest less than 2 alphabet")]
        public string Text { get; set; }

        [Option('S', "size", Default = 300, Required = false, HelpText = "Size of PNG file, the unit is px, width and height are equal, default is 300")]
        public int Size { get; set; }

        [Option('C', "background-color", Default = "#66ccff", Required = false, HelpText = "Background color, CSS sytle, default is #66ccff")]
        public string BackgroundColor { get; set; }

        [Option('s', "text-size", Default = 0, Required = false, HelpText = "Text size, based on 120px, you can add or minus it. Unit is px, default is 0")]
        public int TextSize { get; set; }

        [Option('c', "text-color", Default = "white", Required = false, HelpText = "Text color, CSS sytle, default is white")]
        public string TextColor { get; set; }

        [Option('f', "font-family", Default = "Microsoft YaHei", Required = false, HelpText = "Text size, CSS sytle, default is \"Microsoft YaHei\"")]
        public string FontFamily { get; set; }

        [Option('o', "output-file", Required = true, HelpText = "Output File name. Only support extensions below: jpg, jpeg, png, gif, bmp, tif, tiff, wmf, emf")]
        public string FileName { get; set; }
    }
}
