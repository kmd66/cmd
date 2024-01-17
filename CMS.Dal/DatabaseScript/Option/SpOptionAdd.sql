USE [TalaPishe]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pbl.SpOptionAdd') AND type in (N'P', N'PC'))
    DROP PROCEDURE pbl.SpOptionAdd
GO

CREATE PROCEDURE pbl.SpOptionAdd
	@Json NVARCHAR(max)
--WITH ENCRYPTION
AS
BEGIN
    SET NOCOUNT ON;

	
	DELETE opin FROM 
		OPENJSON(@Json)
				WITH (
					Id UNIQUEIDENTIFIER,
					[Type] INT,
					[Text] NVARCHAR(max)
		) item
	INNER JOIN pbl.Options opin on opin.[Type] = item. [Type]
		AND opin.Id <>item.Id
	 
	UPDATE opin SET opin.[Text]  = item.[Text]
	FROM 
		OPENJSON(@Json)
				WITH (
					Id UNIQUEIDENTIFIER,
					[Type] INT,
					[Text] NVARCHAR(max)
		) item
	INNER JOIN pbl.Options opin on opin.[Type] = item. [Type]
		AND opin.Id  = item.Id
		AND opin.[Text]  <> item.[Text]
	
	INSERT INTO pbl.Options
	SELECT item.* FROM 
		OPENJSON(@Json)
				WITH (
					Id UNIQUEIDENTIFIER,
					[Type] INT,
					[Text] NVARCHAR(max)
		) item
	LEFT JOIN pbl.Options opin on opin.[Type] = item. [Type]
	WHERE opin.Id IS NULL

	SELECT * FROM pbl.Options

END 
