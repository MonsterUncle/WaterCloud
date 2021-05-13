/*
 Navicat Premium Data Transfer

 Source Server         : 本地mssql
 Source Server Type    : SQL Server
 Source Server Version : 15002000
 Source Host           : localhost:1433
 Source Catalog        : WaterCloudNetDb
 Source Schema         : dbo

 Target Server Type    : SQL Server
 Target Server Version : 15002000
 File Encoding         : 65001

 Date: 13/05/2021 14:23:34
*/


-- ----------------------------
-- Table structure for cms_articlecategory
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_articlecategory]') AND type IN ('U'))
	DROP TABLE [dbo].[cms_articlecategory]
GO

CREATE TABLE [dbo].[cms_articlecategory] (
  [F_Id] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_FullName] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_ParentId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_SortCode] int  NOT NULL,
  [F_Description] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_LinkUrl] nvarchar(255) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_ImgUrl] nvarchar(255) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_SeoTitle] nvarchar(255) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_SeoKeywords] nvarchar(255) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_SeoDescription] nvarchar(255) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_IsHot] tinyint  NULL,
  [F_EnabledMark] tinyint  NULL,
  [F_DeleteMark] tinyint  NULL,
  [F_CreatorTime] datetime2(7)  NULL,
  [F_CreatorUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_LastModifyTime] datetime2(7)  NULL,
  [F_LastModifyUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_DeleteTime] datetime2(7)  NULL,
  [F_DeleteUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[cms_articlecategory] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'主键Id',
'SCHEMA', N'dbo',
'TABLE', N'cms_articlecategory',
'COLUMN', N'F_Id'
GO

EXEC sp_addextendedproperty
'MS_Description', N'类别名称',
'SCHEMA', N'dbo',
'TABLE', N'cms_articlecategory',
'COLUMN', N'F_FullName'
GO

EXEC sp_addextendedproperty
'MS_Description', N'父级Id',
'SCHEMA', N'dbo',
'TABLE', N'cms_articlecategory',
'COLUMN', N'F_ParentId'
GO

EXEC sp_addextendedproperty
'MS_Description', N'排序',
'SCHEMA', N'dbo',
'TABLE', N'cms_articlecategory',
'COLUMN', N'F_SortCode'
GO

EXEC sp_addextendedproperty
'MS_Description', N'描述',
'SCHEMA', N'dbo',
'TABLE', N'cms_articlecategory',
'COLUMN', N'F_Description'
GO

EXEC sp_addextendedproperty
'MS_Description', N'链接地址',
'SCHEMA', N'dbo',
'TABLE', N'cms_articlecategory',
'COLUMN', N'F_LinkUrl'
GO

EXEC sp_addextendedproperty
'MS_Description', N'图片地址',
'SCHEMA', N'dbo',
'TABLE', N'cms_articlecategory',
'COLUMN', N'F_ImgUrl'
GO

EXEC sp_addextendedproperty
'MS_Description', N'SEO标题',
'SCHEMA', N'dbo',
'TABLE', N'cms_articlecategory',
'COLUMN', N'F_SeoTitle'
GO

EXEC sp_addextendedproperty
'MS_Description', N'SEO关键字',
'SCHEMA', N'dbo',
'TABLE', N'cms_articlecategory',
'COLUMN', N'F_SeoKeywords'
GO

EXEC sp_addextendedproperty
'MS_Description', N'SEO描述',
'SCHEMA', N'dbo',
'TABLE', N'cms_articlecategory',
'COLUMN', N'F_SeoDescription'
GO

EXEC sp_addextendedproperty
'MS_Description', N'是否热门',
'SCHEMA', N'dbo',
'TABLE', N'cms_articlecategory',
'COLUMN', N'F_IsHot'
GO

EXEC sp_addextendedproperty
'MS_Description', N'是否启用',
'SCHEMA', N'dbo',
'TABLE', N'cms_articlecategory',
'COLUMN', N'F_EnabledMark'
GO

EXEC sp_addextendedproperty
'MS_Description', N'删除标志',
'SCHEMA', N'dbo',
'TABLE', N'cms_articlecategory',
'COLUMN', N'F_DeleteMark'
GO


-- ----------------------------
-- Records of cms_articlecategory
-- ----------------------------

-- ----------------------------
-- Table structure for cms_articlenews
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_articlenews]') AND type IN ('U'))
	DROP TABLE [dbo].[cms_articlenews]
GO

CREATE TABLE [dbo].[cms_articlenews] (
  [F_Id] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_CategoryId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_Title] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_LinkUrl] nvarchar(255) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_ImgUrl] nvarchar(255) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_SeoTitle] nvarchar(255) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_SeoKeywords] nvarchar(255) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_SeoDescription] nvarchar(255) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_Tags] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_Zhaiyao] nvarchar(255) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_Description] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_SortCode] int  NULL,
  [F_IsTop] tinyint  NULL,
  [F_IsHot] tinyint  NULL,
  [F_IsRed] tinyint  NULL,
  [F_Click] int  NULL,
  [F_Source] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_Author] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_EnabledMark] tinyint  NULL,
  [F_DeleteMark] tinyint  NULL,
  [F_CreatorTime] datetime2(7)  NULL,
  [F_CreatorUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_LastModifyTime] datetime2(7)  NULL,
  [F_LastModifyUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_DeleteTime] datetime2(7)  NULL,
  [F_DeleteUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[cms_articlenews] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'文章主键Id',
'SCHEMA', N'dbo',
'TABLE', N'cms_articlenews',
'COLUMN', N'F_Id'
GO

EXEC sp_addextendedproperty
'MS_Description', N'类别Id',
'SCHEMA', N'dbo',
'TABLE', N'cms_articlenews',
'COLUMN', N'F_CategoryId'
GO

EXEC sp_addextendedproperty
'MS_Description', N'标题',
'SCHEMA', N'dbo',
'TABLE', N'cms_articlenews',
'COLUMN', N'F_Title'
GO

EXEC sp_addextendedproperty
'MS_Description', N'链接地址',
'SCHEMA', N'dbo',
'TABLE', N'cms_articlenews',
'COLUMN', N'F_LinkUrl'
GO

EXEC sp_addextendedproperty
'MS_Description', N'图片地址',
'SCHEMA', N'dbo',
'TABLE', N'cms_articlenews',
'COLUMN', N'F_ImgUrl'
GO

EXEC sp_addextendedproperty
'MS_Description', N'SEO标题',
'SCHEMA', N'dbo',
'TABLE', N'cms_articlenews',
'COLUMN', N'F_SeoTitle'
GO

EXEC sp_addextendedproperty
'MS_Description', N'SEO关键字',
'SCHEMA', N'dbo',
'TABLE', N'cms_articlenews',
'COLUMN', N'F_SeoKeywords'
GO

EXEC sp_addextendedproperty
'MS_Description', N'SEO描述',
'SCHEMA', N'dbo',
'TABLE', N'cms_articlenews',
'COLUMN', N'F_SeoDescription'
GO

EXEC sp_addextendedproperty
'MS_Description', N'标签',
'SCHEMA', N'dbo',
'TABLE', N'cms_articlenews',
'COLUMN', N'F_Tags'
GO

EXEC sp_addextendedproperty
'MS_Description', N'摘要',
'SCHEMA', N'dbo',
'TABLE', N'cms_articlenews',
'COLUMN', N'F_Zhaiyao'
GO

EXEC sp_addextendedproperty
'MS_Description', N'内容',
'SCHEMA', N'dbo',
'TABLE', N'cms_articlenews',
'COLUMN', N'F_Description'
GO

EXEC sp_addextendedproperty
'MS_Description', N'排序',
'SCHEMA', N'dbo',
'TABLE', N'cms_articlenews',
'COLUMN', N'F_SortCode'
GO

EXEC sp_addextendedproperty
'MS_Description', N'是否置顶',
'SCHEMA', N'dbo',
'TABLE', N'cms_articlenews',
'COLUMN', N'F_IsTop'
GO

EXEC sp_addextendedproperty
'MS_Description', N'是否热门',
'SCHEMA', N'dbo',
'TABLE', N'cms_articlenews',
'COLUMN', N'F_IsHot'
GO

EXEC sp_addextendedproperty
'MS_Description', N'点击次数',
'SCHEMA', N'dbo',
'TABLE', N'cms_articlenews',
'COLUMN', N'F_Click'
GO

EXEC sp_addextendedproperty
'MS_Description', N'来源',
'SCHEMA', N'dbo',
'TABLE', N'cms_articlenews',
'COLUMN', N'F_Source'
GO

EXEC sp_addextendedproperty
'MS_Description', N'作者',
'SCHEMA', N'dbo',
'TABLE', N'cms_articlenews',
'COLUMN', N'F_Author'
GO

EXEC sp_addextendedproperty
'MS_Description', N'是否启用',
'SCHEMA', N'dbo',
'TABLE', N'cms_articlenews',
'COLUMN', N'F_EnabledMark'
GO

EXEC sp_addextendedproperty
'MS_Description', N'逻辑删除标志',
'SCHEMA', N'dbo',
'TABLE', N'cms_articlenews',
'COLUMN', N'F_DeleteMark'
GO

EXEC sp_addextendedproperty
'MS_Description', N'创建时间',
'SCHEMA', N'dbo',
'TABLE', N'cms_articlenews',
'COLUMN', N'F_CreatorTime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'创建人',
'SCHEMA', N'dbo',
'TABLE', N'cms_articlenews',
'COLUMN', N'F_CreatorUserId'
GO

EXEC sp_addextendedproperty
'MS_Description', N'最后修改时间',
'SCHEMA', N'dbo',
'TABLE', N'cms_articlenews',
'COLUMN', N'F_LastModifyTime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'最后修改人',
'SCHEMA', N'dbo',
'TABLE', N'cms_articlenews',
'COLUMN', N'F_LastModifyUserId'
GO

EXEC sp_addextendedproperty
'MS_Description', N'删除时间',
'SCHEMA', N'dbo',
'TABLE', N'cms_articlenews',
'COLUMN', N'F_DeleteTime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'删除人',
'SCHEMA', N'dbo',
'TABLE', N'cms_articlenews',
'COLUMN', N'F_DeleteUserId'
GO


-- ----------------------------
-- Records of cms_articlenews
-- ----------------------------

-- ----------------------------
-- Table structure for oms_flowinstance
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[oms_flowinstance]') AND type IN ('U'))
	DROP TABLE [dbo].[oms_flowinstance]
GO

CREATE TABLE [dbo].[oms_flowinstance] (
  [F_Id] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_InstanceSchemeId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_Code] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_CustomName] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_ActivityId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_ActivityType] int  NULL,
  [F_ActivityName] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_PreviousId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_SchemeContent] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_SchemeId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_DbName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_FrmData] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_FrmType] int  NOT NULL,
  [F_FrmContentData] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_FrmContentParse] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_FrmId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_SchemeType] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_FlowLevel] int  NOT NULL,
  [F_Description] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_IsFinish] int  NOT NULL,
  [F_MakerList] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_OrganizeId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_EnabledMark] tinyint  NULL,
  [F_CreatorTime] datetime2(7)  NULL,
  [F_CreatorUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_CreatorUserName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_FrmContent] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[oms_flowinstance] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'主键Id',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstance',
'COLUMN', N'F_Id'
GO

EXEC sp_addextendedproperty
'MS_Description', N'流程实例模板Id',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstance',
'COLUMN', N'F_InstanceSchemeId'
GO

EXEC sp_addextendedproperty
'MS_Description', N'实例编号',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstance',
'COLUMN', N'F_Code'
GO

EXEC sp_addextendedproperty
'MS_Description', N'自定义名称',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstance',
'COLUMN', N'F_CustomName'
GO

EXEC sp_addextendedproperty
'MS_Description', N'当前节点ID',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstance',
'COLUMN', N'F_ActivityId'
GO

EXEC sp_addextendedproperty
'MS_Description', N'当前节点类型（0会签节点）',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstance',
'COLUMN', N'F_ActivityType'
GO

EXEC sp_addextendedproperty
'MS_Description', N'当前节点名称',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstance',
'COLUMN', N'F_ActivityName'
GO

EXEC sp_addextendedproperty
'MS_Description', N'前一个ID',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstance',
'COLUMN', N'F_PreviousId'
GO

EXEC sp_addextendedproperty
'MS_Description', N'流程模板内容',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstance',
'COLUMN', N'F_SchemeContent'
GO

EXEC sp_addextendedproperty
'MS_Description', N'流程模板ID',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstance',
'COLUMN', N'F_SchemeId'
GO

EXEC sp_addextendedproperty
'MS_Description', N'数据库名称',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstance',
'COLUMN', N'F_DbName'
GO

EXEC sp_addextendedproperty
'MS_Description', N'表单数据',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstance',
'COLUMN', N'F_FrmData'
GO

EXEC sp_addextendedproperty
'MS_Description', N'表单类型',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstance',
'COLUMN', N'F_FrmType'
GO

EXEC sp_addextendedproperty
'MS_Description', N'表单字段',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstance',
'COLUMN', N'F_FrmContentData'
GO

EXEC sp_addextendedproperty
'MS_Description', N'表单参数（冗余）',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstance',
'COLUMN', N'F_FrmContentParse'
GO

EXEC sp_addextendedproperty
'MS_Description', N'表单ID',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstance',
'COLUMN', N'F_FrmId'
GO

EXEC sp_addextendedproperty
'MS_Description', N'流程类型',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstance',
'COLUMN', N'F_SchemeType'
GO

EXEC sp_addextendedproperty
'MS_Description', N'等级',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstance',
'COLUMN', N'F_FlowLevel'
GO

EXEC sp_addextendedproperty
'MS_Description', N'实例备注',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstance',
'COLUMN', N'F_Description'
GO

EXEC sp_addextendedproperty
'MS_Description', N'是否完成',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstance',
'COLUMN', N'F_IsFinish'
GO

EXEC sp_addextendedproperty
'MS_Description', N'执行人',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstance',
'COLUMN', N'F_MakerList'
GO

EXEC sp_addextendedproperty
'MS_Description', N'所属部门',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstance',
'COLUMN', N'F_OrganizeId'
GO

EXEC sp_addextendedproperty
'MS_Description', N'有效',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstance',
'COLUMN', N'F_EnabledMark'
GO

EXEC sp_addextendedproperty
'MS_Description', N'创建时间',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstance',
'COLUMN', N'F_CreatorTime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'创建用户主键',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstance',
'COLUMN', N'F_CreatorUserId'
GO

EXEC sp_addextendedproperty
'MS_Description', N'创建用户',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstance',
'COLUMN', N'F_CreatorUserName'
GO

EXEC sp_addextendedproperty
'MS_Description', N'表单元素json',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstance',
'COLUMN', N'F_FrmContent'
GO

EXEC sp_addextendedproperty
'MS_Description', N'工作流流程实例表',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstance'
GO


-- ----------------------------
-- Records of oms_flowinstance
-- ----------------------------

-- ----------------------------
-- Table structure for oms_flowinstancehis
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[oms_flowinstancehis]') AND type IN ('U'))
	DROP TABLE [dbo].[oms_flowinstancehis]
GO

CREATE TABLE [dbo].[oms_flowinstancehis] (
  [F_Id] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_InstanceId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_FromNodeId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_FromNodeType] int  NULL,
  [F_FromNodeName] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_ToNodeId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_ToNodeType] int  NULL,
  [F_ToNodeName] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_TransitionSate] tinyint  NOT NULL,
  [F_IsFinish] tinyint  NOT NULL,
  [F_CreatorTime] datetime2(7)  NOT NULL,
  [F_CreatorUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_CreatorUserName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[oms_flowinstancehis] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'主键Id',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstancehis',
'COLUMN', N'F_Id'
GO

EXEC sp_addextendedproperty
'MS_Description', N'实例Id',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstancehis',
'COLUMN', N'F_InstanceId'
GO

EXEC sp_addextendedproperty
'MS_Description', N'开始节点Id',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstancehis',
'COLUMN', N'F_FromNodeId'
GO

EXEC sp_addextendedproperty
'MS_Description', N'开始节点类型',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstancehis',
'COLUMN', N'F_FromNodeType'
GO

EXEC sp_addextendedproperty
'MS_Description', N'开始节点名称',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstancehis',
'COLUMN', N'F_FromNodeName'
GO

EXEC sp_addextendedproperty
'MS_Description', N'结束节点Id',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstancehis',
'COLUMN', N'F_ToNodeId'
GO

EXEC sp_addextendedproperty
'MS_Description', N'结束节点类型',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstancehis',
'COLUMN', N'F_ToNodeType'
GO

EXEC sp_addextendedproperty
'MS_Description', N'结束节点名称',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstancehis',
'COLUMN', N'F_ToNodeName'
GO

EXEC sp_addextendedproperty
'MS_Description', N'转化状态',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstancehis',
'COLUMN', N'F_TransitionSate'
GO

EXEC sp_addextendedproperty
'MS_Description', N'是否结束',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstancehis',
'COLUMN', N'F_IsFinish'
GO

EXEC sp_addextendedproperty
'MS_Description', N'转化时间',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstancehis',
'COLUMN', N'F_CreatorTime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'操作人Id',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstancehis',
'COLUMN', N'F_CreatorUserId'
GO

EXEC sp_addextendedproperty
'MS_Description', N'操作人名称',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstancehis',
'COLUMN', N'F_CreatorUserName'
GO

EXEC sp_addextendedproperty
'MS_Description', N'工作流实例流转历史记录',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstancehis'
GO


-- ----------------------------
-- Records of oms_flowinstancehis
-- ----------------------------

-- ----------------------------
-- Table structure for oms_flowinstanceinfo
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[oms_flowinstanceinfo]') AND type IN ('U'))
	DROP TABLE [dbo].[oms_flowinstanceinfo]
GO

CREATE TABLE [dbo].[oms_flowinstanceinfo] (
  [F_Id] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_InstanceId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_Content] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_CreatorTime] datetime2(7)  NOT NULL,
  [F_CreatorUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_CreatorUserName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[oms_flowinstanceinfo] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'主键Id',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstanceinfo',
'COLUMN', N'F_Id'
GO

EXEC sp_addextendedproperty
'MS_Description', N'实例进程Id',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstanceinfo',
'COLUMN', N'F_InstanceId'
GO

EXEC sp_addextendedproperty
'MS_Description', N'操作内容',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstanceinfo',
'COLUMN', N'F_Content'
GO

EXEC sp_addextendedproperty
'MS_Description', N'创建时间',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstanceinfo',
'COLUMN', N'F_CreatorTime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'创建用户主键',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstanceinfo',
'COLUMN', N'F_CreatorUserId'
GO

EXEC sp_addextendedproperty
'MS_Description', N'创建用户',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstanceinfo',
'COLUMN', N'F_CreatorUserName'
GO

EXEC sp_addextendedproperty
'MS_Description', N'工作流实例操作记录',
'SCHEMA', N'dbo',
'TABLE', N'oms_flowinstanceinfo'
GO


-- ----------------------------
-- Records of oms_flowinstanceinfo
-- ----------------------------

-- ----------------------------
-- Table structure for oms_formtest
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[oms_formtest]') AND type IN ('U'))
	DROP TABLE [dbo].[oms_formtest]
GO

CREATE TABLE [dbo].[oms_formtest] (
  [F_Id] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_UserName] nvarchar(10) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_RequestType] nvarchar(20) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_StartTime] datetime2(7)  NULL,
  [F_EndTime] datetime2(7)  NULL,
  [F_RequestComment] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_Attachment] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_CreatorTime] datetime2(7)  NOT NULL,
  [F_CreatorUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_CreatorUserName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_FlowInstanceId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[oms_formtest] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'ID',
'SCHEMA', N'dbo',
'TABLE', N'oms_formtest',
'COLUMN', N'F_Id'
GO

EXEC sp_addextendedproperty
'MS_Description', N'请假人姓名',
'SCHEMA', N'dbo',
'TABLE', N'oms_formtest',
'COLUMN', N'F_UserName'
GO

EXEC sp_addextendedproperty
'MS_Description', N'请假分类，病假，事假，公休等',
'SCHEMA', N'dbo',
'TABLE', N'oms_formtest',
'COLUMN', N'F_RequestType'
GO

EXEC sp_addextendedproperty
'MS_Description', N'开始时间',
'SCHEMA', N'dbo',
'TABLE', N'oms_formtest',
'COLUMN', N'F_StartTime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'结束时间',
'SCHEMA', N'dbo',
'TABLE', N'oms_formtest',
'COLUMN', N'F_EndTime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'请假说明',
'SCHEMA', N'dbo',
'TABLE', N'oms_formtest',
'COLUMN', N'F_RequestComment'
GO

EXEC sp_addextendedproperty
'MS_Description', N'附件，用于提交病假证据等',
'SCHEMA', N'dbo',
'TABLE', N'oms_formtest',
'COLUMN', N'F_Attachment'
GO

EXEC sp_addextendedproperty
'MS_Description', N'创建时间',
'SCHEMA', N'dbo',
'TABLE', N'oms_formtest',
'COLUMN', N'F_CreatorTime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'创建用户主键',
'SCHEMA', N'dbo',
'TABLE', N'oms_formtest',
'COLUMN', N'F_CreatorUserId'
GO

EXEC sp_addextendedproperty
'MS_Description', N'创建用户',
'SCHEMA', N'dbo',
'TABLE', N'oms_formtest',
'COLUMN', N'F_CreatorUserName'
GO

EXEC sp_addextendedproperty
'MS_Description', N'所属流程ID',
'SCHEMA', N'dbo',
'TABLE', N'oms_formtest',
'COLUMN', N'F_FlowInstanceId'
GO

EXEC sp_addextendedproperty
'MS_Description', N'模拟一个自定页面的表单，该数据会关联到流程实例FrmData，可用于复杂页面的设计及后期的数据分析',
'SCHEMA', N'dbo',
'TABLE', N'oms_formtest'
GO


-- ----------------------------
-- Records of oms_formtest
-- ----------------------------

-- ----------------------------
-- Table structure for oms_message
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[oms_message]') AND type IN ('U'))
	DROP TABLE [dbo].[oms_message]
GO

CREATE TABLE [dbo].[oms_message] (
  [F_Id] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_MessageType] int  NULL,
  [F_ToUserId] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_ToUserName] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_MessageInfo] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_EnabledMark] tinyint  NULL,
  [F_CreatorTime] datetime2(7)  NULL,
  [F_CreatorUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_CreatorUserName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_HrefTarget] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_Href] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_KeyValue] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_ClickRead] tinyint  NULL
)
GO

ALTER TABLE [dbo].[oms_message] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'主键Id',
'SCHEMA', N'dbo',
'TABLE', N'oms_message',
'COLUMN', N'F_Id'
GO

EXEC sp_addextendedproperty
'MS_Description', N'信息类型（通知、私信、处理）',
'SCHEMA', N'dbo',
'TABLE', N'oms_message',
'COLUMN', N'F_MessageType'
GO

EXEC sp_addextendedproperty
'MS_Description', N'收件人主键',
'SCHEMA', N'dbo',
'TABLE', N'oms_message',
'COLUMN', N'F_ToUserId'
GO

EXEC sp_addextendedproperty
'MS_Description', N'收件人',
'SCHEMA', N'dbo',
'TABLE', N'oms_message',
'COLUMN', N'F_ToUserName'
GO

EXEC sp_addextendedproperty
'MS_Description', N'内容',
'SCHEMA', N'dbo',
'TABLE', N'oms_message',
'COLUMN', N'F_MessageInfo'
GO

EXEC sp_addextendedproperty
'MS_Description', N'有效',
'SCHEMA', N'dbo',
'TABLE', N'oms_message',
'COLUMN', N'F_EnabledMark'
GO

EXEC sp_addextendedproperty
'MS_Description', N'创建时间',
'SCHEMA', N'dbo',
'TABLE', N'oms_message',
'COLUMN', N'F_CreatorTime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'创建用户主键',
'SCHEMA', N'dbo',
'TABLE', N'oms_message',
'COLUMN', N'F_CreatorUserId'
GO

EXEC sp_addextendedproperty
'MS_Description', N'创建用户',
'SCHEMA', N'dbo',
'TABLE', N'oms_message',
'COLUMN', N'F_CreatorUserName'
GO

EXEC sp_addextendedproperty
'MS_Description', N'跳转类型',
'SCHEMA', N'dbo',
'TABLE', N'oms_message',
'COLUMN', N'F_HrefTarget'
GO

EXEC sp_addextendedproperty
'MS_Description', N'跳转地址',
'SCHEMA', N'dbo',
'TABLE', N'oms_message',
'COLUMN', N'F_Href'
GO

EXEC sp_addextendedproperty
'MS_Description', N'待办关联键',
'SCHEMA', N'dbo',
'TABLE', N'oms_message',
'COLUMN', N'F_KeyValue'
GO

EXEC sp_addextendedproperty
'MS_Description', N'点击已读',
'SCHEMA', N'dbo',
'TABLE', N'oms_message',
'COLUMN', N'F_ClickRead'
GO


