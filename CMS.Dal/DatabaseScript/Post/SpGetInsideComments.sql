USE [TalaPishe]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'cnt.SpGetInsideComments') AND type in (N'P', N'PC'))
    DROP PROCEDURE cnt.SpGetInsideComments
GO

CREATE PROCEDURE cnt.SpGetInsideComments
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

	; WITH MainSelect AS
	(
		SELECT 
			com.*
			,0 Score
 		FROM cnt.Comments com
		WHERE (@Type = 0 OR com.[Type] = @Type)
			AND (@Name IS NULL OR com.[Name] LIKE '%' + @Name + '%')
	)
	SELECT 
		count(*) OVER() [Count],* 
	FROM MainSelect
	ORDER BY [Date] DESC
	OFFSET ((@PageIndex - 1) * @PageSize) ROWS FETCH NEXT @PageSize ROWS ONLY

END 