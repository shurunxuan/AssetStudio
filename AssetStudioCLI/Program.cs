using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace AssetStudioCLI
{
    class Program
    {
        private static List<string> inputFiles = null;
        static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<Properties.Settings>(args)
                .WithParsed(options => Properties.Settings.Default = options);

            if (!File.Exists(Properties.Settings.Default.input) &&
                !Directory.Exists(Properties.Settings.Default.input))
            {
                Console.WriteLine("Input File/Folder Doesn't Exist!");
                return;
            }

            FileAttributes attr = File.GetAttributes(Properties.Settings.Default.input);
            inputFiles = new List<string>();
            if (!attr.HasFlag(FileAttributes.Directory))
            {
                inputFiles.Add(Properties.Settings.Default.input);
            }
            else
            {
                ProcessDirectory(Properties.Settings.Default.input);
            }

            Studio.assetsManager.LoadFiles(inputFiles.ToArray());
            var (productName, treeNodeCollection) = Studio.BuildAssetData();
            //Console.WriteLine(Studio.exportableAssets.Count);

            if (!Directory.Exists(Properties.Settings.Default.output))
            {
                Directory.CreateDirectory(Properties.Settings.Default.output);
            }

            List<AssetItem> exportAssets = Studio.exportableAssets.Where(x => x.Type != AssetStudio.ClassIDType.Sprite).ToList();

            Studio.ExportAssets(Properties.Settings.Default.output, exportAssets, ExportType.Convert);
        }

        public static void ProcessDirectory(string targetDirectory)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                inputFiles.Add(fileName);

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);
        }
    }
}