-- ----------------------------
-- Records of oms_message
-- ----------------------------

-- ----------------------------
-- Table structure for oms_messagehis
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[oms_messagehis]') AND type IN ('U'))
	DROP TABLE [dbo].[oms_messagehis]
GO

CREATE TABLE [dbo].[oms_messagehis] (
  [F_Id] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_MessageId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_CreatorTime] datetime2(7)  NULL,
  [F_CreatorUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_CreatorUserName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[oms_messagehis] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'主键Id',
'SCHEMA', N'dbo',
'TABLE', N'oms_messagehis',
'COLUMN', N'F_Id'
GO

EXEC sp_addextendedproperty
'MS_Description', N'信息Id',
'SCHEMA', N'dbo',
'TABLE', N'oms_messagehis',
'COLUMN', N'F_MessageId'
GO

EXEC sp_addextendedproperty
'MS_Description', N'创建时间',
'SCHEMA', N'dbo',
'TABLE', N'oms_messagehis',
'COLUMN', N'F_CreatorTime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'创建用户主键',
'SCHEMA', N'dbo',
'TABLE', N'oms_messagehis',
'COLUMN', N'F_CreatorUserId'
GO

EXEC sp_addextendedproperty
'MS_Description', N'创建用户',
'SCHEMA', N'dbo',
'TABLE', N'oms_messagehis',
'COLUMN', N'F_CreatorUserName'
GO


-- ----------------------------
-- Records of oms_messagehis
-- ----------------------------

-- ----------------------------
-- Table structure for oms_uploadfile
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[oms_uploadfile]') AND type IN ('U'))
	DROP TABLE [dbo].[oms_uploadfile]
GO

CREATE TABLE [dbo].[oms_uploadfile] (
  [F_Id] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_FilePath] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_FileName] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_FileType] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_FileSize] int  NULL,
  [F_FileExtension] nvarchar(20) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_FileBy] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_Description] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_OrganizeId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_EnabledMark] tinyint  NULL,
  [F_CreatorTime] datetime2(7)  NULL,
  [F_CreatorUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_CreatorUserName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[oms_uploadfile] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'主键Id',
'SCHEMA', N'dbo',
'TABLE', N'oms_uploadfile',
'COLUMN', N'F_Id'
GO

EXEC sp_addextendedproperty
'MS_Description', N'文件路径',
'SCHEMA', N'dbo',
'TABLE', N'oms_uploadfile',
'COLUMN', N'F_FilePath'
GO

EXEC sp_addextendedproperty
'MS_Description', N'文件名称',
'SCHEMA', N'dbo',
'TABLE', N'oms_uploadfile',
'COLUMN', N'F_FileName'
GO

EXEC sp_addextendedproperty
'MS_Description', N'文件类型（0 文件，1 图片）',
'SCHEMA', N'dbo',
'TABLE', N'oms_uploadfile',
'COLUMN', N'F_FileType'
GO

EXEC sp_addextendedproperty
'MS_Description', N'文件大小',
'SCHEMA', N'dbo',
'TABLE', N'oms_uploadfile',
'COLUMN', N'F_FileSize'
GO

EXEC sp_addextendedproperty
'MS_Description', N'文件扩展名',
'SCHEMA', N'dbo',
'TABLE', N'oms_uploadfile',
'COLUMN', N'F_FileExtension'
GO

EXEC sp_addextendedproperty
'MS_Description', N'文件所属',
'SCHEMA', N'dbo',
'TABLE', N'oms_uploadfile',
'COLUMN', N'F_FileBy'
GO

EXEC sp_addextendedproperty
'MS_Description', N'备注',
'SCHEMA', N'dbo',
'TABLE', N'oms_uploadfile',
'COLUMN', N'F_Description'
GO

EXEC sp_addextendedproperty
'MS_Description', N'所属部门',
'SCHEMA', N'dbo',
'TABLE', N'oms_uploadfile',
'COLUMN', N'F_OrganizeId'
GO

EXEC sp_addextendedproperty
'MS_Description', N'有效',
'SCHEMA', N'dbo',
'TABLE', N'oms_uploadfile',
'COLUMN', N'F_EnabledMark'
GO

EXEC sp_addextendedproperty
'MS_Description', N'创建时间',
'SCHEMA', N'dbo',
'TABLE', N'oms_uploadfile',
'COLUMN', N'F_CreatorTime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'创建用户主键',
'SCHEMA', N'dbo',
'TABLE', N'oms_uploadfile',
'COLUMN', N'F_CreatorUserId'
GO

EXEC sp_addextendedproperty
'MS_Description', N'创建用户',
'SCHEMA', N'dbo',
'TABLE', N'oms_uploadfile',
'COLUMN', N'F_CreatorUserName'
GO


-- ----------------------------
-- Records of oms_uploadfile
-- ----------------------------

-- ----------------------------
-- Table structure for sys_area
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[sys_area]') AND type IN ('U'))
	DROP TABLE [dbo].[sys_area]
GO

CREATE TABLE [dbo].[sys_area] (
  [F_Id] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_ParentId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_Layers] int  NULL,
  [F_EnCode] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_FullName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_SimpleSpelling] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_SortCode] bigint  NULL,
  [F_DeleteMark] tinyint  NULL,
  [F_EnabledMark] tinyint  NULL,
  [F_Description] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_CreatorTime] datetime2(7)  NULL,
  [F_CreatorUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_LastModifyTime] datetime2(7)  NULL,
  [F_LastModifyUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_DeleteTime] datetime2(7)  NULL,
  [F_DeleteUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[sys_area] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'主键',
'SCHEMA', N'dbo',
'TABLE', N'sys_area',
'COLUMN', N'F_Id'
GO

EXEC sp_addextendedproperty
'MS_Description', N'父级',
'SCHEMA', N'dbo',
'TABLE', N'sys_area',
'COLUMN', N'F_ParentId'
GO

EXEC sp_addextendedproperty
'MS_Description', N'层次',
'SCHEMA', N'dbo',
'TABLE', N'sys_area',
'COLUMN', N'F_Layers'
GO

EXEC sp_addextendedproperty
'MS_Description', N'编码',
'SCHEMA', N'dbo',
'TABLE', N'sys_area',
'COLUMN', N'F_EnCode'
GO

EXEC sp_addextendedproperty
'MS_Description', N'名称',
'SCHEMA', N'dbo',
'TABLE', N'sys_area',
'COLUMN', N'F_FullName'
GO

EXEC sp_addextendedproperty
'MS_Description', N'简拼',
'SCHEMA', N'dbo',
'TABLE', N'sys_area',
'COLUMN', N'F_SimpleSpelling'
GO

EXEC sp_addextendedproperty
'MS_Description', N'排序码',
'SCHEMA', N'dbo',
'TABLE', N'sys_area',
'COLUMN', N'F_SortCode'
GO

EXEC sp_addextendedproperty
'MS_Description', N'删除标志',
'SCHEMA', N'dbo',
'TABLE', N'sys_area',
'COLUMN', N'F_DeleteMark'
GO

EXEC sp_addextendedproperty
'MS_Description', N'有效标志',
'SCHEMA', N'dbo',
'TABLE', N'sys_area',
'COLUMN', N'F_EnabledMark'
GO

EXEC sp_addextendedproperty
'MS_Description', N'描述',
'SCHEMA', N'dbo',
'TABLE', N'sys_area',
'COLUMN', N'F_Description'
GO

EXEC sp_addextendedproperty
'MS_Description', N'创建日期',
'SCHEMA', N'dbo',
'TABLE', N'sys_area',
'COLUMN', N'F_CreatorTime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'创建用户主键',
'SCHEMA', N'dbo',
'TABLE', N'sys_area',
'COLUMN', N'F_CreatorUserId'
GO

EXEC sp_addextendedproperty
'MS_Description', N'最后修改时间',
'SCHEMA', N'dbo',
'TABLE', N'sys_area',
'COLUMN', N'F_LastModifyTime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'最后修改用户',
'SCHEMA', N'dbo',
'TABLE', N'sys_area',
'COLUMN', N'F_LastModifyUserId'
GO

EXEC sp_addextendedproperty
'MS_Description', N'删除时间',
'SCHEMA', N'dbo',
'TABLE', N'sys_area',
'COLUMN', N'F_DeleteTime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'删除用户',
'SCHEMA', N'dbo',
'TABLE', N'sys_area',
'COLUMN', N'F_DeleteUserId'
GO

EXEC sp_addextendedproperty
'MS_Description', N'行政区域表',
'SCHEMA', N'dbo',
'TABLE', N'sys_area'
GO


-- ----------------------------
-- Records of sys_area
-- ----------------------------

-- ----------------------------
-- Table structure for sys_dataprivilegerule
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[sys_dataprivilegerule]') AND type IN ('U'))
	DROP TABLE [dbo].[sys_dataprivilegerule]
GO

CREATE TABLE [dbo].[sys_dataprivilegerule] (
  [F_Id] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_ModuleId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_ModuleCode] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_PrivilegeRules] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_SortCode] int  NULL,
  [F_DeleteMark] tinyint  NULL,
  [F_EnabledMark] tinyint  NULL,
  [F_Description] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_CreatorTime] datetime2(7)  NULL,
  [F_CreatorUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_LastModifyTime] datetime2(7)  NULL,
  [F_LastModifyUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_DeleteTime] datetime2(7)  NULL,
  [F_DeleteUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[sys_dataprivilegerule] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of sys_dataprivilegerule
-- ----------------------------

-- ----------------------------
-- Table structure for sys_filterip
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[sys_filterip]') AND type IN ('U'))
	DROP TABLE [dbo].[sys_filterip]
GO

CREATE TABLE [dbo].[sys_filterip] (
  [F_Id] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_Type] tinyint  NULL,
  [F_StartIP] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_EndIP] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_SortCode] int  NULL,
  [F_DeleteMark] tinyint  NULL,
  [F_EnabledMark] tinyint  NULL,
  [F_Description] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_CreatorTime] datetime2(7)  NULL,
  [F_CreatorUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_LastModifyTime] datetime2(7)  NULL,
  [F_LastModifyUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_DeleteTime] datetime2(7)  NULL,
  [F_DeleteUserId] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[sys_filterip] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of sys_filterip
-- ----------------------------

-- ----------------------------
-- Table structure for sys_flowscheme
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[sys_flowscheme]') AND type IN ('U'))
	DROP TABLE [dbo].[sys_flowscheme]
GO

CREATE TABLE [dbo].[sys_flowscheme] (
  [F_Id] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_SchemeCode] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_SchemeName] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_SchemeType] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_SchemeVersion] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_SchemeCanUser] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_SchemeContent] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_FrmId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_FrmType] int  NOT NULL,
  [F_AuthorizeType] int  NOT NULL,
  [F_SortCode] int  NULL,
  [F_DeleteMark] tinyint  NULL,
  [F_EnabledMark] tinyint  NULL,
  [F_Description] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_CreatorTime] datetime2(7)  NULL,
  [F_CreatorUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_CreatorUserName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_LastModifyTime] datetime2(7)  NULL,
  [F_LastModifyUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_LastModifyUserName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_OrganizeId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_DeleteTime] datetime2(7)  NULL,
  [F_DeleteUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[sys_flowscheme] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'主键Id',
'SCHEMA', N'dbo',
'TABLE', N'sys_flowscheme',
'COLUMN', N'F_Id'
GO

EXEC sp_addextendedproperty
'MS_Description', N'流程编号',
'SCHEMA', N'dbo',
'TABLE', N'sys_flowscheme',
'COLUMN', N'F_SchemeCode'
GO

EXEC sp_addextendedproperty
'MS_Description', N'流程名称',
'SCHEMA', N'dbo',
'TABLE', N'sys_flowscheme',
'COLUMN', N'F_SchemeName'
GO

EXEC sp_addextendedproperty
'MS_Description', N'流程分类',
'SCHEMA', N'dbo',
'TABLE', N'sys_flowscheme',
'COLUMN', N'F_SchemeType'
GO

EXEC sp_addextendedproperty
'MS_Description', N'流程内容版本',
'SCHEMA', N'dbo',
'TABLE', N'sys_flowscheme',
'COLUMN', N'F_SchemeVersion'
GO

EXEC sp_addextendedproperty
'MS_Description', N'流程模板使用者',
'SCHEMA', N'dbo',
'TABLE', N'sys_flowscheme',
'COLUMN', N'F_SchemeCanUser'
GO

EXEC sp_addextendedproperty
'MS_Description', N'流程内容',
'SCHEMA', N'dbo',
'TABLE', N'sys_flowscheme',
'COLUMN', N'F_SchemeContent'
GO

EXEC sp_addextendedproperty
'MS_Description', N'表单ID',
'SCHEMA', N'dbo',
'TABLE', N'sys_flowscheme',
'COLUMN', N'F_FrmId'
GO

EXEC sp_addextendedproperty
'MS_Description', N'表单类型',
'SCHEMA', N'dbo',
'TABLE', N'sys_flowscheme',
'COLUMN', N'F_FrmType'
GO

EXEC sp_addextendedproperty
'MS_Description', N'模板权限类型：0完全公开,1指定部门/人员',
'SCHEMA', N'dbo',
'TABLE', N'sys_flowscheme',
'COLUMN', N'F_AuthorizeType'
GO

EXEC sp_addextendedproperty
'MS_Description', N'排序码',
'SCHEMA', N'dbo',
'TABLE', N'sys_flowscheme',
'COLUMN', N'F_SortCode'
GO

EXEC sp_addextendedproperty
'MS_Description', N'删除标记',
'SCHEMA', N'dbo',
'TABLE', N'sys_flowscheme',
'COLUMN', N'F_DeleteMark'
GO

EXEC sp_addextendedproperty
'MS_Description', N'有效',
'SCHEMA', N'dbo',
'TABLE', N'sys_flowscheme',
'COLUMN', N'F_EnabledMark'
GO

EXEC sp_addextendedproperty
'MS_Description', N'备注',
'SCHEMA', N'dbo',
'TABLE', N'sys_flowscheme',
'COLUMN', N'F_Description'
GO

EXEC sp_addextendedproperty
'MS_Description', N'创建时间',
'SCHEMA', N'dbo',
'TABLE', N'sys_flowscheme',
'COLUMN', N'F_CreatorTime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'创建用户主键',
'SCHEMA', N'dbo',
'TABLE', N'sys_flowscheme',
'COLUMN', N'F_CreatorUserId'
GO

EXEC sp_addextendedproperty
'MS_Description', N'创建用户',
'SCHEMA', N'dbo',
'TABLE', N'sys_flowscheme',
'COLUMN', N'F_CreatorUserName'
GO

EXEC sp_addextendedproperty
'MS_Description', N'修改时间',
'SCHEMA', N'dbo',
'TABLE', N'sys_flowscheme',
'COLUMN', N'F_LastModifyTime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'修改用户主键',
'SCHEMA', N'dbo',
'TABLE', N'sys_flowscheme',
'COLUMN', N'F_LastModifyUserId'
GO

EXEC sp_addextendedproperty
'MS_Description', N'修改用户',
'SCHEMA', N'dbo',
'TABLE', N'sys_flowscheme',
'COLUMN', N'F_LastModifyUserName'
GO

EXEC sp_addextendedproperty
'MS_Description', N'所属部门',
'SCHEMA', N'dbo',
'TABLE', N'sys_flowscheme',
'COLUMN', N'F_OrganizeId'
GO

EXEC sp_addextendedproperty
'MS_Description', N'删除时间',
'SCHEMA', N'dbo',
'TABLE', N'sys_flowscheme',
'COLUMN', N'F_DeleteTime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'删除人',
'SCHEMA', N'dbo',
'TABLE', N'sys_flowscheme',
'COLUMN', N'F_DeleteUserId'
GO

EXEC sp_addextendedproperty
'MS_Description', N'工作流模板信息表',
'SCHEMA', N'dbo',
'TABLE', N'sys_flowscheme'
GO


-- ----------------------------
-- Records of sys_flowscheme
-- ----------------------------

-- ----------------------------
-- Table structure for sys_form
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[sys_form]') AND type IN ('U'))
	DROP TABLE [dbo].[sys_form]
GO

CREATE TABLE [dbo].[sys_form] (
  [F_Id] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_Name] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_FrmType] int  NULL,
  [F_WebId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_Fields] int  NULL,
  [F_ContentData] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_ContentParse] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_Content] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_SortCode] int  NULL,
  [F_EnabledMark] tinyint  NULL,
  [F_DeleteMark] tinyint  NULL,
  [F_CreatorTime] datetime2(7)  NULL,
  [F_CreatorUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_LastModifyTime] datetime2(7)  NULL,
  [F_LastModifyUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_DeleteTime] datetime2(7)  NULL,
  [F_DeleteUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_Description] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_OrganizeId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_DbName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[sys_form] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'表单模板Id',
'SCHEMA', N'dbo',
'TABLE', N'sys_form',
'COLUMN', N'F_Id'
GO

EXEC sp_addextendedproperty
'MS_Description', N'表单名称',
'SCHEMA', N'dbo',
'TABLE', N'sys_form',
'COLUMN', N'F_Name'
GO

EXEC sp_addextendedproperty
'MS_Description', N'表单类型，0：默认动态表单；1：Web自定义表单',
'SCHEMA', N'dbo',
'TABLE', N'sys_form',
'COLUMN', N'F_FrmType'
GO

EXEC sp_addextendedproperty
'MS_Description', N'系统页面标识，当表单类型为用Web自定义的表单时，需要标识加载哪个页面',
'SCHEMA', N'dbo',
'TABLE', N'sys_form',
'COLUMN', N'F_WebId'
GO

EXEC sp_addextendedproperty
'MS_Description', N'字段个数',
'SCHEMA', N'dbo',
'TABLE', N'sys_form',
'COLUMN', N'F_Fields'
GO

EXEC sp_addextendedproperty
'MS_Description', N'表单中的控件属性描述',
'SCHEMA', N'dbo',
'TABLE', N'sys_form',
'COLUMN', N'F_ContentData'
GO

EXEC sp_addextendedproperty
'MS_Description', N'表单控件位置模板',
'SCHEMA', N'dbo',
'TABLE', N'sys_form',
'COLUMN', N'F_ContentParse'
GO

EXEC sp_addextendedproperty
'MS_Description', N'表单原html模板未经处理的',
'SCHEMA', N'dbo',
'TABLE', N'sys_form',
'COLUMN', N'F_Content'
GO

EXEC sp_addextendedproperty
'MS_Description', N'排序码',
'SCHEMA', N'dbo',
'TABLE', N'sys_form',
'COLUMN', N'F_SortCode'
GO

EXEC sp_addextendedproperty
'MS_Description', N'是否启用',
'SCHEMA', N'dbo',
'TABLE', N'sys_form',
'COLUMN', N'F_EnabledMark'
GO

EXEC sp_addextendedproperty
'MS_Description', N'逻辑删除标志',
'SCHEMA', N'dbo',
'TABLE', N'sys_form',
'COLUMN', N'F_DeleteMark'
GO

EXEC sp_addextendedproperty
'MS_Description', N'创建时间',
'SCHEMA', N'dbo',
'TABLE', N'sys_form',
'COLUMN', N'F_CreatorTime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'创建人',
'SCHEMA', N'dbo',
'TABLE', N'sys_form',
'COLUMN', N'F_CreatorUserId'
GO

EXEC sp_addextendedproperty
'MS_Description', N'最后修改时间',
'SCHEMA', N'dbo',
'TABLE', N'sys_form',
'COLUMN', N'F_LastModifyTime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'最后修改人',
'SCHEMA', N'dbo',
'TABLE', N'sys_form',
'COLUMN', N'F_LastModifyUserId'
GO

EXEC sp_addextendedproperty
'MS_Description', N'删除时间',
'SCHEMA', N'dbo',
'TABLE', N'sys_form',
'COLUMN', N'F_DeleteTime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'删除人',
'SCHEMA', N'dbo',
'TABLE', N'sys_form',
'COLUMN', N'F_DeleteUserId'
GO

EXEC sp_addextendedproperty
'MS_Description', N'内容',
'SCHEMA', N'dbo',
'TABLE', N'sys_form',
'COLUMN', N'F_Description'
GO

EXEC sp_addextendedproperty
'MS_Description', N'所属组织',
'SCHEMA', N'dbo',
'TABLE', N'sys_form',
'COLUMN', N'F_OrganizeId'
GO

EXEC sp_addextendedproperty
'MS_Description', N'数据库名称',
'SCHEMA', N'dbo',
'TABLE', N'sys_form',
'COLUMN', N'F_DbName'
GO

EXEC sp_addextendedproperty
'MS_Description', N'表单模板表',
'SCHEMA', N'dbo',
'TABLE', N'sys_form'
GO


-- ----------------------------
-- Records of sys_form
-- ----------------------------

-- ----------------------------
-- Table structure for sys_items
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[sys_items]') AND type IN ('U'))
	DROP TABLE [dbo].[sys_items]
GO

