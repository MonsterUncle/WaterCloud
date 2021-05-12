using System;

namespace WaterCloud.Code
{
    /// <summary>
    /// ID 生成器
    /// </summary>
    public static class IDGenerator
    {
        /// <summary>
        /// 生成唯一 ID
        /// </summary>
        /// <param name="idGeneratorOptions"></param>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static object NextID(object idGeneratorOptions)
        {
            return ((IDistributedIDGenerator)GlobalContext.ServiceProvider.GetService(typeof(IDistributedIDGenerator))).Create(idGeneratorOptions);
        }

        /// <summary>
        /// 生成连续 GUID
        /// </summary>
        /// <param name="guidType"></param>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static Guid NextID(SequentialGuidType guidType = SequentialGuidType.SequentialAsString)
        {
            var sequentialGuid = GlobalContext.ServiceProvider.GetService(typeof(IDistributedIDGenerator)) as IDistributedIDGenerator;
            return (Guid)sequentialGuid.Create(new SequentialGuidSettings { GuidType = guidType });
        }
    }
}