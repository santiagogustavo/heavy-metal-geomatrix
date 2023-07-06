using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class FileHandler {
    public static void SaveToJSON<T>(T content, string filename) {
        WriteFile(GetPath(filename), JsonUtility.ToJson(content));
    }

    public static T ReadFromJSON<T>(string filename) {
        string json = ReadFile(GetPath(filename));

        if (string.IsNullOrEmpty(json) || json == "{}") {
            return default(T);
        }

        T fromJson = JsonUtility.FromJson<T>(json);
        return fromJson;
    }

    public static string GetPath(string filename) {
        return Application.persistentDataPath + "/" + filename;
    }

    public static void WriteFile(string path, string content) {
        FileStream fileStream = new FileStream(path, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(fileStream)) {
            writer.Write(content);
        }
    }

    public static string ReadFile(string path) {
        if (File.Exists(path)) {
            using (StreamReader reader = new StreamReader(path)) {
                string content = reader.ReadToEnd();
                return content;
            }
        }
        return "";
    }
}
