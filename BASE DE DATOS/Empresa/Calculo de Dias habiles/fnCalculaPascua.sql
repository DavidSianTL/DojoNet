Use DBProyectoGrupalDojoGeko
GO

CREATE FUNCTION dbo.fnCalcularPascua (@Anio INT)
RETURNS DATE
AS
BEGIN
    DECLARE @a INT = @Anio % 19
    DECLARE @b INT = @Anio / 100
    DECLARE @c INT = @Anio % 100
    DECLARE @d INT = @b / 4
    DECLARE @e INT = @b % 4
    DECLARE @f INT = (@b + 8) / 25
    DECLARE @g INT = (@b - @f + 1) / 3
    DECLARE @h INT = (19 * @a + @b - @d - @g + 15) % 30
    DECLARE @i INT = @c / 4
    DECLARE @k INT = @c % 4
    DECLARE @l INT = (32 + 2 * @e + 2 * @i - @h - @k) % 7
    DECLARE @m INT = (@a + 11 * @h + 22 * @l) / 451
    DECLARE @mes INT = (@h + @l - 7 * @m + 114) / 31
    DECLARE @dia INT = ((@h + @l - 7 * @m + 114) % 31) + 1

    RETURN CAST(CONCAT(@Anio, '-', @mes, '-', @dia) AS DATE)
END