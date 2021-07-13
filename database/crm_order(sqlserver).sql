
-- ----------------------------
-- Table structure for crm_order
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[crm_order]') AND type IN ('U'))
	DROP TABLE [dbo].[crm_order]
GO

CREATE TABLE [dbo].[crm_order] (
  [F_Id] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_OrderCode] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_OrderState] int  NOT NULL,
  [F_NeedTime] datetime2(7)  NULL,
  [F_ActualTime] datetime2(7)  NULL,
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

ALTER TABLE [dbo].[crm_order] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'主键',
'SCHEMA', N'dbo',
'TABLE', N'crm_order',
'COLUMN', N'F_Id'
GO

EXEC sp_addextendedproperty
'MS_Description', N'订单编号',
'SCHEMA', N'dbo',
'TABLE', N'crm_order',
'COLUMN', N'F_OrderCode'
GO

EXEC sp_addextendedproperty
'MS_Description', N'订单状态(0未完成，1已完成)',
'SCHEMA', N'dbo',
'TABLE', N'crm_order',
'COLUMN', N'F_OrderState'
GO

EXEC sp_addextendedproperty
'MS_Description', N'需求时间',
'SCHEMA', N'dbo',
'TABLE', N'crm_order',
'COLUMN', N'F_NeedTime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'实际完成时间',
'SCHEMA', N'dbo',
'TABLE', N'crm_order',
'COLUMN', N'F_ActualTime'
GO


-- ----------------------------
-- Records of crm_order
-- ----------------------------
INSERT INTO [dbo].[crm_order] ([F_Id], [F_OrderCode], [F_OrderState], [F_NeedTime], [F_ActualTime], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_CreatorUserName], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId]) VALUES (N'08d9459c-5cd7-453f-8240-d566f1fe058c', N'OR-20210713091957', N'1', N'2021-07-13 00:00:00.0000000', N'2021-07-13 00:00:00.0000000', N'0', N'1', N'', N'2021-07-13 09:20:12.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'超级管理员', N'2021-07-13 09:29:45.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL)
GO


-- ----------------------------
-- Table structure for crm_orderdetail
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[crm_orderdetail]') AND type IN ('U'))
	DROP TABLE [dbo].[crm_orderdetail]
GO

CREATE TABLE [dbo].[crm_orderdetail] (
  [F_Id] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_OrderId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [F_OrderState] int  NOT NULL,
  [F_ProductName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_ProductDescription] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_ProductUnit] nvarchar(5) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_NeedNum] int  NULL,
  [F_ActualNum] int  NULL,
  [F_Description] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_CreatorTime] datetime2(7)  NULL,
  [F_CreatorUserId] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_CreatorUserName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [F_NeedTime] datetime2(7)  NULL,
  [F_ActualTime] datetime2(7)  NULL
)
GO

ALTER TABLE [dbo].[crm_orderdetail] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'主键',
'SCHEMA', N'dbo',
'TABLE', N'crm_orderdetail',
'COLUMN', N'F_Id'
GO

EXEC sp_addextendedproperty
'MS_Description', N'订单状态',
'SCHEMA', N'dbo',
'TABLE', N'crm_orderdetail',
'COLUMN', N'F_OrderState'
GO

EXEC sp_addextendedproperty
'MS_Description', N'产品名称',
'SCHEMA', N'dbo',
'TABLE', N'crm_orderdetail',
'COLUMN', N'F_ProductName'
GO

EXEC sp_addextendedproperty
'MS_Description', N'产品规格',
'SCHEMA', N'dbo',
'TABLE', N'crm_orderdetail',
'COLUMN', N'F_ProductDescription'
GO

EXEC sp_addextendedproperty
'MS_Description', N'产品单位',
'SCHEMA', N'dbo',
'TABLE', N'crm_orderdetail',
'COLUMN', N'F_ProductUnit'
GO

EXEC sp_addextendedproperty
'MS_Description', N'需求数量',
'SCHEMA', N'dbo',
'TABLE', N'crm_orderdetail',
'COLUMN', N'F_NeedNum'
GO

