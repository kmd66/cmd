USE [TalaPishe]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'cnt.SpGetRelatedPosts') AND type in (N'P', N'PC'))
    DROP PROCEDURE cnt.SpGetRelatedPosts
GO

CREATE PROCEDURE cnt.SpGetRelatedPosts
	@Id BIGINT,
	@MenuId UNIQUEIDENTIFIER,
	@Count INT
--WITH ENCRYPTION
AS
BEGIN
    SET NOCOUNT ON;

	SELECT TOP(@Count) 
		*
		,'' MenuName 
		,0 Total
	FROM cnt.Posts
	WHERE Published = 1 AND Id <> @Id AND MenuId = @MenuId
	order by NEWID()

END 