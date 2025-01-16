using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

    public static class PathUtils
    {
        public const string kAssetsRoot = "Assets";
        public const char kSeparator = '/';
        public const string kPictemSave = "Pictems";
        public const string kShapeSave = "Shapes";
        public const string kGifSave = "Gif";
        public const string kGameShotSave = "GameShots";
        
        
        private static string s_ExternalPath;
        private static string s_TemporaryPath;

        public static string PictemSavePath => PathUtils.GetExternalPath(PathUtils.kPictemSave);
        public static string GameShotSavePath => PathUtils.GetExternalPath(PathUtils.kGameShotSave);
        public static string PlayerProfileSaveFile => PathUtils.GetExternalPath("profileLocal.json");
        public static string ShapeSavePath => PathUtils.GetExternalPath(PathUtils.kShapeSave);
        public static string GifSavePath => PathUtils.GetExternalPath(PathUtils.kGifSave);
        
        public static string TestImagePath => PathUtils.GetExternalPath("test.png");
        private static readonly char[] s_PathSeparators =
        {
            '/',
            '\\'
        };


        private static List<string> s_Cached = new List<string>();



        public static string GetExternalPath(string path)
        {
            if (null == s_ExternalPath)
            {
                s_ExternalPath = GetStoragePath("External");
            }
            if(!Directory.Exists(s_ExternalPath))
            {
                Directory.CreateDirectory(s_ExternalPath);
            }
            return $"{s_ExternalPath}/{NormalizePath(path)}";
        }

        public static string GetTemporaryPath(string path)
        {
            if (null == s_TemporaryPath)
            {
                s_TemporaryPath = GetStoragePath("Temporary");
            }

            return $"{s_TemporaryPath}/{NormalizePath(path)}";
        }

        private static string GetStoragePath(string path)
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            var rootPath = Application.dataPath + "/..";
#else
            var rootPath = Application.persistentDataPath;
#endif

            return Path.GetFullPath($"{rootPath}/{NormalizePath(path)}");
        }

        public static string GetInternalPath(string path)
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            var rootPath = Application.dataPath + "!assets";
#else
            var rootPath = Application.streamingAssetsPath;
#endif
            return $"{rootPath}/{NormalizePath(path)}";
        }

        public static string GetInternalPath()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            var rootPath = Application.dataPath + "!assets";
#else
            var rootPath = Application.streamingAssetsPath;
