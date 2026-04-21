using PeinRecoilRework.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace SPTBattleAmbience.Helpers
{
    public static class FileHelper
    {
        public static string PluginDirectory => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static string AssetsPath => Path.Combine(PluginDirectory, "assets");
        public static string SoundsPath => Path.Combine(AssetsPath, "sounds");
        public static string ConfigsPath => Path.Combine(AssetsPath, "configs");
        public static string MapConfigsPath => Path.Combine(ConfigsPath, "maps");

        public static string[] ReadDirectories(string path)
        {
            return Directory.GetDirectories(path);
        }

        public static string[] ReadFiles(string path, string searchPattern)
        {
            return Directory.GetFiles(path, searchPattern);
        }

        public static async Task<AudioClip> LoadAudioClip(string path, string fileName)
        {
            string finalPath = Path.Combine(path, fileName);

            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip($"file://{finalPath}", AudioType.WAV))
            {
                UnityWebRequestAsyncOperation request =  www.SendWebRequest();
                while (!request.isDone) await Task.Yield();

                if (www.isHttpError || www.isNetworkError)
                {
                    DebugLogger.LogError($"Error loading audio clip from {fileName}: {www.error}");
                }
                else
                {
                    AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                    clip.name = fileName;

                    return clip;
                }
            }

            return null;
        }

        public static async Task<Dictionary<string, AudioClip>> LoadAudioClipsFromDirectory(string path)
        {
            string[] filePaths = ReadFiles(path, "*.wav");
            Dictionary<string, AudioClip> clips = [];

            foreach (string filePath in filePaths)
            {
                string fileName = Path.GetFileName(filePath);
                AudioClip clip = await LoadAudioClip(path, fileName);

                clips[fileName] = clip;
            }

            return clips;
        }
    }
}
