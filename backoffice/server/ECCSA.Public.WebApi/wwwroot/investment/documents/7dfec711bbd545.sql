/*
 Navicat Premium Data Transfer

 Source Server         : localhost
 Source Server Type    : PostgreSQL
 Source Server Version : 130002
 Source Host           : localhost:5432
 Source Catalog        : eccsadb
 Source Schema         : services

 Target Server Type    : PostgreSQL
 Target Server Version : 130002
 File Encoding         : 65001

 Date: 30/09/2021 11:29:17
*/


-- ----------------------------
-- Table structure for service_followups
-- ----------------------------
DROP TABLE IF EXISTS "services"."service_followups";
CREATE TABLE "services"."service_followups" (
  "id" uuid NOT NULL,
  "service_id" uuid NOT NULL,
  "task_type_id" uuid NOT NULL,
  "start_date" timestamp(0) NOT NULL,
  "description" text COLLATE "pg_catalog"."default" NOT NULL,
  "is_last_task" bool NOT NULL,
  "created_by" uuid NOT NULL,
  "create_stamp" timestamp(6) NOT NULL DEFAULT '2021-09-17 13:15:49.131518'::timestamp without time zone,
  "updated_by" uuid NOT NULL,
  "update_stamp" timestamp(6) NOT NULL DEFAULT '2021-09-17 13:15:49.131562'::timestamp without time zone,
  "status" int4,
  "next_task_id" uuid,
  "duration_hr" numeric(255) NOT NULL DEFAULT 0,
  "extension" varchar(10) COLLATE "pg_catalog"."default" DEFAULT ''::character varying
)
;

-- ----------------------------
-- Records of service_followups
-- ----------------------------
INSERT INTO "services"."service_followups" VALUES ('7c693b0d-4aad-4270-bb04-54d7ba7563f4', '654ae377-27d9-43e2-a5c7-bda08dca6d19', '2f697041-35c2-44d6-9b9f-909544aa2063', '9000-12-31 00:00:00', '', 'f', '00000000-0000-0000-0000-0000000003f1', '2021-09-21 09:03:57.660526', '00000000-0000-0000-0000-0000000003f1', '2021-09-21 09:03:57.661283', 0, NULL, 0, '');
INSERT INTO "services"."service_followups" VALUES ('acdc58c2-f309-4973-a957-23efe8db1d87', '654ae377-27d9-43e2-a5c7-bda08dca6d19', '4af086f4-5f6d-4c12-a73e-c6fcc38b8dc1', '9000-12-31 00:00:00', '', 'f', '00000000-0000-0000-0000-0000000003f1', '2021-09-21 09:03:57.700297', '00000000-0000-0000-0000-0000000003f1', '2021-09-21 09:03:57.700299', 0, NULL, 0, '');
INSERT INTO "services"."service_followups" VALUES ('3b0abba3-d850-4587-943b-a3ba99479d63', '654ae377-27d9-43e2-a5c7-bda08dca6d19', '6fb2195f-c246-4d5b-8173-1b80840c068f', '9000-12-31 00:00:00', '', 'f', '00000000-0000-0000-0000-0000000003f1', '2021-09-21 09:03:57.700368', '00000000-0000-0000-0000-0000000003f1', '2021-09-21 09:03:57.700368', 0, NULL, 0, '');
INSERT INTO "services"."service_followups" VALUES ('9b82cc1b-7580-4c08-b160-d9ccfcd5227a', '654ae377-27d9-43e2-a5c7-bda08dca6d19', '10b53f93-fae2-444f-9976-eeb9172d46e7', '9000-12-31 00:00:00', '', 'f', '00000000-0000-0000-0000-0000000003f1', '2021-09-21 09:03:57.700395', '00000000-0000-0000-0000-0000000003f1', '2021-09-21 09:03:57.700396', 0, NULL, 0, '');
INSERT INTO "services"."service_followups" VALUES ('c9e2d9ee-fe7a-440a-813b-4d90283de80c', '3c23bd83-5829-40e8-9484-1fba78e6ed72', '2f697041-35c2-44d6-9b9f-909544aa2063', '9000-12-31 00:00:00', '', 'f', '00000000-0000-0000-0000-0000000003f1', '2021-09-22 05:02:08.671586', '00000000-0000-0000-0000-0000000003f1', '2021-09-22 05:02:08.672104', 0, NULL, 0, '');
INSERT INTO "services"."service_followups" VALUES ('1309f261-ea6d-4b5e-a020-44148eeb4bb8', '3c23bd83-5829-40e8-9484-1fba78e6ed72', '4af086f4-5f6d-4c12-a73e-c6fcc38b8dc1', '9000-12-31 00:00:00', '', 'f', '00000000-0000-0000-0000-0000000003f1', '2021-09-22 05:02:08.674396', '00000000-0000-0000-0000-0000000003f1', '2021-09-22 05:02:08.674399', 0, NULL, 0, '');
INSERT INTO "services"."service_followups" VALUES ('21a129a8-a315-4e12-9e1d-0a83ab41e9bd', '3c23bd83-5829-40e8-9484-1fba78e6ed72', '6fb2195f-c246-4d5b-8173-1b80840c068f', '9000-12-31 00:00:00', '', 'f', '00000000-0000-0000-0000-0000000003f1', '2021-09-22 05:02:08.674452', '00000000-0000-0000-0000-0000000003f1', '2021-09-22 05:02:08.674452', 0, NULL, 0, '');
INSERT INTO "services"."service_followups" VALUES ('b84332e9-d10c-4f7f-84ee-d6c3cea13f87', '3c23bd83-5829-40e8-9484-1fba78e6ed72', '10b53f93-fae2-444f-9976-eeb9172d46e7', '9000-12-31 00:00:00', '', 'f', '00000000-0000-0000-0000-0000000003f1', '2021-09-22 05:02:08.674472', '00000000-0000-0000-0000-0000000003f1', '2021-09-22 05:02:08.674473', 0, NULL, 0, '');
INSERT INTO "services"."service_followups" VALUES ('7d9d727b-f5cb-4bd7-9a27-7eded8f19534', '8deb3a4e-6d37-4ba6-8cbe-7410f6be2bb1', '2f697041-35c2-44d6-9b9f-909544aa2063', '2021-09-22 18:18:03', 'completed', 'f', '00000000-0000-0000-0000-0000000003f1', '2021-09-22 15:56:40.10222', '00000000-0000-0000-0000-0000000003f1', '2021-09-22 18:19:37.167573', 3, NULL, 0, '');
INSERT INTO "services"."service_followups" VALUES ('777a9f4d-d681-470b-a6a3-a244cd3b2a0b', '8deb3a4e-6d37-4ba6-8cbe-7410f6be2bb1', '4af086f4-5f6d-4c12-a73e-c6fcc38b8dc1', '2021-09-22 18:20:24', '', 'f', '00000000-0000-0000-0000-0000000003f1', '2021-09-22 15:56:40.104168', '00000000-0000-0000-0000-0000000003f1', '2021-09-22 18:22:12.82264', 3, NULL, 0, '');
INSERT INTO "services"."service_followups" VALUES ('848282e3-0fa1-4360-ad6b-8ddcd9b078fd', '8deb3a4e-6d37-4ba6-8cbe-7410f6be2bb1', '6fb2195f-c246-4d5b-8173-1b80840c068f', '9000-12-31 00:00:00', '', 'f', '00000000-0000-0000-0000-0000000003f1', '2021-09-22 15:56:40.104212', '00000000-0000-0000-0000-0000000003f1', '2021-09-22 18:22:27.118452', 3, NULL, -61179030, '');
INSERT INTO "services"."service_followups" VALUES ('cd8afae2-6c17-41ac-9895-e3c8e7208796', '8deb3a4e-6d37-4ba6-8cbe-7410f6be2bb1', '10b53f93-fae2-444f-9976-eeb9172d46e7', '9000-12-31 00:00:00', '', 'f', '00000000-0000-0000-0000-0000000003f1', '2021-09-22 15:56:40.10423', '00000000-0000-0000-0000-0000000003f1', '2021-09-22 18:23:28.042283', 3, NULL, -61179030, '.pdf');
INSERT INTO "services"."service_followups" VALUES ('b4cb253b-e18a-46c5-ab26-e40644245999', 'fa01fabc-1e66-4eb6-8813-970703b74cf8', '4af086f4-5f6d-4c12-a73e-c6fcc38b8dc1', '9001-01-01 00:00:00', '', 'f', '00000000-0000-0000-0000-0000000003f1', '2021-09-24 05:57:35.357745', '00000000-0000-0000-0000-0000000003f1', '2021-09-24 05:57:35.357752', 0, NULL, 0, '');
INSERT INTO "services"."service_followups" VALUES ('c84a21e0-5efb-4053-8da7-ef1d90d7c3c7', 'fa01fabc-1e66-4eb6-8813-970703b74cf8', '6fb2195f-c246-4d5b-8173-1b80840c068f', '9001-01-01 00:00:00', '', 'f', '00000000-0000-0000-0000-0000000003f1', '2021-09-24 05:57:35.357816', '00000000-0000-0000-0000-0000000003f1', '2021-09-24 05:57:35.357817', 0, NULL, 0, '');
INSERT INTO "services"."service_followups" VALUES ('dff6f5d4-02c0-4716-b559-217768eb4ff4', 'fa01fabc-1e66-4eb6-8813-970703b74cf8', '10b53f93-fae2-444f-9976-eeb9172d46e7', '9001-01-01 00:00:00', '', 'f', '00000000-0000-0000-0000-0000000003f1', '2021-09-24 05:57:35.357881', '00000000-0000-0000-0000-0000000003f1', '2021-09-24 05:57:35.357882', 0, NULL, 0, '');
INSERT INTO "services"."service_followups" VALUES ('3159fad0-d27b-46db-9598-a6cd6b74365c', 'fa01fabc-1e66-4eb6-8813-970703b74cf8', '2f697041-35c2-44d6-9b9f-909544aa2063', '2021-09-24 05:58:24', '', 'f', '00000000-0000-0000-0000-0000000003f1', '2021-09-24 05:57:35.354417', '00000000-0000-0000-0000-0000000003f1', '2021-09-24 06:01:35.614898', 3, NULL, 0, '');
INSERT INTO "services"."service_followups" VALUES ('8efd3c8e-8d37-41a0-8cc9-51e7a4fbbcfd', '997b05c6-ae37-4b2d-b799-044dd622466a', '2f697041-35c2-44d6-9b9f-909544aa2063', '9001-01-01 00:00:00', '', 'f', '00000000-0000-0000-0000-0000000003f1', '2021-09-28 07:19:03.212168', '00000000-0000-0000-0000-0000000003f1', '2021-09-28 07:19:03.215697', 0, NULL, 0, '');
INSERT INTO "services"."service_followups" VALUES ('c5a589e0-f132-489a-8736-d92ce12749b2', '997b05c6-ae37-4b2d-b799-044dd622466a', '4af086f4-5f6d-4c12-a73e-c6fcc38b8dc1', '9001-01-01 00:00:00', '', 'f', '00000000-0000-0000-0000-0000000003f1', '2021-09-28 07:19:03.299932', '00000000-0000-0000-0000-0000000003f1', '2021-09-28 07:19:03.299939', 0, NULL, 0, '');
INSERT INTO "services"."service_followups" VALUES ('904f136b-c575-4f14-b4a3-de31f7bcd856', '997b05c6-ae37-4b2d-b799-044dd622466a', '6fb2195f-c246-4d5b-8173-1b80840c068f', '9001-01-01 00:00:00', '', 'f', '00000000-0000-0000-0000-0000000003f1', '2021-09-28 07:19:03.300037', '00000000-0000-0000-0000-0000000003f1', '2021-09-28 07:19:03.300038', 0, NULL, 0, '');
INSERT INTO "services"."service_followups" VALUES ('17ed5bac-9491-4e72-a4d8-20b4bf0dedba', '997b05c6-ae37-4b2d-b799-044dd622466a', '10b53f93-fae2-444f-9976-eeb9172d46e7', '9001-01-01 00:00:00', '', 'f', '00000000-0000-0000-0000-0000000003f1', '2021-09-28 07:19:03.300087', '00000000-0000-0000-0000-0000000003f1', '2021-09-28 07:19:03.300088', 0, NULL, 0, '');