#endif
            return rootPath;
        }


        public static string Combine(string path1, string path2)
        {
            var path = Path.Combine(path1, path2);
            return NormalizePath(path);
        }

        public static string GetParentFolderPath(string path)
        {
            var index = path.LastIndexOf('/');
            if (-1 == index)
            {
                index = path.LastIndexOf('\\');
            }

            if (index < 1)
            {
                Debug.LogError(path);
            }

            return NormalizePath(path.Remove(index));
        }

        public static bool IsSampeFolder(string lhsPath, string rhsPath)
        {
            var lhsDir = new DirectoryInfo(lhsPath);
            var rhsDir = new DirectoryInfo(rhsPath);

            if (0 == string.Compare(lhsDir.Name, rhsDir.Name, StringComparison.OrdinalIgnoreCase)
             && 0 == string.Compare(lhsDir.Parent.FullName, rhsDir.Parent.FullName, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        public static string NormalizePath(string path)
        {
            if (Path.DirectorySeparatorChar == '\\')
            {
                return path.Replace('\\', kSeparator).Trim(s_PathSeparators);
            }

            return path.Trim(s_PathSeparators);
        }

        public static void MakeDirectory(string path, bool remake = false)
        {
            var dir = Path.GetDirectoryName(path);
            if (string.IsNullOrEmpty(dir))
            {
                dir = path;
            }

            if (remake && Directory.Exists(dir))
            {
                Directory.Delete(dir, true);
            }

            Directory.CreateDirectory(dir);
        }

    public static string[] GetFilePaths(string folderPath,
                                            SearchOption searchOption,
                                            string regexPattern = ".*",
                                            bool not = false)
    {
        s_Cached.Clear();
        if (Directory.Exists(folderPath))
        {
            foreach (var path in Directory.EnumerateFiles(folderPath, "*", searchOption))
            {
                if (Path.GetExtension(path) == ".meta")
                    continue;

                if (not != Regex.IsMatch(path, regexPattern))
                {
                    s_Cached.Add(NormalizePath(path));
                }
            }
        }

        return s_Cached.ToArray();
    }

    //public static string[] GetFileExtensions(string folderPath,
    //                                         SearchOption searchOption = SearchOption.TopDirectoryOnly,
    //                                         string regexPattern = ".*",
    //                                         bool not = false)
    //{
    //    s_Cached.Clear();
    //    if (Directory.Exists(folderPath))
    //    {
    //        foreach (var path in Directory.EnumerateFiles(folderPath, "*", searchOption))
    //        {
    //            if (not == Regex.IsMatch(path, regexPattern)) continue;

    //            var ext = GetFileExtension(path);
    //            if (!s_Cached.Contains(ext))
    //            {
    //                s_Cached.Add(ext);
    //            }
    //        }
    //    }

    //    return s_Cached.ToArray();
    //}
    public static string[] GetFileNames(
        string folderPath,
        SearchOption searchOption = SearchOption.TopDirectoryOnly,
        params string[] extensions)
    {
        s_Cached.Clear();
        if (!Directory.Exists(folderPath))
        {
            return s_Cached.ToArray();
        }

        var pattern = $"\\.({string.Join("|", extensions)})$";
        foreach (var path in Directory.EnumerateFiles(folderPath, "*", searchOption))
        {
            if (Regex.IsMatch(path, pattern))
            {
                s_Cached.Add(Path.GetFileNameWithoutExtension(path));
            }
        }
        return s_Cached.ToArray();
    }


    //public static string GetFileExtension(string path)
    //{
    //    return Path.GetExtension(path);
    //}

    //public static bool IsSampleExtension(string path, string extension)
    //{
    //    return 0 == string.Compare(extension, GetFileExtension(path), StringComparison.OrdinalIgnoreCase);
    //}

    //public static void CopyFile(string sourceFilePath, string targetFilePath)
    //{
    //    var parentDirectoryPath = Path.GetDirectoryName(targetFilePath);
    //    // ReSharper disable once AssignNullToNotNullAttribute
    //    Directory.CreateDirectory(parentDirectoryPath);
    //    File.Copy(sourceFilePath, targetFilePath, true);
    //}

    //public static string GetFileOrFolderName(string path)
    //{
    //    string result;
    //    if (File.Exists(path))
    //    {
    //        result = Path.GetFileName(path);
    //    }
    //    else
    //    {
    //        if (!Directory.Exists(path))
    //        {
    //            throw new ArgumentException("Target '" + path + "' does not exist.");
    //        }

    //        var array = Split(path);
    //        result = array[array.Length - 1];
    //    }

    //    return result;
    //}

    //private static string[] Split(string path)
    //{
    //    s_Cached.Clear();
    //    s_Cached.AddRange(path.Split(s_PathSeparators));
    //    var i = 0;
    //    while (i < s_Cached.Count)
    //    {
    //        s_Cached[i] = s_Cached[i].Trim();
    //        if (s_Cached[i].Equals(string.Empty))
    //        {
    //            s_Cached.RemoveAt(i);
    //        }
    //        else
    //        {
    //            i++;
    //        }
    //    }

    //    return s_Cached.ToArray();
    //}


    //public static string MakeRelativePath(string basePath, string filePath)
    //{
    //    var basePathComponents = basePath.Split(s_PathSeparators, StringSplitOptions.RemoveEmptyEntries);
    //    var filePathComponents = filePath.Split(s_PathSeparators, StringSplitOptions.RemoveEmptyEntries);

    //    int commonParts;

    //    for (commonParts = 0;
    //        commonParts < basePathComponents.Length && commonParts < filePathComponents.Length;
    //        commonParts++)
    //    {
    //        if (basePathComponents[commonParts].ToLower() != filePathComponents[commonParts].ToLower())
    //            break;
    //    }

    //    var relativePathParts = new List<string>();
    //    var parentDirCount = basePathComponents.Length - commonParts;

    //    for (var i = 0; i < parentDirCount; i++)
    //        relativePathParts.Add("..");

    //    for (var i = commonParts; i < filePathComponents.Length; i++)
    //        relativePathParts.Add(filePathComponents[i]);

    //    return string.Join("/", relativePathParts.ToArray());
    //}
}
