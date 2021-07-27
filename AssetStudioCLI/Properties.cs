using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

namespace AssetStudioCLI.Properties
{
    class Settings
    {
        public bool displayAll = false;
        public bool enablePreview = true;
        public bool displayInfo = true;
        public bool openAfterExport = false;
        public int assetGroupOption = 1;
        public bool convertTexture = true;
        public bool convertAudio = true;
        public AssetStudio.ImageFormat convertType = AssetStudio.ImageFormat.Png;
        public bool eulerFilter = true;
        public float filterPrecision = 0.25f;
        public bool exportAllNodes = true;
        public bool exportSkins = true;
        public bool exportAnimations = true;
        public float boneSize = 10.0f;
        public int fbxVersion = 3;
        public int fbxFormat = 0;
        public float scaleFactor = 1.0f;
        public bool exportBlendShape = true;
        public bool castToBone = false;
        public bool restoreExtensionName = true;
        public bool exportAllUvsAsDiffuseMaps = false;

        [Option('i', "input", Required = true, HelpText = "Input File/Folder")]
        public string input { get; set; }

        [Option('o', "output", Required = true, HelpText = "Output Folder")]
        public string output { get; set; }

        public static Settings Default = new Settings();
    }
}
