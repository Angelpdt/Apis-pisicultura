USE [PROYECTO_psi]
GO

/****** Object:  StoredProcedure [dbo].[sp_borrar_produccion]    Script Date: 5/12/2023 7:15:45 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[sp_borrar_produccion]
@id_lote int
as
begin 
	begin transaction tx;
	if exists (select 1 from produccion where id_lote=@id_lote)
	begin 
	delete from produccion where id_lote=@id_lote
	commit transaction tx;
	select 'se borro existosamnete' as respuesta 
	end
	else
	begin 
		rollback transaction tx;
		select 'error'as respuesta 
		end;
		end;
GO

