USE [TalaPishe]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'cnt.SpGetPostProducts') AND type in (N'P', N'PC'))
    DROP PROCEDURE cnt.SpGetPostProducts
GO

CREATE PROCEDURE cnt.SpGetPostProducts
	@Alias NVARCHAR(max),
	@MenuId UNIQUEIDENTIFIER,	
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
		SELECT post.*
			,menu.[Name] MenuName
			,menu.Alias MenuAlias
			,prd.Price
			,prd.SpecialPrice
 		FROM cnt.[Posts] post
		INNER JOIN pbl.[Menus] menu On menu.UnicId = post.MenuId
		LEFT JOIN cnt.Products prd On prd.UnicId = post.UnicId
		WHERE post.Published = 1
			AND (@Alias IS NULL OR post.Alias LIKE '%' + @Alias + '%')
			AND (@MenuId IS NULL OR post.MenuId = @MenuId)
	)
	SELECT 
		count(*) OVER() Total,* 
	FROM MainSelect
	ORDER BY [Date] DESC
	OFFSET ((@PageIndex - 1) * @PageSize) ROWS FETCH NEXT @PageSize ROWS ONLY
	OPTION (RECOMPILE);

END 