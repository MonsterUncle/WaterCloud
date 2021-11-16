/*
 Navicat Premium Data Transfer

 Source Server         : 常熟mssql
 Source Server Type    : SQL Server
 Source Server Version : 13001601
 Source Host           : 58.211.68.202:2433
 Source Catalog        : WaterCloudNetDb
 Source Schema         : dbo

 Target Server Type    : SQL Server
 Target Server Version : 13001601
 File Encoding         : 65001

 Date: 16/11/2021 11:38:47
*/


-- ----------------------------
-- Table structure for sys_openjob
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[sys_openjob]') AND type IN ('U'))
	DROP TABLE [dbo].[sys_openjob]
GO

CREATE TABLE [dbo].[sys_openjob] (
  [F_Id] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_FileName] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_JobName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_JobGroup] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_StarRunTime] datetime2(7)  NULL,
  [F_EndRunTime] datetime2(7)  NULL,
  [F_CronExpress] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_DeleteMark] tinyint  NULL,
  [F_EnabledMark] tinyint  NULL,
  [F_Description] nvarchar(500) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_CreatorTime] datetime2(7)  NULL,
  [F_CreatorUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_LastModifyTime] datetime2(7)  NULL,
  [F_LastModifyUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_DeleteTime] datetime2(7)  NULL,
  [F_DeleteUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_LastRunTime] datetime2(7)  NULL,
  [F_LastRunMark] tinyint  NULL,
  [F_LastRunErrTime] datetime2(7)  NULL,
  [F_LastRunErrMsg] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_JobType] int  NOT NULL,
  [F_IsLog] char(2) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_RequestHeaders] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_RequestString] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_RequestUrl] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_DbNumber] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[sys_openjob] SET (LOCK_ESCALATION = TABLE)
GO

