/*
   Samstag, 20. Mai 201718:30:06
   Benutzer: 
   Server: zeus
   Datenbank: AvonDatabaseProd
   Anwendung: 
*/

/* Überprüfen Sie das Skript gründlich, bevor Sie es außerhalb des Datenbank-Designer-Kontexts ausführen, um potenzielle Datenverluste zu vermeiden.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Bestellung
	DROP CONSTRAINT Bestellung_FK_Bestellung_Status
GO
ALTER TABLE dbo.Bestellstatus SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Bestellstatus', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Bestellstatus', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Bestellstatus', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.Bestellung
	DROP CONSTRAINT Bestellung_FK_Bestellung_Kunden
GO
ALTER TABLE dbo.Kunden SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Kunden', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Kunden', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Kunden', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.Bestellung
	DROP CONSTRAINT DF_Bestellung_Loeschvormerkung
GO
ALTER TABLE dbo.Bestellung
	DROP CONSTRAINT DF_Bestellung_ChangedAt
GO
CREATE TABLE dbo.Tmp_Bestellung
	(
	BestellId int NOT NULL IDENTITY (8376, 1),
	KundenId int NULL,
	Datum datetime NULL,
	Bemerkung nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	StatusId int NULL,
	Loeschvormerkung bit NULL,
	ChangedAt datetime NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Bestellung SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_Bestellung ADD CONSTRAINT
	DF_Bestellung_Loeschvormerkung DEFAULT ((0)) FOR Loeschvormerkung
GO
ALTER TABLE dbo.Tmp_Bestellung ADD CONSTRAINT
	DF_Bestellung_ChangedAt DEFAULT (getdate()) FOR ChangedAt
GO
SET IDENTITY_INSERT dbo.Tmp_Bestellung ON
GO
IF EXISTS(SELECT * FROM dbo.Bestellung)
	 EXEC('INSERT INTO dbo.Tmp_Bestellung (BestellId, KundenId, Datum, Bemerkung, StatusId, Loeschvormerkung, ChangedAt)
		SELECT BestellId, KundenId, Datum, Bemerkung, StatusId, Loeschvormerkung, ChangedAt FROM dbo.Bestellung WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Bestellung OFF
GO
ALTER TABLE dbo.Bestelldetails
	DROP CONSTRAINT Bestelldetails_FK__Bestelldetails__0000000000000046
GO
DROP TABLE dbo.Bestellung
GO
EXECUTE sp_rename N'dbo.Tmp_Bestellung', N'Bestellung', 'OBJECT' 
GO
ALTER TABLE dbo.Bestellung ADD CONSTRAINT
	Bestellung_Bestellnr_PK PRIMARY KEY CLUSTERED 
	(
	BestellId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Bestellung ADD CONSTRAINT
	Bestellung_FK_Bestellung_Kunden FOREIGN KEY
	(
	KundenId
	) REFERENCES dbo.Kunden
	(
	KundenId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Bestellung ADD CONSTRAINT
	Bestellung_FK_Bestellung_Status FOREIGN KEY
	(
	StatusId
	) REFERENCES dbo.Bestellstatus
	(
	StatusId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Bestellung', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Bestellung', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Bestellung', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.Bestelldetails ADD
	Position int NULL,
	Bemerkung nvarchar(255) NULL
GO
ALTER TABLE dbo.Bestelldetails ADD CONSTRAINT
	Bestelldetails_FK__Bestelldetails__0000000000000046 FOREIGN KEY
	(
	BestellId
	) REFERENCES dbo.Bestellung
	(
	BestellId
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.Bestelldetails SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Bestelldetails', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Bestelldetails', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Bestelldetails', 'Object', 'CONTROL') as Contr_Per 
