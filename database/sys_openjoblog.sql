/*
 Navicat Premium Data Transfer

 Source Server         : local
 Source Server Type    : MySQL
 Source Server Version : 80022
 Source Host           : localhost:3306
 Source Schema         : watercloudnetdb

 Target Server Type    : MySQL
 Target Server Version : 80022
 File Encoding         : 65001

 Date: 02/12/2020 13:17:46
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for sys_openjoblog
-- ----------------------------
DROP TABLE IF EXISTS `sys_openjoblog`;
CREATE TABLE `sys_openjoblog`  (
  `F_Id` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `F_JobId` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '任务Id',
  `F_Description` longtext CHARACTER SET utf8 COLLATE utf8_general_ci NULL COMMENT '任务信息',
  `F_CreatorTime` datetime(0) NULL DEFAULT NULL COMMENT '执行时间',
  `F_EnabledMark` tinyint NOT NULL COMMENT '执行状态',
  `F_JobName` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '任务名称',
  PRIMARY KEY (`F_Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

INSERT INTO `watercloudnetdb`.`sys_modulebutton`(`F_Id`, `F_ModuleId`, `F_ParentId`, `F_Layers`, `F_EnCode`, `F_FullName`, `F_Icon`, `F_Location`, `F_JsEvent`, `F_UrlAddress`, `F_Split`, `F_IsPublic`, `F_AllowEdit`, `F_AllowDelete`, `F_SortCode`, `F_DeleteMark`, `F_EnabledMark`, `F_Description`, `F_CreatorTime`, `F_CreatorUserId`, `F_LastModifyTime`, `F_LastModifyUserId`, `F_DeleteTime`, `F_DeleteUserId`) VALUES ('cc115cef-c2d1-4b97-adbc-ea885aea6190', 'e3188a69-de3a-40ef-a5ff-5eaf460f5d20', '0', 1, 'NF-log', '日志', NULL, 1, 'log', '/SystemSecurity/OpenJobs/Details', NULL, 0, 0, 0, 6, 0, 1, '', '2020-12-02 13:14:32', '9f2ec079-7d0f-4fe2-90ab-8b09a8302aba', NULL, NULL, NULL, NULL);

-- ----------------------------
-- Records of sys_openjoblog
-- ----------------------------

SET FOREIGN_KEY_CHECKS = 1;
