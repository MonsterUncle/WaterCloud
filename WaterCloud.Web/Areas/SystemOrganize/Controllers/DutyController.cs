/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/

using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using MiniExcelLibs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WaterCloud.Code;
using WaterCloud.Domain.SystemOrganize;
using WaterCloud.Service.SystemOrganize;

namespace WaterCloud.Web.Areas.SystemOrganize.Controllers
{
	[Area("SystemOrganize")]
	public class DutyController : BaseController
	{
		public DutyService _service { get; set; }
		public SystemSetService _setService { get; set; }

		public virtual ActionResult Import()
		{
			return View();
		}

		[HandlerAjaxOnly]
		[IgnoreAntiforgeryToken]
		public async Task<ActionResult> GetGridJson(SoulPage<RoleExtend> pagination, string keyword)
		{
			if (string.IsNullOrEmpty(pagination.field))
			{
				pagination.field = "F_Id";
				pagination.order = "desc";
			}
			var data = await _service.GetLookList(pagination, keyword);
			return Content(pagination.setData(data).ToJson());
		}

		[HttpGet]
		[HandlerAjaxOnly]
		public async Task<ActionResult> GetListJson(string keyword)
		{
			var data = await _service.GetList(keyword);
			return Content(data.ToJson());
		}

		[HttpGet]
		[HandlerAjaxOnly]
		public async Task<ActionResult> GetFormJson(string keyValue)
		{
			var data = await _service.GetLookForm(keyValue);
			return Content(data.ToJson());
		}

		[HttpPost]
		[HandlerAjaxOnly]
		public async Task<ActionResult> CheckFile()
		{
			try
			{
				//获取文件参数，创建临时文件，使用完成就删除
				var files = HttpContext.Request.Form.Files;
				long size = files.Sum(f => f.Length);
				if (size > 104857600)
				{
					throw new Exception("文件大小必须小于100M");
				}
				var file = files.First();
				var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
				if (string.IsNullOrEmpty(fileName))
				{
					throw new Exception("文件不存在");
				}
				if (!FileHelper.IsExcel(fileName))
				{
					throw new Exception("请上传Excel");
				}
				string filePath = GlobalContext.HostingEnvironment.WebRootPath + $@"/" + "file" + $@"/";
				fileName = Utils.CreateNo() + fileName.Substring(fileName.LastIndexOf("."));
				string fileFullName = filePath + fileName;
				if (!Directory.Exists(filePath))
				{
					Directory.CreateDirectory(filePath);
				}
				using (FileStream fs = System.IO.File.Create(fileFullName))
				{
					file.CopyTo(fs);
					fs.Flush();
				}
				var data = await _service.CheckFile(fileFullName);
				return Content(new { code = 0, msg = "操作成功", data = data }.ToJson());
			}
			catch (Exception ex)
			{
				return Content(new { code = 400, msg = "操作失败," + ex.Message }.ToJson());
			}
		}

		[HttpPost]
		[HandlerAjaxOnly]
		public async Task<ActionResult> SubmitForm(RoleEntity roleEntity, string keyValue)
		{
			try
			{
				await _service.SubmitForm(roleEntity, keyValue);
				return await Success("操作成功。", "", keyValue);
			}
			catch (Exception ex)
			{
				return await Error(ex.Message, "", keyValue);
			}
		}

		[HttpPost]
		[HandlerAjaxOnly]
		[HandlerAuthorize]
		public async Task<ActionResult> DeleteForm(string keyValue)
		{
			try
			{
				await _service.DeleteForm(keyValue);
				return await Success("操作成功。", "", keyValue, DbLogType.Delete);
			}
			catch (Exception ex)
			{
				return await Error(ex.Message, "", keyValue, DbLogType.Delete);
			}
		}

		[HttpPost]
		[HandlerAjaxOnly]
		public async Task<ActionResult> ImportForm(string listData)
		{
			var filterList = JsonConvert.DeserializeObject<List<RoleEntity>>(listData);
			if (filterList == null || filterList.Count == 0)
			{
				return Error("导入数据不存在!");
			}
			if (filterList.Where(a => a.F_EnabledMark == false).Any())
			{
				return Error("导入数据存在错误!");
			}
			try
			{
				await _service.ImportForm(filterList);
				return await Success("导入成功。", "", "");
			}
			catch (Exception ex)
			{
				return await Error("导入失败，" + ex.Message, "", "");
			}
		}

		[HttpGet]
		public async Task<FileResult> Download()
		{
			return await Task.Run(() =>
			{
				string fileName = "岗位导入模板.xlsx";
				string fileValue = "model";
				string filePath = GlobalContext.HostingEnvironment.WebRootPath + $@"/" + fileValue + $@"/" + fileName;
				if (!FileHelper.IsExistFile(filePath))
				{
					throw new Exception("文件不存在");
				}
				///定义并实例化一个内存流，以存放图片的字节数组。
				MemoryStream ms = new MemoryStream();
				///图片读入FileStream
				FileStream f = new FileStream(filePath, FileMode.Open);
				///把FileStream写入MemoryStream
				ms.SetLength(f.Length);
				f.Read(ms.GetBuffer(), 0, (int)f.Length);
				ms.Flush();
				f.Close();
				var contentType = MimeMapping.GetMimeMapping(fileName);
				return File(ms, contentType, fileName);
			});
		}