CREATE TABLE [dbo].[sys_items] (
  [F_Id] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_ParentId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_EnCode] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_FullName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_IsTree] tinyint  NULL,
  [F_Layers] int  NULL,
  [F_SortCode] int  NULL,
  [F_DeleteMark] tinyint  NULL,
  [F_EnabledMark] tinyint  NULL,
  [F_Description] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_CreatorTime] datetime2(7)  NULL,
  [F_CreatorUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_LastModifyTime] datetime2(7)  NULL,
  [F_LastModifyUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_DeleteTime] datetime2(7)  NULL,
  [F_DeleteUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[sys_items] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of sys_items
-- ----------------------------
INSERT INTO [dbo].[sys_items] ([F_Id], [F_ParentId], [F_EnCode], [F_FullName], [F_IsTree], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'00F76465-DBBA-484A-B75C-E81DEE9313E6', N'77070117-3F1A-41BA-BF81-B8B85BF10D5E', N'Education', N'学历', N'0', N'2', N'8', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_items] ([F_Id], [F_ParentId], [F_EnCode], [F_FullName], [F_IsTree], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'0DF5B725-5FB8-487F-B0E4-BC563A77EB04', N'77070117-3F1A-41BA-BF81-B8B85BF10D5E', N'DbType', N'数据库类型', N'0', N'2', N'4', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_items] ([F_Id], [F_ParentId], [F_EnCode], [F_FullName], [F_IsTree], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'15023A4E-4856-44EB-BE71-36A106E2AA59', N'77070117-3F1A-41BA-BF81-B8B85BF10D5E', N'103', N'民族', N'0', N'2', N'14', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_items] ([F_Id], [F_ParentId], [F_EnCode], [F_FullName], [F_IsTree], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'2748F35F-4EE2-417C-A907-3453146AAF67', N'77070117-3F1A-41BA-BF81-B8B85BF10D5E', N'Certificate', N'证件名称', N'0', N'2', N'7', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_items] ([F_Id], [F_ParentId], [F_EnCode], [F_FullName], [F_IsTree], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'64c10822-0c85-4516-9b59-879b818547ae', N'77070117-3F1A-41BA-BF81-B8B85BF10D5E', N'MessageType', N'信息类型', N'0', N'2', N'16', N'0', N'1', N'', N'2020-07-29 16:51:19.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_items] ([F_Id], [F_ParentId], [F_EnCode], [F_FullName], [F_IsTree], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'77070117-3F1A-41BA-BF81-B8B85BF10D5E', N'0', N'Sys_Items', N'通用字典', N'1', N'1', N'1', N'0', N'1', NULL, NULL, NULL, N'2020-04-20 17:17:39.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL)
GO

INSERT INTO [dbo].[sys_items] ([F_Id], [F_ParentId], [F_EnCode], [F_FullName], [F_IsTree], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'8CEB2F71-026C-4FA6-9A61-378127AE7320', N'77070117-3F1A-41BA-BF81-B8B85BF10D5E', N'102', N'生育', N'0', N'2', N'13', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_items] ([F_Id], [F_ParentId], [F_EnCode], [F_FullName], [F_IsTree], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'954AB9A1-9928-4C6D-820A-FC1CDC85CDE0', N'77070117-3F1A-41BA-BF81-B8B85BF10D5E', N'AuditState', N'审核状态', N'0', N'2', N'6', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_items] ([F_Id], [F_ParentId], [F_EnCode], [F_FullName], [F_IsTree], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'9a7079bd-0660-4549-9c2d-db5e8616619f', N'77070117-3F1A-41BA-BF81-B8B85BF10D5E', N'DbLogType', N'系统日志', N'0', N'2', N'16', N'0', N'1', NULL, N'2016-07-19 17:09:45.0000000', NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_items] ([F_Id], [F_ParentId], [F_EnCode], [F_FullName], [F_IsTree], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'9EB4602B-BF9A-4710-9D80-C73CE89BEC5D', N'77070117-3F1A-41BA-BF81-B8B85BF10D5E', N'OrganizeCategory', N'机构分类', N'0', N'2', N'2', N'0', N'1', NULL, NULL, NULL, N'2020-04-28 09:07:08.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL)
GO

INSERT INTO [dbo].[sys_items] ([F_Id], [F_ParentId], [F_EnCode], [F_FullName], [F_IsTree], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'BDD797C3-2323-4868-9A63-C8CC3437AEAA', N'77070117-3F1A-41BA-BF81-B8B85BF10D5E', N'104', N'性别', N'0', N'2', N'15', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_items] ([F_Id], [F_ParentId], [F_EnCode], [F_FullName], [F_IsTree], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'D94E4DC1-C2FD-4D19-9D5D-3886D39900CE', N'77070117-3F1A-41BA-BF81-B8B85BF10D5E', N'RoleType', N'角色类型', N'0', N'2', N'3', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_items] ([F_Id], [F_ParentId], [F_EnCode], [F_FullName], [F_IsTree], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'FA7537E2-1C64-4431-84BF-66158DD63269', N'77070117-3F1A-41BA-BF81-B8B85BF10D5E', N'101', N'婚姻', N'0', N'2', N'12', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO


-- ----------------------------
-- Table structure for sys_itemsdetail
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[sys_itemsdetail]') AND type IN ('U'))
	DROP TABLE [dbo].[sys_itemsdetail]
GO

CREATE TABLE [dbo].[sys_itemsdetail] (
  [F_Id] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_ItemId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_ParentId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_ItemCode] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_ItemName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_SimpleSpelling] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_IsDefault] tinyint  NULL,
  [F_Layers] int  NULL,
  [F_SortCode] int  NULL,
  [F_DeleteMark] tinyint  NULL,
  [F_EnabledMark] tinyint  NULL,
  [F_Description] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_CreatorTime] datetime2(7)  NULL,
  [F_CreatorUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_LastModifyTime] datetime2(7)  NULL,
  [F_LastModifyUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_DeleteTime] datetime2(7)  NULL,
  [F_DeleteUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[sys_itemsdetail] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of sys_itemsdetail
-- ----------------------------
INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'0096ad81-4317-486e-9144-a6a02999ff19', N'2748F35F-4EE2-417C-A907-3453146AAF67', NULL, N'4', N'护照', NULL, N'0', NULL, N'4', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'04aba88d-f09b-46c6-bd90-a38471399b0e', N'D94E4DC1-C2FD-4D19-9D5D-3886D39900CE', NULL, N'2', N'业务角色', NULL, N'0', NULL, N'2', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'0a2ba6b9-716f-410f-8e89-929ec2277333', N'64c10822-0c85-4516-9b59-879b818547ae', NULL, N'1', N'私信', NULL, N'0', NULL, N'1', N'0', N'1', N'', N'2020-07-29 16:51:59.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'1950efdf-8685-4341-8d2c-ac85ac7addd0', N'00F76465-DBBA-484A-B75C-E81DEE9313E6', NULL, N'1', N'小学', NULL, N'0', NULL, N'1', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'19EE595A-E775-409D-A48F-B33CF9F262C7', N'9EB4602B-BF9A-4710-9D80-C73CE89BEC5D', NULL, N'WorkGroup', N'小组', NULL, N'0', NULL, N'7', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'24e39617-f04e-4f6f-9209-ad71e870e7c6', N'9a7079bd-0660-4549-9c2d-db5e8616619f', NULL, N'Submit', N'提交', NULL, N'0', NULL, N'7', N'0', N'1', NULL, N'2016-07-19 17:11:19.0000000', NULL, N'2016-07-19 18:20:54.0000000', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'27e85cb8-04e7-447b-911d-dd1e97dfab83', N'0DF5B725-5FB8-487F-B0E4-BC563A77EB04', NULL, N'Oracle', N'Oracle', NULL, N'0', NULL, N'2', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'2B540AC5-6E64-4688-BB60-E0C01DFA982C', N'9EB4602B-BF9A-4710-9D80-C73CE89BEC5D', NULL, N'SubCompany', N'子公司', NULL, N'0', NULL, N'4', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'2C3715AC-16F7-48FC-AB40-B0931DB1E729', N'9EB4602B-BF9A-4710-9D80-C73CE89BEC5D', NULL, N'Area', N'区域', NULL, N'0', NULL, N'2', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'34222d46-e0c6-446e-8150-dbefc47a1d5f', N'00F76465-DBBA-484A-B75C-E81DEE9313E6', NULL, N'6', N'本科', NULL, N'0', NULL, N'6', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'34a642b2-44d4-485f-b3fc-6cce24f68b0f', N'0DF5B725-5FB8-487F-B0E4-BC563A77EB04', NULL, N'MySql', N'MySql', NULL, N'0', NULL, N'3', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'355ad7a4-c4f8-4bd3-9c72-ff07983da0f0', N'00F76465-DBBA-484A-B75C-E81DEE9313E6', NULL, N'9', N'其他', NULL, N'0', NULL, N'9', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'392f88a8-02c2-49eb-8aed-b2acf474272a', N'9a7079bd-0660-4549-9c2d-db5e8616619f', NULL, N'Update', N'修改', NULL, N'0', NULL, N'6', N'0', N'1', NULL, N'2016-07-19 17:11:14.0000000', NULL, N'2016-07-19 18:20:49.0000000', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'3c884a03-4f34-4150-b134-966387f1de2a', N'9a7079bd-0660-4549-9c2d-db5e8616619f', NULL, N'Exit', N'退出', NULL, N'0', NULL, N'2', N'0', N'1', NULL, N'2016-07-19 17:10:49.0000000', NULL, N'2016-07-19 18:20:23.0000000', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'3f280e2b-92f6-466c-8cc3-d7c8ff48cc8d', N'00F76465-DBBA-484A-B75C-E81DEE9313E6', NULL, N'7', N'硕士', NULL, N'0', NULL, N'7', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'41053517-215d-4e11-81cd-367c0e9578d7', N'954AB9A1-9928-4C6D-820A-FC1CDC85CDE0', NULL, N'3', N'通过', NULL, N'0', NULL, N'3', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'433511a9-78bd-41a0-ab25-e4d4b3423055', N'00F76465-DBBA-484A-B75C-E81DEE9313E6', NULL, N'2', N'初中', NULL, N'0', NULL, N'2', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'486a82e3-1950-425e-b2ce-b5d98f33016a', N'00F76465-DBBA-484A-B75C-E81DEE9313E6', NULL, N'5', N'大专', NULL, N'0', NULL, N'5', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'48c4e0f5-f570-4601-8946-6078762db3bf', N'2748F35F-4EE2-417C-A907-3453146AAF67', NULL, N'3', N'军官证', NULL, N'0', NULL, N'3', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'49300258-1227-4b85-b6a2-e948dbbe57a4', N'15023A4E-4856-44EB-BE71-36A106E2AA59', NULL, N'汉族', N'汉族', NULL, N'0', NULL, N'1', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'49b68663-ad01-4c43-b084-f98e3e23fee8', N'954AB9A1-9928-4C6D-820A-FC1CDC85CDE0', NULL, N'7', N'废弃', NULL, N'0', NULL, N'7', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'4c2f2428-2e00-4336-b9ce-5a61f24193f6', N'2748F35F-4EE2-417C-A907-3453146AAF67', NULL, N'2', N'士兵证', NULL, N'0', NULL, N'2', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'582e0b66-2ee9-4885-9f0c-3ce3ebf96e12', N'8CEB2F71-026C-4FA6-9A61-378127AE7320', NULL, N'1', N'已育', NULL, N'0', NULL, N'1', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'5d6def0e-e2a7-48eb-b43c-cc3631f60dd7', N'BDD797C3-2323-4868-9A63-C8CC3437AEAA', NULL, N'1', N'男', NULL, N'0', NULL, N'1', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'63acf96d-6115-4d76-a994-438f59419aad', N'954AB9A1-9928-4C6D-820A-FC1CDC85CDE0', NULL, N'5', N'退回', NULL, N'0', NULL, N'5', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'643209c8-931b-4641-9e04-b8bdd11800af', N'9a7079bd-0660-4549-9c2d-db5e8616619f', NULL, N'Login', N'登录', NULL, N'0', NULL, N'1', N'0', N'1', NULL, N'2016-07-19 17:10:39.0000000', NULL, N'2016-07-19 18:20:17.0000000', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'738edf2a-d59f-4992-97ef-d847db23bcb8', N'FA7537E2-1C64-4431-84BF-66158DD63269', NULL, N'1', N'已婚', NULL, N'0', NULL, N'1', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'795f2695-497a-4f5e-ab1d-706095c1edb9', N'9a7079bd-0660-4549-9c2d-db5e8616619f', NULL, N'Other', N'其他', NULL, N'0', NULL, N'0', N'0', N'1', NULL, N'2016-07-19 17:10:33.0000000', NULL, N'2016-07-19 18:20:09.0000000', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'7a6d1bc4-3ec7-4c57-be9b-b4c97d60d5f6', N'954AB9A1-9928-4C6D-820A-FC1CDC85CDE0', NULL, N'1', N'草稿', NULL, N'0', NULL, N'1', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'7c1135be-0148-43eb-ae49-62a1e16ebbe3', N'FA7537E2-1C64-4431-84BF-66158DD63269', NULL, N'5', N'其他', NULL, N'0', NULL, N'5', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'7fc8fa11-4acf-409a-a771-aaf1eb81e814', N'9a7079bd-0660-4549-9c2d-db5e8616619f', NULL, N'Exception', N'异常', NULL, N'0', NULL, N'8', N'0', N'1', NULL, N'2016-07-19 17:11:25.0000000', NULL, N'2016-07-19 18:21:01.0000000', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'822baf7c-abdb-4257-9b78-1f550806f544', N'BDD797C3-2323-4868-9A63-C8CC3437AEAA', NULL, N'0', N'女', NULL, N'0', NULL, N'2', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'8892186f-22ff-40c4-9907-e80721f9c5fe', N'64c10822-0c85-4516-9b59-879b818547ae', NULL, N'2', N'待办', NULL, N'0', NULL, N'2', N'0', N'1', N'', N'2020-07-29 16:52:21.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-07-31 17:33:40.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'8b7b38bf-07c5-4f71-a853-41c5add4a94e', N'954AB9A1-9928-4C6D-820A-FC1CDC85CDE0', NULL, N'6', N'完成', NULL, N'0', NULL, N'6', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'930b8de2-049f-4753-b9fd-87f484911ee4', N'00F76465-DBBA-484A-B75C-E81DEE9313E6', NULL, N'8', N'博士', NULL, N'0', NULL, N'8', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'9b6a2225-6138-4cf2-9845-1bbecdf9b3ed', N'8CEB2F71-026C-4FA6-9A61-378127AE7320', NULL, N'3', N'其他', NULL, N'0', NULL, N'3', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'a13ccf0d-ac8f-44ac-a522-4a54edf1f0fa', N'9a7079bd-0660-4549-9c2d-db5e8616619f', NULL, N'Delete', N'删除', NULL, N'0', NULL, N'5', N'0', N'1', NULL, N'2016-07-19 17:11:09.0000000', NULL, N'2016-07-19 18:20:43.0000000', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'a4974810-d88d-4d54-82cc-fd779875478f', N'00F76465-DBBA-484A-B75C-E81DEE9313E6', NULL, N'4', N'中专', NULL, N'0', NULL, N'4', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'A64EBB80-6A24-48AF-A10E-B6A532C32CA6', N'9EB4602B-BF9A-4710-9D80-C73CE89BEC5D', NULL, N'Department', N'部门', NULL, N'0', NULL, N'5', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'a6f271f9-8653-4c5c-86cf-4cd00324b3c3', N'FA7537E2-1C64-4431-84BF-66158DD63269', NULL, N'4', N'丧偶', NULL, N'0', NULL, N'4', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'a7c4aba2-a891-4558-9b0a-bd7a1100a645', N'FA7537E2-1C64-4431-84BF-66158DD63269', NULL, N'2', N'未婚', NULL, N'0', NULL, N'2', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'acb128a6-ff63-4e25-b1e8-0a336ed3ab18', N'00F76465-DBBA-484A-B75C-E81DEE9313E6', NULL, N'3', N'高中', NULL, N'0', NULL, N'3', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'ace2d5e8-56d4-4c8b-8409-34bc272df404', N'2748F35F-4EE2-417C-A907-3453146AAF67', NULL, N'5', N'其它', NULL, N'0', NULL, N'5', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'B97BD7F5-B212-40C1-A1F7-DD9A2E63EEF2', N'9EB4602B-BF9A-4710-9D80-C73CE89BEC5D', NULL, N'Group', N'集团', NULL, N'0', NULL, N'1', N'0', N'1', N'', NULL, NULL, N'2020-06-29 17:35:07.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'ba1d27db-cf19-4cc0-9b18-0745e98f8088', N'64c10822-0c85-4516-9b59-879b818547ae', NULL, N'0', N'通知', NULL, N'0', NULL, N'0', N'0', N'1', N'', N'2020-07-29 16:51:50.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'cc6daa5c-a71c-4b2c-9a98-336bc3ee13c8', N'D94E4DC1-C2FD-4D19-9D5D-3886D39900CE', NULL, N'3', N'其他角色', NULL, N'0', NULL, N'3', N'0', N'1', N'', NULL, NULL, N'2020-06-18 10:15:51.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'ccc8e274-75da-4eb8-bed0-69008ab7c41c', N'9a7079bd-0660-4549-9c2d-db5e8616619f', NULL, N'Visit', N'访问', NULL, N'0', NULL, N'3', N'0', N'1', NULL, N'2016-07-19 17:10:55.0000000', NULL, N'2016-07-19 18:20:29.0000000', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'ce340c73-5048-4940-b86e-e3b3d53fdb2c', N'954AB9A1-9928-4C6D-820A-FC1CDC85CDE0', NULL, N'2', N'提交', NULL, N'0', NULL, N'2', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'D082BDB9-5C34-49BF-BD51-4E85D7BFF646', N'9EB4602B-BF9A-4710-9D80-C73CE89BEC5D', NULL, N'Company', N'公司', NULL, N'0', NULL, N'3', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'D1F439B9-D80E-4547-9EF0-163391854AB5', N'9EB4602B-BF9A-4710-9D80-C73CE89BEC5D', NULL, N'SubDepartment', N'子部门', NULL, N'0', NULL, N'6', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'd69cb819-2bb3-4e1d-9917-33b9a439233d', N'2748F35F-4EE2-417C-A907-3453146AAF67', NULL, N'1', N'身份证', NULL, N'0', NULL, N'1', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'de2167f3-40fe-4bf7-b8cb-5b1c554bad7a', N'8CEB2F71-026C-4FA6-9A61-378127AE7320', NULL, N'2', N'未育', NULL, N'0', NULL, N'2', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'e1979a4f-7fc1-42b9-a0e2-52d7059e8fb9', N'954AB9A1-9928-4C6D-820A-FC1CDC85CDE0', NULL, N'4', N'待审', NULL, N'0', NULL, N'4', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'e5079bae-a8c0-4209-9019-6a2b4a3a7dac', N'D94E4DC1-C2FD-4D19-9D5D-3886D39900CE', NULL, N'1', N'系统角色', NULL, N'0', NULL, N'1', N'0', N'1', N'', NULL, NULL, N'2020-06-24 09:08:22.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'e545061c-93fd-4ca2-ab29-b43db9db798b', N'9a7079bd-0660-4549-9c2d-db5e8616619f', NULL, N'Create', N'新增', NULL, N'0', NULL, N'4', N'0', N'1', NULL, N'2016-07-19 17:11:03.0000000', NULL, N'2016-07-19 18:20:35.0000000', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'f9609400-7caf-49af-ae3c-7671a9292fb3', N'FA7537E2-1C64-4431-84BF-66158DD63269', NULL, N'3', N'离异', NULL, N'0', NULL, N'3', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_itemsdetail] ([F_Id], [F_ItemId], [F_ParentId], [F_ItemCode], [F_ItemName], [F_SimpleSpelling], [F_IsDefault], [F_Layers], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'fa6c1873-888c-4b70-a2cc-59fccbb22078', N'0DF5B725-5FB8-487F-B0E4-BC563A77EB04', NULL, N'SqlServer', N'SqlServer', NULL, N'0', NULL, N'1', N'0', N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO


-- ----------------------------
-- Table structure for sys_log
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[sys_log]') AND type IN ('U'))
	DROP TABLE [dbo].[sys_log]
GO

CREATE TABLE [dbo].[sys_log] (
  [F_Id] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_Date] datetime2(7)  NULL,
  [F_Account] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_NickName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_Type] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_IPAddress] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_IPAddressName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_ModuleId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_ModuleName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_Result] tinyint  NULL,
  [F_Description] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_CreatorTime] datetime2(7)  NULL,
  [F_CreatorUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_KeyValue] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_CompanyId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[sys_log] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of sys_log
-- ----------------------------
INSERT INTO [dbo].[sys_log] ([F_Id], [F_Date], [F_Account], [F_NickName], [F_Type], [F_IPAddress], [F_IPAddressName], [F_ModuleId], [F_ModuleName], [F_Result], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_KeyValue], [F_CompanyId]) VALUES (N'39fc7684-9d50-e1dc-4bd5-00d9749c4253', N'2021-05-13 14:09:04.0000000', N'admin', N'超级管理员', N'Login', N'0.0.0.1', N'iana保留地址', NULL, N'系统登录', N'1', N'登录成功', N'2021-05-13 14:09:04.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, N'd69fd66a-6a77-4011-8a25-53a79bdf5001')
GO

INSERT INTO [dbo].[sys_log] ([F_Id], [F_Date], [F_Account], [F_NickName], [F_Type], [F_IPAddress], [F_IPAddressName], [F_ModuleId], [F_ModuleName], [F_Result], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_KeyValue], [F_CompanyId]) VALUES (N'39fc7686-b98a-b0c7-a30a-99366c6b053b', N'2021-05-13 14:11:23.0000000', N'admin', N'超级管理员', N'Login', N'0.0.0.1', N'iana保留地址', NULL, N'系统登录', N'1', N'登录成功', N'2021-05-13 14:11:23.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, N'd69fd66a-6a77-4011-8a25-53a79bdf5001')
GO

INSERT INTO [dbo].[sys_log] ([F_Id], [F_Date], [F_Account], [F_NickName], [F_Type], [F_IPAddress], [F_IPAddressName], [F_ModuleId], [F_ModuleName], [F_Result], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_KeyValue], [F_CompanyId]) VALUES (N'39fc7686-d08d-1a88-af33-615faa1f63a0', N'2021-05-13 14:11:29.0000000', N'admin', N'超级管理员', N'Update', N'0.0.0.1', N'iana保留地址', NULL, N'常规管理-单位组织-租户设置', N'1', N'租户设置操作,修改操作成功。', N'2021-05-13 14:11:29.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'd69fd66a-6a77-4011-8a25-53a79bdf5001', N'd69fd66a-6a77-4011-8a25-53a79bdf5001')
GO

INSERT INTO [dbo].[sys_log] ([F_Id], [F_Date], [F_Account], [F_NickName], [F_Type], [F_IPAddress], [F_IPAddressName], [F_ModuleId], [F_ModuleName], [F_Result], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_KeyValue], [F_CompanyId]) VALUES (N'39fc7689-d1cf-ca55-cd31-31720cfd607e', N'2021-05-13 14:14:45.0000000', N'admin', N'超级管理员', N'Login', N'0.0.0.1', N'iana保留地址', NULL, N'系统登录', N'1', N'登录成功', N'2021-05-13 14:14:45.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, N'd69fd66a-6a77-4011-8a25-53a79bdf5001')
GO

INSERT INTO [dbo].[sys_log] ([F_Id], [F_Date], [F_Account], [F_NickName], [F_Type], [F_IPAddress], [F_IPAddressName], [F_ModuleId], [F_ModuleName], [F_Result], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_KeyValue], [F_CompanyId]) VALUES (N'39fc768a-437c-1b28-56f2-b11bd53f8dcd', N'2021-05-13 14:15:15.0000000', N'admin', N'超级管理员', N'Create', N'0.0.0.1', N'iana保留地址', NULL, N'常规管理-系统管理-代码生成', N'1', N'代码生成操作,新增操作成功。', N'2021-05-13 14:15:15.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'', N'd69fd66a-6a77-4011-8a25-53a79bdf5001')
GO

INSERT INTO [dbo].[sys_log] ([F_Id], [F_Date], [F_Account], [F_NickName], [F_Type], [F_IPAddress], [F_IPAddressName], [F_ModuleId], [F_ModuleName], [F_Result], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_KeyValue], [F_CompanyId]) VALUES (N'39fc768d-4ced-62f3-fd69-b0365867a895', N'2021-05-13 14:18:33.0000000', N'admin', N'超级管理员', N'Login', N'0.0.0.1', N'iana保留地址', NULL, N'系统登录', N'1', N'登录成功', N'2021-05-13 14:18:34.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, N'd69fd66a-6a77-4011-8a25-53a79bdf5001')
GO

INSERT INTO [dbo].[sys_log] ([F_Id], [F_Date], [F_Account], [F_NickName], [F_Type], [F_IPAddress], [F_IPAddressName], [F_ModuleId], [F_ModuleName], [F_Result], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_KeyValue], [F_CompanyId]) VALUES (N'39fc768d-7981-09a3-b0fe-d47ec4cd3e3e', N'2021-05-13 14:18:45.0000000', N'admin', N'超级管理员', N'Delete', N'0.0.0.1', N'iana保留地址', NULL, N'常规管理-系统管理-系统菜单', N'1', N'系统菜单操作,删除操作成功。', N'2021-05-13 14:18:45.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'39fc768a-430b-9760-c945-80b622d5e8c4', N'd69fd66a-6a77-4011-8a25-53a79bdf5001')
GO


-- ----------------------------
-- Table structure for sys_module
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[sys_module]') AND type IN ('U'))
	DROP TABLE [dbo].[sys_module]
GO

CREATE TABLE [dbo].[sys_module] (
  [F_Id] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_ParentId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_Layers] int  NULL,
  [F_EnCode] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_FullName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_Icon] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_UrlAddress] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_Target] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_IsMenu] tinyint  NULL,
  [F_IsExpand] tinyint  NULL,
  [F_IsFields] tinyint  NULL,
  [F_IsPublic] tinyint  NULL,
  [F_AllowEdit] tinyint  NULL,
  [F_AllowDelete] tinyint  NULL,
  [F_SortCode] int  NULL,
  [F_DeleteMark] tinyint  NULL,
  [F_EnabledMark] tinyint  NULL,
  [F_Description] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_CreatorTime] datetime2(7)  NULL,
  [F_CreatorUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_LastModifyTime] datetime2(7)  NULL,
  [F_LastModifyUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_DeleteTime] datetime2(7)  NULL,
  [F_DeleteUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_Authorize] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[sys_module] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of sys_module