-- ----------------------------
-- Table structure for service_requests
-- ----------------------------
DROP TABLE IF EXISTS "services"."service_requests";
CREATE TABLE "services"."service_requests" (
  "id" uuid NOT NULL,
  "tin" varchar(20) COLLATE "pg_catalog"."default" NOT NULL,
  "full_name" varchar(200) COLLATE "pg_catalog"."default" NOT NULL,
  "email" varchar(200) COLLATE "pg_catalog"."default" NOT NULL,
  "mobile" varchar(20) COLLATE "pg_catalog"."default" NOT NULL,
  "tracing_no" varchar(20) COLLATE "pg_catalog"."default" NOT NULL,
  "requesting_user_id" uuid NOT NULL,
  "service_type_id" uuid NOT NULL,
  "request_date" date NOT NULL,
  "invoice_no" varchar(50) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "receipt_no" varchar(50) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "payment_method" int4 NOT NULL DEFAULT 0,
  "request_status" int4 NOT NULL DEFAULT 0,
  "description" text COLLATE "pg_catalog"."default",
  "previous_step" uuid,
  "current_step" uuid NOT NULL,
  "created_by" uuid NOT NULL,
  "create_stamp" timestamp(6) NOT NULL DEFAULT '2021-09-17 13:15:49.127931'::timestamp without time zone,
  "updated_by" uuid NOT NULL,
  "update_stamp" timestamp(6) NOT NULL DEFAULT '2021-09-17 13:15:49.127979'::timestamp without time zone,
  "remark" varchar(255) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "amount" numeric(255) NOT NULL DEFAULT 0,
  "rate_basis" int4 NOT NULL DEFAULT 0,
  "signature" varchar(500) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "extension" varchar(10) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying
)
;

