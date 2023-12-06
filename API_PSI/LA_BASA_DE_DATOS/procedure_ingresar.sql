USE [PROYECTO_psi]
GO

/****** Object:  StoredProcedure [dbo].[sp_ingresar_produccion]    Script Date: 5/12/2023 7:15:27 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[sp_ingresar_produccion]
@cantidad int,
@fecha_inicio varchar(50),
@fecha_finalizacion varchar(50),
@vendido char(2)
as 
begin 
	begin transaction tx;
	if exists (select 1 from produccion)
	begin
		insert into produccion (cantidad, fecha_inicio, fecha_finalizacion, vendido) values (@cantidad,@fecha_inicio,@fecha_finalizacion,@vendido);
		 commit transaction tx;
		select 'Se guardó exitosamente' as respuesta
	end
	else
	begin
		rollback transaction tx;
		select 'Error: ' as respuesta;
	end;
end;
GO

