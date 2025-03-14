using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace Utilities.Helpers;

public static class BaseHelper
{
    private static readonly JsonSerializerOptions jsonSerializerOptions
         = new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
    public static T GetJson<T>(string path)
        => JsonSerializer.Deserialize<T>(File.ReadAllText(path, Encoding.Default), options: jsonSerializerOptions);

    public static string GetRootPath()
        => Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, @"../../../"));
}
