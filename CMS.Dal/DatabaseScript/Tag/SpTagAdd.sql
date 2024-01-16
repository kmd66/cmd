USE [TalaPishe]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'cnt.SpTagAdd') AND type in (N'P', N'PC'))
    DROP PROCEDURE cnt.SpTagAdd
GO

CREATE PROCEDURE cnt.SpTagAdd
	@PostId BIGINT,
	@Json NVARCHAR(max)
--WITH ENCRYPTION
AS
BEGIN
    SET NOCOUNT ON;

	DELETE tag
	FROM cnt.Tags tag
		LEFT JOIN OPENJSON(@Json) id ON id.value = tag.Text
	WHERE tag.PostID = @PostId
	AND id.value IS NULL
	
	;WITH items AS(
		SELECT 
			@PostId PostId
			, value v
		FROM OPENJSON(@Json) 
	)
	INSERT INTO cnt.Tags
	SELECT 
		NEWID() , i.v, i.PostId
	FROM items i
		LEFT JOIN cnt.Tags tag ON i.v = tag.Text
		AND tag.PostID = i.PostId
	WHERE i.PostID = @PostId
	AND tag.Id IS NULL

	SELECT * FROM cnt.Tags
	WHERE PostID = @PostId

END 
