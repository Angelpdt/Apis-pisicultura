create table productividad
(
id int identity primary key,
Fecha varchar (20),
NumerodeLote varchar(20),
CantidadPeces int,
PesoTotal int,	---Peso total de los peces 
alimentoConsumido int,
costoAlimento int,
tasaMortalidad decimal(3,1)
)

CREATE PROCEDURE sp_ingresar_Product
(

    @Fecha VARCHAR(20),
    @NumerodeLote VARCHAR(20),
    @CantidadPeces INT,
    @PesoTotal INT,
    @alimentoConsumido INT,
    @costoAlimento INT,
    @tasaMortalidad DECIMAL(3,1) 
)
AS
BEGIN
    BEGIN TRANSACTION TX;

    -- Verificar si el código de barras ya existe en la base de datos
    IF EXISTS (SELECT 1 FROM productividad )
    BEGIN
        INSERT INTO productividad (Fecha,NumerodeLote,CantidadPeces,PesoTotal,alimentoConsumido,costoAlimento,tasaMortalidad)VALUES (@Fecha,@NumerodeLote,@CantidadPeces,@PesoTotal,@alimentoConsumido,@costoAlimento,@tasaMortalidad);
       
		COMMIT TRANSACTION TX;
            SELECT 'SE GUARDÓ EXITOSAMENTE' AS RESPUESTA;
            
        END
        else
        BEGIN
            ROLLBACK TRANSACTION TX;
            SELECT 'HUBO UN ERROR' AS RESPUESTA;
     END;
END;
  
    


 
 -- Llamada al procedimiento para insertar el primer registro
EXEC sp_ingresar_Product '2024-06-07', 'Lote7', 330, 660, 880, 90, 2.5

-- Llamada al procedimiento para insertar el segundo registro
EXEC sp_ingresar_Product '2023-01-04', 'Lote3', 120, 600, 250, 60, 3.5






create proc sp_listar_product

as 
begin 
select* from productividad
end
exec sp_listar_product




create procedure sp_borrar_productiv
@id int
as
begin
	begin transaction tx;
	if exists (select 1 from productividad where id=@id)
	begin
	delete from productividad where id=@id
	commit transaction tx;
		select 'se borro existosamnete' as respuesta
	end
	else
	begin
	rollback transaction tx;
		select 'error'as respuesta
	end;
end;

exec sp_borrar_productiv 4
--------------------------------------------------------------------------------------crear procedimiento de actualizar
create procedure sp_actualizar_productivi
	@id int,
	@Fecha VARCHAR(20),
    @NumerodeLote VARCHAR(20),
    @CantidadPeces INT,
    @PesoTotal INT,
    @alimentoConsumido INT,
    @costoAlimento INT,
    @tasaMortalidad DECIMAL(3,1) 
as
begin
begin transaction tx;
if exists ( select 1 from productividad where id=@id)
begin
update productividad
set
Fecha=@Fecha,
NumerodeLote=@NumerodeLote,
CantidadPeces=@CantidadPeces,
PesoTotal=@PesoTotal,
alimentoConsumido=@alimentoConsumido,
costoAlimento=@costoAlimento,
tasaMortalidad=@tasaMortalidad
where id=@id
		commit transaction tx;
		select 'se actualizo correctaente' as respuesta
	end
	else
	begin
		rollback transaction tx;
		select 'error al actualizar mira que fue lo que ocurrio' as respuesta
	end;
end;
exec sp_actualizar_productivi
@id=2,
@Fecha='12-23-12',
@NumerodeLote=3,
@CantidadPeces=44444,
@PesoTotal=22,
@alimentoConsumido=1222,
@costoAlimento=2333,
@tasaMortalidad=2.5
exec sp_listar_product