		[HttpGet]
		public async Task<FileResult> ExportExcel(string keyword = "")
		{
			var list = await _service.GetList(keyword);
			string filename = "岗位信息" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xls";
			var contentType = MimeMapping.GetMimeMapping(filename);
			var values = new List<Dictionary<string, object>>();
			var set = await _setService.GetForm(_service.currentuser.CompanyId);
			for (int i = 0; i < list.Count; i++)
			{
				var dic = new Dictionary<string, object> ();
				dic.Add("序号",i+1);
				dic.Add("岗位编号", list[i].F_EnCode);
				dic.Add("岗位名称", list[i].F_FullName);
				dic.Add("归属公司", set.F_CompanyName);
				dic.Add("有效状态", list[i].F_EnabledMark);
				dic.Add("创建时间", list[i].F_CreatorTime);
				dic.Add("备注", list[i].F_Description);
				values.Add(dic);
			}
			var memoryStream = new MemoryStream();
			memoryStream.SaveAs(values);
			memoryStream.Seek(0, SeekOrigin.Begin);
			return new FileStreamResult(memoryStream, contentType)
			{
				FileDownloadName = filename
			};
		}

		[HttpGet]
		public async Task<FileResult> Export(string keyword = "")
		{
			var list = await _service.GetList(keyword);
			//生成pdf
			string fileName = "岗位信息" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf";
			//步骤1
			Document Doc = new Document(PageSize.A4);
			Doc.SetMargins(60, 60, 20, 40);

			MemoryStream stream = new MemoryStream();
			//步骤2
			PdfWriter pdfWriter = PdfWriter.GetInstance(Doc, stream);
			//步骤3
			Doc.Open();

			#region 相关元素准备

			BaseFont bfChinese;
			bfChinese = BaseFont.CreateFont(GlobalContext.HostingEnvironment.WebRootPath + "/fonts/simsun.ttc,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
			Font Font16 = new Font(bfChinese, 16);
			Font Font14 = new Font(bfChinese, 14);
			Font Font12 = new Font(bfChinese, 12);
			Font Font12Bold = new Font(bfChinese, 12, Font.BOLD);
			Font Font12Italic = new Font(bfChinese, 12, Font.BOLDITALIC);
			Font Font10Bold = new Font(bfChinese, 10, Font.BOLD);

			Paragraph parag;
			//Chunk chunk;
			PdfPTable table;

			#endregion 相关元素准备

			#region 文件标题

			Doc.Open();
			Doc.AddAuthor(_service.currentuser.UserName);
			Doc.AddTitle("岗位信息");

			#endregion 文件标题

			#region 正文

			parag = new Paragraph("岗位信息\r\n\r\n", Font16);
			parag.Alignment = Element.ALIGN_CENTER;
			Doc.Add(parag);

			table = new PdfPTable(new float[] { 5, 5, 5, 5, 5, 5, 5 });
			table.WidthPercentage = 100f;
			table.AddCell(new Phrase("序号", Font12Bold));
			table.AddCell(new Phrase("岗位编号", Font12Bold));
			table.AddCell(new Phrase("岗位名称", Font12Bold));
			table.AddCell(new Phrase("归属公司", Font12Bold));
			table.AddCell(new Phrase("有效状态", Font12Bold));
			table.AddCell(new Phrase("创建时间", Font12Bold));
			table.AddCell(new Phrase("备注", Font12Bold));
			Doc.Add(table);

			int i = 0;
			foreach (var item in list)
			{
				i++;
				table = new PdfPTable(new float[] { 5, 5, 5, 5, 5, 5, 5 });
				table.WidthPercentage = 100f;
				table.AddCell(new Phrase(i.ToString(), Font12));
				table.AddCell(new Phrase(item.F_EnCode != null ? item.F_EnCode.ToString() : "", Font12));
				table.AddCell(new Phrase(item.F_FullName != null ? item.F_EnCode.ToString() : "", Font12));
				var set = await _setService.GetForm(_service.currentuser.CompanyId);
				table.AddCell(new Phrase(set != null ? set.F_CompanyName.ToString() : "", Font12));
				table.AddCell(new Phrase(item.F_EnabledMark != true ? "无效" : "有效", Font12));
				table.AddCell(new Phrase(item.F_CreatorTime != null ? ((DateTime)item.F_CreatorTime).ToString("yyyy-MM-dd") : "", Font12));
				table.AddCell(new Phrase(item.F_Description, Font12));
				Doc.Add(table);
			}
			table = new PdfPTable(new float[] { 35 });
			table.WidthPercentage = 100f;
			table.AddCell(new Phrase("合计:一共" + i + "项", Font12));
			table.AddCell(new Phrase("", Font12));
			Doc.Add(table);
			Doc.Close();

			#endregion 正文

			////页脚
			//PDFFooter footer = new PDFFooter();
			//footer.OnEndPage(pdfWriter, Doc);
			Doc.Close();
			var contentType = MimeMapping.GetMimeMapping(fileName);
			FileResult fileResult = new FileContentResult(stream.ToArray(), contentType);
			fileResult.FileDownloadName = fileName;
			return fileResult;
		}
	}
}