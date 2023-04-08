-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Hôte : 127.0.0.1
-- Généré le : sam. 08 avr. 2023 à 12:19
-- Version du serveur : 10.4.27-MariaDB
-- Version de PHP : 8.1.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de données : `ensa_tanger`
--

DELIMITER $$
--
-- Procédures
--
CREATE DEFINER=`root`@`localhost` PROCEDURE `Ajouter` (IN `code_eleve` VARCHAR(255), IN `code_mat` VARCHAR(255), IN `note` FLOAT(8))   BEGIN
INSERT INTO Notes(code_eleve, code_mat, note)
VALUES (code_eleve, code_mat, note);
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `Modifier` (IN `in_code_eleve` VARCHAR(255), IN `in_code_mat` VARCHAR(255), IN `in_note` FLOAT(8))   BEGIN
UPDATE Notes
SET note = in_note
WHERE code_eleve = in_code_eleve AND code_mat = in_code_mat;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `Supprimer` (IN `code_eleve` VARCHAR(255), IN `code_mat` VARCHAR(255))   BEGIN
DELETE FROM Notes
WHERE code_eleve = code_eleve AND code_mat = code_mat;
END$$

DELIMITER ;

-- --------------------------------------------------------

--
-- Structure de la table `eleve`
--

CREATE TABLE `eleve` (
  `id` int(11) NOT NULL,
  `code` varchar(255) DEFAULT NULL,
  `nom` varchar(255) DEFAULT NULL,
  `prenom` varchar(255) DEFAULT NULL,
  `niveau` varchar(255) DEFAULT NULL,
  `code_fil` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déchargement des données de la table `eleve`
--

INSERT INTO `eleve` (`id`, `code`, `nom`, `prenom`, `niveau`, `code_fil`) VALUES
(1, 'KS2134', 'Hamza', 'ZOUAK', 'GINF1', 'GINF'),
(2, 'Z1284', 'Zakaria', 'HICHOU', 'GSTR1', 'GSTR');

-- --------------------------------------------------------

--
-- Structure de la table `filiere`
--

CREATE TABLE `filiere` (
  `id` int(11) NOT NULL,
  `code` varchar(255) DEFAULT NULL,
  `designation` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déchargement des données de la table `filiere`
--

INSERT INTO `filiere` (`id`, `code`, `designation`) VALUES
(1, 'GINF', 'genie informatique'),
(2, 'GSTR', 'genie systeme de telecommunication et reseaux');

-- --------------------------------------------------------

--
-- Structure de la table `matiere`
--

CREATE TABLE `matiere` (
  `id` int(11) NOT NULL,
  `code` varchar(255) DEFAULT NULL,
  `designation` varchar(255) DEFAULT NULL,
  `VH` float DEFAULT NULL,
  `code_module` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déchargement des données de la table `matiere`
--

INSERT INTO `matiere` (`id`, `code`, `designation`, `VH`, `code_module`) VALUES
(1, 'GINF111', 'Programmation C1', 12, 'GINF11'),
(2, 'GINF112', 'Programmation C2', 12, 'GINF11'),
(3, 'GINF121', 'BD1', 12, 'GINF12'),
(4, 'GINF122', 'BD2', 12, 'GINF12'),
(5, 'GINF311', 'Java1', 12, 'GINF31'),
(6, 'GINF312', 'Java2', 12, 'GINF31'),
(7, 'GINF321', 'NoSQL1', 12, 'GINF32'),
(8, 'GINF322', 'NoSQL2', 12, 'GINF32'),
(9, 'GSTR111', 'Ondes1', 12, 'GSTR11'),
(10, 'GSTR112', 'Ondes2', 12, 'GSTR11'),
(11, 'GSTR121', 'Antennes1', 12, 'GSTR12'),
(12, 'GSTR122', 'Antennes2', 12, 'GSTR12');

-- --------------------------------------------------------

--
-- Structure de la table `module`
--

CREATE TABLE `module` (
  `id` int(11) NOT NULL,
  `code` varchar(255) DEFAULT NULL,
  `designation` varchar(255) DEFAULT NULL,
  `niveau` varchar(255) DEFAULT NULL,
  `semestre` varchar(255) DEFAULT NULL,
  `code_fil` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déchargement des données de la table `module`
--

INSERT INTO `module` (`id`, `code`, `designation`, `niveau`, `semestre`, `code_fil`) VALUES
(1, 'GINF11', 'Programmation C', 'GINF1', 'S1', 'GINF'),
(2, 'GINF12', 'Base de données', 'GINF1', 'S1', 'GINF'),
(3, 'GINF31', 'Java', 'GINF2', 'S3', 'GINF'),
(4, 'GINF32', 'BD NOSQL', 'GINF2', 'S3', 'GINF'),
(5, 'GSTR11', 'Ondes', 'GSTR1', 'S1', 'GSTR'),
(6, 'GSTR12', 'Antennes', 'GSTR1', 'S1', 'GSTR');

-- --------------------------------------------------------

--
-- Structure de la table `moyennes`
--

CREATE TABLE `moyennes` (
  `id` int(11) NOT NULL,
  `code_eleve` varchar(255) DEFAULT NULL,
  `code_fil` varchar(255) DEFAULT NULL,
  `niveau` varchar(255) DEFAULT NULL,
  `moyenne` float DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Structure de la table `notes`
--

CREATE TABLE `notes` (
  `id` int(11) NOT NULL,
  `code_eleve` varchar(255) DEFAULT NULL,
  `code_mat` varchar(255) DEFAULT NULL,
  `note` float DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déchargement des données de la table `notes`
--

INSERT INTO `notes` (`id`, `code_eleve`, `code_mat`, `note`) VALUES
(1, 'KS2134', 'GINF111', 14),
(2, 'Z1284', 'GSTR111', 16);

--
-- Index pour les tables déchargées
--

--
-- Index pour la table `eleve`
--
ALTER TABLE `eleve`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `code` (`code`),
  ADD KEY `code_fil` (`code_fil`);

--
-- Index pour la table `filiere`
--
ALTER TABLE `filiere`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `code` (`code`);

--
-- Index pour la table `matiere`
--
ALTER TABLE `matiere`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `code` (`code`),
  ADD KEY `code_module` (`code_module`);

--
-- Index pour la table `module`
--
ALTER TABLE `module`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `code` (`code`),
  ADD KEY `code_fil` (`code_fil`);

--
-- Index pour la table `moyennes`
--
ALTER TABLE `moyennes`
  ADD PRIMARY KEY (`id`),
  ADD KEY `code_eleve` (`code_eleve`),
  ADD KEY `code_fil` (`code_fil`);

--
-- Index pour la table `notes`
--
ALTER TABLE `notes`
  ADD PRIMARY KEY (`id`),
  ADD KEY `code_eleve` (`code_eleve`),
  ADD KEY `code_mat` (`code_mat`);

--
-- AUTO_INCREMENT pour les tables déchargées
--

--
-- AUTO_INCREMENT pour la table `eleve`
--
ALTER TABLE `eleve`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT pour la table `notes`
--
ALTER TABLE `notes`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- Contraintes pour les tables déchargées
--

--
-- Contraintes pour la table `eleve`
--
ALTER TABLE `eleve`
  ADD CONSTRAINT `eleve_ibfk_1` FOREIGN KEY (`code_fil`) REFERENCES `filiere` (`code`);

--
-- Contraintes pour la table `matiere`
--
ALTER TABLE `matiere`
  ADD CONSTRAINT `matiere_ibfk_1` FOREIGN KEY (`code_module`) REFERENCES `module` (`code`);

--
-- Contraintes pour la table `module`
--
ALTER TABLE `module`
  ADD CONSTRAINT `module_ibfk_1` FOREIGN KEY (`code_fil`) REFERENCES `filiere` (`code`);

--
-- Contraintes pour la table `moyennes`
--
ALTER TABLE `moyennes`
  ADD CONSTRAINT `moyennes_ibfk_1` FOREIGN KEY (`code_eleve`) REFERENCES `eleve` (`code`),
  ADD CONSTRAINT `moyennes_ibfk_2` FOREIGN KEY (`code_fil`) REFERENCES `filiere` (`code`);

--
-- Contraintes pour la table `notes`
--
ALTER TABLE `notes`
  ADD CONSTRAINT `notes_ibfk_1` FOREIGN KEY (`code_eleve`) REFERENCES `eleve` (`code`),
  ADD CONSTRAINT `notes_ibfk_2` FOREIGN KEY (`code_mat`) REFERENCES `matiere` (`code`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