-- ----------------------------
-- Records of service_requests
-- ----------------------------
INSERT INTO "services"."service_requests" VALUES ('8deb3a4e-6d37-4ba6-8cbe-7410f6be2bb1', '00539334111', 'test company one', 'test1@gmail.com', '0911010101', 'RRMRR6AZUL', '00000000-0000-0000-0000-000000000000', '6786fd9b-3916-4a8c-823a-037dfe4cb0aa', '2021-09-22', '26G548P8UM4I', '00124569', 4, 5, NULL, NULL, '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-0000000003f1', '2021-09-22 15:56:40.081972', '00000000-0000-0000-0000-0000000003f1', '2021-09-22 19:35:52.970812', '', 350, 2, '7AXPL', '.pdf');
INSERT INTO "services"."service_requests" VALUES ('3c23bd83-5829-40e8-9484-1fba78e6ed72', '00539334111', 'test company one', 'test1@gmail.com', '0911010101', 'RRMRR6DBHP', '00000000-0000-0000-0000-000000000000', '6786fd9b-3916-4a8c-823a-037dfe4cb0aa', '2021-09-14', '26G52IR2S7EW', '00124567', 3, 5, NULL, NULL, '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-0000000003f1', '2021-09-22 05:02:08.649131', '00000000-0000-0000-0000-0000000003f1', '2021-09-22 13:35:34.301267', '', 150, 1, 'JB98X', '.pdf');
INSERT INTO "services"."service_requests" VALUES ('654ae377-27d9-43e2-a5c7-bda08dca6d19', '00539334111', 'test company one', 'test1@gmail.com', '0911010101', '210921-0001-064-89', '00000000-0000-0000-0000-000000000000', '6786fd9b-3916-4a8c-823a-037dfe4cb0aa', '2021-09-08', '26G52F3HYYZW', '00124568', 3, 5, NULL, NULL, '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-0000000003f1', '2021-09-21 09:03:12.958806', '00000000-0000-0000-0000-0000000003f1', '2021-09-22 13:45:03.63043', '', 150, 1, '10XXQ1', '.pdf');
INSERT INTO "services"."service_requests" VALUES ('fa01fabc-1e66-4eb6-8813-970703b74cf8', '00539334112', 'test company one', 'test1@gmail.com', '0911010101', 'RRN2LZ3AGC', '00000000-0000-0000-0000-000000000000', '6786fd9b-3916-4a8c-823a-037dfe4cb0aa', '2021-09-24', '26G5YD4LUDDI', '00124568', 2, 2, NULL, NULL, '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-0000000003f1', '2021-09-24 05:57:35.325581', '00000000-0000-0000-0000-0000000003f1', '2021-09-24 05:59:57.97783', '', 350, 2, '', '');
INSERT INTO "services"."service_requests" VALUES ('ef4c7b20-929f-4f7c-9116-484dd3a68cd8', '00530001', 'Company 2', 'company2@cc.cc', '09000002', 'RRN2LZ35KV', '00000000-0000-0000-0000-000000000000', '32b80253-8392-4bf4-a65a-65ddee7877dd', '2021-09-24', '26G5YLFXKR1W', '', 0, 3, NULL, NULL, '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-0000000003f1', '2021-09-24 07:10:20.705899', '00000000-0000-0000-0000-0000000003f1', '2021-09-24 07:30:41.235502', '', 500, 1, '', '');
INSERT INTO "services"."service_requests" VALUES ('04da81fd-8c1d-410f-bfd3-5a01300c24fc', '0053933412', 'Company 1', 'company1@cc.cc', '09000001', 'RRN2LZ35IW', '00000000-0000-0000-0000-000000000000', '32b80253-8392-4bf4-a65a-65ddee7877dd', '2021-09-24', '26G5YL8B1A9J', '00123456', 1, 5, NULL, NULL, '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-0000000003f1', '2021-09-24 07:07:05.482275', '00000000-0000-0000-0000-0000000003f1', '2021-09-25 01:16:38.364988', 'sdfsdfs', 500, 1, '163NKT', '.pdf');
INSERT INTO "services"."service_requests" VALUES ('997b05c6-ae37-4b2d-b799-044dd622466a', '00123457894', 'ADFASD', 'test@ec.com', '0911630791', 'RRNPDJMWMY', '00000000-0000-0000-0000-000000000000', '6786fd9b-3916-4a8c-823a-037dfe4cb0aa', '2021-09-28', '26G7QSYPZZUX', '012234', 1, 5, NULL, NULL, '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-0000000003f1', '2021-09-28 07:19:03.148481', '00000000-0000-0000-0000-0000000003f1', '2021-09-28 07:24:59.850739', '', 350, 2, 'LQDJA', '.pdf');
INSERT INTO "services"."service_requests" VALUES ('5c460928-cb29-4e07-a46b-ff71f6c34d54', '014', 'asdf', 'aa@g.com', '1234', 'RRNPDJMRT6', '00000000-0000-0000-0000-000000000000', '3f960d16-ea99-4982-92c4-0a05ac1c9197', '2021-09-29', '', '', 0, 1, NULL, NULL, '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-0000000003f1', '2021-09-28 23:59:38.207763', '00000000-0000-0000-0000-0000000003f1', '2021-09-29 04:22:34.978106', '', 0, 0, '', '');
INSERT INTO "services"."service_requests" VALUES ('cc4431cf-29c8-451f-8186-a70d740a7a45', '00123', 'adf', 'aa@cc.com', '011', 'RRNUTYIY1W', '00000000-0000-0000-0000-000000000000', '3c3e48a8-9f1a-4284-bb3c-10e90757be65', '2021-09-29', '26G87AYDB2FP', '', 0, 2, NULL, NULL, '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-0000000003f1', '2021-09-29 13:45:34.576579', '00000000-0000-0000-0000-0000000003f1', '2021-09-29 13:46:08.425738', '', 8754, 3, '', '');

-- ----------------------------
-- Table structure for service_requirements
-- ----------------------------
DROP TABLE IF EXISTS "services"."service_requirements";
CREATE TABLE "services"."service_requirements" (
  "id" uuid NOT NULL,
  "service_type_id" uuid NOT NULL,
  "requirement_type" int4 NOT NULL,
  "order_no" varchar(10) COLLATE "pg_catalog"."default" NOT NULL,
  "name_eng" varchar(100) COLLATE "pg_catalog"."default" NOT NULL,
  "name_local" text COLLATE "pg_catalog"."default",
  "description_eng" varchar(100) COLLATE "pg_catalog"."default" NOT NULL,
  "description_local" text COLLATE "pg_catalog"."default" NOT NULL,
  "input_method" int4 NOT NULL,
  "supported_formats" varchar(100) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "upper_limit" int4 NOT NULL,
  "lower_limit" int4 NOT NULL,
  "lu_source" jsonb,
  "label" varchar(100) COLLATE "pg_catalog"."default" NOT NULL DEFAULT ''::character varying,
  "status" int4 NOT NULL DEFAULT 0
)
;