-- ----------------------------
INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'01849cc9-c6da-4184-92f8-34875dac1d42', N'462027E0-0848-41DD-BCC3-025DCAE65555', N'2', N'CodeGenerator', N'代码生成', N'fa fa-code', N'/SystemManage/CodeGenerator/Index', N'iframe', N'1', N'0', N'0', N'0', N'0', N'0', N'2', N'0', N'1', N'', N'2020-05-06 13:11:32.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-06-16 09:27:33.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'06bb3ea8-ec7f-4556-a427-8ff0ce62e873', N'873e2274-6884-4849-b636-7f04cca8242c', N'2', N'TextTool', N'富文本编辑器', N'fa fa-credit-card', N'../page/editor.html', N'expand', N'1', N'0', N'0', N'0', N'0', N'0', N'5', N'0', N'1', N'', N'2020-06-23 11:07:34.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-07-02 08:44:44.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'152a8e93-cebb-4574-ae74-2a86595ff986', N'462027E0-0848-41DD-BCC3-025DCAE65555', N'2', N'ModuleFields', N'字段管理', N'fa fa-table', N'/SystemManage/ModuleFields/Index', N'iframe', N'0', N'0', N'0', N'0', N'0', N'0', N'4', N'0', N'1', N'', N'2020-05-21 14:39:20.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-06-15 14:55:50.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'1dff096a-db2f-410c-af2f-12294bdbeccd', N'873e2274-6884-4849-b636-7f04cca8242c', N'2', N'UploadTool', N'文件上传', N'fa fa-arrow-up', N'../page/upload.html', N'expand', N'1', N'0', N'0', N'0', N'0', N'0', N'4', N'0', N'1', N'', N'2020-06-23 11:06:48.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-07-02 08:42:17.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'1e60fce5-3164-439d-8d29-4950b33011e2', N'873e2274-6884-4849-b636-7f04cca8242c', N'2', N'ColorTool', N'颜色选择', N'fa fa-dashboard', N'../page/color-select.html', N'expand', N'1', N'0', N'0', N'0', N'0', N'0', N'2', N'0', N'1', N'', N'2020-06-23 11:05:36.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-07-02 08:41:52.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'253646c6-ffd8-4c7f-9673-f349bbafcbe5', N'87dc2de3-ccbc-4dab-bb90-89fc68cbde4f', N'2', N'SystemOrganize', N'单位组织', N'fa fa-reorder', NULL, N'expand', N'1', N'1', N'0', N'0', N'0', N'0', N'0', N'0', N'1', N'', N'2020-06-15 14:52:19.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-10-14 10:35:20.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'2536fbf0-53ff-40a6-a093-73aa0a8fc035', N'873e2274-6884-4849-b636-7f04cca8242c', N'2', N'IconSelect', N'图标选择', N'fa fa-adn', N'../page/icon-picker.html', N'expand', N'1', N'0', N'0', N'0', N'0', N'0', N'1', N'0', N'1', N'', N'2020-06-23 11:05:01.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-07-02 08:41:12.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'262ca754-1c73-436c-a9a2-b6374451a845', N'253646c6-ffd8-4c7f-9673-f349bbafcbe5', N'2', N'DataPrivilegeRule', N'数据权限', N'fa fa-database', N'/SystemOrganize/DataPrivilegeRule/Index', N'iframe', N'1', N'0', N'0', N'0', N'0', N'0', N'3', N'0', N'1', N'', N'2020-06-01 09:44:58.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-06-16 08:11:44.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'26452c9a-243d-4c81-97b9-a3ad581c3bf4', N'253646c6-ffd8-4c7f-9673-f349bbafcbe5', N'3', N'Organize', N'机构管理', N'fa fa-sitemap', N'/SystemOrganize/Organize/Index', N'iframe', N'1', N'0', N'0', N'0', N'0', N'0', N'2', N'0', N'1', N'', N'2020-04-09 15:24:34.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-10-14 10:33:58.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'2c2ddbce-ee87-4134-9b32-54d0bd572910', N'462027E0-0848-41DD-BCC3-025DCAE65555', N'3', N'Form', N'表单设计', N'fa fa-wpforms', N'/SystemManage/Form/Index', N'iframe', N'1', N'0', N'0', N'0', N'0', N'0', N'8', N'0', N'1', N'', N'2020-07-08 14:34:38.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-07-08 15:26:44.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'30c629a0-910e-404b-8c29-a73a6291fd95', N'73FD1267-79BA-4E23-A152-744AF73117E9', N'3', N'AppLog', N'系统日志', N'fa fa-file', N'/SystemSecurity/AppLog/Index', N'iframe', N'1', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'1', N'', N'2020-07-08 10:12:42.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-07-08 10:14:06.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'337A4661-99A5-4E5E-B028-861CACAF9917', N'462027E0-0848-41DD-BCC3-025DCAE65555', N'2', N'Area', N'区域管理', N'fa fa-area-chart', N'/SystemManage/Area/Index', N'iframe', N'1', N'0', N'0', N'0', N'0', N'0', N'7', N'0', N'1', N'', NULL, NULL, N'2020-06-15 14:57:10.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'38CA5A66-C993-4410-AF95-50489B22939C', N'253646c6-ffd8-4c7f-9673-f349bbafcbe5', N'2', N'User', N'用户管理', N'fa fa-address-card-o', N'/SystemOrganize/User/Index', N'iframe', N'1', N'0', N'0', N'0', N'0', N'0', N'6', N'0', N'1', N'', NULL, NULL, N'2020-06-16 08:11:59.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'423A200B-FA5F-4B29-B7B7-A3F5474B725F', N'462027E0-0848-41DD-BCC3-025DCAE65555', N'2', N'ItemsData', N'数据字典', N'fa fa-align-justify', N'/SystemManage/ItemsData/Index', N'iframe', N'1', N'0', N'0', N'0', N'0', N'0', N'5', N'0', N'1', N'', NULL, NULL, N'2020-06-15 14:57:31.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'462027E0-0848-41DD-BCC3-025DCAE65555', N'87dc2de3-ccbc-4dab-bb90-89fc68cbde4f', N'2', N'SystemManage', N'系统管理', N'fa fa-gears', NULL, N'expand', N'1', N'1', N'0', N'0', N'0', N'0', N'1', N'0', N'1', N'', NULL, NULL, N'2020-06-23 10:38:07.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'484269cb-9aea-4af1-b7f6-f99e7e396ad1', N'462027E0-0848-41DD-BCC3-025DCAE65555', N'2', N'SystemOptions', N'系统配置', N'fa fa-gears', N'/SystemOrganize/SystemSet/SetForm', N'iframe', N'1', N'0', N'1', N'0', N'0', N'0', N'0', N'0', N'1', N'', N'2020-06-12 14:32:52.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-06-16 09:27:42.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'49F61713-C1E4-420E-BEEC-0B4DBC2D7DE8', N'73FD1267-79BA-4E23-A152-744AF73117E9', N'3', N'ServerMonitoring', N'服务器监控', N'fa fa-desktop', N'/SystemSecurity/ServerMonitoring/Index', N'expand', N'1', N'0', N'0', N'0', N'0', N'0', N'4', N'0', N'1', N'', NULL, NULL, N'2020-07-02 08:45:07.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'4efd6f84-a4a9-4176-aedd-153e7748cbac', N'bcd52760-009f-4673-80e5-ff166aa07687', N'2', N'ArticleCategory', N'新闻类别', N'fa fa-clone', N'/ContentManage/ArticleCategory/Index', N'iframe', N'1', N'0', N'0', N'0', N'0', N'0', N'1', N'0', N'1', N'', N'2020-06-09 19:42:39.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-06-23 15:59:53.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'5f9873e9-0308-4a8e-84b7-1c4c61f37654', N'87dc2de3-ccbc-4dab-bb90-89fc68cbde4f', N'2', N'FlowManage', N'流程中心', N'fa fa-stack-overflow', NULL, N'expand', N'1', N'1', N'0', N'0', N'0', N'0', N'3', N'0', N'1', N'', N'2020-07-14 15:39:44.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-08-12 11:17:33.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'605444e5-704f-4cca-8d00-75175e2aef05', N'5f9873e9-0308-4a8e-84b7-1c4c61f37654', N'3', N'ToDoFlow', N'待处理流程', N'fa fa-volume-control-phone', N'/FlowManage/Flowinstance/ToDoFlow', N'iframe', N'1', N'0', N'0', N'0', N'0', N'0', N'1', N'0', N'1', N'', N'2020-07-15 15:03:12.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'64A1C550-2C61-4A8C-833D-ACD0C012260F', N'462027E0-0848-41DD-BCC3-025DCAE65555', N'3', N'Module', N'系统菜单', N'fa fa-music', N'/SystemManage/Module/Index', N'iframe', N'1', N'0', N'0', N'0', N'0', N'0', N'1', N'0', N'1', N'测试', NULL, NULL, N'2020-07-14 15:45:36.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'6b196514-0df1-41aa-ae64-9bb598960709', N'87dc2de3-ccbc-4dab-bb90-89fc68cbde4f', N'2', N'FileManage', N'文件中心', N'fa fa-file-text-o', NULL, N'expand', N'1', N'1', N'0', N'0', N'0', N'0', N'4', N'0', N'1', N'', N'2020-07-22 11:43:27.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-08-12 11:17:44.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'73FD1267-79BA-4E23-A152-744AF73117E9', N'87dc2de3-ccbc-4dab-bb90-89fc68cbde4f', N'2', N'SystemSecurity', N'系统安全', N'fa fa-desktop', NULL, N'expand', N'1', N'1', N'0', N'0', N'0', N'0', N'2', N'0', N'1', N'', NULL, NULL, N'2020-06-23 10:54:41.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'7cb65e00-8af2-4cf2-b318-8ba28b3c154e', N'6b196514-0df1-41aa-ae64-9bb598960709', N'3', N'Uploadfile', N'文件管理', N'fa fa-file-text-o', N'/FileManage/Uploadfile/Index', N'iframe', N'1', N'0', N'0', N'0', N'0', N'0', N'1', N'0', N'1', N'', N'2020-07-22 12:05:35.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-07-22 17:20:34.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'7e4e4a48-4d51-4159-a113-2a211186f13a', N'253646c6-ffd8-4c7f-9673-f349bbafcbe5', N'3', N'Notice', N'系统公告', N'fa fa-book', N'/SystemOrganize/Notice/Index', N'iframe', N'1', N'0', N'1', N'0', N'0', N'0', N'0', N'0', N'1', N'', N'2020-04-14 15:39:29.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-10-14 10:35:17.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'873e2274-6884-4849-b636-7f04cca8242c', N'87dc2de3-ccbc-4dab-bb90-89fc68cbde4f', N'2', N'ToolManage', N'组件管理', N'fa fa-connectdevelop', NULL, N'expand', N'1', N'1', N'0', N'0', N'0', N'0', N'99', N'0', N'1', N'', N'2020-06-23 11:02:34.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-08-12 11:19:29.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'87dc2de3-ccbc-4dab-bb90-89fc68cbde4f', N'0', N'1', N'GeneralManage', N'常规管理', N'', NULL, N'expand', N'1', N'1', N'0', N'0', N'0', N'0', N'0', N'0', N'1', N'', N'2020-06-23 10:37:39.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-10-14 10:35:23.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'8e52143d-2f97-49e5-89a4-13469f66fc77', N'873e2274-6884-4849-b636-7f04cca8242c', N'2', N'SelectTool', N'下拉选择', N'fa fa-angle-double-down', N'../page/table-select.html', N'expand', N'1', N'0', N'0', N'0', N'0', N'0', N'3', N'0', N'1', N'', N'2020-06-23 11:06:12.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-07-02 08:42:05.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'91A6CFAD-B2F9-4294-BDAE-76DECF412C6C', N'253646c6-ffd8-4c7f-9673-f349bbafcbe5', N'2', N'Role', N'角色管理', N'fa fa-user-circle', N'/SystemOrganize/Role/Index', N'iframe', N'1', N'0', N'0', N'0', N'0', N'0', N'4', N'0', N'1', N'', NULL, NULL, N'2020-06-16 08:11:48.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'96EE855E-8CD2-47FC-A51D-127C131C9FB9', N'73FD1267-79BA-4E23-A152-744AF73117E9', N'3', N'Log', N'操作日志', N'fa fa-clock-o', N'/SystemSecurity/Log/Index', N'iframe', N'1', N'0', N'0', N'0', N'0', N'0', N'1', N'0', N'1', N'', NULL, NULL, N'2020-07-08 10:13:23.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'a303cbe1-60eb-437b-9a69-77ff8b48f173', N'253646c6-ffd8-4c7f-9673-f349bbafcbe5', N'3', N'SystemSet', N'租户设置', N'fa fa-connectdevelop', N'/SystemOrganize/SystemSet/Index', N'iframe', N'0', N'0', N'0', N'0', N'0', N'0', N'1', N'0', N'0', N'', N'2020-06-12 13:54:25.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-06-23 16:37:02.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'a3a4742d-ca39-42ec-b95a-8552a6fae579', N'73FD1267-79BA-4E23-A152-744AF73117E9', N'2', N'FilterIP', N'访问控制', N'fa fa-filter', N'/SystemSecurity/FilterIP/Index', N'iframe', N'1', N'0', N'0', N'0', N'0', N'0', N'2', N'0', N'1', NULL, N'2016-07-17 22:06:10.0000000', NULL, N'2020-04-16 14:10:38.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'a5b323e7-db24-468f-97d7-a17bf5396742', N'87dc2de3-ccbc-4dab-bb90-89fc68cbde4f', N'2', N'InfoManage', N'信息中心', N'fa fa-info', NULL, N'expand', N'1', N'1', N'0', N'0', N'0', N'0', N'5', N'0', N'1', N'', N'2020-07-29 16:40:58.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-08-12 11:17:52.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'bcd52760-009f-4673-80e5-ff166aa07687', N'87dc2de3-ccbc-4dab-bb90-89fc68cbde4f', N'2', N'ContentManage', N'内容管理', N'fa fa-building-o', NULL, N'expand', N'1', N'1', N'0', N'0', N'0', N'0', N'6', N'0', N'1', N'', N'2020-06-08 20:07:27.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-08-12 11:18:03.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'c14ab4f2-a1cf-4abd-953b-bacd70e78e8c', N'873e2274-6884-4849-b636-7f04cca8242c', N'2', N'AreaTool', N'省市县区选择器', N'fa fa-rocket', N'../page/area.html', N'expand', N'1', N'0', N'0', N'0', N'0', N'0', N'6', N'0', N'1', N'', N'2020-06-23 11:08:09.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-07-02 08:42:30.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'c87cd44f-d064-4d3c-a43e-de01a7a8785e', N'5f9873e9-0308-4a8e-84b7-1c4c61f37654', N'3', N'Flowinstance', N'我的流程', N'fa fa-user-o', N'/FlowManage/Flowinstance/Index', N'iframe', N'1', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'1', N'', N'2020-07-14 09:21:41.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-07-24 15:59:25.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'ca45b5ae-0252-4783-a23d-8633fc35e7e3', N'873e2274-6884-4849-b636-7f04cca8242c', N'3', N'cardTable', N'卡片表格', N'fa fa-cc-mastercard', N'../page/cardTable.html', N'expand', N'1', N'0', N'0', N'0', N'0', N'0', N'7', N'0', N'1', N'', N'2020-12-21 10:34:17.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-12-21 10:34:27.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'd419160a-0a54-4da2-98fe-fc57f2461a2d', N'873e2274-6884-4849-b636-7f04cca8242c', N'2', N'IconTool', N'图标列表', N'fa fa-dot-circle-o', N'../page/icon.html', N'expand', N'1', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'1', N'', N'2020-06-23 11:03:50.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-07-02 08:40:56.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'd742c96e-b61c-4cea-afeb-81805789687b', N'462027E0-0848-41DD-BCC3-025DCAE65555', N'2', N'ItemsType', N'字典分类', N'fa fa-align-justify', N'/SystemManage/ItemsType/Index', N'iframe', N'0', N'0', N'0', N'0', N'0', N'0', N'6', N'0', N'1', N'', N'2020-04-27 16:51:07.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-06-15 14:57:37.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'e3188a69-de3a-40ef-a5ff-5eaf460f5d20', N'73FD1267-79BA-4E23-A152-744AF73117E9', N'3', N'OpenJobs', N'定时任务', N'fa fa-paper-plane-o', N'/SystemSecurity/OpenJobs/Index', N'iframe', N'1', N'0', N'0', N'0', N'0', N'0', N'3', N'0', N'1', N'', N'2020-05-26 13:55:22.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-07-08 10:13:54.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'e5dc1c07-4234-46d1-bddb-d0442196c6b6', N'87dc2de3-ccbc-4dab-bb90-89fc68cbde4f', N'2', N'SmartScreen', N'自适应大屏', N'fa fa-tv', N'../page/smartscreen.html', N'blank', N'1', N'0', N'0', N'0', N'0', N'0', N'100', N'0', N'1', N'', N'2021-01-11 12:23:59.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2021-05-12 16:15:52.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, N'')
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'e9190a56-e173-4483-8a3e-f17b86e4766e', N'a5b323e7-db24-468f-97d7-a17bf5396742', N'3', N'Message', N'通知管理', N'fa fa-info-circle', N'/InfoManage/Message/Index', N'iframe', N'1', N'0', N'0', N'0', N'0', N'0', N'1', N'0', N'1', N'', N'2020-07-29 16:44:08.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-08-03 16:13:56.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'ed757a25-82d5-43cc-89f4-ed26a51fb4f0', N'462027E0-0848-41DD-BCC3-025DCAE65555', N'2', N'ModuleButton', N'菜单按钮', N'fa fa-arrows-alt', N'/SystemManage/ModuleButton/Index', N'iframe', N'0', N'0', N'0', N'0', N'0', N'0', N'3', N'0', N'1', N'', N'2020-04-27 16:56:30.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-06-15 14:55:45.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'ee136db7-178a-4bb0-b878-51287a5e2e2b', N'5f9873e9-0308-4a8e-84b7-1c4c61f37654', N'3', N'DoneFlow', N'已处理流程', N'fa fa-history', N'/FlowManage/Flowinstance/DoneFlow', N'iframe', N'1', N'0', N'0', N'0', N'0', N'0', N'2', N'0', N'1', N'', N'2020-07-15 15:05:33.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'F298F868-B689-4982-8C8B-9268CBF0308D', N'253646c6-ffd8-4c7f-9673-f349bbafcbe5', N'2', N'Duty', N'岗位管理', N'fa fa-users', N'/SystemOrganize/Duty/Index', N'iframe', N'1', N'0', N'0', N'0', N'0', N'0', N'5', N'0', N'1', N'', NULL, NULL, N'2020-06-16 08:11:54.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'f3277ddd-1bf1-4202-8a4b-15c29a405bd5', N'bcd52760-009f-4673-80e5-ff166aa07687', N'2', N'ArticleNews', N'新闻管理', N'fa fa-bell-o', N'/ContentManage/ArticleNews/Index', N'iframe', N'1', N'0', N'0', N'0', N'0', N'0', N'2', N'0', N'1', N'', N'2020-06-09 19:43:14.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-06-23 16:00:03.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'f82fd629-5f3a-45d6-8681-5ec47e66a807', N'462027E0-0848-41DD-BCC3-025DCAE65555', N'3', N'Flowscheme', N'流程设计', N'fa fa-list-alt', N'/SystemManage/Flowscheme/Index', N'iframe', N'1', N'0', N'0', N'0', N'0', N'0', N'9', N'0', N'1', N'', N'2020-07-10 08:50:52.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-07-14 08:53:52.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO


-- ----------------------------
-- Table structure for sys_modulebutton
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[sys_modulebutton]') AND type IN ('U'))
	DROP TABLE [dbo].[sys_modulebutton]
GO

CREATE TABLE [dbo].[sys_modulebutton] (
  [F_Id] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_ModuleId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_ParentId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_Layers] int  NULL,
  [F_EnCode] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_FullName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_Icon] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_Location] int  NULL,
  [F_JsEvent] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_UrlAddress] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_Split] tinyint  NULL,
  [F_IsPublic] tinyint  NULL,
  [F_AllowEdit] tinyint  NULL,
  [F_AllowDelete] tinyint  NULL,
  [F_SortCode] int  NULL,
  [F_DeleteMark] tinyint  NULL,
  [F_EnabledMark] tinyint  NULL,
  [F_Description] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_CreatorTime] datetime2(7)  NULL,
  [F_CreatorUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_LastModifyTime] datetime2(7)  NULL,
  [F_LastModifyUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_DeleteTime] datetime2(7)  NULL,
  [F_DeleteUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_Authorize] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[sys_modulebutton] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of sys_modulebutton
