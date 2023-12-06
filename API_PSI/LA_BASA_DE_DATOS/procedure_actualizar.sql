USE [PROYECTO_psi]
GO

/****** Object:  StoredProcedure [dbo].[sp_actualizar_produccion]    Script Date: 5/12/2023 7:16:06 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER procedure [dbo].[sp_actualizar_produccion] 
@id_lote int,
@cantidad int,
@fecha_inicio varchar,
@fecha_finalizacion varchar,
@vendido char
as 
begin 
	begin transaction tx;
	if exists ( select 1 from produccion where id_lote=@id_lote)
	begin 
	update produccion
	set
	cantidad=@cantidad,
	fecha_inicio=@fecha_inicio,
	fecha_finalizacion=@fecha_finalizacion,
	vendido=@vendido
	where id_lote=@id_lote
	commit transaction tx;
	select 'se actualizo correctaente' as respuesta 
	end
	else
	begin
		rollback transaction tx;
		select 'error al actualizar
		mira que fue lo que ocurrio' as respuesta
		end;
		end;
GO

