﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WaterCloud.Service;

namespace WaterCloud.WebApi
{
    public static class DataServiceExtension
    {
        /// <summary>
        /// 注入数据
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddDataService(this IServiceCollection services)
        {
            #region 依赖注入
            var baseType = typeof(IDenpendency);
            var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
            var referencedAssemblies = Directory.GetFiles(path, "*.dll").Select(Assembly.LoadFrom).ToArray();
            var types = referencedAssemblies
                .SelectMany(a => a.DefinedTypes)
                .Select(type => type.AsType())
                .Where(x => x != baseType && baseType.IsAssignableFrom(x)).ToArray();
            var implementTypes = types.Where(x => x.IsClass).ToArray();
            var interfaceTypes = types.Where(x => x.IsInterface).ToArray();
            foreach (var implementType in implementTypes)
            {
                var interfaceType = interfaceTypes.FirstOrDefault(x => x.IsAssignableFrom(implementType));
                if (interfaceType != null)
                {
                    //接口服务
                    services.AddScoped(interfaceType, implementType);
                }
                else
                {
                    //只有实现服务（项目只有实现）
                    services.AddScoped(implementType);
                }
            }
            #endregion

            return services;
        }
    }
}
