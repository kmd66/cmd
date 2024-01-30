USE [TalaPishe]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pbl.SpGetStatus') AND type in (N'P', N'PC'))
    DROP PROCEDURE pbl.SpGetStatus
GO

CREATE PROCEDURE pbl.SpGetStatus
--WITH ENCRYPTION
AS
BEGIN
    SET NOCOUNT ON;
	
	DECLARE @Comment INT, @Message INT
	
	SET @Comment  = (
		SELECT 
			COUNT(Id)
		FROM [cnt].[Comments]
		WHERE Type = 1
	)
	SET @Message  = (
		SELECT 
			COUNT(Id)
		FROM [pbl].[Messages]
		WHERE Type = 1
	)
	SELECT @Comment CountComment, @Message CountMessage

END 
