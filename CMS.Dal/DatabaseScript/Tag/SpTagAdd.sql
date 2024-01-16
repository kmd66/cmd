USE [TalaPishe]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pbl.SpTagAdd') AND type in (N'P', N'PC'))
    DROP PROCEDURE pbl.SpTagAdd
GO

CREATE PROCEDURE pbl.SpTagAdd
	@PostId BIGINT,
	@Json NVARCHAR(max)
--WITH ENCRYPTION
AS
BEGIN
    SET NOCOUNT ON;

	DELETE tag
	FROM pbl.Tags tag
		LEFT JOIN OPENJSON(@Json) id ON id.value = tag.Text
	WHERE tag.PostID = @PostId
	AND id.value IS NULL
	
	;WITH items AS(
		SELECT 
			@PostId PostId
			, value v
		FROM OPENJSON(@Json) 
	)
	INSERT INTO pbl.Tags
	SELECT 
		NEWID() , i.v, i.PostId
	FROM items i
		LEFT JOIN pbl.Tags tag ON i.v = tag.Text
		AND tag.PostID = i.PostId
	WHERE i.PostID = @PostId
	AND tag.Id IS NULL

	SELECT * FROM pbl.Tags
	WHERE PostID = @PostId

END 
