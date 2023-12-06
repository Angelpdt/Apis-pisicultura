create database Administracion;

CREATE TABLE Gastos (
    GastoID INT PRIMARY KEY,
    FechaGasto varchar(20),
    TipoGasto VARCHAR(50),
    Descripcion VARCHAR(255),
    Monto DECIMAL(10, 2),
    Notas VARCHAR(50)
);


-- Inserci�n de algunos datos de gastos
INSERT INTO Gastos (GastoID, FechaGasto, TipoGasto, Descripcion, Monto, Notas)
VALUES
(1, '2023-01-15', 'Alimentaci�n', 'Compra de alimento para peces', 500.00, 'Pago a proveedor A'),
(2, '2023-02-05', 'Mantenimiento', 'Reparaci�n de bomba de agua', 300.00, 'Mano de obra incluida'),
(3, '2023-03-20', 'Suministros', 'Compra de redes y herramientas', 150.00, 'Pedido a proveedor B');

-- Otros ejemplos
INSERT INTO Gastos (GastoID, FechaGasto, TipoGasto, Descripcion, Monto, Notas)
VALUES
(4, '2023-04-10', 'Mano de Obra', 'Jornada de limpieza de estanques', 200.00, 'Trabajadores temporales'),
(5, '2023-05-02', 'Suministros', 'Compra de medicamentos para peces', 100.00, 'Producto X');

-- Puedes continuar agregando m�s inserciones seg�n sea necesario

CREATE PROCEDURE sp_listar_gastos
AS
BEGIN
    BEGIN TRANSACTION TX;

    -- Verifica si hay alg�n gasto en la tabla
    IF EXISTS (SELECT 1 FROM Gastos)
    BEGIN
        -- Si hay gastos, selecciona todos los gastos
        SELECT * FROM Gastos;
    END
    ELSE
    BEGIN
        ROLLBACK TRANSACTION TX;
        PRINT 'Error al intentar listar gastos. No hay registros.';
        RETURN;
    END

    -- Confirma la transacci�n
    COMMIT TRANSACTION TX;
END;



CREATE PROCEDURE sp_guardar_gasto
(
    @fechaGasto VARCHAR(20),
    @tipoGasto VARCHAR(50),
    @descripcion VARCHAR(255),
    @monto DECIMAL(10,2),
    @notas VARCHAR(50)
)
AS
BEGIN
    BEGIN TRANSACTION TX;

    -- Inserta el nuevo gasto
    INSERT INTO Gastos (FechaGasto, TipoGasto, Descripcion, Monto, Notas)
    VALUES (@fechaGasto, @tipoGasto, @descripcion, @monto, @notas);

    -- Verifica si se gener� un nuevo valor de GastoID
    IF SCOPE_IDENTITY() IS NOT NULL
    BEGIN
        COMMIT TRANSACTION TX;
        SELECT 'INSERCI�N EXITOSA' AS RESPUESTA;
    END
    ELSE
    BEGIN
        ROLLBACK TRANSACTION TX;
        SELECT 'HUBO UN ERROR. La inserci�n no se pudo realizar correctamente.' AS RESPUESTA;
    END
END;


CREATE PROCEDURE sp_eliminar_gasto
(
    @GastoID INT
)
AS
BEGIN
    BEGIN TRANSACTION TX;

    -- Verifica si el gasto a eliminar existe
    IF EXISTS (SELECT 1 FROM Gastos WHERE GastoID = @gastoID)
    BEGIN
        -- Si el gasto existe, procede con la eliminaci�n
        DELETE FROM Gastos WHERE GastoID = @gastoID;

        -- Verifica si se elimin� correctamente
        IF @@ROWCOUNT > 0
        BEGIN
            COMMIT TRANSACTION TX;
            SELECT 'ELIMINACI�N EXITOSA' AS RESPUESTA;
        END
        ELSE
        BEGIN
            ROLLBACK TRANSACTION TX;
            SELECT 'HUBO UN ERROR. La eliminaci�n no se pudo realizar correctamente.' AS RESPUESTA;
        END
    END
    ELSE
    BEGIN
        ROLLBACK TRANSACTION TX;
        SELECT 'HUBO UN ERROR. El gasto con ID ' + CONVERT(VARCHAR, @gastoID) + ' no existe.' AS RESPUESTA;
    END
END;


CREATE PROCEDURE sp_editar_gasto
(
    @gastoID INT,
    @fechaGasto VARCHAR(20) = NULL,
    @tipoGasto VARCHAR(50) = NULL,
    @descripcion VARCHAR(255) = NULL,
    @monto DECIMAL(10, 2) = NULL,
    @notas TEXT = NULL
)
AS
BEGIN
    BEGIN TRANSACTION TX;

    -- Intenta actualizar el gasto
    UPDATE Gastos
    SET
        fechaGasto = ISNULL(@fechaGasto, fechaGasto),
        tipoGasto = ISNULL(@tipoGasto, tipoGasto),
        descripcion = ISNULL(@descripcion, descripcion),
        monto = ISNULL(@monto, monto),
        notas = ISNULL(@notas, notas)
    WHERE gastoID = @gastoID;

    -- Verifica si se afect� alguna fila en la actualizaci�n
    IF @@ERROR > 0
    BEGIN
        -- Si hubo un error en la actualizaci�n, revierte la transacci�n
        ROLLBACK TRANSACTION TX;
        SELECT 'HUBO UN ERROR durante la actualizaci�n.' AS RESPUESTA;
    END
    ELSE
    BEGIN
        -- Verifica cu�ntas filas se actualizaron
        IF @@ROWCOUNT = 0
        BEGIN
            -- Si ninguna fila se actualiz�, revierte la transacci�n
            ROLLBACK TRANSACTION TX;
            SELECT 'HUBO UN ERROR. El gasto con gastoID ' + CONVERT(VARCHAR, @gastoID) + ' no existe.' AS RESPUESTA;
        END
        ELSE
        BEGIN
            -- Si se realiz� la actualizaci�n con �xito, confirma la transacci�n
            COMMIT TRANSACTION TX;
            SELECT 'ACTUALIZACI�N EXITOSA' AS RESPUESTA;
        END
    END
END;




CREATE PROCEDURE sp_obtener_total_gastos
AS
BEGIN
    -- Declarar una variable para almacenar el total de los montos
    DECLARE @totalMontos DECIMAL(10, 2);

    -- Obtener la suma de los montos de todos los gastos
    SELECT @totalMontos = SUM(Monto) FROM Gastos;

    -- Devolver el resultado
    SELECT @totalMontos AS TotalGastos;
END;



-- Ejecutar el procedimiento para listar gastos
EXEC sp_listar_gastos;

-- Ejecutar el procedimiento para guardar un nuevo gasto
EXEC sp_guardar_gasto '2023-11-14', 'Mantenimiento', 'Compra de herramientas', 50.00, 'Sin notas';

-- Ejecutar el procedimiento para editar un gasto existente
EXEC sp_editar_gasto 1, NULL, 'Nuevo Tipo', NULL, 75.00, NULL;

-- Ejecutar el procedimiento para eliminar un gasto
EXEC sp_eliminar_gasto 3;

-- Ejecutar el procedimiento para obtener el total de los gastos
EXEC sp_obtener_total_gastos;


