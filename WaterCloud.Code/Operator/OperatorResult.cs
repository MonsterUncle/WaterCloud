namespace WaterCloud.Code
{
	public class OperatorResult
	{
		/// <summary>
		/// 状态码-1未登录,1登录成功,0登录过期
		/// </summary>
		public int stateCode { get; set; }

		/// <summary>
		/// 登录者用户信息
		/// </summary>
		public OperatorModel userInfo { get; set; }
	}
}