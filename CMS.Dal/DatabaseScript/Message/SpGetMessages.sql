USE [TalaPishe]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pbl.SpGetMessages') AND type in (N'P', N'PC'))
    DROP PROCEDURE pbl.SpGetMessages
GO

CREATE PROCEDURE pbl.SpGetMessages
	@Type TINYINT,	
	@Name NVARCHAR(MAX),	
	@PageSize INT,
	@PageIndex INT
--WITH ENCRYPTION
AS
BEGIN
    SET NOCOUNT ON;
	
	IF @PageIndex = 0 OR @PageIndex IS NULL 
	BEGIN
		SET @pagesize = 100
		SET @PageIndex = 1
	END

	SELECT * FROM [pbl].[Messages]
		WHERE (@Type = 0 OR [Type] = @Type)
			AND (@Name IS NULL OR [Name] LIKE '%' + @Name + '%')
	ORDER BY Id
	OFFSET ((@PageIndex - 1) * @PageSize) ROWS FETCH NEXT @PageSize ROWS ONLY

END 
