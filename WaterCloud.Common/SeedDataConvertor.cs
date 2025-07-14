using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace WaterCloud.Common;
/// <summary>
///     种子数据格式实体类,遵循Navicat导出json格式
/// </summary>
/// <typeparam name="T"></typeparam>
public class SeedDataRecords<T> {
    /// <summary>
    ///     数据
    /// </summary>
    public List<T> Records { get; init; }
}


public static class SeedDataConvertor {
    
    private static readonly JsonSerializerOptions options = new() {
        // 启用属性名不区分大小写
        PropertyNameCaseInsensitive = true,
        AllowTrailingCommas = true,
        // 允许JSON注释（// 或 /* */）
        ReadCommentHandling = JsonCommentHandling.Skip,
        // 忽略Null值属性
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
    public static List<T> GetSeedData<T>(string jsonName) {
        var jsonPath = Path.Combine(AppContext.BaseDirectory, "SeedData", "Json",jsonName); //获取文件路径
        if (!File.Exists(jsonPath)) {
            Console.WriteLine($"种子数据文件不存在: {jsonPath}");
            return [];
        }
        try {
            var jsonContent = File.ReadAllText(jsonPath);
            jsonContent = ProcessNestedJson(jsonContent);
            // 解析为包装对象
            var wrapper = JsonSerializer.Deserialize<SeedDataRecords<T>>(jsonContent, options);
            return wrapper?.Records ?? [];
        }catch (Exception ex) {
            throw new Exception($"读取种子数据{jsonName}时出错:{ex}\n");
        }
    }
    private static string ProcessNestedJson(string json) {
        if (string.IsNullOrEmpty(json)) {
            return json;
        }

        // 处理简单的嵌套JSON字符串
        const string pattern = "\"([^\"]+)\": \"\\{([^}]+)\\}\"";
        return Regex.Replace(json, pattern, "\"$1\": {$2}");
    }
}