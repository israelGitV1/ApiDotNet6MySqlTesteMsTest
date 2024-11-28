-- MySQL dump 10.13  Distrib 8.0.40, for Linux (x86_64)
--
-- Host: localhost    Database: MinimalApi
-- ------------------------------------------------------
-- Server version	9.0.1

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `Administradores`
--

DROP TABLE IF EXISTS `Administradores`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Administradores` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Email` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Senha` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Perfil` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=34 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Administradores`
--

LOCK TABLES `Administradores` WRITE;
/*!40000 ALTER TABLE `Administradores` DISABLE KEYS */;
INSERT INTO `Administradores` VALUES (1,'administrador@teste.com','123456','Adm'),(3,'Adm','string','Adm'),(4,'Editor','string','Editor'),(5,'test@gamil.com','string','Adm'),(6,'Lucas@gamil.com','123456','Editor'),(8,'string','string','string'),(9,'string','string','string'),(10,'string','string','string'),(11,'string','string','string'),(12,'string','string','string'),(13,'string','string','string'),(14,'string','string','string'),(15,'string','string','string'),(16,'string','string','string'),(17,'string','string','string'),(18,'string','string','string'),(19,'string','string','string'),(20,'string','string','string'),(21,'string','string','string'),(22,'string','string','string'),(23,'string','string','string'),(24,'string','string','string'),(25,'string','string','Adm'),(26,'string','string','2'),(27,'string','string','2'),(28,'string','string','Adm'),(29,'string','string','Adm'),(30,'string','string','Editor'),(31,'string','string','Adm'),(32,'Israel@gmail.com','4614365','Adm'),(33,'Gustavo','string','Adm');
/*!40000 ALTER TABLE `Administradores` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Veiculos`
--

DROP TABLE IF EXISTS `Veiculos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Veiculos` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nome` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Ano` int NOT NULL DEFAULT '0',
  `Marca` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL DEFAULT '',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Veiculos`
--

LOCK TABLES `Veiculos` WRITE;
/*!40000 ALTER TABLE `Veiculos` DISABLE KEYS */;
INSERT INTO `Veiculos` VALUES (1,'Onix',2025,'Chevrolet'),(3,'Corola',2021,'Toyota'),(4,'Gol',2019,'Volkswagen'),(5,'Toyota Etios',2012,'Toyota'),(7,'Fiat Argo',2018,'Fiat'),(8,'Kwid',2023,'Renault');
/*!40000 ALTER TABLE `Veiculos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `__EFMigrationsHistory`
--

DROP TABLE IF EXISTS `__EFMigrationsHistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__EFMigrationsHistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__EFMigrationsHistory`
--

LOCK TABLES `__EFMigrationsHistory` WRITE;
/*!40000 ALTER TABLE `__EFMigrationsHistory` DISABLE KEYS */;
INSERT INTO `__EFMigrationsHistory` VALUES ('20241115111049_AdministradorMigration','6.0.13'),('20241115114114_SeedAdministrador','6.0.13'),('20241117121147_VeiculosMigration','6.0.13'),('20241118044326_VeiculosMigrationUp','6.0.13'),('20241118114253_VeiculosMigrationUpdate1','6.0.13');
/*!40000 ALTER TABLE `__EFMigrationsHistory` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-11-27  8:36:45
