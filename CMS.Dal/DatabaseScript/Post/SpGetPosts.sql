USE [TalaPishe]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'cnt.SpGetPosts') AND type in (N'P', N'PC'))
    DROP PROCEDURE cnt.SpGetPosts
GO

CREATE PROCEDURE cnt.SpGetPosts
	@Title NVARCHAR(max),
	@Alias NVARCHAR(max),
	@Special NVARCHAR(max),
	@Published NVARCHAR(max),	
	@IsProduct BIT,	
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
 		FROM [TalaPishe].cnt.[Posts] post
		LEFT JOIN pbl.[Menus] menu On menu.UnicId = post.MenuId
		WHERE
			(@Title IS NULL OR Title LIKE '%' + @Title + '%')
			AND (@Alias IS NULL OR post.Alias LIKE '%' + @Alias + '%')
			AND (@Special IS NULL OR Special = @Special)
			AND (@Published IS NULL OR post.Published = @Published)
			AND (@IsProduct IS NULL OR post.IsProduct = @IsProduct)
	)
	SELECT 
		count(*) OVER() Total,* 
	FROM MainSelect
	ORDER BY [Date] DESC
	OFFSET ((@PageIndex - 1) * @PageSize) ROWS FETCH NEXT @PageSize ROWS ONLY
	OPTION (RECOMPILE);

END 