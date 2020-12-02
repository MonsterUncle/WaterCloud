/*
 Navicat Premium Data Transfer

 Source Server         : my
 Source Server Type    : SQL Server
 Source Server Version : 15002000
 Source Host           : localhost:1433
 Source Catalog        : WaterCloudNetDb
 Source Schema         : dbo

 Target Server Type    : SQL Server
 Target Server Version : 15002000
 File Encoding         : 65001

 Date: 02/12/2020 13:21:11
*/


-- ----------------------------
-- Table structure for sys_openjoblog
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[sys_openjoblog]') AND type IN ('U'))
	DROP TABLE [dbo].[sys_openjoblog]
GO

CREATE TABLE [dbo].[sys_openjoblog] (
  [F_Id] varchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_JobId] varchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_Description] text COLLATE Chinese_PRC_CI_AS  NULL,
  [F_CreatorTime] datetime2(7)  NULL,
  [F_EnabledMark] tinyint  NOT NULL,
  [F_JobName] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[sys_openjoblog] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'任务Id',
'SCHEMA', N'dbo',
'TABLE', N'sys_openjoblog',
'COLUMN', N'F_JobId'
GO

EXEC sp_addextendedproperty
'MS_Description', N'任务信息',
'SCHEMA', N'dbo',
'TABLE', N'sys_openjoblog',
'COLUMN', N'F_Description'
GO

EXEC sp_addextendedproperty
'MS_Description', N'执行时间',
'SCHEMA', N'dbo',
'TABLE', N'sys_openjoblog',
'COLUMN', N'F_CreatorTime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'执行状态',
'SCHEMA', N'dbo',
'TABLE', N'sys_openjoblog',
'COLUMN', N'F_EnabledMark'
GO

EXEC sp_addextendedproperty
'MS_Description', N'任务名称',
'SCHEMA', N'dbo',
'TABLE', N'sys_openjoblog',
'COLUMN', N'F_JobName'
GO


-- ----------------------------
-- Records of sys_openjoblog
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table sys_openjoblog
-- ----------------------------
ALTER TABLE [dbo].[sys_openjoblog] ADD CONSTRAINT [PK__sys_open__2C6EC723BDA80F55] PRIMARY KEY CLUSTERED ([F_Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO
INSERT INTO [dbo].[sys_modulebutton]([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'2ec7f52c-43b7-45bb-8714-17511817d1b8', N'e3188a69-de3a-40ef-a5ff-5eaf460f5d20', N'0', 1, N'NF-log', N'日志', NULL, 1, N'log', N'/SystemSecurity/OpenJobs/Details', NULL, 0, 0, 0, 6, 0, 1, N'', '2020-12-02 12:17:27.1800000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL);


