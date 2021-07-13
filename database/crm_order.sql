CREATE TABLE `crm_order`  (
  `F_Id` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '主键',
  `F_OrderCode` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '订单编号',
  `F_OrderState` int NOT NULL COMMENT '订单状态(0未完成，1已完成)',
  `F_NeedTime` datetime NULL DEFAULT NULL COMMENT '需求时间',
  `F_ActualTime` datetime NULL DEFAULT NULL COMMENT '实际完成时间',
  `F_DeleteMark` tinyint(1) NULL DEFAULT NULL,
  `F_EnabledMark` tinyint(1) NULL DEFAULT NULL,
  `F_Description` text CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL,
  `F_CreatorTime` datetime NULL DEFAULT NULL,
  `F_CreatorUserId` varchar(50) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `F_CreatorUserName` varchar(50) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `F_LastModifyTime` datetime NULL DEFAULT NULL,
  `F_LastModifyUserId` varchar(50) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `F_DeleteTime` datetime NULL DEFAULT NULL,
  `F_DeleteUserId` varchar(50) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  PRIMARY KEY (`F_Id`) USING BTREE,
  UNIQUE INDEX `crm_order_key1`(`F_OrderCode`) USING BTREE COMMENT '编号唯一'
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of crm_order
-- ----------------------------
INSERT INTO `crm_order` VALUES ('08d9459c-5cd7-453f-8240-d566f1fe058c', 'OR-20210713091957', 1, '2021-07-13 00:00:00', '2021-07-13 00:00:00', 0, 1, '', '2021-07-13 09:20:12', '9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', '超级管理员', '2021-07-13 09:29:45', '9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL);

-- ----------------------------
-- Table structure for crm_orderdetail
-- ----------------------------
CREATE TABLE `crm_orderdetail`  (
  `F_Id` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '主键',
  `F_OrderId` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `F_OrderState` int NOT NULL COMMENT '订单状态',
  `F_ProductName` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '产品名称',
  `F_ProductDescription` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '产品规格',
  `F_ProductUnit` varchar(5) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '产品单位',
  `F_NeedNum` int NULL DEFAULT NULL COMMENT '需求数量',
  `F_ActualNum` int NULL DEFAULT NULL COMMENT '实际数量',
  `F_Description` text CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL,
  `F_CreatorTime` datetime NULL DEFAULT NULL,
  `F_CreatorUserId` varchar(50) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `F_CreatorUserName` varchar(50) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `F_NeedTime` datetime NULL DEFAULT NULL COMMENT '需求时间',
  `F_ActualTime` datetime NULL DEFAULT NULL COMMENT '实际时间',
  PRIMARY KEY (`F_Id`) USING BTREE,
  UNIQUE INDEX `crm_orderdetail_key1`(`F_OrderId`, `F_ProductName`) USING BTREE COMMENT '唯一键'
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of crm_orderdetail
-- ----------------------------
INSERT INTO `crm_orderdetail` VALUES ('08d9459d-b222-4ad5-8e4e-c5153e69a752', '08d9459c-5cd7-453f-8240-d566f1fe058c', 1, '222', '', '', 3, 2, '', NULL, '9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', '超级管理员', '2021-07-13 00:00:00', '2021-07-13 00:00:00');

INSERT INTO `sys_module` (`F_Id`, `F_ParentId`, `F_Layers`, `F_EnCode`, `F_FullName`, `F_Icon`, `F_UrlAddress`, `F_Target`, `F_IsMenu`, `F_IsExpand`, `F_IsFields`, `F_IsPublic`, `F_AllowEdit`, `F_AllowDelete`, `F_SortCode`, `F_DeleteMark`, `F_EnabledMark`, `F_Description`, `F_CreatorTime`, `F_CreatorUserId`, `F_LastModifyTime`, `F_LastModifyUserId`, `F_DeleteTime`, `F_DeleteUserId`, `F_Authorize`) VALUES ('08d94532-98ac-4751-8cd4-c6c5ec4048e6', '08d94532-39d1-4822-80b3-0c25a8183155', 3, 'Order', '订单信息', 'fa fa-anchor', '/OrderManagement/Order/Index', 'iframe', 1, 0, 0, 0, 0, 0, 1, 0, 1, '', '2021-07-12 20:43:06', '9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', '2021-07-13 09:45:05', '9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, '');
INSERT INTO `sys_module` (`F_Id`, `F_ParentId`, `F_Layers`, `F_EnCode`, `F_FullName`, `F_Icon`, `F_UrlAddress`, `F_Target`, `F_IsMenu`, `F_IsExpand`, `F_IsFields`, `F_IsPublic`, `F_AllowEdit`, `F_AllowDelete`, `F_SortCode`, `F_DeleteMark`, `F_EnabledMark`, `F_Description`, `F_CreatorTime`, `F_CreatorUserId`, `F_LastModifyTime`, `F_LastModifyUserId`, `F_DeleteTime`, `F_DeleteUserId`, `F_Authorize`) VALUES ('08d94532-39d1-4822-80b3-0c25a8183155', '87dc2de3-ccbc-4dab-bb90-89fc68cbde4f', 2, 'OrderManagement', '订单管理', 'fa fa-first-order', NULL, 'expand', 1, 1, 0, 0, 0, 0, 7, 0, 1, '', '2021-07-12 20:40:27', '9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, '');
INSERT INTO `sys_modulebutton` (`F_Id`, `F_ModuleId`, `F_ParentId`, `F_Layers`, `F_EnCode`, `F_FullName`, `F_Icon`, `F_Location`, `F_JsEvent`, `F_UrlAddress`, `F_Split`, `F_IsPublic`, `F_AllowEdit`, `F_AllowDelete`, `F_SortCode`, `F_DeleteMark`, `F_EnabledMark`, `F_Description`, `F_CreatorTime`, `F_CreatorUserId`, `F_LastModifyTime`, `F_LastModifyUserId`, `F_DeleteTime`, `F_DeleteUserId`, `F_Authorize`) VALUES ('08d94532-98b0-4b60-86a7-eca606765531', '08d94532-98ac-4751-8cd4-c6c5ec4048e6', '0', 1, 'NF-add', '新增', NULL, 1, 'add', '/OrderManagement/Order/Form', 0, 0, 0, 0, 0, 0, 1, NULL, '2021-07-12 20:43:06', '9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, NULL);
INSERT INTO `sys_modulebutton` (`F_Id`, `F_ModuleId`, `F_ParentId`, `F_Layers`, `F_EnCode`, `F_FullName`, `F_Icon`, `F_Location`, `F_JsEvent`, `F_UrlAddress`, `F_Split`, `F_IsPublic`, `F_AllowEdit`, `F_AllowDelete`, `F_SortCode`, `F_DeleteMark`, `F_EnabledMark`, `F_Description`, `F_CreatorTime`, `F_CreatorUserId`, `F_LastModifyTime`, `F_LastModifyUserId`, `F_DeleteTime`, `F_DeleteUserId`, `F_Authorize`) VALUES ('08d94532-98b0-4b92-8a2f-4da9f59afa21', '08d94532-98ac-4751-8cd4-c6c5ec4048e6', '0', 1, 'NF-edit', '修改', NULL, 2, 'edit', '/OrderManagement/Order/Form', 0, 0, 0, 0, 1, 0, 1, NULL, '2021-07-12 20:43:06', '9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, NULL);
INSERT INTO `sys_modulebutton` (`F_Id`, `F_ModuleId`, `F_ParentId`, `F_Layers`, `F_EnCode`, `F_FullName`, `F_Icon`, `F_Location`, `F_JsEvent`, `F_UrlAddress`, `F_Split`, `F_IsPublic`, `F_AllowEdit`, `F_AllowDelete`, `F_SortCode`, `F_DeleteMark`, `F_EnabledMark`, `F_Description`, `F_CreatorTime`, `F_CreatorUserId`, `F_LastModifyTime`, `F_LastModifyUserId`, `F_DeleteTime`, `F_DeleteUserId`, `F_Authorize`) VALUES ('08d94532-98b0-4ba2-87b3-6c976665cb6b', '08d94532-98ac-4751-8cd4-c6c5ec4048e6', '0', 1, 'NF-delete', '删除', NULL, 2, 'delete', '/OrderManagement/Order/DeleteForm', 0, 0, 0, 0, 2, 0, 1, NULL, '2021-07-12 20:43:06', '9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, NULL);
INSERT INTO `sys_modulebutton` (`F_Id`, `F_ModuleId`, `F_ParentId`, `F_Layers`, `F_EnCode`, `F_FullName`, `F_Icon`, `F_Location`, `F_JsEvent`, `F_UrlAddress`, `F_Split`, `F_IsPublic`, `F_AllowEdit`, `F_AllowDelete`, `F_SortCode`, `F_DeleteMark`, `F_EnabledMark`, `F_Description`, `F_CreatorTime`, `F_CreatorUserId`, `F_LastModifyTime`, `F_LastModifyUserId`, `F_DeleteTime`, `F_DeleteUserId`, `F_Authorize`) VALUES ('08d94532-98b0-4bac-8e49-471d5b61be95', '08d94532-98ac-4751-8cd4-c6c5ec4048e6', '0', 1, 'NF-details', '查看', NULL, 2, 'details', '/OrderManagement/Order/Details', 0, 0, 0, 0, 3, 0, 1, NULL, '2021-07-12 20:43:06', '9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, NULL);
INSERT INTO `sys_modulefields` (`F_Id`, `F_ModuleId`, `F_EnCode`, `F_FullName`, `F_DeleteMark`, `F_EnabledMark`, `F_Description`, `F_CreatorTime`, `F_CreatorUserId`, `F_LastModifyTime`, `F_LastModifyUserId`, `F_DeleteTime`, `F_DeleteUserId`, `F_IsPublic`) VALUES ('08d94532-98b0-4bb9-843f-6a42d29d5b04', '08d94532-98ac-4751-8cd4-c6c5ec4048e6', 'F_OrderCode', '订单编号', 0, 1, NULL, '2021-07-12 20:43:06', '9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, 1);
INSERT INTO `sys_modulefields` (`F_Id`, `F_ModuleId`, `F_EnCode`, `F_FullName`, `F_DeleteMark`, `F_EnabledMark`, `F_Description`, `F_CreatorTime`, `F_CreatorUserId`, `F_LastModifyTime`, `F_LastModifyUserId`, `F_DeleteTime`, `F_DeleteUserId`, `F_IsPublic`) VALUES ('08d94532-98b0-4bca-822e-9e44e86864e5', '08d94532-98ac-4751-8cd4-c6c5ec4048e6', 'F_OrderState', '订单状态(0待确认，待采购，1已完成)', 0, 1, NULL, '2021-07-12 20:43:06', '9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, 1);
INSERT INTO `sys_modulefields` (`F_Id`, `F_ModuleId`, `F_EnCode`, `F_FullName`, `F_DeleteMark`, `F_EnabledMark`, `F_Description`, `F_CreatorTime`, `F_CreatorUserId`, `F_LastModifyTime`, `F_LastModifyUserId`, `F_DeleteTime`, `F_DeleteUserId`, `F_IsPublic`) VALUES ('08d94532-98b0-4bd5-8ba3-fc7834814f3e', '08d94532-98ac-4751-8cd4-c6c5ec4048e6', 'F_NeedTime', '需求时间', 0, 1, NULL, '2021-07-12 20:43:06', '9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, 1);
INSERT INTO `sys_modulefields` (`F_Id`, `F_ModuleId`, `F_EnCode`, `F_FullName`, `F_DeleteMark`, `F_EnabledMark`, `F_Description`, `F_CreatorTime`, `F_CreatorUserId`, `F_LastModifyTime`, `F_LastModifyUserId`, `F_DeleteTime`, `F_DeleteUserId`, `F_IsPublic`) VALUES ('08d94532-98b0-4be2-8991-3b929d082f6c', '08d94532-98ac-4751-8cd4-c6c5ec4048e6', 'F_ActualTime', '实际时间', 0, 1, NULL, '2021-07-12 20:43:06', '9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, 1);
INSERT INTO `sys_modulefields` (`F_Id`, `F_ModuleId`, `F_EnCode`, `F_FullName`, `F_DeleteMark`, `F_EnabledMark`, `F_Description`, `F_CreatorTime`, `F_CreatorUserId`, `F_LastModifyTime`, `F_LastModifyUserId`, `F_DeleteTime`, `F_DeleteUserId`, `F_IsPublic`) VALUES ('08d94532-98b0-4beb-89d3-425505eb2d36', '08d94532-98ac-4751-8cd4-c6c5ec4048e6', 'F_EnabledMark', 'F_EnabledMark', 0, 1, NULL, '2021-07-12 20:43:06', '9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, 1);
INSERT INTO `sys_modulefields` (`F_Id`, `F_ModuleId`, `F_EnCode`, `F_FullName`, `F_DeleteMark`, `F_EnabledMark`, `F_Description`, `F_CreatorTime`, `F_CreatorUserId`, `F_LastModifyTime`, `F_LastModifyUserId`, `F_DeleteTime`, `F_DeleteUserId`, `F_IsPublic`) VALUES ('08d94532-98b0-4bf6-82f3-9f62b0b5cec6', '08d94532-98ac-4751-8cd4-c6c5ec4048e6', 'F_Description', 'F_Description', 0, 1, NULL, '2021-07-12 20:43:06', '9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, 1);
INSERT INTO `sys_modulefields` (`F_Id`, `F_ModuleId`, `F_EnCode`, `F_FullName`, `F_DeleteMark`, `F_EnabledMark`, `F_Description`, `F_CreatorTime`, `F_CreatorUserId`, `F_LastModifyTime`, `F_LastModifyUserId`, `F_DeleteTime`, `F_DeleteUserId`, `F_IsPublic`) VALUES ('08d94532-98b0-4c01-86ff-7b056e669066', '08d94532-98ac-4751-8cd4-c6c5ec4048e6', 'F_CreatorTime', 'F_CreatorTime', 0, 1, NULL, '2021-07-12 20:43:06', '9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, 1);
INSERT INTO `sys_modulefields` (`F_Id`, `F_ModuleId`, `F_EnCode`, `F_FullName`, `F_DeleteMark`, `F_EnabledMark`, `F_Description`, `F_CreatorTime`, `F_CreatorUserId`, `F_LastModifyTime`, `F_LastModifyUserId`, `F_DeleteTime`, `F_DeleteUserId`, `F_IsPublic`) VALUES ('08d94532-98b0-4c0c-81e2-b66009a2420f', '08d94532-98ac-4751-8cd4-c6c5ec4048e6', 'F_CreatorUserName', 'F_CreatorUserName', 0, 1, NULL, '2021-07-12 20:43:06', '9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL, 1);