-- ----------------------------
INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'01600a2b-c218-48d6-bb37-842daa727248', N'152a8e93-cebb-4574-ae74-2a86595ff986', N'0', N'1', N'NF-delete', N'删除字段', NULL, N'2', N'delete', N'/SystemManage/ModuleFields/DeleteForm', N'0', N'0', N'0', N'0', N'2', N'0', N'1', N'', N'2020-05-21 14:39:20.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-05-21 15:15:16.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'071f5982-efb2-4fa3-a6cf-a02f3f1f9d92', N'ed757a25-82d5-43cc-89f4-ed26a51fb4f0', N'0', N'1', N'NF-add', N'新增按钮', NULL, N'1', N'add', N'/SystemManage/ModuleButton/Form', N'0', N'0', N'0', N'0', N'1', N'0', N'1', NULL, NULL, NULL, N'2020-04-27 16:56:56.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'0a1ba1d7-b4f3-45a4-a4da-e70fb25bb766', N'e9190a56-e173-4483-8a3e-f17b86e4766e', N'0', N'1', N'NF-delete', N'删除', NULL, N'2', N'delete', N'/InfoManage/Message/DeleteForm', N'0', N'0', N'0', N'0', N'2', N'0', N'1', NULL, N'2020-07-29 16:44:08.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'0b1b307b-2aac-456b-acfb-484a05c71bd7', N'26452c9a-243d-4c81-97b9-a3ad581c3bf4', N'0', N'1', N'NF-edit', N'修改机构', NULL, N'2', N'edit', N'/SystemOrganize/Organize/Form', N'0', N'0', N'0', N'0', N'2', N'0', N'1', N'', NULL, NULL, N'2020-07-23 10:47:04.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'0d777b07-041a-4205-a393-d1a009aaafc7', N'423A200B-FA5F-4B29-B7B7-A3F5474B725F', N'0', N'1', N'NF-edit', N'修改字典', NULL, N'2', N'edit', N'/SystemManage/ItemsData/Form', N'0', N'0', N'0', N'0', N'3', N'0', N'1', NULL, NULL, NULL, N'2016-07-25 15:37:42.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'0e156a57-8133-4d1b-9d0f-9b7554e7b1fc', N'd742c96e-b61c-4cea-afeb-81805789687b', N'0', N'1', N'NF-edit', N'修改分类', NULL, N'2', N'edit', N'/SystemManage/ItemsType/Form', N'0', N'0', N'0', N'0', N'3', N'0', N'1', NULL, NULL, NULL, N'2020-04-27 16:52:20.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'0fa5e0a8-c786-40af-81af-b133b42dded5', N'262ca754-1c73-436c-a9a2-b6374451a845', N'0', N'1', N'NF-delete', N'删除', NULL, N'2', N'delete', N'/SystemOrganize/DataPrivilegeRule/DeleteForm', N'0', N'0', N'0', N'0', N'2', N'0', N'1', N'', N'2020-06-01 09:44:58.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-06-16 08:13:22.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'104bcc01-0cfd-433f-87f4-29a8a3efb313', N'423A200B-FA5F-4B29-B7B7-A3F5474B725F', N'0', N'1', N'NF-add', N'新建字典', NULL, N'1', N'add', N'/SystemManage/ItemsData/Form', N'0', N'0', N'0', N'0', N'1', N'0', N'1', NULL, NULL, NULL, N'2016-07-25 15:37:35.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'13c9a15f-c50d-4f09-8344-fd0050f70086', N'F298F868-B689-4982-8C8B-9268CBF0308D', N'0', N'1', N'NF-add', N'新建岗位', NULL, N'1', N'add', N'/SystemOrganize/Duty/Form', N'0', N'0', N'0', N'0', N'1', N'0', N'1', N'', NULL, NULL, N'2020-06-16 08:13:52.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'14617a4f-bfef-4bc2-b943-d18d3ff8d22f', N'38CA5A66-C993-4410-AF95-50489B22939C', N'0', N'1', N'NF-delete', N'删除用户', NULL, N'2', N'delete', N'/SystemOrganize/User/DeleteForm', N'0', N'0', N'0', N'0', N'3', N'0', N'1', N'', NULL, NULL, N'2020-06-16 08:14:19.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'15362a59-b242-494a-bc6e-413b4a63580e', N'38CA5A66-C993-4410-AF95-50489B22939C', N'0', N'1', N'NF-disabled', N'禁用', NULL, N'2', N'disabled', N'/SystemOrganize/User/DisabledAccount', N'0', N'0', N'0', N'0', N'6', N'0', N'1', N'', N'2016-07-25 15:25:54.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-06-16 08:14:30.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'153e4773-7425-403f-abf7-42db13f84c8d', N'c87cd44f-d064-4d3c-a43e-de01a7a8785e', N'0', N'1', N'NF-details', N'进度', NULL, N'2', N'details', N'/FlowManage/Flowinstance/Details', N'0', N'0', N'0', N'0', N'3', N'0', N'1', N'', N'2020-07-14 09:21:41.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-07-14 13:58:40.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'17a0e46f-28f9-4787-832c-0da25c321ce4', N'7cb65e00-8af2-4cf2-b318-8ba28b3c154e', N'0', N'1', N'NF-download', N'下载', NULL, N'1', N'download', N'/FileManage/Uploadfile/Download', N'0', N'0', N'0', N'0', N'0', N'0', N'1', N'', N'2020-07-22 12:05:35.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-07-22 14:47:39.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'1a588a3b-95d7-4b3a-b50a-d3bc40de9fe3', N'7cb65e00-8af2-4cf2-b318-8ba28b3c154e', N'0', N'1', N'NF-details', N'查看', NULL, N'2', N'details', N'/FileManage/Uploadfile/Details', N'0', N'0', N'0', N'0', N'1', N'0', N'1', N'', N'2020-07-22 12:05:35.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-07-22 14:47:46.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'1b72be70-e44d-43d6-91d0-dc3ad628d22e', N'26452c9a-243d-4c81-97b9-a3ad581c3bf4', N'0', N'1', N'NF-details', N'查看机构', NULL, N'2', N'details', N'/SystemOrganize/Organize/Details', N'0', N'0', N'0', N'0', N'4', N'0', N'1', N'', NULL, NULL, N'2020-07-23 10:47:15.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'1d1e71a6-dd8b-4052-8093-f1d7d347b9bc', N'a303cbe1-60eb-437b-9a69-77ff8b48f173', N'0', N'1', N'NF-details', N'查看', NULL, N'2', N'details', N'/SystemOrganize/SystemSet/Details', N'0', N'0', N'0', N'0', N'2', N'0', N'1', N'', N'2020-06-12 13:54:25.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-06-16 08:12:44.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'1ee1c46b-e767-4532-8636-936ea4c12003', N'423A200B-FA5F-4B29-B7B7-A3F5474B725F', N'0', N'1', N'NF-delete', N'删除字典', NULL, N'2', N'delete', N'/SystemManage/ItemsData/DeleteForm', N'0', N'0', N'0', N'0', N'4', N'0', N'1', NULL, NULL, NULL, N'2016-07-25 15:37:53.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'208c2915-d6d0-4bb0-8ec4-154f86561f5a', N'e3188a69-de3a-40ef-a5ff-5eaf460f5d20', N'0', N'1', N'NF-enabled', N'启用', NULL, N'2', N'enabled', N'/SystemSecurity/OpenJobs/ChangeStatus', N'0', N'0', N'0', N'0', N'4', N'0', N'1', N'', N'2020-05-26 13:55:50.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-05-27 08:42:27.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'23780fa8-b92c-4c0e-830e-ddcbe6cf4463', N'64A1C550-2C61-4A8C-833D-ACD0C012260F', N'0', N'1', N'NF-modulefields', N'字段管理', NULL, N'2', N'modulefields', N'/SystemManage/ModuleFields/Index', N'0', N'0', N'0', N'0', N'6', N'0', N'1', N'', N'2020-05-21 14:28:48.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'239077ff-13e1-4720-84e1-67b6f0276979', N'91A6CFAD-B2F9-4294-BDAE-76DECF412C6C', N'0', N'1', N'NF-delete', N'删除角色', NULL, N'2', N'delete', N'/SystemOrganize/Role/DeleteForm', N'0', N'0', N'0', N'0', N'3', N'0', N'1', N'', NULL, NULL, N'2020-06-16 08:13:39.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'29306956-f9b2-4e76-bc23-4b8f02d21be3', N'F298F868-B689-4982-8C8B-9268CBF0308D', N'0', N'1', N'NF-import', N'导入', NULL, N'1', N'import', N'/SystemOrganize/Duty/Import', NULL, N'0', N'0', N'0', N'5', N'0', N'1', N'', N'2020-08-12 10:17:30.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-08-12 10:17:48.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'2a8f5342-5eb7-491c-a1a9-a2631d8eb5d6', N'38CA5A66-C993-4410-AF95-50489B22939C', N'0', N'1', N'NF-enabled', N'启用', NULL, N'2', N'enabled', N'/SystemOrganize/User/EnabledAccount', N'0', N'0', N'0', N'0', N'7', N'0', N'1', N'', N'2016-07-25 15:28:08.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-06-16 08:14:33.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'2cde1cd0-cfc8-4901-96ef-1fe0c8bf997c', N'2c2ddbce-ee87-4134-9b32-54d0bd572910', N'0', N'1', N'NF-view', N'视图模型', NULL, N'1', N'view', N'/SystemManage/Form/Index', NULL, N'0', N'0', N'0', N'5', N'0', N'1', N'', N'2020-07-09 12:06:05.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'30bf72ed-f62f-49a9-adfc-49693871605f', N'd742c96e-b61c-4cea-afeb-81805789687b', N'0', N'1', N'NF-details', N'查看分类', NULL, N'2', N'details', N'/SystemManage/ItemsType/Details', N'0', N'0', N'0', N'0', N'5', N'0', N'1', NULL, NULL, NULL, N'2020-04-27 16:52:39.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'310bb831-a46f-4117-9d02-a3e551611dcf', N'e3188a69-de3a-40ef-a5ff-5eaf460f5d20', N'0', N'1', N'NF-delete', N'删除任务', NULL, N'2', N'delete', N'/SystemSecurity/OpenJobs/DeleteForm', N'0', N'0', N'0', N'0', N'3', N'0', N'1', N'', N'2020-05-26 13:55:50.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-05-26 13:56:35.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'329c0326-ce68-4a24-904d-aade67a90fc7', N'a3a4742d-ca39-42ec-b95a-8552a6fae579', N'0', N'1', N'NF-details', N'查看策略', NULL, N'2', N'details', N'/SystemSecurity/FilterIP/Details', N'0', N'0', N'0', N'0', N'4', N'0', N'1', NULL, NULL, NULL, N'2020-04-17 12:51:25.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'35fc1b7c-40b0-42b8-a0f9-c67087566289', N'f82fd629-5f3a-45d6-8681-5ec47e66a807', N'0', N'1', N'NF-edit', N'修改', NULL, N'2', N'edit', N'/SystemManage/Flowscheme/Form', N'0', N'0', N'0', N'0', N'1', N'0', N'1', NULL, N'2020-07-10 08:50:52.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'38e39592-6e86-42fb-8f72-adea0c82cbc1', N'38CA5A66-C993-4410-AF95-50489B22939C', N'0', N'1', N'NF-revisepassword', N'密码重置', NULL, N'2', N'revisepassword', N'/SystemOrganize/User/RevisePassword', N'0', N'0', N'0', N'0', N'5', N'0', N'1', N'', N'2016-07-25 14:18:19.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-06-16 08:14:26.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'3a35c662-a356-45e4-953d-00ebd981cad6', N'96EE855E-8CD2-47FC-A51D-127C131C9FB9', N'0', N'1', N'NF-removelog', N'清空日志', NULL, N'1', N'removeLog', N'/SystemSecurity/Log/RemoveLog', N'0', N'0', N'0', N'0', N'1', N'0', N'1', NULL, NULL, NULL, N'2020-04-07 14:34:56.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'3c8bc8ed-4cc4-43bc-accd-d4acb2a0358d', N'30c629a0-910e-404b-8c29-a73a6291fd95', N'0', N'1', N'NF-details', N'查看日志', NULL, N'2', N'details', N'/SystemSecurity/AppLog/Details', N'0', N'1', N'0', N'0', N'0', N'0', N'1', N'', N'2020-07-08 10:41:26.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-07-08 11:04:45.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'3d0e99d1-a150-43dc-84ae-f0e2e0ad2217', N'ed757a25-82d5-43cc-89f4-ed26a51fb4f0', N'0', N'1', N'NF-edit', N'修改按钮', NULL, N'2', N'edit', N'/SystemManage/ModuleButton/Form', N'0', N'0', N'0', N'0', N'2', N'0', N'1', NULL, NULL, NULL, N'2020-04-27 16:57:01.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'3f69d32f-cb3b-4fa0-863b-98b9a090d7e9', N'7e4e4a48-4d51-4159-a113-2a211186f13a', N'0', N'1', N'NF-add', N'新建公告', NULL, N'1', N'add', N'/SystemOrganize/Notice/Form', N'0', N'0', N'0', N'0', N'1', N'0', N'1', N'', NULL, NULL, N'2020-06-16 08:12:18.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'43e09a61-c2b0-46c1-9b81-76d686b390d4', N'ed757a25-82d5-43cc-89f4-ed26a51fb4f0', N'0', N'1', N'NF-clonebutton', N'克隆按钮', NULL, N'1', N'clonebutton', N'/SystemManage/ModuleButton/CloneButton', N'0', N'0', N'0', N'0', N'5', N'0', N'1', NULL, N'2020-04-28 09:54:52.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-05-11 14:55:36.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'4727adf7-5525-4c8c-9de5-39e49c268349', N'38CA5A66-C993-4410-AF95-50489B22939C', N'0', N'1', N'NF-edit', N'修改用户', NULL, N'2', N'edit', N'/SystemOrganize/User/Form', N'0', N'0', N'0', N'0', N'2', N'0', N'1', N'', NULL, NULL, N'2020-06-16 08:14:16.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'48afe7b3-e158-4256-b50c-cd0ee7c6dcc9', N'337A4661-99A5-4E5E-B028-861CACAF9917', N'0', N'1', N'NF-add', N'新建区域', NULL, N'1', N'add', N'/SystemManage/Area/Form', N'0', N'0', N'0', N'0', N'1', N'0', N'1', NULL, NULL, NULL, N'2016-07-25 15:32:26.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'4b876abc-1b85-47b0-abc7-96e313b18ed8', N'423A200B-FA5F-4B29-B7B7-A3F5474B725F', N'0', N'1', N'NF-itemstype', N'分类管理', NULL, N'1', N'itemstype', N'/SystemManage/ItemsType/Index', N'0', N'0', N'0', N'0', N'2', N'0', N'1', NULL, N'2016-07-25 15:36:21.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-04-07 14:33:58.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'4bb19533-8e81-419b-86a1-7ee56bf1dd45', N'252229DB-35CA-47AE-BDAE-C9903ED5BA7B', N'0', N'1', N'NF-delete', N'删除机构', NULL, N'2', N'delete', N'/SystemManage/Organize/DeleteForm', N'0', N'0', N'0', N'0', N'3', N'0', N'1', NULL, NULL, NULL, N'2020-04-07 14:22:56.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'4c794628-9b09-4d60-8fb5-63c1a37b2b60', N'2c2ddbce-ee87-4134-9b32-54d0bd572910', N'0', N'1', N'NF-edit', N'修改', NULL, N'2', N'edit', N'/SystemManage/Form/Form', N'0', N'0', N'0', N'0', N'1', N'0', N'1', NULL, N'2020-07-08 14:34:38.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'4f727b61-0aa4-45f0-83b5-7fcddfe034e8', N'ed757a25-82d5-43cc-89f4-ed26a51fb4f0', N'0', N'1', N'NF-delete', N'删除按钮', NULL, N'2', N'delete', N'/SystemManage/ModuleButton/DeleteForm', N'0', N'0', N'0', N'0', N'3', N'0', N'1', NULL, NULL, NULL, N'2020-04-27 16:57:10.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'55cc5aba-8121-4151-8df5-f6846396d1a3', N'2c2ddbce-ee87-4134-9b32-54d0bd572910', N'0', N'1', N'NF-add', N'新增', NULL, N'1', N'add', N'/SystemManage/Form/Form', N'0', N'0', N'0', N'0', N'0', N'0', N'1', NULL, N'2020-07-08 14:34:38.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'5c321b1f-4f56-4276-a1aa-dd23ce12a1fc', N'c87cd44f-d064-4d3c-a43e-de01a7a8785e', N'0', N'1', N'NF-delete', N'删除', NULL, N'2', N'delete', N'/FlowManage/Flowinstance/DeleteForm', N'0', N'0', N'0', N'0', N'2', N'0', N'1', NULL, N'2020-07-14 09:21:41.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'5d708d9d-6ebe-40ea-8589-e3efce9e74ec', N'91A6CFAD-B2F9-4294-BDAE-76DECF412C6C', N'0', N'1', N'NF-add', N'新建角色', NULL, N'1', N'add', N'/SystemOrganize/Role/Form', N'0', N'0', N'0', N'0', N'1', N'0', N'1', N'', NULL, NULL, N'2020-06-16 08:13:32.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'63cd2162-ab5f-4b7f-9bbd-5c2e7625e639', N'152a8e93-cebb-4574-ae74-2a86595ff986', N'0', N'1', N'NF-details', N'查看字段', NULL, N'2', N'details', N'/SystemManage/ModuleFields/Details', N'0', N'0', N'0', N'0', N'3', N'0', N'1', N'', N'2020-05-21 14:39:20.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-05-21 15:11:22.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'6cd4c3ac-048c-485a-bd4d-e0923f8d7f6e', N'f3277ddd-1bf1-4202-8a4b-15c29a405bd5', N'0', N'1', N'NF-edit', N'修改新闻', NULL, N'2', N'edit', N'/ContentManage/ArticleNews/Form', N'0', N'0', N'0', N'0', N'2', N'0', N'1', N'', N'2020-06-23 15:29:43.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-06-23 16:00:44.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'6f872aa0-1aae-4f42-a3ba-a61079057749', N'e9190a56-e173-4483-8a3e-f17b86e4766e', N'0', N'1', N'NF-edit', N'修改', NULL, N'2', N'edit', N'/InfoManage/Message/Form', N'0', N'0', N'0', N'0', N'1', N'0', N'1', NULL, N'2020-07-29 16:44:08.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'709a4a7b-4d98-462d-b47c-351ef11db06f', N'252229DB-35CA-47AE-BDAE-C9903ED5BA7B', N'0', N'1', N'NF-Details', N'查看机构', NULL, N'2', N'details', N'/SystemManage/Organize/Details', N'0', N'0', N'0', N'0', N'4', N'0', N'1', NULL, NULL, NULL, N'2020-04-07 14:23:01.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'73ac1957-7558-49f6-8642-59946d05b8e6', N'f82fd629-5f3a-45d6-8681-5ec47e66a807', N'0', N'1', N'NF-details', N'查看', NULL, N'2', N'details', N'/SystemManage/Flowscheme/Details', N'0', N'0', N'0', N'0', N'3', N'0', N'1', NULL, N'2020-07-10 08:50:52.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'746629aa-858b-4c5e-9335-71b0fa08a584', N'ed757a25-82d5-43cc-89f4-ed26a51fb4f0', N'0', N'1', N'NF-details', N'查看按钮', NULL, N'2', N'details', N'/SystemManage/ModuleButton/Details', N'0', N'0', N'0', N'0', N'4', N'0', N'1', NULL, NULL, NULL, N'2020-04-27 17:37:40.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'74eecdfb-3bee-405d-be07-27a78219c179', N'38CA5A66-C993-4410-AF95-50489B22939C', N'0', N'1', N'NF-add', N'新建用户', NULL, N'1', N'add', N'/SystemOrganize/User/Form', N'0', N'0', N'0', N'0', N'1', N'0', N'1', N'', NULL, NULL, N'2020-06-16 08:14:13.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'761f50a6-c1b2-4234-b0af-8f515ec74fe8', N'f3277ddd-1bf1-4202-8a4b-15c29a405bd5', N'0', N'1', N'NF-details', N'查看新闻', NULL, N'2', N'details', N'/ContentManage/ArticleNews/Details', N'0', N'0', N'0', N'0', N'4', N'0', N'1', N'', N'2020-06-23 15:29:43.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-06-23 16:00:53.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'772eb88a-5f67-4bb1-a122-0c83a2bdb5ef', N'c87cd44f-d064-4d3c-a43e-de01a7a8785e', N'0', N'1', N'NF-add', N'申请', NULL, N'1', N'add', N'/FlowManage/Flowinstance/Form', N'0', N'0', N'0', N'0', N'0', N'0', N'1', N'', N'2020-07-14 09:21:41.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-07-14 13:58:30.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'7e10a7ac-8b65-4c7c-8eee-92d69d7dcbd9', N'26452c9a-243d-4c81-97b9-a3ad581c3bf4', N'0', N'1', N'NF-add', N'新建机构', NULL, N'1', N'add', N'/SystemOrganize/Organize/Form', N'0', N'0', N'0', N'0', N'1', N'0', N'1', N'', NULL, NULL, N'2020-07-23 10:46:58.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'7ee3ff62-ab18-4886-9451-89b1d152172e', N'7e4e4a48-4d51-4159-a113-2a211186f13a', N'0', N'1', N'NF-details', N'查看公告', NULL, N'2', N'details', N'/SystemOrganize/Notice/Details', N'0', N'0', N'0', N'0', N'4', N'0', N'1', N'', NULL, NULL, N'2020-06-16 08:12:28.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'82b2f4a2-55a1-4f44-b667-3449739643f6', N'262ca754-1c73-436c-a9a2-b6374451a845', N'0', N'1', N'NF-edit', N'修改', NULL, N'2', N'edit', N'/SystemOrganize/DataPrivilegeRule/Form', N'0', N'0', N'0', N'0', N'1', N'0', N'1', N'', N'2020-06-01 09:44:58.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-06-16 08:13:18.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'82f162cb-beb9-4a79-8924-cd1860e26e2e', N'423A200B-FA5F-4B29-B7B7-A3F5474B725F', N'0', N'1', N'NF-details', N'查看字典', NULL, N'2', N'details', N'/SystemManage/ItemsData/Details', N'0', N'0', N'0', N'0', N'5', N'0', N'1', NULL, NULL, NULL, N'2020-04-17 12:50:57.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'832f5195-f3ab-4683-82ad-a66a71735ffc', N'2c2ddbce-ee87-4134-9b32-54d0bd572910', N'0', N'1', N'NF-details', N'查看', NULL, N'2', N'details', N'/SystemManage/Form/Details', N'0', N'0', N'0', N'0', N'3', N'0', N'1', NULL, N'2020-07-08 14:34:38.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'8379135e-5b13-4236-bfb1-9289e6129034', N'a3a4742d-ca39-42ec-b95a-8552a6fae579', N'0', N'1', N'NF-delete', N'删除策略', NULL, N'2', N'delete', N'/SystemSecurity/FilterIP/DeleteForm', N'0', N'0', N'0', N'0', N'3', N'0', N'1', NULL, NULL, NULL, N'2016-07-25 15:57:57.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'85bfbb9d-24f0-4a6f-8bb8-0f87826d04fa', N'152a8e93-cebb-4574-ae74-2a86595ff986', N'0', N'1', N'NF-add', N'新增字段', NULL, N'1', N'add', N'/SystemManage/ModuleFields/Form', N'0', N'0', N'0', N'0', N'0', N'0', N'1', N'', N'2020-05-21 14:39:20.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-05-21 15:38:53.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'85F5212F-E321-4124-B155-9374AA5D9C10', N'64A1C550-2C61-4A8C-833D-ACD0C012260F', N'0', N'1', N'NF-delete', N'删除菜单', NULL, N'2', N'delete', N'/SystemManage/Module/DeleteForm', N'0', N'0', N'0', N'0', N'3', N'0', N'1', NULL, NULL, NULL, N'2016-07-25 15:41:15.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'87068c95-42c8-4f20-b786-27cb9d3d5ff7', N'e3188a69-de3a-40ef-a5ff-5eaf460f5d20', N'0', N'1', N'NF-add', N'新建任务', NULL, N'1', N'add', N'/SystemSecurity/OpenJobs/Form', N'0', N'0', N'0', N'0', N'1', N'0', N'1', N'', N'2020-05-26 13:55:50.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-05-26 13:56:06.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'88f7b3a8-fd6d-4f8e-a861-11405f434868', N'F298F868-B689-4982-8C8B-9268CBF0308D', N'0', N'1', N'NF-details', N'查看岗位', NULL, N'2', N'details', N'/SystemOrganize/Duty/Details', N'0', N'0', N'0', N'0', N'4', N'0', N'1', N'', NULL, NULL, N'2020-06-16 08:14:01.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'89d7a69d-b953-4ce2-9294-db4f50f2a157', N'337A4661-99A5-4E5E-B028-861CACAF9917', N'0', N'1', N'NF-edit', N'修改区域', NULL, N'2', N'edit', N'/SystemManage/Area/Form', N'0', N'0', N'0', N'0', N'2', N'0', N'1', NULL, NULL, NULL, N'2016-07-25 15:32:42.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'8a9993af-69b2-4d8a-85b3-337745a1f428', N'F298F868-B689-4982-8C8B-9268CBF0308D', N'0', N'1', N'NF-delete', N'删除岗位', NULL, N'2', N'delete', N'/SystemOrganize/Duty/DeleteForm', N'0', N'0', N'0', N'0', N'3', N'0', N'1', N'', NULL, NULL, N'2020-06-16 08:13:58.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'8c7013a9-3682-4367-8bc6-c77ca89f346b', N'337A4661-99A5-4E5E-B028-861CACAF9917', N'0', N'1', N'NF-delete', N'删除区域', NULL, N'2', N'delete', N'/SystemManage/Area/DeleteForm', N'0', N'0', N'0', N'0', N'3', N'0', N'1', NULL, NULL, NULL, N'2016-07-25 15:32:53.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'8f32069f-20f3-48c9-8e35-cd245fffcf64', N'01849cc9-c6da-4184-92f8-34875dac1d42', N'0', N'1', N'NF-add', N'模板生成', NULL, N'2', N'add', N'/SystemManage/CodeGenerator/Form', N'0', N'0', N'0', N'0', N'0', N'0', N'1', N'', NULL, NULL, N'2020-07-23 15:36:31.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'8f698747-a1c3-468d-9279-99990987e0f9', N'7e4e4a48-4d51-4159-a113-2a211186f13a', N'0', N'1', N'NF-delete', N'删除公告', NULL, N'2', N'delete', N'/SystemOrganize/Notice/DeleteForm', N'0', N'0', N'0', N'0', N'3', N'0', N'1', N'', NULL, NULL, N'2020-06-16 08:12:24.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'91be873e-ccb7-434f-9a3b-d312d6d5798a', N'252229DB-35CA-47AE-BDAE-C9903ED5BA7B', N'0', N'1', N'NF-edit', N'修改机构', NULL, N'2', N'edit', N'/SystemManage/Organize/Form', N'0', N'0', N'0', N'0', N'2', N'0', N'1', NULL, NULL, NULL, N'2020-04-07 14:22:50.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'91d768bb-fb68-4807-b3b6-db355bdd6e09', N'2c2ddbce-ee87-4134-9b32-54d0bd572910', N'0', N'1', N'NF-delete', N'删除', NULL, N'2', N'delete', N'/SystemManage/Form/DeleteForm', N'0', N'0', N'0', N'0', N'2', N'0', N'1', NULL, N'2020-07-08 14:34:38.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'926ae4a9-0ecb-4d5e-a66e-5bae15ae27c2', N'a303cbe1-60eb-437b-9a69-77ff8b48f173', N'0', N'1', N'NF-edit', N'修改', NULL, N'2', N'edit', N'/SystemOrganize/SystemSet/Form', N'0', N'0', N'0', N'0', N'1', N'0', N'1', N'', N'2020-06-12 13:54:25.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-06-16 08:12:41.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'9450c723-d64d-459c-9c52-555773a8b50e', N'4efd6f84-a4a9-4176-aedd-153e7748cbac', N'0', N'1', N'NF-add', N'新建类别', NULL, N'1', N'add', N'/ContentManage/ArticleCategory/Form', N'0', N'0', N'0', N'0', N'1', N'0', N'1', N'', N'2020-06-23 15:27:36.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-06-23 16:00:12.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'957a355d-d931-40f6-9da0-dddfd9135fe0', N'e9190a56-e173-4483-8a3e-f17b86e4766e', N'0', N'1', N'NF-details', N'查看', NULL, N'2', N'details', N'/InfoManage/Message/Details', N'0', N'0', N'0', N'0', N'3', N'0', N'1', NULL, N'2020-07-29 16:44:08.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'98c2519c-b39f-4bf3-9543-5cc2630a4bbd', N'152a8e93-cebb-4574-ae74-2a86595ff986', N'0', N'1', N'NF-clonefields', N'克隆字段', NULL, N'1', N'clonefields', N'/SystemManage/ModuleFields/CloneFields', N'0', N'0', N'0', N'0', N'5', N'0', N'1', N'', N'2020-05-21 15:39:48.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-05-21 15:40:35.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'9fc77888-bbca-4996-9240-a0f389819f6f', N'7e4e4a48-4d51-4159-a113-2a211186f13a', N'0', N'1', N'NF-edit', N'修改公告', NULL, N'2', N'edit', N'/SystemOrganize/Notice/Form', N'0', N'0', N'0', N'0', N'2', N'0', N'1', N'', NULL, NULL, N'2020-06-16 08:12:21.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'9FD543DB-C5BB-4789-ACFF-C5865AFB032C', N'64A1C550-2C61-4A8C-833D-ACD0C012260F', N'0', N'1', N'NF-add', N'新增菜单', NULL, N'1', N'add', N'/SystemManage/Module/Form', N'0', N'0', N'0', N'0', N'1', N'0', N'1', NULL, NULL, NULL, N'2016-07-25 15:41:08.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'a0a41d87-494b-40b5-bd03-0f75c75be7cb', N'337A4661-99A5-4E5E-B028-861CACAF9917', N'0', N'1', N'NF-details', N'查看区域', NULL, N'2', N'details', N'/SystemManage/Area/Details', N'0', N'0', N'0', N'0', N'4', N'0', N'1', NULL, NULL, NULL, N'2020-04-27 17:38:21.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'a2e2a8ba-9311-4699-bcef-b79a2b59b08f', N'4efd6f84-a4a9-4176-aedd-153e7748cbac', N'0', N'1', N'NF-delete', N'删除类别', NULL, N'2', N'delete', N'/ContentManage/ArticleCategory/DeleteForm', N'0', N'0', N'0', N'0', N'3', N'0', N'1', N'', N'2020-06-23 15:27:36.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-06-23 16:00:20.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'a5619a09-f283-4ed7-82e0-9609815cb62a', N'f3277ddd-1bf1-4202-8a4b-15c29a405bd5', N'0', N'1', N'NF-delete', N'删除新闻', NULL, N'2', N'delete', N'/ContentManage/ArticleNews/DeleteForm', N'0', N'0', N'0', N'0', N'3', N'0', N'1', N'', N'2020-06-23 15:29:43.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-06-23 16:00:48.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'aaf58c1b-4af2-4e5f-a3e4-c48e86378191', N'a3a4742d-ca39-42ec-b95a-8552a6fae579', N'0', N'1', N'NF-edit', N'修改策略', NULL, N'2', N'edit', N'/SystemSecurity/FilterIP/Form', N'0', N'0', N'0', N'0', N'2', N'0', N'1', NULL, NULL, NULL, N'2016-07-25 15:57:49.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'abfdff21-8ebf-4024-8555-401b4df6acd9', N'38CA5A66-C993-4410-AF95-50489B22939C', N'0', N'1', N'NF-details', N'查看用户', NULL, N'2', N'details', N'/SystemOrganize/User/Details', N'0', N'0', N'0', N'0', N'4', N'0', N'1', N'', NULL, NULL, N'2020-06-16 08:14:22.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'b4be6eee-3509-4685-8064-34b9cacc690a', N'ee136db7-178a-4bb0-b878-51287a5e2e2b', N'0', N'1', N'NF-details', N'进度', NULL, N'2', N'details', N'/FlowManage/Flowinstance/Details', N'0', N'0', N'0', N'0', N'1', N'0', N'1', N'', N'2020-07-15 15:05:48.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-07-15 15:04:15.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'b83c84e4-6264-4b8e-b319-a49fbf34860d', N'262ca754-1c73-436c-a9a2-b6374451a845', N'0', N'1', N'NF-add', N'新增', NULL, N'1', N'add', N'/SystemOrganize/DataPrivilegeRule/Form', N'0', N'0', N'0', N'0', N'0', N'0', N'1', N'', N'2020-06-01 09:44:58.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-06-16 08:13:15.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'ba72435b-1185-4108-8020-7310c5a70233', N'01849cc9-c6da-4184-92f8-34875dac1d42', N'0', N'1', N'NF-details', N'查看数据表', NULL, N'2', N'details', N'/SystemManage/CodeGenerator/Details', N'0', N'0', N'0', N'0', N'2', N'0', N'1', NULL, NULL, NULL, N'2020-05-06 13:12:42.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'c8eed325-56ad-4210-b610-3e3bb68eb0be', N'c87cd44f-d064-4d3c-a43e-de01a7a8785e', N'0', N'1', N'NF-edit', N'修改', NULL, N'2', N'edit', N'/FlowManage/Flowinstance/Form', N'0', N'0', N'0', N'0', N'1', N'0', N'1', NULL, N'2020-07-14 09:21:41.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'cba403cb-6418-44b7-868d-19e04af673ce', N'd742c96e-b61c-4cea-afeb-81805789687b', N'0', N'1', N'NF-delete', N'删除分类', NULL, N'2', N'delete', N'/SystemManage/ItemsType/DeleteForm', N'0', N'0', N'0', N'0', N'4', N'0', N'1', NULL, NULL, NULL, N'2020-04-27 16:52:32.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'cc115cef-c2d1-4b97-adbc-ea885aea6190', N'e3188a69-de3a-40ef-a5ff-5eaf460f5d20', N'0', N'1', N'NF-log', N'日志', NULL, N'1', N'log', N'/SystemSecurity/OpenJobs/Details', NULL, N'0', N'0', N'0', N'6', N'0', N'1', N'', N'2020-12-02 13:14:32.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'cd65e50a-0bea-45a9-b82e-f2eacdbd209e', N'252229DB-35CA-47AE-BDAE-C9903ED5BA7B', N'0', N'1', N'NF-add', N'新建机构', NULL, N'1', N'add', N'/SystemManage/Organize/Form', N'0', N'0', N'0', N'0', N'1', N'0', N'1', NULL, NULL, NULL, N'2020-04-07 14:22:42.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'd1086ccf-e605-44a4-9777-629810cec02d', N'152a8e93-cebb-4574-ae74-2a86595ff986', N'0', N'1', N'NF-edit', N'修改字段', NULL, N'2', N'edit', N'/SystemManage/ModuleFields/Form', N'0', N'0', N'0', N'0', N'1', N'0', N'1', N'', N'2020-05-21 14:39:20.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-05-21 15:15:11.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'd26da420-7e73-41ef-8361-86551b8dd1bb', N'a303cbe1-60eb-437b-9a69-77ff8b48f173', N'0', N'1', N'NF-add', N'新增', NULL, N'1', N'add', N'/SystemOrganize/SystemSet/Form', N'0', N'0', N'0', N'0', N'0', N'0', N'1', N'', N'2020-06-12 13:54:25.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-06-16 08:12:37.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'd2ecb5e8-e5cc-49c8-ba86-dbd7e51ca20b', N'e3188a69-de3a-40ef-a5ff-5eaf460f5d20', N'0', N'1', N'NF-edit', N'修改任务', NULL, N'2', N'edit', N'/SystemSecurity/OpenJobs/Form', N'0', N'0', N'0', N'0', N'2', N'0', N'1', N'', N'2020-05-26 13:55:50.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-05-26 13:56:25.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'd30ff0f3-39da-4033-a320-56f26edd5b51', N'f82fd629-5f3a-45d6-8681-5ec47e66a807', N'0', N'1', N'NF-delete', N'删除', NULL, N'2', N'delete', N'/SystemManage/Flowscheme/DeleteForm', N'0', N'0', N'0', N'0', N'2', N'0', N'1', NULL, N'2020-07-10 08:50:52.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'd3a41d48-6288-49ec-90c5-952fa676591f', N'f3277ddd-1bf1-4202-8a4b-15c29a405bd5', N'0', N'1', N'NF-add', N'新建新闻', NULL, N'1', N'add', N'/ContentManage/ArticleNews/Form', N'0', N'0', N'0', N'0', N'1', N'0', N'1', N'', N'2020-06-23 15:29:43.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-06-23 16:00:40.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'd4074121-0d4f-465e-ad37-409bbe15bf8a', N'a3a4742d-ca39-42ec-b95a-8552a6fae579', N'0', N'1', N'NF-add', N'新建策略', NULL, N'1', N'add', N'/SystemSecurity/FilterIP/Form', N'0', N'0', N'0', N'0', N'1', N'0', N'1', NULL, NULL, NULL, N'2016-07-25 15:57:40.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'd42aaaae-4973-427c-ad86-7a6b20b09325', N'605444e5-704f-4cca-8d00-75175e2aef05', N'0', N'1', N'NF-vft', N'处理', NULL, N'1', N'vft', N'/FlowManage/Flowinstance/Verification', N'0', N'0', N'0', N'0', N'0', N'0', N'1', N'', N'2020-07-15 15:03:33.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-07-15 15:04:24.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'D4FCAFED-7640-449E-80B7-622DDACD5012', N'64A1C550-2C61-4A8C-833D-ACD0C012260F', N'0', N'1', N'NF-details', N'查看菜单', NULL, N'2', N'details', N'/SystemManage/Module/Details', N'0', N'0', N'0', N'0', N'4', N'0', N'1', NULL, NULL, NULL, N'2020-04-27 17:37:29.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'd6ed1d69-84f8-4933-9072-4009a3fcba85', N'4efd6f84-a4a9-4176-aedd-153e7748cbac', N'0', N'1', N'NF-edit', N'修改类别', NULL, N'2', N'edit', N'/ContentManage/ArticleCategory/Form', N'0', N'0', N'0', N'0', N'2', N'0', N'1', N'', N'2020-06-23 15:27:36.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-06-23 16:00:16.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'd7a452f3-3596-4339-8803-d61fb4eec013', N'F298F868-B689-4982-8C8B-9268CBF0308D', N'0', N'1', N'NF-export', N'导出', NULL, N'1', N'export', N'/SystemOrganize/Duty/Export', NULL, N'0', N'0', N'0', N'6', N'0', N'1', N'', N'2020-08-12 10:17:30.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-08-12 10:18:15.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'd9e74251-61ff-4472-adec-ad316cb9a307', N'd742c96e-b61c-4cea-afeb-81805789687b', N'0', N'1', N'NF-add', N'新建分类', NULL, N'1', N'add', N'/SystemManage/ItemsType/Form', N'0', N'0', N'0', N'0', N'1', N'0', N'1', NULL, NULL, NULL, N'2020-04-27 16:52:12.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'de205812-51c2-4a64-857d-b5638c06c65c', N'4efd6f84-a4a9-4176-aedd-153e7748cbac', N'0', N'1', N'NF-details', N'查看类别', NULL, N'2', N'details', N'/ContentManage/ArticleCategory/Details', N'0', N'0', N'0', N'0', N'4', N'0', N'1', N'', N'2020-06-23 15:27:36.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-06-23 16:00:24.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'e06965bc-b693-4b91-96f9-fc10ca2aa1f0', N'e3188a69-de3a-40ef-a5ff-5eaf460f5d20', N'0', N'1', N'NF-disabled', N'关闭', NULL, N'2', N'disabled', N'/SystemSecurity/OpenJobs/ChangeStatus', N'0', N'0', N'0', N'0', N'5', N'0', N'1', N'', N'2020-05-26 13:55:50.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-05-27 08:42:32.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'E29FCBA7-F848-4A8B-BC41-A3C668A9005D', N'64A1C550-2C61-4A8C-833D-ACD0C012260F', N'0', N'1', N'NF-edit', N'修改菜单', NULL, N'2', N'edit', N'/SystemManage/Module/Form', N'0', N'0', N'0', N'0', N'2', N'0', N'1', NULL, NULL, NULL, N'2016-07-25 15:41:02.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'e376d482-023e-4715-a9c8-2a393c24426e', N'605444e5-704f-4cca-8d00-75175e2aef05', N'0', N'1', N'NF-details', N'进度', NULL, N'2', N'details', N'/FlowManage/Flowinstance/Details', N'0', N'0', N'0', N'0', N'1', N'0', N'1', N'', N'2020-07-15 15:03:33.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-07-15 15:04:15.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'e6514544-1436-431d-acbc-c44802831ea8', N'01849cc9-c6da-4184-92f8-34875dac1d42', N'0', N'1', N'NF-entitycode', N'实体生成', NULL, N'2', N'entitycode', N'/SystemManage/CodeGenerator/EntityCode', NULL, N'0', N'0', N'0', N'1', N'0', N'1', N'', N'2020-07-23 15:36:23.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-07-23 15:36:42.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'e75e4efc-d461-4334-a764-56992fec38e6', N'F298F868-B689-4982-8C8B-9268CBF0308D', N'0', N'1', N'NF-edit', N'修改岗位', NULL, N'2', N'edit', N'/SystemOrganize/Duty/Form', N'0', N'0', N'0', N'0', N'2', N'0', N'1', N'', NULL, NULL, N'2020-06-16 08:13:55.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'ec452d72-4969-4880-b52f-316ffdfa19bd', N'f82fd629-5f3a-45d6-8681-5ec47e66a807', N'0', N'1', N'NF-add', N'新增', NULL, N'1', N'add', N'/SystemManage/Flowscheme/Form', N'0', N'0', N'0', N'0', N'0', N'0', N'1', NULL, N'2020-07-10 08:50:52.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'f51da6f6-8511-49f3-982b-a30ed0946706', N'26452c9a-243d-4c81-97b9-a3ad581c3bf4', N'0', N'1', N'NF-delete', N'删除机构', NULL, N'2', N'delete', N'/SystemOrganize/Organize/DeleteForm', N'0', N'0', N'0', N'0', N'3', N'0', N'1', N'', NULL, NULL, N'2020-07-23 10:47:09.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'f93763ff-51a1-478d-9585-3c86084c54f3', N'91A6CFAD-B2F9-4294-BDAE-76DECF412C6C', N'0', N'1', N'NF-details', N'查看角色', NULL, N'2', N'details', N'/SystemOrganize/Role/Details', N'0', N'0', N'0', N'0', N'4', N'0', N'1', N'', NULL, NULL, N'2020-06-16 08:13:42.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'fcb4d9f0-63f0-4bd0-9779-eed26da5c4b3', N'e9190a56-e173-4483-8a3e-f17b86e4766e', N'0', N'1', N'NF-add', N'新增', NULL, N'1', N'add', N'/InfoManage/Message/Form', N'0', N'0', N'0', N'0', N'0', N'0', N'1', NULL, N'2020-07-29 16:44:08.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'FD3D073C-4F88-467A-AE3B-CDD060952CE6', N'64A1C550-2C61-4A8C-833D-ACD0C012260F', N'0', N'1', N'NF-modulebutton', N'按钮管理', NULL, N'2', N'modulebutton', N'/SystemManage/ModuleButton/Index', N'0', N'0', N'0', N'0', N'5', N'0', N'1', NULL, NULL, NULL, N'2020-04-07 14:34:09.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'ffffe7f8-900c-413a-9970-bee7d6599cce', N'91A6CFAD-B2F9-4294-BDAE-76DECF412C6C', N'0', N'1', N'NF-edit', N'修改角色', NULL, N'2', N'edit', N'/SystemOrganize/Role/Form', N'0', N'0', N'0', N'0', N'2', N'0', N'1', N'', NULL, NULL, N'2020-06-16 08:13:35.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL)
GO


