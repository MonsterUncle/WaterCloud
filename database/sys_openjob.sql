/*
 Navicat Premium Data Transfer

 Source Server         : 本地mysql
 Source Server Type    : MySQL
 Source Server Version : 80025
 Source Host           : 127.0.0.1:3306
 Source Schema         : watercloudnetdb

 Target Server Type    : MySQL
 Target Server Version : 80025
 File Encoding         : 65001

 Date: 16/11/2021 11:34:46
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for sys_openjob
-- ----------------------------
DROP TABLE IF EXISTS `sys_openjob`;
CREATE TABLE `sys_openjob`  (
  `F_Id` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `F_FileName` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `F_JobName` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `F_JobGroup` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `F_StarRunTime` datetime NULL DEFAULT NULL,
  `F_EndRunTime` datetime NULL DEFAULT NULL,
  `F_CronExpress` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `F_DeleteMark` tinyint NULL DEFAULT NULL,
  `F_EnabledMark` tinyint NULL DEFAULT NULL,
  `F_Description` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `F_CreatorTime` datetime NULL DEFAULT NULL,
  `F_CreatorUserId` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `F_LastModifyTime` datetime NULL DEFAULT NULL,
  `F_LastModifyUserId` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `F_DeleteTime` datetime NULL DEFAULT NULL,
  `F_DeleteUserId` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `F_LastRunTime` datetime NULL DEFAULT NULL,
  `F_LastRunMark` tinyint NULL DEFAULT NULL,
  `F_LastRunErrTime` datetime NULL DEFAULT NULL,
  `F_LastRunErrMsg` longtext CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `F_JobType` int NOT NULL,
  `F_IsLog` char(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `F_RequestHeaders` longtext CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `F_RequestString` longtext CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `F_RequestUrl` longtext CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `F_DbNumber` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

SET FOREIGN_KEY_CHECKS = 1;