-- ----------------------------
-- Records of service_requirements
-- ----------------------------
INSERT INTO "services"."service_requirements" VALUES ('015374ab-5cda-4bc3-b0bb-c635d122ef60', '6786fd9b-3916-4a8c-823a-037dfe4cb0aa', 0, '060', 'Bill of Loading', 'ቢል ኦፍ ሎዲንግ', 'Bill of loading / air way bill', 'ቢል ኦፍ ሎዲንግ', 1, '.pdf, .png, .jpg', 500, 1, '[]', 'Bill of Loading', 1);
INSERT INTO "services"."service_requirements" VALUES ('87913190-1dc3-4e8d-950e-dd8cd0d77bb2', '6786fd9b-3916-4a8c-823a-037dfe4cb0aa', 5, '020', 'Company Name', 'የድርጅቱ ስም', 'Company Name', 'የድርጅቱ ስም', 5, '', 100, 3, '[]', 'Company Name [ግከመቁ]', 1);
INSERT INTO "services"."service_requirements" VALUES ('b1d243c9-7008-4f73-85e0-b1b050d39794', '6786fd9b-3916-4a8c-823a-037dfe4cb0aa', 0, '050', 'Bank Permit', 'የባንክ ፈቃድ', 'Bank Permit with Bank Stamp', 'የባንኩ ማሕተም ያረፈበት የባንክ ፈቃድ ', 1, '.pdf, .png, .jpg', 500, 0, '[]', 'Bank Permit', 1);
INSERT INTO "services"."service_requirements" VALUES ('bb0d9af1-7495-40fd-9baa-ef0d046c7298', '6786fd9b-3916-4a8c-823a-037dfe4cb0aa', 6, '030', 'Email', 'ኢሜል', 'Email address', 'የኢሜል አድራሻ', 9, '', 50, 4, '[]', 'Email [ኢሜል]', 1);
INSERT INTO "services"."service_requirements" VALUES ('e1554f11-44fa-4b8e-beb2-6d19889294fd', '6786fd9b-3916-4a8c-823a-037dfe4cb0aa', 7, '040', 'MobileNo', 'ሞባይል', 'MobileNo Number', 'የሞባይል ቁጥር ', 5, '', 12, 10, '[]', 'MobileNo [ሞባይል]', 1);
INSERT INTO "services"."service_requirements" VALUES ('f726fbec-5b7d-4af2-a39c-3e0c7be2b7ed', '6786fd9b-3916-4a8c-823a-037dfe4cb0aa', 1, '010', 'TIN', 'ግከመቁ', 'Tax Identification Number', 'የግብር ከፋይ መለያ ቁጥር ', 5, ' ', 11, 11, '[]', 'TIN[ታመቁ]', 1);
INSERT INTO "services"."service_requirements" VALUES ('05b9eec3-a224-4a50-a8aa-6378ebdcad2e', 'cc284bbc-d668-4836-bc66-e913e4e444cd', 1, '000', 'TIN', 'ግከመቁ', 'Tax Identification Number', 'የግብር ከፋይ መለያ ቁጥር ', 5, '', 11, 11, NULL, 'TIN [ግከመቁ]', 1);
INSERT INTO "services"."service_requirements" VALUES ('4d079065-c787-4174-aa96-4c1492865878', 'cc284bbc-d668-4836-bc66-e913e4e444cd', 5, '010', 'Company Name', 'ግከመቁ', 'Company Name', 'የግብር ከፋይ መለያ ቁጥር ', 5, '', 100, 3, NULL, 'Company Name [ግከመቁ]', 1);
INSERT INTO "services"."service_requirements" VALUES ('ac935ffc-f891-4ec1-9aae-f30a01694bf6', 'cc284bbc-d668-4836-bc66-e913e4e444cd', 6, '020', 'Email', 'ኢሜል', 'Email address', 'የኢሜል አድራሻ', 9, '', 50, 4, NULL, 'Email [ኢሜል]', 1);
INSERT INTO "services"."service_requirements" VALUES ('3c643340-a663-426e-878d-1a44c729bfb1', 'cc284bbc-d668-4836-bc66-e913e4e444cd', 7, '030', 'MobileNo', 'ሞባይል', 'MobileNo Number', 'የሞባይል ቁጥር ', 5, '', 12, 10, NULL, 'MobileNo [ሞባይል]', 1);
INSERT INTO "services"."service_requirements" VALUES ('2930d758-566c-4bca-b2c1-0cea552adfd1', 'cc284bbc-d668-4836-bc66-e913e4e444cd', 0, '040', 'Trade License', 'የንግድ ምዝገባ', 'Attach a valid trade license', 'የንግድ ምዝገባ ምስክር ወረቀት አያይዝ', 1, '.pdf', 0, 0, NULL, 'Trade License', 1);
INSERT INTO "services"."service_requirements" VALUES ('a9593823-a4ea-4485-a7b2-d4cdef92617f', 'cc284bbc-d668-4836-bc66-e913e4e444cd', 0, '050', 'Purpose', NULL, 'Describe the business sector and purpose ', 'በምን ዘርፍ እና ለምን አይነት ስራ እንደሚፈልጉ ይግለጹ', 1, '.pdf', 0, 0, NULL, 'Purpose', 1);
INSERT INTO "services"."service_requirements" VALUES ('fc6b6ec0-77fa-4d15-aa47-da4e634c3e31', 'cc284bbc-d668-4836-bc66-e913e4e444cd', 0, '060', 'Deadline (min. 14 days after payment)', '', 'Deadline (min. 14 days after payment)', '', 4, '', 180, 14, NULL, 'Deadline', 1);
INSERT INTO "services"."service_requirements" VALUES ('1732c3c9-14de-4a11-9667-d1d26f0ec928', '32b80253-8392-4bf4-a65a-65ddee7877dd', 5, '010', 'Company Name', 'ግከመቁ', 'Company Name', 'የግብር ከፋይ መለያ ቁጥር ', 5, '', 100, 3, '[]', 'Company Name [ግከመቁ]', 1);
INSERT INTO "services"."service_requirements" VALUES ('b9a0d523-24c0-4e45-9480-cb0e2945f63c', '32b80253-8392-4bf4-a65a-65ddee7877dd', 7, '030', 'MobileNo', 'ሞባይል', 'MobileNo Number', 'የሞባይል ቁጥር ', 5, '', 12, 10, '[]', 'MobileNo [ሞባይል]', 1);
INSERT INTO "services"."service_requirements" VALUES ('f26b2d16-f956-49af-b366-f0f0444db289', '32b80253-8392-4bf4-a65a-65ddee7877dd', 6, '020', 'Email', 'ኢሜል', 'Email address', 'የኢሜል አድራሻ', 9, '', 50, 4, '[]', 'Email [ኢሜል]', 1);
INSERT INTO "services"."service_requirements" VALUES ('fe3186ba-4593-478e-9d72-6de5e91249e4', '32b80253-8392-4bf4-a65a-65ddee7877dd', 1, '000', 'TIN', 'ግከመቁ', 'Tax Identification Number', 'የግብር ከፋይ መለያ ቁጥር ', 5, '', 11, 11, '[]', 'TIN [ግከመቁ]', 1);
INSERT INTO "services"."service_requirements" VALUES ('29d11f66-e285-4946-94d9-5bf44688db2a', '3f960d16-ea99-4982-92c4-0a05ac1c9197', 5, '010', 'Company Name', 'ግከመቁ', 'Company Name', 'የግብር ከፋይ መለያ ቁጥር ', 5, '', 100, 3, '[]', 'Company Name [ግከመቁ]', 1);
INSERT INTO "services"."service_requirements" VALUES ('4fe19dd8-c5f4-4df5-86d6-a82d4b673c2e', '3f960d16-ea99-4982-92c4-0a05ac1c9197', 0, '040', 'dddd', 'dddd', 'ddd', 'ddd', 1, '.pdf, .png, .jpg', 0, 0, '[]', 'ddd', 1);
INSERT INTO "services"."service_requirements" VALUES ('9ee8bb55-b5f4-4227-93e7-5af90eb59e3a', '3f960d16-ea99-4982-92c4-0a05ac1c9197', 7, '030', 'MobileNo', 'ሞባይል', 'MobileNo Number', 'የሞባይል ቁጥር ', 5, '', 12, 10, '[]', 'MobileNo [ሞባይል]', 1);
INSERT INTO "services"."service_requirements" VALUES ('b10e736a-cdc9-4ff4-8088-12a9cc30b422', '3f960d16-ea99-4982-92c4-0a05ac1c9197', 1, '000', 'TIN', 'ግከመቁ', 'Tax Identification Number', 'የግብር ከፋይ መለያ ቁጥር ', 5, '', 11, 11, '[]', 'TIN [ግከመቁ]', 1);
INSERT INTO "services"."service_requirements" VALUES ('ee4f82ec-f531-408c-aa38-8b6f0a8ac115', '3f960d16-ea99-4982-92c4-0a05ac1c9197', 6, '020', 'Email', 'ኢሜል', 'Email address', 'የኢሜል አድራሻ', 9, '', 50, 4, '[]', 'Email [ኢሜል]', 1);
INSERT INTO "services"."service_requirements" VALUES ('28ae8734-8bbd-47d3-8c55-cf454436f443', '3c3e48a8-9f1a-4284-bb3c-10e90757be65', 7, '030', 'MobileNo', 'ሞባይል', 'MobileNo Number', 'የሞባይል ቁጥር ', 5, '', 12, 10, '[]', 'MobileNo [ሞባይል]', 1);
INSERT INTO "services"."service_requirements" VALUES ('4d9038b1-e58a-4fae-b048-5112543efbcf', '3c3e48a8-9f1a-4284-bb3c-10e90757be65', 0, '040', 'Disputed amount', 'Disputed amount', 'Disputed amount', 'Disputed amount', 3, '', 5000, 1, '[]', 'Disputed amount', 1);
INSERT INTO "services"."service_requirements" VALUES ('7d64ce97-d9eb-4053-823c-589d1dfc3bb0', '3c3e48a8-9f1a-4284-bb3c-10e90757be65', 5, '010', 'Company Name', 'ግከመቁ', 'Company Name', 'የግብር ከፋይ መለያ ቁጥር ', 5, '', 100, 3, '[]', 'Company Name [ግከመቁ]', 1);
INSERT INTO "services"."service_requirements" VALUES ('7e4a46da-b2d1-4976-9d86-bb1e8354a194', '3c3e48a8-9f1a-4284-bb3c-10e90757be65', 1, '000', 'TIN', 'ግከመቁ', 'Tax Identification Number', 'የግብር ከፋይ መለያ ቁጥር ', 5, '', 11, 11, '[]', 'TIN [ግከመቁ]', 1);
INSERT INTO "services"."service_requirements" VALUES ('9d02254a-2113-457c-9d09-44f6a08a6a18', '3c3e48a8-9f1a-4284-bb3c-10e90757be65', 0, '050', 'Plaintiff', '', ' Plaintiff', 'Plaintiff', 5, '', 100, 0, '[]', 'Plaintiff', 1);
INSERT INTO "services"."service_requirements" VALUES ('d9370fd2-d2de-40f6-ad43-48ab4b7f5fcf', '3c3e48a8-9f1a-4284-bb3c-10e90757be65', 0, '060', 'Summary', '', 'Summary of the dispute', 'የልዩነቱ አጭር መግለጫ', 6, '', 250, 0, '[]', 'Summary', 1);
INSERT INTO "services"."service_requirements" VALUES ('f2120d91-6a43-4789-a23d-827e92b62242', '3c3e48a8-9f1a-4284-bb3c-10e90757be65', 6, '020', 'Email', 'ኢሜል', 'Email address', 'የኢሜል አድራሻ', 9, '', 50, 4, '[]', 'Email [ኢሜል]', 1);

