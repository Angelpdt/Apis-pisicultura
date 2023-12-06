USE [PROYECTO_psi]
GO

/****** Object:  StoredProcedure [dbo].[sp_listar_produccion]    Script Date: 5/12/2023 7:14:59 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_listar_produccion]
AS 
BEGIN
    SET NOCOUNT ON; -- Evitar contar el número de filas afectadas

    IF EXISTS (SELECT 1 FROM produccion)
    BEGIN
        SELECT id_lote, cantidad, fecha_inicio, fecha_finalizacion, vendido
        FROM produccion;

        SELECT 'Se encontraron registros' AS respuesta;
    END
    ELSE
    BEGIN
        SELECT 'No hay registros en la tabla' AS respuesta;
    END;
END;
GO