-- ----------------------------
-- Table structure for sys_modulefields
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[sys_modulefields]') AND type IN ('U'))
	DROP TABLE [dbo].[sys_modulefields]
GO

CREATE TABLE [dbo].[sys_modulefields] (
  [F_Id] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_ModuleId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_EnCode] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_FullName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_DeleteMark] tinyint  NULL,
  [F_EnabledMark] tinyint  NULL,
  [F_Description] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_CreatorTime] datetime2(7)  NULL,
  [F_CreatorUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_LastModifyTime] datetime2(7)  NULL,
  [F_LastModifyUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_DeleteTime] datetime2(7)  NULL,
  [F_DeleteUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_IsPublic] tinyint  NULL
)
GO

ALTER TABLE [dbo].[sys_modulefields] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of sys_modulefields
-- ----------------------------
INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'00a79cc3-a490-4772-909a-38567e3ea6da', N'484269cb-9aea-4af1-b7f6-f99e7e396ad1', N'F_ProjectName', N'项目名称', N'0', N'1', N'', N'2020-06-12 14:33:15.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-06-12 16:13:46.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'035d9296-1e17-42b7-9d8f-c9cc3b1d8e3f', N'7cb65e00-8af2-4cf2-b318-8ba28b3c154e', N'F_FileExtension', N'文件扩展名', N'0', N'1', NULL, N'2020-07-22 12:05:35.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'0917606f-f448-49d3-b78d-e08a17a1cc4f', N'2c2ddbce-ee87-4134-9b32-54d0bd572910', N'F_CreatorTime', N'创建时间', N'0', N'1', NULL, N'2020-07-08 14:34:38.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'0927895a-9d35-435c-b980-13f7102043c3', N'484269cb-9aea-4af1-b7f6-f99e7e396ad1', N'F_PrincipalMan', N'联系人', N'0', N'1', NULL, N'2020-06-12 14:33:15.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'0986da5b-16a3-4330-8449-0508699c93e3', N'f82fd629-5f3a-45d6-8681-5ec47e66a807', N'F_SchemeName', N'流程名称', N'0', N'1', NULL, N'2020-07-10 08:50:52.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'0d216246-f372-48fb-8c2f-dda9924a4625', N'2c2ddbce-ee87-4134-9b32-54d0bd572910', N'F_Content', N'表单原html模板未经处理的', N'0', N'1', NULL, N'2020-07-08 14:34:38.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'1406d021-de90-4246-af02-6950716214c1', N'a303cbe1-60eb-437b-9a69-77ff8b48f173', N'F_Description', N'备注', N'0', N'1', NULL, N'2020-06-12 13:54:25.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'1641a15d-87cf-4658-8d39-a1197fb26c43', N'c87cd44f-d064-4d3c-a43e-de01a7a8785e', N'F_ActivityName', N'当前节点名称', N'0', N'1', NULL, N'2020-07-14 09:21:41.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'186b9cc1-f4d2-43ad-9369-3f34c1dd7b90', N'c87cd44f-d064-4d3c-a43e-de01a7a8785e', N'F_Code', N'实例编号', N'0', N'1', NULL, N'2020-07-14 09:21:41.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'18d606fa-4baf-49e7-987d-8dde8561385a', N'484269cb-9aea-4af1-b7f6-f99e7e396ad1', N'F_LogoCode', N'Logo编号', N'0', N'1', NULL, N'2020-06-12 14:33:15.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'1ba8ebaf-b89c-4699-be3a-520b16efeeb4', N'f82fd629-5f3a-45d6-8681-5ec47e66a807', N'F_FrmId', N'表单ID', N'0', N'1', NULL, N'2020-07-10 08:50:52.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'1cecc967-7ea1-46d0-b4fa-f90a15783d1c', N'7e4e4a48-4d51-4159-a113-2a211186f13a', N'F_Title', N'标题', N'0', N'1', N'', N'2020-05-22 16:41:18.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-06-23 09:12:07.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, N'0')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'1ffb5d50-2dc3-41f0-b863-93c45afd7709', N'f82fd629-5f3a-45d6-8681-5ec47e66a807', N'F_LastModifyUserName', N'修改用户', N'0', N'1', NULL, N'2020-07-10 08:50:52.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'21a53e80-9887-4ca3-908f-a858c2def860', N'7cb65e00-8af2-4cf2-b318-8ba28b3c154e', N'F_FilePath', N'文件路径', N'0', N'1', NULL, N'2020-07-22 12:05:35.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'21d40431-d289-415f-bfaf-5a23bf4dac9c', N'484269cb-9aea-4af1-b7f6-f99e7e396ad1', N'F_AdminPassword', N'系统密码', N'0', N'1', N'', N'2020-06-12 14:33:15.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-06-15 14:23:27.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, N'0')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'22289940-a299-4d46-b68a-204bfab51b01', N'c87cd44f-d064-4d3c-a43e-de01a7a8785e', N'F_EnabledMark', N'有效', N'0', N'1', NULL, N'2020-07-14 09:21:41.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'25612b64-9499-46fd-9a3d-779362a3cba2', N'7cb65e00-8af2-4cf2-b318-8ba28b3c154e', N'F_CreatorUserName', N'创建用户', N'0', N'1', NULL, N'2020-07-22 12:05:35.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'263acbf3-44b2-4be5-82ce-8a038d43a5c5', N'7cb65e00-8af2-4cf2-b318-8ba28b3c154e', N'F_Description', N'备注', N'0', N'1', NULL, N'2020-07-22 12:05:35.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'309c622d-2217-499f-aa83-2eccd72205a1', N'484269cb-9aea-4af1-b7f6-f99e7e396ad1', N'F_CreatorTime', N'创建时间', N'0', N'1', N'', N'2020-06-12 14:33:15.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-06-12 14:35:08.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, N'0')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'33f55a8a-1daf-4adb-9931-1b6cace1c13a', N'2c2ddbce-ee87-4134-9b32-54d0bd572910', N'F_EnabledMark', N'是否启用', N'0', N'1', NULL, N'2020-07-08 14:34:38.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'344cf340-e664-446f-ba79-6d37e466f9d8', N'7cb65e00-8af2-4cf2-b318-8ba28b3c154e', N'F_FileSize', N'文件大小', N'0', N'1', NULL, N'2020-07-22 12:05:35.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'36df66b8-bcf1-43bf-92d5-ea915faa8b94', N'2c2ddbce-ee87-4134-9b32-54d0bd572910', N'F_Description', N'内容', N'0', N'1', NULL, N'2020-07-08 14:34:38.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'3961f233-46ef-4fd2-815e-733bb288946c', N'2c2ddbce-ee87-4134-9b32-54d0bd572910', N'F_ContentData', N'表单中的控件属性描述', N'0', N'1', NULL, N'2020-07-08 14:34:38.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'3b304c8d-a54d-47b7-ad21-e6d01c283904', N'91A6CFAD-B2F9-4294-BDAE-76DECF412C6C', N'F_CreatorUserName', N'创建人', N'0', N'1', N'', N'2020-06-03 09:57:59.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'0')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'3fee41bd-64a6-4280-ac93-0ce835fecf41', N'c87cd44f-d064-4d3c-a43e-de01a7a8785e', N'F_CreatorTime', N'创建时间', N'0', N'1', NULL, N'2020-07-14 09:21:41.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'45f62f54-8ad4-45f2-9f37-a7f0d15ee815', N'484269cb-9aea-4af1-b7f6-f99e7e396ad1', N'F_Description', N'备注', N'0', N'1', NULL, N'2020-06-12 14:33:15.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'47b8d043-aa5e-4a09-98b1-aaf24d6589dd', N'7cb65e00-8af2-4cf2-b318-8ba28b3c154e', N'F_FileBy', N'文件所属', N'0', N'1', NULL, N'2020-07-22 12:05:35.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'49d6a83e-646f-48af-b71e-8f8d60f73396', N'a303cbe1-60eb-437b-9a69-77ff8b48f173', N'F_DbString', N'连接字符串', N'0', N'1', N'', N'2020-06-12 13:54:25.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-06-12 13:57:26.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, N'0')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'4b2b3c5b-22f0-4a64-9857-c794f1d8a181', N'a303cbe1-60eb-437b-9a69-77ff8b48f173', N'F_LogoCode', N'Logo编号', N'0', N'1', NULL, N'2020-06-12 13:54:25.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'4e90e4dc-fc8d-456e-aa7d-2420e31212c2', N'e9190a56-e173-4483-8a3e-f17b86e4766e', N'F_ToUserId', N'收件人主键', N'0', N'1', NULL, N'2020-07-29 16:44:08.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'5a218598-40b4-4046-a61e-e7b4f8dd0d85', N'f82fd629-5f3a-45d6-8681-5ec47e66a807', N'F_SchemeContent', N'流程内容', N'0', N'1', NULL, N'2020-07-10 08:50:52.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'5b2cb54c-5fe8-4f8f-b281-d6de27dcfc18', N'7cb65e00-8af2-4cf2-b318-8ba28b3c154e', N'F_FileType', N'文件类型', N'0', N'1', NULL, N'2020-07-22 12:05:35.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'642e8c4b-7762-42b6-9fbd-8495c54606a2', N'a303cbe1-60eb-437b-9a69-77ff8b48f173', N'F_Logo', N'Logo图标', N'0', N'1', NULL, N'2020-06-12 13:54:25.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'6d1a0016-9634-4425-b840-af55f4fb383f', N'a303cbe1-60eb-437b-9a69-77ff8b48f173', N'F_DBProvider', N'数据库类型', N'0', N'1', N'', N'2020-06-12 13:54:25.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-06-12 13:57:21.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, N'0')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'6e177e5f-4ce8-4f7b-b790-b320bb2659db', N'a303cbe1-60eb-437b-9a69-77ff8b48f173', N'F_EnabledMark', N'有效', N'0', N'1', NULL, N'2020-06-12 13:54:25.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'6ec6ed61-884c-4519-904c-2f3cb717aef7', N'a303cbe1-60eb-437b-9a69-77ff8b48f173', N'F_PrincipalMan', N'联系人', N'0', N'1', NULL, N'2020-06-12 13:54:25.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'702f2c2e-b66e-44e8-a846-fd96c38027e3', N'c87cd44f-d064-4d3c-a43e-de01a7a8785e', N'F_CustomName', N'实例名称', N'0', N'1', NULL, N'2020-07-14 09:21:41.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'7496086e-32ec-4875-8013-73ce1c2784a2', N'e9190a56-e173-4483-8a3e-f17b86e4766e', N'F_CreatorUserId', N'创建用户主键', N'0', N'1', NULL, N'2020-07-29 16:44:08.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'756ce041-ad4f-4895-b184-d9c9c4df9a04', N'38CA5A66-C993-4410-AF95-50489B22939C', N'F_OrganizeId', N'部门Id', N'0', N'1', N'', N'2020-06-08 16:25:17.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'0')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'76cbcdd9-ffeb-41a1-8f9c-51dea4a02fa2', N'a303cbe1-60eb-437b-9a69-77ff8b48f173', N'F_EndTime', N'到期时间', N'0', N'1', NULL, N'2020-06-12 13:54:25.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'76e64bb6-cb36-45c4-852f-6a044d5b2c3d', N'f82fd629-5f3a-45d6-8681-5ec47e66a807', N'F_SchemeType', N'流程分类', N'0', N'1', NULL, N'2020-07-10 08:50:52.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'770af4b6-29ef-47b1-aea8-6092562d9800', N'2c2ddbce-ee87-4134-9b32-54d0bd572910', N'F_ContentParse', N'表单控件位置模板', N'0', N'1', NULL, N'2020-07-08 14:34:38.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'78a9a6c0-e854-4225-b75e-5e7cfaf46c67', N'a303cbe1-60eb-437b-9a69-77ff8b48f173', N'F_ProjectName', N'项目名称', N'0', N'1', N'', N'2020-06-12 13:54:25.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-06-12 13:56:54.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, N'0')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'7dfa39c1-a8d3-4460-922b-5a770d6e307f', N'f82fd629-5f3a-45d6-8681-5ec47e66a807', N'F_SchemeCode', N'流程编号', N'0', N'1', NULL, N'2020-07-10 08:50:52.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'8024dfbc-8236-4a86-8869-d09e59c3dfe3', N'91A6CFAD-B2F9-4294-BDAE-76DECF412C6C', N'F_CreatorTime', N'创建时间', N'0', N'1', N'', N'2020-06-03 09:57:59.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-05-22 17:06:41.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, N'0')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'80899139-2938-4e0a-9f80-16bf70d00658', N'7cb65e00-8af2-4cf2-b318-8ba28b3c154e', N'F_CreatorTime', N'创建时间', N'0', N'1', NULL, N'2020-07-22 12:05:35.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'81d404d1-5639-4d5a-8ac1-d47b0414c321', N'e9190a56-e173-4483-8a3e-f17b86e4766e', N'F_MessageType', N'信息类型（通知、私信、处理）', N'0', N'1', NULL, N'2020-07-29 16:44:08.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'81d74921-21be-4360-bae3-653d0fade184', N'c87cd44f-d064-4d3c-a43e-de01a7a8785e', N'F_CreatorUserName', N'创建用户', N'0', N'1', NULL, N'2020-07-14 09:21:41.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'823b9649-030c-4dbb-b790-b184565f4746', N'c87cd44f-d064-4d3c-a43e-de01a7a8785e', N'F_MakerList', N'执行人', N'0', N'1', NULL, N'2020-07-14 09:21:41.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'82f21e4c-0d14-4559-92d4-657b34640a47', N'f82fd629-5f3a-45d6-8681-5ec47e66a807', N'F_SortCode', N'排序码', N'0', N'1', NULL, N'2020-07-10 08:50:52.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'83584b47-0a29-446d-8ff2-6c6d3eccca3d', N'c87cd44f-d064-4d3c-a43e-de01a7a8785e', N'F_FrmId', N'表单ID', N'0', N'1', NULL, N'2020-07-14 09:21:41.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'8376537c-b23b-4b51-a6f0-75fc3467c574', N'2c2ddbce-ee87-4134-9b32-54d0bd572910', N'F_Fields', N'字段个数', N'0', N'1', NULL, N'2020-07-08 14:34:38.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'84b3ac62-5d85-4263-946d-e12be86cbfa1', N'7cb65e00-8af2-4cf2-b318-8ba28b3c154e', N'F_FileName', N'文件名称', N'0', N'1', NULL, N'2020-07-22 12:05:35.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'889fc780-cd2f-45c9-b07c-030e6d3ddc29', N'e9190a56-e173-4483-8a3e-f17b86e4766e', N'F_CreatorUserName', N'创建用户', N'0', N'1', NULL, N'2020-07-29 16:44:08.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'8ba0c532-4b85-4a02-aec8-499d93b97dcb', N'a303cbe1-60eb-437b-9a69-77ff8b48f173', N'F_CreatorTime', N'创建时间', N'0', N'1', NULL, N'2020-06-12 13:54:25.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'8cf11fd0-8ee5-408d-9d5d-15c4342befda', N'f82fd629-5f3a-45d6-8681-5ec47e66a807', N'F_OrganizeId', N'所属部门', N'0', N'1', NULL, N'2020-07-10 08:50:52.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'8f5ce993-986c-4825-b3bc-f34f54d4f37f', N'c87cd44f-d064-4d3c-a43e-de01a7a8785e', N'F_FlowLevel', N'等级', N'0', N'1', NULL, N'2020-07-14 09:21:41.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'90386357-54f8-4aeb-8b24-f45ee8c08ba4', N'c87cd44f-d064-4d3c-a43e-de01a7a8785e', N'F_OrganizeId', N'所属部门', N'0', N'1', NULL, N'2020-07-14 09:21:41.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'91b3ca56-61e8-444d-b506-7dec452f1daa', N'c87cd44f-d064-4d3c-a43e-de01a7a8785e', N'F_SchemeContent', N'流程模板内容', N'0', N'1', NULL, N'2020-07-14 09:21:41.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'936023df-503f-4322-b243-47158c9617a6', N'484269cb-9aea-4af1-b7f6-f99e7e396ad1', N'F_EndTime', N'到期时间', N'0', N'1', NULL, N'2020-06-16 09:38:15.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'9507a93a-a258-4ba1-93db-d51798268c5e', N'a303cbe1-60eb-437b-9a69-77ff8b48f173', N'F_CreatorUserId', N'创建人Id', N'0', N'1', N'', N'2020-06-12 13:54:25.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-06-12 13:57:41.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, N'0')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'98e7930d-37f0-4499-874d-b89207657eaa', N'7cb65e00-8af2-4cf2-b318-8ba28b3c154e', N'F_OrganizeId', N'所属部门', N'0', N'1', NULL, N'2020-07-22 12:05:35.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'99ecc5e7-2b02-49d8-b091-ee1aec8130ee', N'f82fd629-5f3a-45d6-8681-5ec47e66a807', N'F_Description', N'备注', N'0', N'1', NULL, N'2020-07-10 08:50:52.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'a24c6ed3-8c91-4ade-a5c1-8c5eb9719368', N'484269cb-9aea-4af1-b7f6-f99e7e396ad1', N'F_AdminAccount', N'系统账户', N'0', N'1', N'', N'2020-06-12 14:33:15.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-06-15 14:23:30.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, N'0')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'a619715a-46b9-4b3e-81d2-a450038dceb6', N'c87cd44f-d064-4d3c-a43e-de01a7a8785e', N'F_Description', N'实例备注', N'0', N'1', NULL, N'2020-07-14 09:21:41.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'a6320b89-1c15-4afa-9c30-2e1f508212e2', N'484269cb-9aea-4af1-b7f6-f99e7e396ad1', N'F_HostUrl', N'域名', N'0', N'1', N'', N'2020-06-15 17:01:14.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-06-15 17:01:20.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'a983cc2e-d045-4c35-a53e-2f0775edf639', N'f82fd629-5f3a-45d6-8681-5ec47e66a807', N'F_CreatorUserName', N'创建用户', N'0', N'1', NULL, N'2020-07-10 08:50:52.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'aa75975a-bf00-429b-8c58-825b43d29eb4', N'2c2ddbce-ee87-4134-9b32-54d0bd572910', N'F_SortCode', N'排序码', N'0', N'1', NULL, N'2020-07-08 14:34:38.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'b06b2f6f-c392-473f-bea7-96bcf04a025d', N'c87cd44f-d064-4d3c-a43e-de01a7a8785e', N'F_IsFinish', N'是否完成', N'0', N'1', NULL, N'2020-07-14 09:21:41.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'b2e8a59b-99ce-432b-b5ed-e7c8859dcfad', N'c87cd44f-d064-4d3c-a43e-de01a7a8785e', N'F_SchemeType', N'流程类型', N'0', N'1', NULL, N'2020-07-14 09:21:41.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'b3e6bab9-7e4c-4f87-83ff-d0f1bf6f9df8', N'c87cd44f-d064-4d3c-a43e-de01a7a8785e', N'F_FrmType', N'表单类型', N'0', N'1', NULL, N'2020-07-14 09:21:41.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'b56a98de-4f9d-4753-ae06-e3bea339dc9f', N'2c2ddbce-ee87-4134-9b32-54d0bd572910', N'F_WebId', N'系统页面标识，当表单类型为用Web自定义的表单时，需要标识加载哪个页面', N'0', N'1', NULL, N'2020-07-08 14:34:38.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'b67b5eb6-ecae-4156-8ef8-9e80b7a1345a', N'7e4e4a48-4d51-4159-a113-2a211186f13a', N'F_CreatorUserName', N'创建人', N'0', N'1', N'', N'2020-05-22 16:53:20.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'0')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'b68b00b4-6f56-4832-8774-eab1d02e2fc1', N'a303cbe1-60eb-437b-9a69-77ff8b48f173', N'F_AdminPassword', N'系统密码', N'0', N'1', NULL, N'2020-06-12 13:54:25.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'b765ca2a-4337-4e24-b330-9c923ca793f0', N'c87cd44f-d064-4d3c-a43e-de01a7a8785e', N'F_CreatorUserId', N'创建用户主键', N'0', N'1', NULL, N'2020-07-14 09:21:41.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'b8e360f7-817f-4dc7-82c4-11fd51fc77de', N'c87cd44f-d064-4d3c-a43e-de01a7a8785e', N'F_FrmContentParse', N'表单控件位置模板', N'0', N'1', NULL, N'2020-07-14 09:21:41.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'bd8b0f82-43fd-44ed-9814-de1876fced8c', N'f82fd629-5f3a-45d6-8681-5ec47e66a807', N'F_SchemeCanUser', N'流程模板使用者', N'0', N'1', NULL, N'2020-07-10 08:50:52.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'be804654-d6d7-44d1-8950-6841a2626720', N'484269cb-9aea-4af1-b7f6-f99e7e396ad1', N'F_CreatorUserId', N'创建人Id', N'0', N'1', N'', N'2020-06-12 14:33:15.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-06-12 13:57:41.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, N'0')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'bf5a2919-281c-44e2-a83b-74576e08743e', N'7e4e4a48-4d51-4159-a113-2a211186f13a', N'F_EnabledMark', N'状态', N'0', N'1', N'', N'2020-05-22 16:53:00.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-06-08 16:49:52.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, N'0')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'bfbe0195-3fae-42d2-9d46-6bf5400d64ea', N'484269cb-9aea-4af1-b7f6-f99e7e396ad1', N'F_DbString', N'连接字符串', N'0', N'1', N'', N'2020-06-12 14:33:15.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-06-12 13:57:26.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, N'0')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'c077b982-c595-43e8-9095-711bee01e830', N'f82fd629-5f3a-45d6-8681-5ec47e66a807', N'F_AuthorizeType', N'模板权限类型：0完全公开,1指定部门/人员', N'0', N'1', NULL, N'2020-07-10 08:50:52.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'c0a08cd8-43bc-4d57-844a-2a39c4a408e6', N'484269cb-9aea-4af1-b7f6-f99e7e396ad1', N'F_CompanyName', N'公司名称', N'0', N'1', NULL, N'2020-06-12 14:33:15.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'c4a2499d-780c-42db-a2a4-a3c1084533ca', N'e9190a56-e173-4483-8a3e-f17b86e4766e', N'F_MessageInfo', N'内容', N'0', N'1', NULL, N'2020-07-29 16:44:08.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'c4c5840c-90a5-4a4e-aea4-9f284ece3921', N'a303cbe1-60eb-437b-9a69-77ff8b48f173', N'F_AdminAccount', N'系统账户', N'0', N'1', N'', N'2020-06-12 13:54:25.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-06-12 13:56:32.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'c87b90e3-6949-47f3-b8c5-c4f69af92200', N'a303cbe1-60eb-437b-9a69-77ff8b48f173', N'F_CompanyName', N'公司名称', N'0', N'1', NULL, N'2020-06-12 13:54:25.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'cba04ab7-b1b2-406e-a889-53484469cfe7', N'7e4e4a48-4d51-4159-a113-2a211186f13a', N'F_CreatorUserId', N'创建人Id', N'0', N'0', N'', N'2020-06-03 16:42:33.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-06-23 09:05:44.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, N'0')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'cddfb494-6d34-408d-8364-1c0bf270d4cd', N'2c2ddbce-ee87-4134-9b32-54d0bd572910', N'F_FrmType', N'表单类型，0：默认动态表单；1：Web自定义表单', N'0', N'1', NULL, N'2020-07-08 14:34:38.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'd4b49a55-491e-494c-b2d2-082a414bcbb9', N'484269cb-9aea-4af1-b7f6-f99e7e396ad1', N'F_Logo', N'Logo图标', N'0', N'1', NULL, N'2020-06-12 14:33:15.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'd52c6878-9283-45d7-82f9-b465fa33a89b', N'484269cb-9aea-4af1-b7f6-f99e7e396ad1', N'F_DBProvider', N'数据库类型', N'0', N'1', N'', N'2020-06-12 14:33:15.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-06-12 13:57:21.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, N'0')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'd53cf640-037a-4126-9b75-daa77fa712b3', N'c87cd44f-d064-4d3c-a43e-de01a7a8785e', N'F_FrmContentData', N'表单中的控件属性描述', N'0', N'1', NULL, N'2020-07-14 09:21:41.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'd782d010-89af-4c1d-8e96-35833c38c3d8', N'f82fd629-5f3a-45d6-8681-5ec47e66a807', N'F_SchemeVersion', N'流程内容版本', N'0', N'1', NULL, N'2020-07-10 08:50:52.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'd88a3d04-4a0d-4bfe-b34f-4130eb0accc9', N'2c2ddbce-ee87-4134-9b32-54d0bd572910', N'F_Name', N'表单名称', N'0', N'1', NULL, N'2020-07-08 14:34:38.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'd9e2a9fe-8a87-4266-aaae-f8e47b63187b', N'c87cd44f-d064-4d3c-a43e-de01a7a8785e', N'F_FrmData', N'表单数据', N'0', N'1', NULL, N'2020-07-14 09:21:41.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'dc4cd5fd-8933-44f8-9500-fc36285f50b2', N'7e4e4a48-4d51-4159-a113-2a211186f13a', N'F_CreatorTime', N'创建时间', N'0', N'1', N'', N'2020-05-22 16:53:46.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-05-22 17:06:41.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, N'0')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'ddd93ca7-d821-4abd-a342-9be1782dabe9', N'7e4e4a48-4d51-4159-a113-2a211186f13a', N'F_Content', N'内容', N'0', N'1', N'', N'2020-05-22 16:42:41.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2020-05-22 16:53:30.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, N'0')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'e175859e-9284-47fd-a168-d1a12ddd125d', N'a303cbe1-60eb-437b-9a69-77ff8b48f173', N'F_MobilePhone', N'联系方式', N'0', N'1', NULL, N'2020-06-12 13:54:25.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'e7a49b29-0c59-4665-9e73-5f495fced4d4', N'f82fd629-5f3a-45d6-8681-5ec47e66a807', N'F_CreatorTime', N'创建时间', N'0', N'1', NULL, N'2020-07-10 08:50:52.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'eb6b22e7-a804-4f6d-b969-d5c6db5f3043', N'2c2ddbce-ee87-4134-9b32-54d0bd572910', N'F_CreatorUserId', N'创建人', N'0', N'1', NULL, N'2020-07-08 14:34:38.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'ec413c38-4472-4e36-b406-84f883d48609', N'e9190a56-e173-4483-8a3e-f17b86e4766e', N'F_CreatorTime', N'创建时间', N'0', N'1', NULL, N'2020-07-29 16:44:08.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'ed938b87-a291-40cd-8a23-204e15f81cb3', N'f82fd629-5f3a-45d6-8681-5ec47e66a807', N'F_EnabledMark', N'有效', N'0', N'1', NULL, N'2020-07-10 08:50:52.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'edeabb1a-de3c-48d0-b677-5d35807632dc', N'7cb65e00-8af2-4cf2-b318-8ba28b3c154e', N'F_EnabledMark', N'有效', N'0', N'1', NULL, N'2020-07-22 12:05:35.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'edf1d2cb-07dd-41cb-a475-41b982c43dff', N'484269cb-9aea-4af1-b7f6-f99e7e396ad1', N'F_MobilePhone', N'联系方式', N'0', N'1', NULL, N'2020-06-12 14:33:15.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'f0e838e8-c07c-4f24-9dd3-0c1727074441', N'484269cb-9aea-4af1-b7f6-f99e7e396ad1', N'F_EnabledMark', N'有效', N'0', N'1', NULL, N'2020-06-16 09:38:15.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'f2c75a6b-ad06-49b2-93cf-7a7312c97ff5', N'e9190a56-e173-4483-8a3e-f17b86e4766e', N'F_EnabledMark', N'有效', N'0', N'1', NULL, N'2020-07-29 16:44:08.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'f8a85900-44e6-4786-ad64-e219eb8cffbe', N'f82fd629-5f3a-45d6-8681-5ec47e66a807', N'F_FrmType', N'表单类型', N'0', N'1', NULL, N'2020-07-10 08:50:52.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'fc7f572d-6dc2-4592-8d67-4b3155b49dd9', N'e9190a56-e173-4483-8a3e-f17b86e4766e', N'F_ToUserName', N'收件人', N'0', N'1', NULL, N'2020-07-29 16:44:08.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'ff60fd1e-d0df-4847-bc5a-1bf4c3310c9c', N'f82fd629-5f3a-45d6-8681-5ec47e66a807', N'F_CreatorUserId', N'创建用户主键', N'0', N'1', NULL, N'2020-07-10 08:50:52.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'1')
GO


