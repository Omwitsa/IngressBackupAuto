use ingressautodb;

-- MySQL dump 10.13  Distrib 8.0.36, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: ingressautodb
-- ------------------------------------------------------
-- Server version	8.2.0

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `__efmigrationshistory`
--

DROP TABLE IF EXISTS `__efmigrationshistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__efmigrationshistory`
--

LOCK TABLES `__efmigrationshistory` WRITE;
/*!40000 ALTER TABLE `__efmigrationshistory` DISABLE KEYS */;
INSERT INTO `__efmigrationshistory` VALUES ('20240129135326_Initial-Migration','7.0.13');
/*!40000 ALTER TABLE `__efmigrationshistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `hangfire_aggregatedcounter`
--

DROP TABLE IF EXISTS `hangfire_aggregatedcounter`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `hangfire_aggregatedcounter` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Key` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Value` int NOT NULL,
  `ExpireAt` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_Hangfire_CounterAggregated_Key` (`Key`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `hangfire_aggregatedcounter`
--

LOCK TABLES `hangfire_aggregatedcounter` WRITE;
/*!40000 ALTER TABLE `hangfire_aggregatedcounter` DISABLE KEYS */;
/*!40000 ALTER TABLE `hangfire_aggregatedcounter` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `hangfire_counter`
--

DROP TABLE IF EXISTS `hangfire_counter`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `hangfire_counter` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Key` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Value` int NOT NULL,
  `ExpireAt` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Hangfire_Counter_Key` (`Key`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `hangfire_counter`
--

LOCK TABLES `hangfire_counter` WRITE;
/*!40000 ALTER TABLE `hangfire_counter` DISABLE KEYS */;
/*!40000 ALTER TABLE `hangfire_counter` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `hangfire_distributedlock`
--

DROP TABLE IF EXISTS `hangfire_distributedlock`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `hangfire_distributedlock` (
  `Resource` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `CreatedAt` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `hangfire_distributedlock`
--

LOCK TABLES `hangfire_distributedlock` WRITE;
/*!40000 ALTER TABLE `hangfire_distributedlock` DISABLE KEYS */;
/*!40000 ALTER TABLE `hangfire_distributedlock` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `hangfire_hash`
--

DROP TABLE IF EXISTS `hangfire_hash`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `hangfire_hash` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Key` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Field` varchar(40) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Value` longtext,
  `ExpireAt` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_Hangfire_Hash_Key_Field` (`Key`,`Field`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `hangfire_hash`
--

LOCK TABLES `hangfire_hash` WRITE;
/*!40000 ALTER TABLE `hangfire_hash` DISABLE KEYS */;
/*!40000 ALTER TABLE `hangfire_hash` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `hangfire_job`
--

DROP TABLE IF EXISTS `hangfire_job`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `hangfire_job` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `StateId` int DEFAULT NULL,
  `StateName` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `InvocationData` longtext NOT NULL,
  `Arguments` longtext NOT NULL,
  `CreatedAt` datetime NOT NULL,
  `ExpireAt` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Hangfire_Job_StateName` (`StateName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `hangfire_job`
--

LOCK TABLES `hangfire_job` WRITE;
/*!40000 ALTER TABLE `hangfire_job` DISABLE KEYS */;
/*!40000 ALTER TABLE `hangfire_job` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `hangfire_jobparameter`
--

DROP TABLE IF EXISTS `hangfire_jobparameter`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `hangfire_jobparameter` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `JobId` int NOT NULL,
  `Name` varchar(40) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Value` longtext,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_Hangfire_JobParameter_JobId_Name` (`JobId`,`Name`),
  KEY `FK_Hangfire_JobParameter_Job` (`JobId`),
  CONSTRAINT `FK_Hangfire_JobParameter_Job` FOREIGN KEY (`JobId`) REFERENCES `hangfire_job` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `hangfire_jobparameter`
--

LOCK TABLES `hangfire_jobparameter` WRITE;
/*!40000 ALTER TABLE `hangfire_jobparameter` DISABLE KEYS */;
/*!40000 ALTER TABLE `hangfire_jobparameter` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `hangfire_jobqueue`
--

DROP TABLE IF EXISTS `hangfire_jobqueue`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `hangfire_jobqueue` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `JobId` int NOT NULL,
  `FetchedAt` datetime DEFAULT NULL,
  `Queue` varchar(50) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `FetchToken` varchar(36) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Hangfire_JobQueue_QueueAndFetchedAt` (`Queue`,`FetchedAt`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `hangfire_jobqueue`
--

LOCK TABLES `hangfire_jobqueue` WRITE;
/*!40000 ALTER TABLE `hangfire_jobqueue` DISABLE KEYS */;
/*!40000 ALTER TABLE `hangfire_jobqueue` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `hangfire_jobstate`
--

DROP TABLE IF EXISTS `hangfire_jobstate`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `hangfire_jobstate` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `JobId` int NOT NULL,
  `CreatedAt` datetime NOT NULL,
  `Name` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Reason` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `Data` longtext,
  PRIMARY KEY (`Id`),
  KEY `FK_Hangfire_JobState_Job` (`JobId`),
  CONSTRAINT `FK_Hangfire_JobState_Job` FOREIGN KEY (`JobId`) REFERENCES `hangfire_job` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `hangfire_jobstate`
--

LOCK TABLES `hangfire_jobstate` WRITE;
/*!40000 ALTER TABLE `hangfire_jobstate` DISABLE KEYS */;
/*!40000 ALTER TABLE `hangfire_jobstate` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `hangfire_list`
--

DROP TABLE IF EXISTS `hangfire_list`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `hangfire_list` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Key` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Value` longtext,
  `ExpireAt` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `hangfire_list`
--

LOCK TABLES `hangfire_list` WRITE;
/*!40000 ALTER TABLE `hangfire_list` DISABLE KEYS */;
/*!40000 ALTER TABLE `hangfire_list` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `hangfire_server`
--

DROP TABLE IF EXISTS `hangfire_server`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `hangfire_server` (
  `Id` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Data` longtext NOT NULL,
  `LastHeartbeat` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `hangfire_server`
--

LOCK TABLES `hangfire_server` WRITE;
/*!40000 ALTER TABLE `hangfire_server` DISABLE KEYS */;
INSERT INTO `hangfire_server` VALUES ('aaahhq039:11964:0b267bba-0ff3-4572-8832-d972bc059985','{\"WorkerCount\":20,\"Queues\":[\"default\"],\"StartedAt\":\"2024-01-29T13:55:25.8347873Z\"}','2024-01-29 13:55:26.866888');
/*!40000 ALTER TABLE `hangfire_server` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `hangfire_set`
--

DROP TABLE IF EXISTS `hangfire_set`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `hangfire_set` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Key` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Value` varchar(191) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Score` float NOT NULL,
  `ExpireAt` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_Hangfire_Set_Key_Value` (`Key`,`Value`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `hangfire_set`
--

LOCK TABLES `hangfire_set` WRITE;
/*!40000 ALTER TABLE `hangfire_set` DISABLE KEYS */;
/*!40000 ALTER TABLE `hangfire_set` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `hangfire_state`
--

DROP TABLE IF EXISTS `hangfire_state`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `hangfire_state` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `JobId` int NOT NULL,
  `Name` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Reason` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `CreatedAt` datetime NOT NULL,
  `Data` longtext,
  PRIMARY KEY (`Id`),
  KEY `FK_Hangfire_HangFire_State_Job` (`JobId`),
  CONSTRAINT `FK_Hangfire_HangFire_State_Job` FOREIGN KEY (`JobId`) REFERENCES `hangfire_job` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `hangfire_state`
--

LOCK TABLES `hangfire_state` WRITE;
/*!40000 ALTER TABLE `hangfire_state` DISABLE KEYS */;
/*!40000 ALTER TABLE `hangfire_state` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `syssetup`
--

DROP TABLE IF EXISTS `syssetup`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `syssetup` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `OrgName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `SiteName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `SmtpUserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `SmtpPassword` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `SmtpServer` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `SmtpPort` int NOT NULL,
  `SocketOption` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `ReceiverEmail` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `MysqlUserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `MysqlPassword` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `MysqlServer` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `SiteIngressDb` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `HoMysqlUserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `HoMysqlPassword` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `HoMysqlServer` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `HoIngressDb` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `IngressBackMonths` int NOT NULL,
  `Contact` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `LastBackup` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `BackupLoc` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `Closed` tinyint(1) NOT NULL,
  `OnMpls` tinyint(1) NOT NULL,
  `Personnel` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `AutoBackup1At` time DEFAULT NULL,
  `AutoBackup2At` time DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `syssetup`
--

LOCK TABLES `syssetup` WRITE;
/*!40000 ALTER TABLE `syssetup` DISABLE KEYS */;
INSERT INTO `syssetup` VALUES (1,'AAA GROWERS','HO','ingress.bkpauto@aaagrowers.co.ke','h8ts+KZxkevMnDj4kjsFTw==','mail.aaagrowers.co.ke',587,'TLS','ingressbkpauto@aaagrowers.co.ke','root','1UXbgTAFaZ5Omwpt0AesBw==','localhost','ingress','root','1UXbgTAFaZ5Omwpt0AesBw==','localhost','ingress_simba',2,NULL,NULL,NULL,0,1,NULL,'10:00:00','14:00:00');
/*!40000 ALTER TABLE `syssetup` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserID` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `Names` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `Password` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `Email` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `Phone` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `Status` tinyint(1) NOT NULL,
  `Role` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `Personnel` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `DateCreated` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'admin@aaa.com','Administrator','+r4zEPG2DU1MNeA13mM7jQ==','itsupport@aaagrowers.co.ke',NULL,1,'Admin',NULL,NULL);
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-01-29 17:12:40
