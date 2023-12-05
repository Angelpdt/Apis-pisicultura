create database Administracion;

CREATE TABLE Gastos (
    GastoID INT PRIMARY KEY,
    FechaGasto varchar(20),
    TipoGasto VARCHAR(50),
    Descripcion VARCHAR(255),
    Monto DECIMAL(10, 2),
    Notas VARCHAR(50)
);


-- Inserción de algunos datos de gastos
INSERT INTO Gastos (GastoID, FechaGasto, TipoGasto, Descripcion, Monto, Notas)
VALUES
(1, '2023-01-15', 'Alimentación', 'Compra de alimento para peces', 500.00, 'Pago a proveedor A'),
(2, '2023-02-05', 'Mantenimiento', 'Reparación de bomba de agua', 300.00, 'Mano de obra incluida'),
(3, '2023-03-20', 'Suministros', 'Compra de redes y herramientas', 150.00, 'Pedido a proveedor B');

-- Otros ejemplos
INSERT INTO Gastos (GastoID, FechaGasto, TipoGasto, Descripcion, Monto, Notas)
VALUES
(4, '2023-04-10', 'Mano de Obra', 'Jornada de limpieza de estanques', 200.00, 'Trabajadores temporales'),
(5, '2023-05-02', 'Suministros', 'Compra de medicamentos para peces', 100.00, 'Producto X');

-- Puedes continuar agregando más inserciones según sea necesario

CREATE PROCEDURE sp_listar_gastos
AS
BEGIN
    BEGIN TRANSACTION TX;

    -- Verifica si hay algún gasto en la tabla
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

    -- Confirma la transacción
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

    -- Verifica si se generó un nuevo valor de GastoID
    IF SCOPE_IDENTITY() IS NOT NULL
    BEGIN
        COMMIT TRANSACTION TX;
        SELECT 'INSERCIÓN EXITOSA' AS RESPUESTA;
    END
    ELSE
    BEGIN
        ROLLBACK TRANSACTION TX;
        SELECT 'HUBO UN ERROR. La inserción no se pudo realizar correctamente.' AS RESPUESTA;
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
        -- Si el gasto existe, procede con la eliminación
        DELETE FROM Gastos WHERE GastoID = @gastoID;

        -- Verifica si se eliminó correctamente
        IF @@ROWCOUNT > 0
        BEGIN
            COMMIT TRANSACTION TX;
            SELECT 'ELIMINACIÓN EXITOSA' AS RESPUESTA;
        END
        ELSE
        BEGIN
            ROLLBACK TRANSACTION TX;
            SELECT 'HUBO UN ERROR. La eliminación no se pudo realizar correctamente.' AS RESPUESTA;
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

    -- Verifica si se afectó alguna fila en la actualización
    IF @@ERROR > 0
    BEGIN
        -- Si hubo un error en la actualización, revierte la transacción
        ROLLBACK TRANSACTION TX;
        SELECT 'HUBO UN ERROR durante la actualización.' AS RESPUESTA;
    END
    ELSE
    BEGIN
        -- Verifica cuántas filas se actualizaron
        IF @@ROWCOUNT = 0
        BEGIN
            -- Si ninguna fila se actualizó, revierte la transacción
            ROLLBACK TRANSACTION TX;
            SELECT 'HUBO UN ERROR. El gasto con gastoID ' + CONVERT(VARCHAR, @gastoID) + ' no existe.' AS RESPUESTA;
        END
        ELSE
        BEGIN
            -- Si se realizó la actualización con éxito, confirma la transacción
            COMMIT TRANSACTION TX;
            SELECT 'ACTUALIZACIÓN EXITOSA' AS RESPUESTA;
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