-- ----------------------------
-- Table structure for sys_notice
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[sys_notice]') AND type IN ('U'))
	DROP TABLE [dbo].[sys_notice]
GO

CREATE TABLE [dbo].[sys_notice] (
  [F_Id] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_Title] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_Content] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_DeleteMark] tinyint  NULL,
  [F_EnabledMark] tinyint  NULL,
  [F_Description] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_CreatorTime] datetime2(7)  NULL,
  [F_CreatorUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_CreatorUserName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_LastModifyTime] datetime2(7)  NULL,
  [F_LastModifyUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_DeleteTime] datetime2(7)  NULL,
  [F_DeleteUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[sys_notice] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of sys_notice
-- ----------------------------

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
  [F_Description] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_CreatorTime] datetime2(7)  NULL,
  [F_CreatorUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_LastModifyTime] datetime2(7)  NULL,
  [F_LastModifyUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_DeleteTime] datetime2(7)  NULL,
  [F_DeleteUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_LastRunTime] datetime2(7)  NULL
)
GO

ALTER TABLE [dbo].[sys_openjob] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'最后一次执行时间',
'SCHEMA', N'dbo',
'TABLE', N'sys_openjob',
'COLUMN', N'F_LastRunTime'
GO


-- ----------------------------
-- Records of sys_openjob
-- ----------------------------

-- ----------------------------
-- Table structure for sys_openjoblog
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[sys_openjoblog]') AND type IN ('U'))
	DROP TABLE [dbo].[sys_openjoblog]
GO

CREATE TABLE [dbo].[sys_openjoblog] (
  [F_Id] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_JobId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_Description] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_CreatorTime] datetime2(7)  NULL,
  [F_EnabledMark] tinyint  NOT NULL,
  [F_JobName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL
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
-- Table structure for sys_organize
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[sys_organize]') AND type IN ('U'))
	DROP TABLE [dbo].[sys_organize]
GO

CREATE TABLE [dbo].[sys_organize] (
  [F_Id] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_ParentId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_Layers] int  NULL,
  [F_EnCode] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_FullName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_ShortName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_CategoryId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_ManagerId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_TelePhone] nvarchar(20) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_MobilePhone] nvarchar(20) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_WeChat] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_Fax] nvarchar(20) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_Email] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_AreaId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_Address] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_AllowEdit] tinyint  NULL,
  [F_AllowDelete] tinyint  NULL,
  [F_SortCode] int  NULL,
  [F_DeleteMark] tinyint  NULL,
  [F_EnabledMark] tinyint  NULL,
  [F_Description] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_CreatorTime] datetime2(7)  NULL,
  [F_CreatorUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_LastModifyTime] datetime2(7)  NULL,
  [F_LastModifyUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_DeleteTime] datetime2(7)  NULL,
  [F_DeleteUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[sys_organize] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of sys_organize
-- ----------------------------

-- ----------------------------
-- Table structure for sys_quickmodule
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[sys_quickmodule]') AND type IN ('U'))
	DROP TABLE [dbo].[sys_quickmodule]
GO

CREATE TABLE [dbo].[sys_quickmodule] (
  [F_Id] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_ModuleId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_DeleteMark] tinyint  NULL,
  [F_EnabledMark] tinyint  NULL,
  [F_Description] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_CreatorTime] datetime2(7)  NULL,
  [F_CreatorUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_LastModifyTime] datetime2(7)  NULL,
  [F_LastModifyUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_DeleteTime] datetime2(7)  NULL,
  [F_DeleteUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[sys_quickmodule] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of sys_quickmodule
-- ----------------------------
INSERT INTO [dbo].[sys_quickmodule] ([F_Id], [F_ModuleId], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'39fc7684-9fd9-0661-c478-e9686cc95d9f', N'01849cc9-c6da-4184-92f8-34875dac1d42', N'0', N'1', NULL, N'2021-05-13 14:09:05.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_quickmodule] ([F_Id], [F_ModuleId], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'39fc7684-9fde-8b6d-084c-e89072b61f4f', N'06bb3ea8-ec7f-4556-a427-8ff0ce62e873', N'0', N'1', NULL, N'2021-05-13 14:09:05.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_quickmodule] ([F_Id], [F_ModuleId], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'39fc7684-9fe2-c27c-e5ce-528998bf3e5c', N'1dff096a-db2f-410c-af2f-12294bdbeccd', N'0', N'1', NULL, N'2021-05-13 14:09:05.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_quickmodule] ([F_Id], [F_ModuleId], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'39fc7684-9fe4-b911-bb60-ab3d6465489e', N'1e60fce5-3164-439d-8d29-4950b33011e2', N'0', N'1', NULL, N'2021-05-13 14:09:05.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_quickmodule] ([F_Id], [F_ModuleId], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'39fc7684-9feb-06f6-8844-3e2e064b7418', N'2536fbf0-53ff-40a6-a093-73aa0a8fc035', N'0', N'1', NULL, N'2021-05-13 14:09:05.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_quickmodule] ([F_Id], [F_ModuleId], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'39fc7684-9fee-bb8b-c637-bd7c167e78d5', N'262ca754-1c73-436c-a9a2-b6374451a845', N'0', N'1', NULL, N'2021-05-13 14:09:05.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_quickmodule] ([F_Id], [F_ModuleId], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'39fc7684-9ff0-7c5f-c638-0204567240ac', N'26452c9a-243d-4c81-97b9-a3ad581c3bf4', N'0', N'1', NULL, N'2021-05-13 14:09:05.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_quickmodule] ([F_Id], [F_ModuleId], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'39fc7684-9ff4-7365-6fc0-557e07fc4130', N'2c2ddbce-ee87-4134-9b32-54d0bd572910', N'0', N'1', NULL, N'2021-05-13 14:09:05.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL)
GO


-- ----------------------------
-- Table structure for sys_role
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[sys_role]') AND type IN ('U'))
	DROP TABLE [dbo].[sys_role]
GO

CREATE TABLE [dbo].[sys_role] (
  [F_Id] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_OrganizeId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_Category] int  NULL,
  [F_EnCode] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_FullName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_Type] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_AllowEdit] tinyint  NULL,
  [F_AllowDelete] tinyint  NULL,
  [F_SortCode] int  NULL,
  [F_DeleteMark] tinyint  NULL,
  [F_EnabledMark] tinyint  NULL,
  [F_Description] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_CreatorTime] datetime2(7)  NULL,
  [F_CreatorUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_LastModifyTime] datetime2(7)  NULL,
  [F_LastModifyUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_DeleteTime] datetime2(7)  NULL,
  [F_DeleteUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[sys_role] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of sys_role
-- ----------------------------

-- ----------------------------
-- Table structure for sys_roleauthorize
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[sys_roleauthorize]') AND type IN ('U'))
	DROP TABLE [dbo].[sys_roleauthorize]
GO

CREATE TABLE [dbo].[sys_roleauthorize] (
  [F_Id] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_ItemType] int  NULL,
  [F_ItemId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_ObjectType] int  NULL,
  [F_ObjectId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_SortCode] int  NULL,
  [F_CreatorTime] datetime2(7)  NULL,
  [F_CreatorUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[sys_roleauthorize] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of sys_roleauthorize
-- ----------------------------

-- ----------------------------
-- Table structure for sys_serverstate
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[sys_serverstate]') AND type IN ('U'))
	DROP TABLE [dbo].[sys_serverstate]
GO

CREATE TABLE [dbo].[sys_serverstate] (
  [F_Id] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_WebSite] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_ARM] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_CPU] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_IIS] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_Date] date  NULL,
  [F_Cout] int  NULL
)
GO

ALTER TABLE [dbo].[sys_serverstate] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of sys_serverstate
-- ----------------------------

-- ----------------------------
-- Table structure for sys_systemset
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[sys_systemset]') AND type IN ('U'))
	DROP TABLE [dbo].[sys_systemset]
GO

CREATE TABLE [dbo].[sys_systemset] (
  [F_Id] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_Logo] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_LogoCode] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_ProjectName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_CompanyName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_AdminAccount] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_AdminPassword] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_DeleteMark] tinyint  NULL,
  [F_EnabledMark] tinyint  NULL,
  [F_Description] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_CreatorTime] datetime2(7)  NULL,
  [F_CreatorUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_LastModifyTime] datetime2(7)  NULL,
  [F_LastModifyUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_DeleteTime] datetime2(7)  NULL,
  [F_DeleteUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_MobilePhone] nvarchar(20) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_PrincipalMan] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_EndTime] datetime2(7)  NULL,
  [F_DbString] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_DBProvider] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_HostUrl] nvarchar(255) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_DbNumber] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL
)
GO

ALTER TABLE [dbo].[sys_systemset] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'LOGO图标',
'SCHEMA', N'dbo',
'TABLE', N'sys_systemset',
'COLUMN', N'F_Logo'
GO