-- ----------------------------
-- Table structure for service_responses
-- ----------------------------
DROP TABLE IF EXISTS "services"."service_responses";
CREATE TABLE "services"."service_responses" (
  "id" uuid NOT NULL,
  "service_request_id" uuid NOT NULL,
  "requirement_id" uuid NOT NULL,
  "text_response" text COLLATE "pg_catalog"."default",
  "date_response" date,
  "date_submitted" timestamp(6),
  "reviewed_by" uuid,
  "date_reviewed" timestamp(6),
  "remark" text COLLATE "pg_catalog"."default" NOT NULL,
  "review_status" int4 NOT NULL DEFAULT 0,
  "decimal_response" numeric(255),
  "int_response" int4
)
;

-- ----------------------------
-- Records of service_responses
-- ----------------------------
INSERT INTO "services"."service_responses" VALUES ('36a1314d-5b07-44dd-b6de-e853f647b6ef', '8deb3a4e-6d37-4ba6-8cbe-7410f6be2bb1', 'bb0d9af1-7495-40fd-9baa-ef0d046c7298', 'test1@gmail.com', NULL, NULL, '00000000-0000-0000-0000-0000000003f1', '2021-09-22 19:34:48.996026', '', 2, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('b6b12e6d-0c5b-4e69-ac20-96be8d32e20f', '8deb3a4e-6d37-4ba6-8cbe-7410f6be2bb1', 'e1554f11-44fa-4b8e-beb2-6d19889294fd', '0911010101', NULL, NULL, '00000000-0000-0000-0000-0000000003f1', '2021-09-22 19:34:58.525421', '', 2, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('4ac0081a-5457-417a-8a4f-803b76cb775d', '8deb3a4e-6d37-4ba6-8cbe-7410f6be2bb1', 'b1d243c9-7008-4f73-85e0-b1b050d39794', '.pdf', NULL, NULL, '00000000-0000-0000-0000-0000000003f1', '2021-09-22 19:35:06.395814', '', 2, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('bc8df018-344c-43f1-9a95-e210bc3dc086', '8deb3a4e-6d37-4ba6-8cbe-7410f6be2bb1', '015374ab-5cda-4bc3-b0bb-c635d122ef60', '.pdf', NULL, NULL, '00000000-0000-0000-0000-0000000003f1', '2021-09-22 19:35:15.103433', '', 2, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('ea868c56-d2ce-458a-93cf-29d7350db40e', '654ae377-27d9-43e2-a5c7-bda08dca6d19', 'f726fbec-5b7d-4af2-a39c-3e0c7be2b7ed', '00539334111', NULL, NULL, '00000000-0000-0000-0000-0000000003f1', '2021-09-21 18:46:20.757068', '', 2, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('8cad8b36-1830-42ba-9e83-9a22b843bce1', '654ae377-27d9-43e2-a5c7-bda08dca6d19', '87913190-1dc3-4e8d-950e-dd8cd0d77bb2', 'test company one', NULL, NULL, '00000000-0000-0000-0000-0000000003f1', '2021-09-21 18:46:57.847542', '', 2, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('965b32ae-9f65-41c0-afee-cc80756cb6d7', '654ae377-27d9-43e2-a5c7-bda08dca6d19', 'bb0d9af1-7495-40fd-9baa-ef0d046c7298', 'test1@gmail.com', NULL, NULL, '00000000-0000-0000-0000-0000000003f1', '2021-09-21 18:47:07.477327', '', 2, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('f695436d-8a6f-4cdd-9381-fcf2f1978dd6', '654ae377-27d9-43e2-a5c7-bda08dca6d19', 'e1554f11-44fa-4b8e-beb2-6d19889294fd', '0911010101', NULL, NULL, '00000000-0000-0000-0000-0000000003f1', '2021-09-21 18:47:18.943453', '', 2, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('299b9340-287f-4da2-b10c-b76a0fb6ccd5', '3c23bd83-5829-40e8-9484-1fba78e6ed72', 'f726fbec-5b7d-4af2-a39c-3e0c7be2b7ed', '00539334111', NULL, NULL, '00000000-0000-0000-0000-0000000003f1', '2021-09-22 05:39:03.231054', '', 2, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('1a6253b1-179d-4312-a41a-63b5339e2705', '3c23bd83-5829-40e8-9484-1fba78e6ed72', '87913190-1dc3-4e8d-950e-dd8cd0d77bb2', 'test company one', NULL, NULL, '00000000-0000-0000-0000-0000000003f1', '2021-09-22 05:39:12.033869', '', 2, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('644e57c0-63d7-4311-ad30-880bb4e27639', '3c23bd83-5829-40e8-9484-1fba78e6ed72', 'bb0d9af1-7495-40fd-9baa-ef0d046c7298', 'test1@gmail.com', NULL, NULL, '00000000-0000-0000-0000-0000000003f1', '2021-09-22 07:27:22.959573', '', 4, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('42671f2a-cd61-4d43-b64c-24c8d0395f08', '3c23bd83-5829-40e8-9484-1fba78e6ed72', 'e1554f11-44fa-4b8e-beb2-6d19889294fd', '0911010101', NULL, NULL, '00000000-0000-0000-0000-0000000003f1', '2021-09-22 07:28:29.307246', '', 2, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('3ff60c3e-dff9-4b77-84aa-19fb6106e16b', '3c23bd83-5829-40e8-9484-1fba78e6ed72', 'b1d243c9-7008-4f73-85e0-b1b050d39794', '.pdf', NULL, NULL, '00000000-0000-0000-0000-0000000003f1', '2021-09-22 07:28:37.764033', '', 2, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('0c8f1791-45be-4041-837a-c10cc4922c15', '3c23bd83-5829-40e8-9484-1fba78e6ed72', '015374ab-5cda-4bc3-b0bb-c635d122ef60', '.pdf', NULL, NULL, '00000000-0000-0000-0000-0000000003f1', '2021-09-22 07:29:43.973096', '', 2, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('84f53958-41c9-4b89-96bf-2fef57aca990', '654ae377-27d9-43e2-a5c7-bda08dca6d19', 'b1d243c9-7008-4f73-85e0-b1b050d39794', '.sql', NULL, NULL, '00000000-0000-0000-0000-0000000003f1', '2021-09-22 13:43:41.862336', 'Not valid bank permit', 2, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('b7b24d59-ba33-4df0-87ac-d005106fbbb3', '654ae377-27d9-43e2-a5c7-bda08dca6d19', '015374ab-5cda-4bc3-b0bb-c635d122ef60', '.pdf', NULL, NULL, '00000000-0000-0000-0000-0000000003f1', '2021-09-22 13:43:51.752602', 'Not valid Bill of loading', 2, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('4e827556-b359-4155-9cfe-71e987d59441', '8deb3a4e-6d37-4ba6-8cbe-7410f6be2bb1', 'f726fbec-5b7d-4af2-a39c-3e0c7be2b7ed', '00539334111', NULL, NULL, '00000000-0000-0000-0000-0000000003f1', '2021-09-22 19:32:14.656559', '', 2, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('21c12198-7b31-457c-8b68-ba31dc60aa2d', '8deb3a4e-6d37-4ba6-8cbe-7410f6be2bb1', '87913190-1dc3-4e8d-950e-dd8cd0d77bb2', 'test company one', NULL, NULL, '00000000-0000-0000-0000-0000000003f1', '2021-09-22 19:34:41.817362', '', 2, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('1e995cc9-c004-49dc-8a8b-64f987ca920d', 'fa01fabc-1e66-4eb6-8813-970703b74cf8', '015374ab-5cda-4bc3-b0bb-c635d122ef60', '.pdf', NULL, NULL, NULL, NULL, '', 1, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('526d8e19-ad06-41ea-aaaf-b67390111de1', 'cc4431cf-29c8-451f-8186-a70d740a7a45', 'd9370fd2-d2de-40f6-ad43-48ab4b7f5fcf', 'fdsfasdf 
dfadsf adfsdf   ewrwasdf jkasdruewijadf
asdfaksdf', NULL, NULL, NULL, NULL, '', 1, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('4df0df31-40f1-4fb8-82df-943ce95377f1', 'fa01fabc-1e66-4eb6-8813-970703b74cf8', 'e1554f11-44fa-4b8e-beb2-6d19889294fd', '0911010101', NULL, NULL, NULL, NULL, '', 1, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('afa667cf-f816-4c31-9bb5-bd278e3a89e0', 'fa01fabc-1e66-4eb6-8813-970703b74cf8', 'bb0d9af1-7495-40fd-9baa-ef0d046c7298', 'test1@gmail.com', NULL, NULL, NULL, NULL, '', 1, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('347a08ff-3981-4b4b-8534-4c922623ba3e', '997b05c6-ae37-4b2d-b799-044dd622466a', 'bb0d9af1-7495-40fd-9baa-ef0d046c7298', 'test@ec.com', NULL, NULL, '00000000-0000-0000-0000-0000000003f1', '2021-09-28 07:23:41.553064', '', 2, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('b7bd6930-bed4-4a46-b595-f47311b624f7', 'fa01fabc-1e66-4eb6-8813-970703b74cf8', '87913190-1dc3-4e8d-950e-dd8cd0d77bb2', 'test company one', NULL, NULL, NULL, NULL, '', 1, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('b389110d-a894-4fd4-850c-801c723075eb', 'fa01fabc-1e66-4eb6-8813-970703b74cf8', 'f726fbec-5b7d-4af2-a39c-3e0c7be2b7ed', '00539334112', NULL, NULL, '00000000-0000-0000-0000-0000000003f1', '2021-09-24 06:00:59.374442', '', 2, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('1e79f10f-3bd9-404c-8b10-3da4aa0f2c84', '997b05c6-ae37-4b2d-b799-044dd622466a', 'e1554f11-44fa-4b8e-beb2-6d19889294fd', '0911630791', NULL, NULL, '00000000-0000-0000-0000-0000000003f1', '2021-09-28 07:23:50.190926', '', 2, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('9f75491c-9e12-4784-9644-ac6221b3c7d2', '997b05c6-ae37-4b2d-b799-044dd622466a', 'b1d243c9-7008-4f73-85e0-b1b050d39794', '.pdf', NULL, NULL, '00000000-0000-0000-0000-0000000003f1', '2021-09-28 07:24:11.421041', '', 2, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('b98c12bd-00ba-4e80-b37c-f72572e70e5e', '997b05c6-ae37-4b2d-b799-044dd622466a', '015374ab-5cda-4bc3-b0bb-c635d122ef60', '.pdf', NULL, NULL, '00000000-0000-0000-0000-0000000003f1', '2021-09-28 07:24:23.590235', '', 2, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('36048458-c585-49c3-8abf-e463ee5646f3', 'fa01fabc-1e66-4eb6-8813-970703b74cf8', 'b1d243c9-7008-4f73-85e0-b1b050d39794', '.pdf', NULL, NULL, '00000000-0000-0000-0000-0000000003f1', '2021-09-28 07:41:56.898946', 'Not Clear. Already used in another request', 3, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('2d79c34c-f6c0-4bc3-83f9-0d44e0e683e9', 'ef4c7b20-929f-4f7c-9116-484dd3a68cd8', 'fe3186ba-4593-478e-9d72-6de5e91249e4', '00530001', NULL, NULL, NULL, NULL, '', 1, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('3c5294cb-9ff2-458c-97ac-93d208d330c9', 'ef4c7b20-929f-4f7c-9116-484dd3a68cd8', '1732c3c9-14de-4a11-9667-d1d26f0ec928', 'Company 2', NULL, NULL, NULL, NULL, '', 1, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('5249e095-4fbf-49a2-8a3b-a6e140b03a2e', 'ef4c7b20-929f-4f7c-9116-484dd3a68cd8', 'f26b2d16-f956-49af-b366-f0f0444db289', 'company2@cc.cc', NULL, NULL, NULL, NULL, '', 1, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('9e770fcf-0c77-4d82-ad4e-7ab4f814a099', 'ef4c7b20-929f-4f7c-9116-484dd3a68cd8', 'b9a0d523-24c0-4e45-9480-cb0e2945f63c', '09000002', NULL, NULL, NULL, NULL, '', 1, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('df0200b9-61ef-48aa-bc84-08d1465a592b', '04da81fd-8c1d-410f-bfd3-5a01300c24fc', 'fe3186ba-4593-478e-9d72-6de5e91249e4', '0053933412', NULL, NULL, '00000000-0000-0000-0000-0000000003f1', '2021-09-25 01:07:15.762356', '', 2, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('f0be2901-ea87-4c33-bea4-bef0e776785c', '04da81fd-8c1d-410f-bfd3-5a01300c24fc', '1732c3c9-14de-4a11-9667-d1d26f0ec928', 'Company 1', NULL, NULL, '00000000-0000-0000-0000-0000000003f1', '2021-09-25 01:07:23.360479', '', 2, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('1f400851-439a-4465-98f9-9f83d8f16776', '04da81fd-8c1d-410f-bfd3-5a01300c24fc', 'f26b2d16-f956-49af-b366-f0f0444db289', 'company1@cc.cc', NULL, NULL, '00000000-0000-0000-0000-0000000003f1', '2021-09-25 01:07:30.279149', '', 2, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('22238cce-5533-4771-828e-dc2a98bfc1d1', '04da81fd-8c1d-410f-bfd3-5a01300c24fc', 'b9a0d523-24c0-4e45-9480-cb0e2945f63c', '09000001', NULL, NULL, '00000000-0000-0000-0000-0000000003f1', '2021-09-25 01:07:36.908228', '', 2, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('84d34cb5-c199-457a-a0a4-ab4131e0aa64', 'cc4431cf-29c8-451f-8186-a70d740a7a45', '9d02254a-2113-457c-9d09-44f6a08a6a18', 'dfdfd', NULL, NULL, NULL, NULL, '', 1, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('a8e7631e-5b53-4206-8315-d0b2d5a11dd2', 'cc4431cf-29c8-451f-8186-a70d740a7a45', '7e4a46da-b2d1-4976-9d86-bb1e8354a194', '00123', NULL, NULL, NULL, NULL, '', 1, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('b16f4cfd-1738-4a25-b978-03524e7ab9b0', 'cc4431cf-29c8-451f-8186-a70d740a7a45', '7d64ce97-d9eb-4053-823c-589d1dfc3bb0', 'adf', NULL, NULL, NULL, NULL, '', 1, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('c22f0487-9640-4c37-9fe1-ca524eeba906', 'cc4431cf-29c8-451f-8186-a70d740a7a45', '4d9038b1-e58a-4fae-b048-5112543efbcf', NULL, NULL, NULL, NULL, NULL, '', 1, 45000, NULL);
INSERT INTO "services"."service_responses" VALUES ('2fe9af28-c67e-4a79-99fd-02428be316d4', '997b05c6-ae37-4b2d-b799-044dd622466a', 'f726fbec-5b7d-4af2-a39c-3e0c7be2b7ed', '00123457894', NULL, NULL, '00000000-0000-0000-0000-0000000003f1', '2021-09-28 07:23:25.502068', '', 2, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('78900526-bd3b-4a94-b1b2-a17ac83d247a', '997b05c6-ae37-4b2d-b799-044dd622466a', '87913190-1dc3-4e8d-950e-dd8cd0d77bb2', 'ADFASD', NULL, NULL, '00000000-0000-0000-0000-0000000003f1', '2021-09-28 07:23:33.948882', '', 2, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('23289cdb-4227-4e70-bfc7-39c3d3dee07d', '5c460928-cb29-4e07-a46b-ff71f6c34d54', '9ee8bb55-b5f4-4227-93e7-5af90eb59e3a', '1234', NULL, NULL, NULL, NULL, '', 1, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('52ef1f6d-1694-4c5a-a311-139455b48993', '5c460928-cb29-4e07-a46b-ff71f6c34d54', '4fe19dd8-c5f4-4df5-86d6-a82d4b673c2e', '.pdf', NULL, NULL, NULL, NULL, '', 1, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('af2f8eba-409a-4243-83e3-a6da984af681', '5c460928-cb29-4e07-a46b-ff71f6c34d54', 'b10e736a-cdc9-4ff4-8088-12a9cc30b422', '014', NULL, NULL, NULL, NULL, '', 1, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('d0cbe55d-9452-4250-b0fc-62fe0fde4194', '5c460928-cb29-4e07-a46b-ff71f6c34d54', 'ee4f82ec-f531-408c-aa38-8b6f0a8ac115', 'aa@g.com', NULL, NULL, NULL, NULL, '', 1, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('ef057177-da46-4583-9699-5ae2f03d5982', '5c460928-cb29-4e07-a46b-ff71f6c34d54', '29d11f66-e285-4946-94d9-5bf44688db2a', 'asdf', NULL, NULL, NULL, NULL, '', 1, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('329aa1d2-5211-4ff4-9f26-cab11dd21d4e', 'cc4431cf-29c8-451f-8186-a70d740a7a45', 'f2120d91-6a43-4789-a23d-827e92b62242', 'aa@cc.com', NULL, NULL, NULL, NULL, '', 1, NULL, NULL);
INSERT INTO "services"."service_responses" VALUES ('37ab2e09-59d8-4112-9400-f49f9e7a5d3c', 'cc4431cf-29c8-451f-8186-a70d740a7a45', '28ae8734-8bbd-47d3-8c55-cf454436f443', '011', NULL, NULL, NULL, NULL, '', 1, NULL, NULL);

-- ----------------------------
-- Table structure for service_type_tasks
-- ----------------------------
DROP TABLE IF EXISTS "services"."service_type_tasks";
CREATE TABLE "services"."service_type_tasks" (
  "id" uuid NOT NULL,
  "service_type_id" uuid NOT NULL,
  "order_no" varchar(10) COLLATE "pg_catalog"."default" NOT NULL,
  "name_eng" varchar(100) COLLATE "pg_catalog"."default" NOT NULL,
  "name_local" text COLLATE "pg_catalog"."default" NOT NULL,
  "description_eng" text COLLATE "pg_catalog"."default" NOT NULL,
  "description_local" text COLLATE "pg_catalog"."default",
  "status" int4 NOT NULL
)
;

-- ----------------------------
-- Records of service_type_tasks
-- ----------------------------
INSERT INTO "services"."service_type_tasks" VALUES ('2f697041-35c2-44d6-9b9f-909544aa2063', '6786fd9b-3916-4a8c-823a-037dfe4cb0aa', '010', 'Generate Invoice ', 'ክፍያ መጠየቂያ ማዘጋጀት', 'After checking Membership status, generate correct invoice', 'ጠያቂው አባል መሆኑን አጣርቶ ተገቢውን የክፍያ መጠየቂያ ማዘጋጀት', 1);
INSERT INTO "services"."service_type_tasks" VALUES ('4af086f4-5f6d-4c12-a73e-c6fcc38b8dc1', '6786fd9b-3916-4a8c-823a-037dfe4cb0aa', '020', 'Verify Payment', 'ክፍያን ማጣራት', 'Wait till payment is made and verify ', 'ክፍያ እስኪፈጸም ጠብቆ ማጣራት', 1);
INSERT INTO "services"."service_type_tasks" VALUES ('6fb2195f-c246-4d5b-8173-1b80840c068f', '6786fd9b-3916-4a8c-823a-037dfe4cb0aa', '030', 'verify document', 'መረጃ ማጣራት', 'verify the documents'' provided', 'የቀረበውን መረጃ ማጣራት', 1);
INSERT INTO "services"."service_type_tasks" VALUES ('10b53f93-fae2-444f-9976-eeb9172d46e7', '6786fd9b-3916-4a8c-823a-037dfe4cb0aa', '040', 'Generate Authentication code', 'ማረጋገጫ ማዘጋጀት', 'Generate Authentication code', 'ማረጋገጫ ማዘጋጀት', 1);
INSERT INTO "services"."service_type_tasks" VALUES ('ca948bc7-9b36-4ac0-90af-6c4da8f9c424', 'cc284bbc-d668-4836-bc66-e913e4e444cd', '010', 'Generate Invoice ', 'ክፍያ መጠየቂያ ማዘጋጀት', 'Generate Invoice ', 'ክፍያ መጠየቂያ ማዘጋጀት', 1);
INSERT INTO "services"."service_type_tasks" VALUES ('5e220362-2b1a-4dde-93cb-bb81e259a361', 'cc284bbc-d668-4836-bc66-e913e4e444cd', '030', 'Prepare List', 'ዝርዝር ማዘጋጀት', 'Prepare Document', 'ዝርዝር ማዘጋጀት', 1);
INSERT INTO "services"."service_type_tasks" VALUES ('7a298838-753f-4ae4-8ccb-37e7370f693a', 'cc284bbc-d668-4836-bc66-e913e4e444cd', '040', 'Send List', 'ዝርዝሩን መላክ', 'Send Document', 'ዝርዝሩን መላክ', 1);
INSERT INTO "services"."service_type_tasks" VALUES ('4a321399-4bf2-44b7-9134-1055797db2e6', 'cc284bbc-d668-4836-bc66-e913e4e444cd', '020', 'verify document', 'መረጃ ማጣራት', 'Verify Document', 'መረጃ ማጣራት', 1);
INSERT INTO "services"."service_type_tasks" VALUES ('5d019fea-af62-4cb5-8b94-f6d0d674163a', '3f960d16-ea99-4982-92c4-0a05ac1c9197', '1', 'Generate Invoice ', 'ክፍያ መጠየቂያ ማዘጋጀት', 'Generate Invoice ', 'ክፍያ መጠየቂያ ማዘጋጀት', 0);
INSERT INTO "services"."service_type_tasks" VALUES ('1af647f0-dd60-4182-8d9a-68ddd25d5c54', '3f960d16-ea99-4982-92c4-0a05ac1c9197', '2', 'Verify Document', 'መረጃ ማጣራት', 'Verify Document', 'መረጃ ማጣራት', 0);
INSERT INTO "services"."service_type_tasks" VALUES ('8aca93b5-dd0e-4dc0-bff2-ebdd8683c61f', '3f960d16-ea99-4982-92c4-0a05ac1c9197', '3', 'Prepare Document', 'ዝርዝር ማዘጋጀት', 'Prepare Document', 'ዝርዝር ማዘጋጀት', 0);
INSERT INTO "services"."service_type_tasks" VALUES ('bfbf2062-cdc4-4ee8-9a6a-7a47a906b0de', '3f960d16-ea99-4982-92c4-0a05ac1c9197', '4', 'Send Document', 'ዝርዝሩን መላክ', 'Send Document', 'ዝርዝሩን መላክ', 0);

-- ----------------------------
-- Table structure for service_types
-- ----------------------------
DROP TABLE IF EXISTS "services"."service_types";
CREATE TABLE "services"."service_types" (
  "Id" uuid NOT NULL,
  "ServiceOwner" int4 NOT NULL,
  "name_eng" varchar(100) COLLATE "pg_catalog"."default" NOT NULL,
  "name_local" varchar(1000) COLLATE "pg_catalog"."default",
  "description_eng" text COLLATE "pg_catalog"."default",
  "description_local" text COLLATE "pg_catalog"."default",
  "member_service_charge" numeric NOT NULL DEFAULT 0.0,
  "public_service_charge" numeric NOT NULL DEFAULT 0.0,
  "created_by" uuid NOT NULL,
  "create_stamp" timestamp(6) NOT NULL DEFAULT '2021-09-17 13:15:49.11038'::timestamp without time zone,
  "updated_by" uuid NOT NULL,
  "update_stamp" timestamp(6) NOT NULL DEFAULT '2021-09-17 13:15:49.110417'::timestamp without time zone,
  "status" int4 NOT NULL,
  "rate_basis" int4 DEFAULT 0
)
;

-- ----------------------------
-- Records of service_types
-- ----------------------------
INSERT INTO "services"."service_types" VALUES ('6786fd9b-3916-4a8c-823a-037dfe4cb0aa', 64, 'packing list', 'ፓኪንግ ዝርዝር ', 'To get this service     ', '     የማሸጊያ ዝርዝር      ', 150, 350, '00000000-0000-0000-0000-0000000003f1', '2021-09-18 11:37:03.138419', '00000000-0000-0000-0000-0000000003f1', '2021-09-21 07:29:05.525483', 1, 0);
INSERT INTO "services"."service_types" VALUES ('32b80253-8392-4bf4-a65a-65ddee7877dd', 4, 'Joint Venture partner', NULL, 'Looking for partner for a joint venture ', NULL, 500, 500, '00000000-0000-0000-0000-0000000003f1', '2021-09-23 07:09:01.383148', '00000000-0000-0000-0000-0000000003f1', '2021-09-23 07:09:01.383148', 1, 0);
INSERT INTO "services"."service_types" VALUES ('cc284bbc-d668-4836-bc66-e913e4e444cd', 4, 'Find me local agent ', NULL, 'Search for a local agent based on the criteria specified by the client ', NULL, 1300, 1300, '00000000-0000-0000-0000-0000000003f1', '2021-09-23 07:07:59.879083', '00000000-0000-0000-0000-0000000003f1', '2021-09-23 07:59:38.209435', 1, 0);
INSERT INTO "services"."service_types" VALUES ('3f960d16-ea99-4982-92c4-0a05ac1c9197', 4, 'Business advisory ', 'የንግድ ምክር', 'Request for business advisory inquiry ', 'የንግድ ምክር አገልግሎት መጠየቅ ', 1500, 3000, '00000000-0000-0000-0000-0000000003f1', '2021-09-23 07:11:53.8092', '00000000-0000-0000-0000-0000000003f1', '2021-09-28 23:58:57.718121', 1, 0);
INSERT INTO "services"."service_types" VALUES ('3c3e48a8-9f1a-4284-bb3c-10e90757be65', 16, 'Arbitration Service', 'የግልግል ዳኝነት', ' Arbitration Service ', '  የግልግል ዳኝነት ', 0, 0, '00000000-0000-0000-0000-0000000003f1', '2021-09-28 07:59:51.783326', '00000000-0000-0000-0000-0000000003f1', '2021-09-29 13:44:27.079009', 1, 3);

-- ----------------------------
-- Table structure for task_templates
-- ----------------------------
DROP TABLE IF EXISTS "services"."task_templates";
CREATE TABLE "services"."task_templates" (
  "id" uuid NOT NULL,
  "name_eng" varchar(100) COLLATE "pg_catalog"."default" NOT NULL,
  "name_local" text COLLATE "pg_catalog"."default" NOT NULL,
  "description_eng" text COLLATE "pg_catalog"."default" NOT NULL,
  "description_local" text COLLATE "pg_catalog"."default"
)
;

-- ----------------------------
-- Records of task_templates
-- ----------------------------
INSERT INTO "services"."task_templates" VALUES ('00000000-0000-0000-0000-0000000f0004', 'Send Document', 'ዝርዝሩን መላክ', 'Send Document', 'ዝርዝሩን መላክ');
INSERT INTO "services"."task_templates" VALUES ('00000000-0000-0000-0000-0000000f0003', 'Prepare Document', 'ዝርዝር ማዘጋጀት', 'Prepare Document', 'ዝርዝር ማዘጋጀት');
INSERT INTO "services"."task_templates" VALUES ('00000000-0000-0000-0000-0000000f0002', 'Verify Document', 'መረጃ ማጣራት', 'Verify Document', 'መረጃ ማጣራት');
INSERT INTO "services"."task_templates" VALUES ('00000000-0000-0000-0000-0000000f0001', 'Generate Invoice ', 'ክፍያ መጠየቂያ ማዘጋጀት', 'Generate Invoice ', 'ክፍያ መጠየቂያ ማዘጋጀት');

-- ----------------------------
-- Indexes structure for table service_followups
-- ----------------------------
CREATE INDEX "IX_service_followups_service_id" ON "services"."service_followups" USING btree (
  "service_id" "pg_catalog"."uuid_ops" ASC NULLS LAST
);
CREATE INDEX "IX_service_followups_task_type_id" ON "services"."service_followups" USING btree (
  "task_type_id" "pg_catalog"."uuid_ops" ASC NULLS LAST
);

-- ----------------------------
-- Primary Key structure for table service_followups
-- ----------------------------
ALTER TABLE "services"."service_followups" ADD CONSTRAINT "PK_service_followups" PRIMARY KEY ("id");
