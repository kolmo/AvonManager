IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[ArtikelVarianten_FK_ArtikelVarianten_Artikel]') AND parent_object_id = OBJECT_ID(N'[dbo].[ArtikelVarianten]'))
ALTER TABLE [dbo].[ArtikelVarianten] DROP CONSTRAINT [ArtikelVarianten_FK_ArtikelVarianten_Artikel]
GO

/****** Object:  Table [dbo].[ArtikelVarianten]    Script Date: 11/22/2015 14:34:51 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ArtikelVarianten]') AND type in (N'U'))
DROP TABLE [dbo].[ArtikelVarianten]
GO

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
ALTER TABLE dbo.Artikel ADD
	ChangedAt datetime NOT NULL CONSTRAINT DF_Artikel_ChamgedAt DEFAULT GetDate()
GO
ALTER TABLE dbo.Artikel SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
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
ALTER TABLE dbo.Bestellung ADD
	ChangedAt datetime NOT NULL CONSTRAINT DF_Bestellung_ChangedAt DEFAULT GetDate()
GO
ALTER TABLE dbo.Bestellung SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

