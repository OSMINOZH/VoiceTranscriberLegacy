CREATE TABLE `Transcriber` (
	`id` INT NOT NULL AUTO_INCREMENT,
	`pcName` varchar NOT NULL,
	`date` DATETIME NOT NULL,
	`filename` char NOT NULL,
	`fileAudio` longblob NOT NULL,
	`transcribedText` TEXT NOT NULL,
	`excelFile` longblob,
	PRIMARY KEY (`id`)
);

CREATE TABLE `Log` (
	`id` INT NOT NULL AUTO_INCREMENT,
	`date` DATETIME NOT NULL,
	`pcName` varchar NOT NULL,
	`programName` varchar NOT NULL,
	`logText` varchar NOT NULL,
	`issue` varchar NOT NULL,
	`status` varchar NOT NULL,
	PRIMARY KEY (`id`)
);

CREATE TABLE `RandNumber` (
	`id` INT NOT NULL AUTO_INCREMENT,
	`date` DATETIME NOT NULL,
	`pcName` varchar NOT NULL,
	`fedDistrict` varchar,
	`region` varchar,
	`type` varchar NOT NULL,
	`count` INT NOT NULL,
	`numbers` TEXT NOT NULL,
	`excelFile` longblob,
	PRIMARY KEY (`id`)
);