EXEC sp_addextendedproperty
'MS_Description', N'LOGO编号',
'SCHEMA', N'dbo',
'TABLE', N'sys_systemset',
'COLUMN', N'F_LogoCode'
GO

EXEC sp_addextendedproperty
'MS_Description', N'项目名称',
'SCHEMA', N'dbo',
'TABLE', N'sys_systemset',
'COLUMN', N'F_ProjectName'
GO

EXEC sp_addextendedproperty
'MS_Description', N'公司名称',
'SCHEMA', N'dbo',
'TABLE', N'sys_systemset',
'COLUMN', N'F_CompanyName'
GO

EXEC sp_addextendedproperty
'MS_Description', N'管理员账户',
'SCHEMA', N'dbo',
'TABLE', N'sys_systemset',
'COLUMN', N'F_AdminAccount'
GO

EXEC sp_addextendedproperty
'MS_Description', N'管理员密码',
'SCHEMA', N'dbo',
'TABLE', N'sys_systemset',
'COLUMN', N'F_AdminPassword'
GO

EXEC sp_addextendedproperty
'MS_Description', N'有效',
'SCHEMA', N'dbo',
'TABLE', N'sys_systemset',
'COLUMN', N'F_EnabledMark'
GO

EXEC sp_addextendedproperty
'MS_Description', N'描述',
'SCHEMA', N'dbo',
'TABLE', N'sys_systemset',
'COLUMN', N'F_Description'
GO

EXEC sp_addextendedproperty
'MS_Description', N'联系电话',
'SCHEMA', N'dbo',
'TABLE', N'sys_systemset',
'COLUMN', N'F_MobilePhone'
GO

EXEC sp_addextendedproperty
'MS_Description', N'联系人',
'SCHEMA', N'dbo',
'TABLE', N'sys_systemset',
'COLUMN', N'F_PrincipalMan'
GO

EXEC sp_addextendedproperty
'MS_Description', N'到期时间',
'SCHEMA', N'dbo',
'TABLE', N'sys_systemset',
'COLUMN', N'F_EndTime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'连接字符串',
'SCHEMA', N'dbo',
'TABLE', N'sys_systemset',
'COLUMN', N'F_DbString'
GO

EXEC sp_addextendedproperty
'MS_Description', N'数据库类型',
'SCHEMA', N'dbo',
'TABLE', N'sys_systemset',
'COLUMN', N'F_DBProvider'
GO

EXEC sp_addextendedproperty
'MS_Description', N'域名地址',
'SCHEMA', N'dbo',
'TABLE', N'sys_systemset',
'COLUMN', N'F_HostUrl'
GO

EXEC sp_addextendedproperty
'MS_Description', N'数据库序号',
'SCHEMA', N'dbo',
'TABLE', N'sys_systemset',
'COLUMN', N'F_DbNumber'
GO


-- ----------------------------
-- Records of sys_systemset
-- ----------------------------
INSERT INTO [dbo].[sys_systemset] ([F_Id], [F_Logo], [F_LogoCode], [F_ProjectName], [F_CompanyName], [F_AdminAccount], [F_AdminPassword], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_MobilePhone], [F_PrincipalMan], [F_EndTime], [F_DbString], [F_DBProvider], [F_HostUrl], [F_DbNumber]) VALUES (N'd69fd66a-6a77-4011-8a25-53a79bdf5001', N'/icon/favicon.ico', N'WaterCloud', N'水之云信息系统', N'水之云', N'admin', N'0000', N'0', N'1', N'', N'2020-06-12 16:30:00.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'2021-05-13 14:11:28.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, N'13621551864', N'MonsterUncle', N'2032-06-26 00:00:00.0000000', N'data source=localhost;database=watercloudnetdb;uid=root;pwd=root;', N'MySql', N'localhost', N'0')
GO


-- ----------------------------
-- Table structure for sys_user
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[sys_user]') AND type IN ('U'))
	DROP TABLE [dbo].[sys_user]
GO

CREATE TABLE [dbo].[sys_user] (
  [F_Id] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_Account] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_RealName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_NickName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_HeadIcon] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_Gender] tinyint  NULL,
  [F_Birthday] datetime2(7)  NULL,
  [F_MobilePhone] nvarchar(20) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_Email] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_WeChat] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_ManagerId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_SecurityLevel] int  NULL,
  [F_Signature] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_OrganizeId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_DepartmentId] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_RoleId] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_DutyId] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_IsAdmin] tinyint  NULL,
  [F_IsBoss] tinyint  NULL,
  [F_IsLeaderInDepts] tinyint  NULL,
  [F_IsSenior] tinyint  NULL,
  [F_SortCode] int  NULL,
  [F_DeleteMark] tinyint  NULL,
  [F_EnabledMark] tinyint  NULL,
  [F_Description] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_CreatorTime] datetime2(7)  NULL,
  [F_CreatorUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_LastModifyTime] datetime2(7)  NULL,
  [F_LastModifyUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_DeleteTime] datetime2(7)  NULL,
  [F_DeleteUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_DingTalkUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_DingTalkUserName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_DingTalkAvatar] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_WxOpenId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_WxNickName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_HeadImgUrl] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[sys_user] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of sys_user
-- ----------------------------
INSERT INTO [dbo].[sys_user] ([F_Id], [F_Account], [F_RealName], [F_NickName], [F_HeadIcon], [F_Gender], [F_Birthday], [F_MobilePhone], [F_Email], [F_WeChat], [F_ManagerId], [F_SecurityLevel], [F_Signature], [F_OrganizeId], [F_DepartmentId], [F_RoleId], [F_DutyId], [F_IsAdmin], [F_IsBoss], [F_IsLeaderInDepts], [F_IsSenior], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_DingTalkUserId], [F_DingTalkUserName], [F_DingTalkAvatar], [F_WxOpenId], [F_WxNickName], [F_HeadImgUrl]) VALUES (N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'admin', N'超级管理员', N'超级管理员', NULL, N'1', N'2020-03-27 00:00:00.0000000', N'13600000000', N'3333', NULL, NULL, NULL, NULL, N'd69fd66a-6a77-4011-8a25-53a79bdf5001', N'5AB270C0-5D33-4203-A54F-4552699FDA3C', NULL, NULL, N'1', N'0', N'0', N'0', NULL, N'0', N'1', N'系统内置账户', N'2016-07-20 00:00:00.0000000', NULL, NULL, NULL, NULL, NULL, NULL, N'闫志辉', NULL, NULL, NULL, NULL)
GO


-- ----------------------------
-- Table structure for sys_userlogon
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[sys_userlogon]') AND type IN ('U'))
	DROP TABLE [dbo].[sys_userlogon]
GO

CREATE TABLE [dbo].[sys_userlogon] (
  [F_Id] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_UserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_UserPassword] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_UserSecretkey] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_AllowStartTime] datetime2(7)  NULL,
  [F_AllowEndTime] datetime2(7)  NULL,
  [F_LockStartDate] datetime2(7)  NULL,
  [F_LockEndDate] datetime2(7)  NULL,
  [F_FirstVisitTime] datetime2(7)  NULL,
  [F_PreviousVisitTime] datetime2(7)  NULL,
  [F_LastVisitTime] datetime2(7)  NULL,
  [F_ChangePasswordDate] datetime2(7)  NULL,
  [F_MultiUserLogin] tinyint  NULL,
  [F_LogOnCount] int  NULL,
  [F_UserOnLine] tinyint  NULL,
  [F_Question] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_AnswerQuestion] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_CheckIPAddress] tinyint  NULL,
  [F_Language] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_Theme] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_LoginSession] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_ErrorNum] int  NULL
)
GO

ALTER TABLE [dbo].[sys_userlogon] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of sys_userlogon
-- ----------------------------
INSERT INTO [dbo].[sys_userlogon] ([F_Id], [F_UserId], [F_UserPassword], [F_UserSecretkey], [F_AllowStartTime], [F_AllowEndTime], [F_LockStartDate], [F_LockEndDate], [F_FirstVisitTime], [F_PreviousVisitTime], [F_LastVisitTime], [F_ChangePasswordDate], [F_MultiUserLogin], [F_LogOnCount], [F_UserOnLine], [F_Question], [F_AnswerQuestion], [F_CheckIPAddress], [F_Language], [F_Theme], [F_LoginSession], [F_ErrorNum]) VALUES (N'490c2999-9fd9-4139-b1b2-1677512f8f37', N'490c2999-9fd9-4139-b1b2-1677512f8f37', N'a43326e52778fb912886baa9f47ffe50', N'7c054f4bd5055c82', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'0', N'0', NULL, NULL, NULL, NULL, NULL, NULL, N'0')
GO

INSERT INTO [dbo].[sys_userlogon] ([F_Id], [F_UserId], [F_UserPassword], [F_UserSecretkey], [F_AllowStartTime], [F_AllowEndTime], [F_LockStartDate], [F_LockEndDate], [F_FirstVisitTime], [F_PreviousVisitTime], [F_LastVisitTime], [F_ChangePasswordDate], [F_MultiUserLogin], [F_LogOnCount], [F_UserOnLine], [F_Question], [F_AnswerQuestion], [F_CheckIPAddress], [F_Language], [F_Theme], [F_LoginSession], [F_ErrorNum]) VALUES (N'65d196b0-5475-47a0-a9f1-853e265168a2', N'65d196b0-5475-47a0-a9f1-853e265168a2', N'5aadf457a6e108e50a3f78d2203ff1ea', N'137f9e1763a4b7fd', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'0', N'0', NULL, NULL, NULL, NULL, NULL, NULL, N'0')
GO

INSERT INTO [dbo].[sys_userlogon] ([F_Id], [F_UserId], [F_UserPassword], [F_UserSecretkey], [F_AllowStartTime], [F_AllowEndTime], [F_LockStartDate], [F_LockEndDate], [F_FirstVisitTime], [F_PreviousVisitTime], [F_LastVisitTime], [F_ChangePasswordDate], [F_MultiUserLogin], [F_LogOnCount], [F_UserOnLine], [F_Question], [F_AnswerQuestion], [F_CheckIPAddress], [F_Language], [F_Theme], [F_LoginSession], [F_ErrorNum]) VALUES (N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'6a9ba81dc2336411becfd055db00a417', N'fff96a7bef7ae242', NULL, NULL, NULL, NULL, NULL, N'2020-04-17 14:47:44.0000000', N'2020-04-17 14:59:58.0000000', NULL, N'0', N'360', N'0', NULL, NULL, N'0', NULL, NULL, N'evrcyibdv42f3ykhfy1yz3ur', N'0')
GO


-- ----------------------------
-- Primary Key structure for table cms_articlecategory
-- ----------------------------
ALTER TABLE [dbo].[cms_articlecategory] ADD CONSTRAINT [PK__cms_arti__2C6EC723118C739A] PRIMARY KEY CLUSTERED ([F_Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table cms_articlenews
-- ----------------------------
ALTER TABLE [dbo].[cms_articlenews] ADD CONSTRAINT [PK__cms_arti__2C6EC723F82F8C86] PRIMARY KEY CLUSTERED ([F_Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table oms_flowinstance
-- ----------------------------
ALTER TABLE [dbo].[oms_flowinstance] ADD CONSTRAINT [PK__oms_flow__2C6EC723CA0BFDB0] PRIMARY KEY CLUSTERED ([F_Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table oms_flowinstancehis
-- ----------------------------
ALTER TABLE [dbo].[oms_flowinstancehis] ADD CONSTRAINT [PK__oms_flow__2C6EC7237F008691] PRIMARY KEY CLUSTERED ([F_Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table oms_flowinstanceinfo
-- ----------------------------
ALTER TABLE [dbo].[oms_flowinstanceinfo] ADD CONSTRAINT [PK__oms_flow__2C6EC7231586F77E] PRIMARY KEY CLUSTERED ([F_Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table oms_formtest
-- ----------------------------
ALTER TABLE [dbo].[oms_formtest] ADD CONSTRAINT [PK__oms_form__2C6EC723B9D6E206] PRIMARY KEY CLUSTERED ([F_Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table oms_message
-- ----------------------------
ALTER TABLE [dbo].[oms_message] ADD CONSTRAINT [PK__oms_mess__2C6EC7238583455C] PRIMARY KEY CLUSTERED ([F_Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table oms_messagehis
-- ----------------------------
ALTER TABLE [dbo].[oms_messagehis] ADD CONSTRAINT [PK__oms_mess__2C6EC7239F789320] PRIMARY KEY CLUSTERED ([F_Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Indexes structure for table oms_uploadfile
-- ----------------------------
CREATE UNIQUE NONCLUSTERED INDEX [IX_OMS_UPLOADFile]
ON [dbo].[oms_uploadfile] (
  [F_FileName] ASC
)
GO

EXEC sp_addextendedproperty
'MS_Description', N'唯一键',
'SCHEMA', N'dbo',
'TABLE', N'oms_uploadfile',
'INDEX', N'IX_OMS_UPLOADFile'
GO


-- ----------------------------
-- Primary Key structure for table oms_uploadfile
-- ----------------------------
ALTER TABLE [dbo].[oms_uploadfile] ADD CONSTRAINT [PK__oms_uplo__2C6EC7232F49C59A] PRIMARY KEY CLUSTERED ([F_Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table sys_area
-- ----------------------------
ALTER TABLE [dbo].[sys_area] ADD CONSTRAINT [PK__sys_area__2C6EC72382BADECE] PRIMARY KEY CLUSTERED ([F_Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Indexes structure for table sys_dataprivilegerule
-- ----------------------------
CREATE UNIQUE NONCLUSTERED INDEX [XK_DataPrivilegeRule_1]
ON [dbo].[sys_dataprivilegerule] (
  [F_ModuleId] ASC
)
GO

EXEC sp_addextendedproperty
'MS_Description', N'唯一键',
'SCHEMA', N'dbo',
'TABLE', N'sys_dataprivilegerule',
'INDEX', N'XK_DataPrivilegeRule_1'
GO


-- ----------------------------
-- Primary Key structure for table sys_dataprivilegerule
-- ----------------------------
ALTER TABLE [dbo].[sys_dataprivilegerule] ADD CONSTRAINT [PK__sys_data__2C6EC72399DF8488] PRIMARY KEY CLUSTERED ([F_Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table sys_filterip
-- ----------------------------
ALTER TABLE [dbo].[sys_filterip] ADD CONSTRAINT [PK__sys_filt__2C6EC7238880D279] PRIMARY KEY CLUSTERED ([F_Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table sys_flowscheme
-- ----------------------------
ALTER TABLE [dbo].[sys_flowscheme] ADD CONSTRAINT [PK__sys_flow__2C6EC72341F99DA5] PRIMARY KEY CLUSTERED ([F_Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Indexes structure for table sys_form
-- ----------------------------
CREATE UNIQUE NONCLUSTERED INDEX [IX_SYS_FORM]
ON [dbo].[sys_form] (
  [F_Name] ASC
)
GO

EXEC sp_addextendedproperty
'MS_Description', N'唯一',
'SCHEMA', N'dbo',
'TABLE', N'sys_form',
'INDEX', N'IX_SYS_FORM'
GO


-- ----------------------------
-- Primary Key structure for table sys_form
-- ----------------------------
ALTER TABLE [dbo].[sys_form] ADD CONSTRAINT [PK__sys_form__2C6EC7235CFE6F8D] PRIMARY KEY CLUSTERED ([F_Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Indexes structure for table sys_items
-- ----------------------------
CREATE UNIQUE NONCLUSTERED INDEX [IX_Sys_Items]
ON [dbo].[sys_items] (
  [F_EnCode] ASC
)
GO

EXEC sp_addextendedproperty
'MS_Description', N'唯一键',
'SCHEMA', N'dbo',
'TABLE', N'sys_items',
'INDEX', N'IX_Sys_Items'
GO


-- ----------------------------
-- Primary Key structure for table sys_items
-- ----------------------------
ALTER TABLE [dbo].[sys_items] ADD CONSTRAINT [PK__sys_item__2C6EC723996C098F] PRIMARY KEY CLUSTERED ([F_Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Indexes structure for table sys_itemsdetail
-- ----------------------------
CREATE UNIQUE NONCLUSTERED INDEX [IX_Sys_ItemsDetail]
ON [dbo].[sys_itemsdetail] (
  [F_ItemId] ASC,
  [F_ItemCode] ASC
)
GO

EXEC sp_addextendedproperty
'MS_Description', N'唯一键',
'SCHEMA', N'dbo',
'TABLE', N'sys_itemsdetail',
'INDEX', N'IX_Sys_ItemsDetail'
GO


-- ----------------------------
-- Primary Key structure for table sys_itemsdetail
-- ----------------------------
ALTER TABLE [dbo].[sys_itemsdetail] ADD CONSTRAINT [PK__sys_item__2C6EC7231967A541] PRIMARY KEY CLUSTERED ([F_Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table sys_log
-- ----------------------------
ALTER TABLE [dbo].[sys_log] ADD CONSTRAINT [PK__sys_log__2C6EC723F1682072] PRIMARY KEY CLUSTERED ([F_Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Indexes structure for table sys_module
-- ----------------------------
CREATE UNIQUE NONCLUSTERED INDEX [IX_Sys_Module]
ON [dbo].[sys_module] (
  [F_FullName] ASC
)
GO

EXEC sp_addextendedproperty
'MS_Description', N'唯一键',
'SCHEMA', N'dbo',
'TABLE', N'sys_module',
'INDEX', N'IX_Sys_Module'
GO


-- ----------------------------
-- Primary Key structure for table sys_module
-- ----------------------------
ALTER TABLE [dbo].[sys_module] ADD CONSTRAINT [PK__sys_modu__2C6EC723C940632D] PRIMARY KEY CLUSTERED ([F_Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Indexes structure for table sys_modulebutton
-- ----------------------------
CREATE UNIQUE NONCLUSTERED INDEX [IX_Sys_ModuleButton]
ON [dbo].[sys_modulebutton] (
  [F_ModuleId] ASC,
  [F_Layers] ASC,
  [F_EnCode] ASC
)
GO

EXEC sp_addextendedproperty
'MS_Description', N'唯一键',
'SCHEMA', N'dbo',
'TABLE', N'sys_modulebutton',
'INDEX', N'IX_Sys_ModuleButton'
GO


-- ----------------------------
-- Primary Key structure for table sys_modulebutton
-- ----------------------------
ALTER TABLE [dbo].[sys_modulebutton] ADD CONSTRAINT [PK__sys_modu__2C6EC723FB4E75A7] PRIMARY KEY CLUSTERED ([F_Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Indexes structure for table sys_modulefields
-- ----------------------------
CREATE UNIQUE NONCLUSTERED INDEX [IX_Sys_ModuleFields]
ON [dbo].[sys_modulefields] (
  [F_ModuleId] ASC,
  [F_EnCode] ASC
)
GO

EXEC sp_addextendedproperty
'MS_Description', N'唯一键',
'SCHEMA', N'dbo',
'TABLE', N'sys_modulefields',
'INDEX', N'IX_Sys_ModuleFields'
GO


-- ----------------------------
-- Primary Key structure for table sys_modulefields
-- ----------------------------
ALTER TABLE [dbo].[sys_modulefields] ADD CONSTRAINT [PK__sys_modu__2C6EC72399F9B3BB] PRIMARY KEY CLUSTERED ([F_Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Indexes structure for table sys_notice
-- ----------------------------
CREATE UNIQUE NONCLUSTERED INDEX [IX_Sys_Notice]
ON [dbo].[sys_notice] (
  [F_Title] ASC
)
GO

EXEC sp_addextendedproperty
'MS_Description', N'唯一键',
'SCHEMA', N'dbo',
'TABLE', N'sys_notice',
'INDEX', N'IX_Sys_Notice'
GO


-- ----------------------------
-- Primary Key structure for table sys_notice
-- ----------------------------
ALTER TABLE [dbo].[sys_notice] ADD CONSTRAINT [PK__sys_noti__2C6EC723B3DA0D07] PRIMARY KEY CLUSTERED ([F_Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table sys_openjob
-- ----------------------------
ALTER TABLE [dbo].[sys_openjob] ADD CONSTRAINT [PK__sys_open__2C6EC72338EC609F] PRIMARY KEY CLUSTERED ([F_Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table sys_openjoblog
-- ----------------------------
ALTER TABLE [dbo].[sys_openjoblog] ADD CONSTRAINT [PK__sys_open__2C6EC7235420A006] PRIMARY KEY CLUSTERED ([F_Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Indexes structure for table sys_organize
-- ----------------------------
CREATE UNIQUE NONCLUSTERED INDEX [IX_Sys_Organize]
ON [dbo].[sys_organize] (
  [F_EnCode] ASC
)
GO

EXEC sp_addextendedproperty
'MS_Description', N'唯一键',
'SCHEMA', N'dbo',
'TABLE', N'sys_organize',
'INDEX', N'IX_Sys_Organize'
GO


-- ----------------------------
-- Primary Key structure for table sys_organize
-- ----------------------------
ALTER TABLE [dbo].[sys_organize] ADD CONSTRAINT [PK__sys_orga__2C6EC723197ED13F] PRIMARY KEY CLUSTERED ([F_Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Indexes structure for table sys_quickmodule
-- ----------------------------
CREATE UNIQUE NONCLUSTERED INDEX [IX_Sys_QuickModule]
ON [dbo].[sys_quickmodule] (
  [F_ModuleId] ASC,
  [F_CreatorUserId] ASC
)
GO

EXEC sp_addextendedproperty
'MS_Description', N'唯一键',
'SCHEMA', N'dbo',
'TABLE', N'sys_quickmodule',
'INDEX', N'IX_Sys_QuickModule'
GO


-- ----------------------------
-- Primary Key structure for table sys_quickmodule
-- ----------------------------
ALTER TABLE [dbo].[sys_quickmodule] ADD CONSTRAINT [PK__sys_quic__2C6EC72323784CD2] PRIMARY KEY CLUSTERED ([F_Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Indexes structure for table sys_role
-- ----------------------------
CREATE UNIQUE NONCLUSTERED INDEX [IX_Sys_Role]
ON [dbo].[sys_role] (
  [F_EnCode] ASC
)
GO

EXEC sp_addextendedproperty
'MS_Description', N'唯一键',
'SCHEMA', N'dbo',
'TABLE', N'sys_role',
'INDEX', N'IX_Sys_Role'
GO


-- ----------------------------
-- Primary Key structure for table sys_role
-- ----------------------------
ALTER TABLE [dbo].[sys_role] ADD CONSTRAINT [PK__sys_role__2C6EC72309CB2D1C] PRIMARY KEY CLUSTERED ([F_Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table sys_roleauthorize
-- ----------------------------
ALTER TABLE [dbo].[sys_roleauthorize] ADD CONSTRAINT [PK__sys_role__2C6EC723EF69CB10] PRIMARY KEY CLUSTERED ([F_Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Indexes structure for table sys_serverstate
-- ----------------------------
CREATE UNIQUE NONCLUSTERED INDEX [IX_Sys_ServerState]
ON [dbo].[sys_serverstate] (
  [F_WebSite] ASC,
  [F_Date] ASC
)
GO

EXEC sp_addextendedproperty
'MS_Description', N'唯一键',
'SCHEMA', N'dbo',
'TABLE', N'sys_serverstate',
'INDEX', N'IX_Sys_ServerState'
GO


-- ----------------------------
-- Primary Key structure for table sys_serverstate
-- ----------------------------
ALTER TABLE [dbo].[sys_serverstate] ADD CONSTRAINT [PK__sys_serv__2C6EC723054231C1] PRIMARY KEY CLUSTERED ([F_Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Indexes structure for table sys_systemset
-- ----------------------------
CREATE UNIQUE NONCLUSTERED INDEX [IX_systemset]
ON [dbo].[sys_systemset] (
  [F_DbNumber] ASC
)
GO

EXEC sp_addextendedproperty
'MS_Description', N'ConfigId',
'SCHEMA', N'dbo',
'TABLE', N'sys_systemset',
'INDEX', N'IX_systemset'
GO


-- ----------------------------
-- Primary Key structure for table sys_systemset
-- ----------------------------
ALTER TABLE [dbo].[sys_systemset] ADD CONSTRAINT [PK__sys_syst__2C6EC723B599FEE9] PRIMARY KEY CLUSTERED ([F_Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Indexes structure for table sys_user
-- ----------------------------
CREATE UNIQUE NONCLUSTERED INDEX [IX_Sys_User]
ON [dbo].[sys_user] (
  [F_Account] ASC
)
GO

EXEC sp_addextendedproperty
'MS_Description', N'唯一键',
'SCHEMA', N'dbo',
'TABLE', N'sys_user',
'INDEX', N'IX_Sys_User'
GO


-- ----------------------------
-- Primary Key structure for table sys_user
-- ----------------------------
ALTER TABLE [dbo].[sys_user] ADD CONSTRAINT [PK__sys_user__2C6EC723D3D5AD8F] PRIMARY KEY CLUSTERED ([F_Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table sys_userlogon
-- ----------------------------
ALTER TABLE [dbo].[sys_userlogon] ADD CONSTRAINT [PK__sys_user__2C6EC723E7E20414] PRIMARY KEY CLUSTERED ([F_Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO

