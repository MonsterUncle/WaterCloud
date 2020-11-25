using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterCloud.Code
{
    /// <summary>
    /// Location Based Service地理位置服务类
    /// 计算左边经纬度，距离，范围查询等
    /// </summary>
    public class LBS
    {
        public const double Ea = 6378137;     //   赤道半径  
        public const double Eb = 6356725;     //   极半径 
        static double EARTH_RADIUS = 6371.0;//km 地球半径 平均值，千米


        /// <summary>
        /// 先计算查询点的经纬度范围
        /// </summary>
        /// <param name="latitude">已知纬度</param>
        /// <param name="longitude">已知纬度</param>
        /// <param name="distance">距离，单位千米</param>
        public static void GetRectRange(double latitude, double longitude,double distance,out double minlat,out double maxlat,out double minlng,out double maxlng)
        {
           
            //先计算查询点的经纬度范围  
            double r = EARTH_RADIUS;//地球半径千米  
            double dis = distance;//0.5千米距离  
            double dlng = 2 * Math.Asin(Math.Sin(dis / (2 * r)) / Math.Cos(latitude * Math.PI / 180));
            dlng = dlng * 180 / Math.PI;//角度转为弧度  
            double dlat = dis / r;
            dlat = dlat * 180 / Math.PI;
            minlat = latitude - dlat;
            maxlat = latitude + dlat;
            minlng = longitude - dlng;
            maxlng = longitude + dlng;

            //得出四个点的坐标：
            //left - top : (lat + dlat, lng – dlng)
            //right - top : (lat + dlat, lng + dlng)
            //left - bottom : (lat – dlat, lng – dlng)
            //right - bottom: (lat – dlat, lng + dlng)
            //综合也就是这样进行筛选查询

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="theta"></param>
        /// <returns></returns>
        private static double HaverSin(double theta)
        {
            //Haversine名字来历是Ha - VERSINE，即Half - Versine ，表示sin的一半的意思。
            //hav(A) = (1 - cos(A)) / 2 = sin(A / 2) * sin(A / 2)
            var v = Math.Sin(theta / 2);
            return v * v;
        }

        /// <summary>
        /// 给定的经度1，纬度1；经度2，纬度2. 计算2个经纬度之间的距离。
        /// </summary>
        /// <param name="lat1">经度1</param>
        /// <param name="lon1">纬度1</param>
        /// <param name="lat2">经度2</param>
        /// <param name="lon2">纬度2</param>
        /// <returns>距离（公里、千米）</returns>
        public static double GetDistance(double lat1, double lon1, double lat2, double lon2)
        {
            //用haversine公式计算球面两点间的距离。
            //经纬度转换成弧度
            lat1 = ConvertDegreesToRadians(lat1);
            lon1 = ConvertDegreesToRadians(lon1);
            lat2 = ConvertDegreesToRadians(lat2);
            lon2 = ConvertDegreesToRadians(lon2);

            //差值
            var vLon = Math.Abs(lon1 - lon2);
            var vLat = Math.Abs(lat1 - lat2);

            //h is the great circle distance in radians, great circle就是一个球体上的切面，它的圆心即是球心的一个周长最大的圆。
            var h = HaverSin(vLat) + Math.Cos(lat1) * Math.Cos(lat2) * HaverSin(vLon);

            var distance = 2 * EARTH_RADIUS * Math.Asin(Math.Sqrt(h));

            return distance;
        }

        /// <summary>
        /// 将角度换算为弧度。
        /// </summary>
        /// <param name="degrees">角度</param>
        /// <returns>弧度</returns>
        public static double ConvertDegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }

        /// <summary>
        /// 将弧度换算为角度。
        /// </summary>
        /// <param name="radian"></param>
        /// <returns></returns>
        public static double ConvertRadiansToDegrees(double radian)
        {
            return radian * 180.0 / Math.PI;
        }

        /// <summary>
        /// 方法二：根据经纬度及距离，获取4点范围坐标（left,top,right,bottom)
        /// 以正北方（due north）为起始角度，顺时针方向，求得maxLatitude, minLatitude, maxLongitude, minLongitude 这样一个MBR
        /// </summary>
        /// <param name="centorlatitude"></param>
        /// <param name="centorLogitude"></param>
        /// <param name="distance"></param>
        /// <param name="maxLatitude"></param>
        /// <param name="minLatitude"></param>
        /// <param name="maxLongitude"></param>
        /// <param name="minLongitude"></param>
        public static void GetRectRange2(double centorlatitude, double centorLogitude, double distance, out double maxLatitude, out double minLatitude, out double maxLongitude, out double minLongitude)
        {
            double temp = 0.0;
            GetlatLon(centorlatitude, centorLogitude, distance, 0, out temp, out maxLatitude);
            GetlatLon(centorlatitude, centorLogitude, distance, 180, out temp, out minLatitude);
            GetlatLon(centorlatitude, centorLogitude, distance, 90, out maxLongitude, out temp);
            GetlatLon(centorlatitude, centorLogitude, distance, 270, out minLongitude, out temp);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LAT"></param>
        /// <param name="LON"></param>
        /// <param name="distance"></param>
        /// <param name="angle"></param>
        /// <param name="newLon"></param>
        /// <param name="newLat"></param>
        private static void GetlatLon(double LAT, double LON, double distance, double angle, out double newLon, out double newLat)
        {
            double dx = distance * 1000 * Math.Sin(angle * Math.PI / 180.0);

            double dy = distance * 1000 * Math.Cos(angle * Math.PI / 180.0);

            double ec = Eb + (Ea - Eb) * (90.0 - LAT) / 90.0;

            double ed = ec * Math.Cos(LAT * Math.PI / 180);

            newLon = (dx / ed + LON * Math.PI / 180.0) * 180.0 / Math.PI;

            newLat = (dy / ec + LAT * Math.PI / 180.0) * 180.0 / Math.PI;
        }

        
    }
}