EXEC sp_addextendedproperty
'MS_Description', N'实际数量',
'SCHEMA', N'dbo',
'TABLE', N'crm_orderdetail',
'COLUMN', N'F_ActualNum'
GO

EXEC sp_addextendedproperty
'MS_Description', N'需求时间',
'SCHEMA', N'dbo',
'TABLE', N'crm_orderdetail',
'COLUMN', N'F_NeedTime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'实际时间',
'SCHEMA', N'dbo',
'TABLE', N'crm_orderdetail',
'COLUMN', N'F_ActualTime'
GO


-- ----------------------------
-- Records of crm_orderdetail
-- ----------------------------
INSERT INTO [dbo].[crm_orderdetail] ([F_Id], [F_OrderId], [F_OrderState], [F_ProductName], [F_ProductDescription], [F_ProductUnit], [F_NeedNum], [F_ActualNum], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_CreatorUserName], [F_NeedTime], [F_ActualTime]) VALUES (N'08d9459d-b222-4ad5-8e4e-c5153e69a752', N'08d9459c-5cd7-453f-8240-d566f1fe058c', N'1', N'222', N'', N'', N'3', N'2', N'', NULL, N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', N'超级管理员', N'2021-07-13 00:00:00.0000000', N'2021-07-13 00:00:00.0000000')
GO


-- ----------------------------
-- Indexes structure for table crm_order
-- ----------------------------
CREATE UNIQUE NONCLUSTERED INDEX [crm_order_key1]
ON [dbo].[crm_order] (
  [F_OrderCode] ASC
)
GO

EXEC sp_addextendedproperty
'MS_Description', N'编号唯一',
'SCHEMA', N'dbo',
'TABLE', N'crm_order',
'INDEX', N'crm_order_key1'
GO


-- ----------------------------
-- Primary Key structure for table crm_order
-- ----------------------------
ALTER TABLE [dbo].[crm_order] ADD CONSTRAINT [PK__crm_orde__2C6EC723652E10FE] PRIMARY KEY CLUSTERED ([F_Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Indexes structure for table crm_orderdetail
-- ----------------------------
CREATE UNIQUE NONCLUSTERED INDEX [crm_orderdetail_key1]
ON [dbo].[crm_orderdetail] (
  [F_OrderId] ASC,
  [F_ProductName] ASC
)
GO

EXEC sp_addextendedproperty
'MS_Description', N'唯一键',
'SCHEMA', N'dbo',
'TABLE', N'crm_orderdetail',
'INDEX', N'crm_orderdetail_key1'
GO


-- ----------------------------
-- Primary Key structure for table crm_orderdetail
-- ----------------------------
ALTER TABLE [dbo].[crm_orderdetail] ADD CONSTRAINT [PK__crm_orde__2C6EC7239DE559CA] PRIMARY KEY CLUSTERED ([F_Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'08d94532-98ac-4751-8cd4-c6c5ec4048e6', N'08d94532-39d1-4822-80b3-0c25a8183155', 3, N'Order', N'订单管理', N'fa fa-anchor', N'/OrderManagement/Order/Index', N'iframe', 1, 0, 0, 0, 0, 0, 1, 0, 1, N'', '2021-07-12 20:43:06.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', '2021-07-13 09:45:05.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, N'')
GO

INSERT INTO [dbo].[sys_module] ([F_Id], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_UrlAddress], [F_Target], [F_IsMenu], [F_IsExpand], [F_IsFields], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'08d94532-39d1-4822-80b3-0c25a8183155', N'87dc2de3-ccbc-4dab-bb90-89fc68cbde4f', 2, N'OrderManagement', N'订单管理', N'fa fa-first-order', NULL, N'expand', 1, 1, 0, 0, 0, 0, 7, 0, 1, N'', '2021-07-12 20:40:27.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, N'')
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'08d94532-98b0-4b60-86a7-eca606765531', N'08d94532-98ac-4751-8cd4-c6c5ec4048e6', N'0', 1, N'NF-add', N'新增', NULL, 1, N'add', N'/OrderManagement/Order/Form', 0, 0, 0, 0, 0, 0, 1, NULL, '2021-07-12 20:43:06.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'08d94532-98b0-4b92-8a2f-4da9f59afa21', N'08d94532-98ac-4751-8cd4-c6c5ec4048e6', N'0', 1, N'NF-edit', N'修改', NULL, 2, N'edit', N'/OrderManagement/Order/Form', 0, 0, 0, 0, 1, 0, 1, NULL, '2021-07-12 20:43:06.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'08d94532-98b0-4ba2-87b3-6c976665cb6b', N'08d94532-98ac-4751-8cd4-c6c5ec4048e6', N'0', 1, N'NF-delete', N'删除', NULL, 2, N'delete', N'/OrderManagement/Order/DeleteForm', 0, 0, 0, 0, 2, 0, 1, NULL, '2021-07-12 20:43:06.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulebutton] ([F_Id], [F_ModuleId], [F_ParentId], [F_Layers], [F_EnCode], [F_FullName], [F_Icon], [F_Location], [F_JsEvent], [F_UrlAddress], [F_Split], [F_IsPublic], [F_AllowEdit], [F_AllowDelete], [F_SortCode], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_Authorize]) VALUES (N'08d94532-98b0-4bac-8e49-471d5b61be95', N'08d94532-98ac-4751-8cd4-c6c5ec4048e6', N'0', 1, N'NF-details', N'查看', NULL, 2, N'details', N'/OrderManagement/Order/Details', 0, 0, 0, 0, 3, 0, 1, NULL, '2021-07-12 20:43:06.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'08d94532-98b0-4bb9-843f-6a42d29d5b04', N'08d94532-98ac-4751-8cd4-c6c5ec4048e6', N'F_OrderCode', N'订单编号', 0, 1, NULL, '2021-07-12 20:43:06.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, 1)
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'08d94532-98b0-4bca-822e-9e44e86864e5', N'08d94532-98ac-4751-8cd4-c6c5ec4048e6', N'F_OrderState', N'订单状态(0待确认，待采购，1已完成)', 0, 1, NULL, '2021-07-12 20:43:06.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, 1)
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'08d94532-98b0-4bd5-8ba3-fc7834814f3e', N'08d94532-98ac-4751-8cd4-c6c5ec4048e6', N'F_NeedTime', N'需求时间', 0, 1, NULL, '2021-07-12 20:43:06.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, 1)
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'08d94532-98b0-4be2-8991-3b929d082f6c', N'08d94532-98ac-4751-8cd4-c6c5ec4048e6', N'F_ActualTime', N'实际时间', 0, 1, NULL, '2021-07-12 20:43:06.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, 1)
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'08d94532-98b0-4beb-89d3-425505eb2d36', N'08d94532-98ac-4751-8cd4-c6c5ec4048e6', N'F_EnabledMark', N'F_EnabledMark', 0, 1, NULL, '2021-07-12 20:43:06.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, 1)
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'08d94532-98b0-4bf6-82f3-9f62b0b5cec6', N'08d94532-98ac-4751-8cd4-c6c5ec4048e6', N'F_Description', N'F_Description', 0, 1, NULL, '2021-07-12 20:43:06.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, 1)
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'08d94532-98b0-4c01-86ff-7b056e669066', N'08d94532-98ac-4751-8cd4-c6c5ec4048e6', N'F_CreatorTime', N'F_CreatorTime', 0, 1, NULL, '2021-07-12 20:43:06.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, 1)
GO

INSERT INTO [dbo].[sys_modulefields] ([F_Id], [F_ModuleId], [F_EnCode], [F_FullName], [F_DeleteMark], [F_EnabledMark], [F_Description], [F_CreatorTime], [F_CreatorUserId], [F_LastModifyTime], [F_LastModifyUserId], [F_DeleteTime], [F_DeleteUserId], [F_IsPublic]) VALUES (N'08d94532-98b0-4c0c-81e2-b66009a2420f', N'08d94532-98ac-4751-8cd4-c6c5ec4048e6', N'F_CreatorUserName', N'F_CreatorUserName', 0, 1, NULL, '2021-07-12 20:43:06.0000000', N'9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, 1)
GO

