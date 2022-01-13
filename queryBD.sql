create database TestCrud
go

USE TestCrud
go

CREATE TABLE tRol
(
cod_rol INT IDENTITY PRIMARY KEY
, txt_desc VARCHAR(500)
, sn_activo INT
)
GO
INSERT INTO trol VALUES ( 'Administrador',-1)
INSERT INTO trol VALUES ( 'Cliente', -1)
GO
CREATE TABLE tUsers
(cod_usuario INT PRIMARY KEY IDENTITY, txt_user VARCHAR(50), txt_password
VARCHAR(50),txt_nombre VARCHAR(200), txt_apellido VARCHAR(200), nro_doc
VARCHAR(50), cod_rol INT, sn_activo INT
, CONSTRAINT fk_user_rol FOREIGN KEY (cod_rol) REFERENCES tRol(cod_rol)
)
GO
/*corregido..*/
INSERT INTO tUsers VALUES ( 'Admin', 'PassAdmin123', 'Administrador', 'Test', '1234321', 1,-1)
INSERT INTO tUsers VALUES ('userTest', 'Test1', 'Ariel', 'ApellidoConA', '12312321', 2, -1)
INSERT INTO tUsers VALUES ('userTest2', 'Test2', 'Bernardo', 'ApellidoConB', '12312322', 2, -1)
INSERT INTO tUsers VALUES ('userTest3', 'Test3', 'Carlos', 'ApellidoConC', '12312323', 2, -1)
GO
CREATE TABLE tPelicula (cod_pelicula INT PRIMARY KEY IDENTITY, txt_desc
VARCHAR(500), cant_disponibles_alquiler INT, cant_disponibles_venta INT,
precio_alquiler NUMERIC(18,2), precio_venta NUMERIC(18,2))
GO
INSERT INTO tPelicula VALUES ('Duro de matar III', 3, 0,1.5,5.0)
INSERT INTO tPelicula VALUES ('Todo Poderoso', 2,1,1.5,7.0)
INSERT INTO tPelicula VALUES ('Stranger than fiction', 1,1,1.5,8.0)
INSERT INTO tPelicula VALUES ('OUIJA', 0,2,2.0,20.50)
GO
CREATE TABLE tGenero (cod_genero INT PRIMARY KEY IDENTITY, txt_desc
VARCHAR(500) )
INSERT INTO tGenero VALUES('Acción')
INSERT INTO tGenero VALUES('Comedia')
INSERT INTO tGenero VALUES('Drama')
INSERT INTO tGenero VALUES('Terror')
GO
CREATE TABLE tGeneroPelicula (cod_pelicula INT, cod_genero INT
, PRIMARY KEY(cod_pelicula, cod_genero)
, CONSTRAINT fk_genero_pelicula FOREIGN KEY(cod_pelicula) REFERENCES
tpelicula(cod_pelicula)
, CONSTRAINT fk_pelicula_genero FOREIGN KEY(cod_genero) REFERENCES
tGenero(cod_genero))
GO
INSERT INTO tGeneroPelicula VALUES(1,1)
INSERT INTO tGeneroPelicula VALUES(2,2)
INSERT INTO tGeneroPelicula VALUES(3,2)
INSERT INTO tGeneroPelicula VALUES(3,3)
INSERT INTO tGeneroPelicula VALUES(4,4)
GO




CREATE TABLE tAlquiler
(cod_alquiler INT PRIMARY KEY IDENTITY,
 cod_usuario INT,
 total NUMERIC(18, 2),
 fecha DATETIME,
 CONSTRAINT fk_cod_usuarioAlquiler FOREIGN KEY (cod_usuario) REFERENCES TUsers(cod_usuario)
)


GO

CREATE TABLE TDetalleAlquiler(
cod_alquiler INT,
cod_detalleAlquiler INT,
cod_pelicula INT,
precio NUMERIC(18, 2),
fechaDevolucion DATETIME,
CONSTRAINT fk_cod_alquiler FOREIGN KEY(cod_alquiler) REFERENCES TAlquiler(cod_alquiler),
CONSTRAINT fk_cod_peliculaAlquiler FOREIGN KEY(cod_pelicula) REFERENCES TPelicula(cod_pelicula),
PRIMARY KEY(cod_alquiler,cod_detalleAlquiler)
)


go
CREATE TABLE tVenta
(cod_venta INT PRIMARY KEY IDENTITY,
 cod_usuario INT,
 total NUMERIC(18, 2),
 fecha DATETIME,
 CONSTRAINT fk_cod_usuarioVenta FOREIGN KEY (cod_usuario) REFERENCES TUsers(cod_usuario)
)

go

CREATE TABLE TDetalleVenta(
cod_venta INT,
cod_detalleVenta INT,
cod_pelicula INT,
cantidad INT,
precioUnidad NUMERIC(18, 2),
precioTotal NUMERIC(18, 2),
CONSTRAINT fk_cod_venta FOREIGN KEY(cod_venta) REFERENCES TVenta(cod_venta),
CONSTRAINT fk_cod_peliculaVenta FOREIGN KEY(cod_pelicula) REFERENCES TPelicula(cod_pelicula),
PRIMARY KEY(cod_venta,cod_detalleVenta)
)
go




/*------------ USUARIO----------*/
CREATE OR ALTER PROCEDURE crearUsuario 
@txtUser varchar(50),  
@txtPassword varchar(50),
@txtNombre varchar(200),
@txtApellido varchar(200),
@txtnro_doc varchar(50),
@cod_rol int,
@sn_activo int = -1
AS
 DECLARE @ErrorMessage NVARCHAR(4000);  
 DECLARE @ErrorSeverity INT;  
 DECLARE @ErrorState INT;  
BEGIN
BEGIN TRY

	
	IF (SELECT COUNT(*) FROM TUsers  
          WHERE nro_doc = @txtnro_doc) > 0 
BEGIN
		  RAISERROR ('Error, Nro. Documento ya existe.',16,1)      
END 
		IF (SELECT COUNT(*) FROM TUsers  
          WHERE txt_user = @txtUser) > 0 
BEGIN
		  RAISERROR ('Error, Usuario ya existe.',16,1)      
END 


insert into tUsers values(@txtUser,@txtPassword,@txtNombre,@txtApellido,@txtnro_doc,@cod_rol,@sn_activo) 

END TRY
		BEGIN CATCH
	 SELECT   
        @ErrorMessage = ERROR_MESSAGE(),  
        @ErrorSeverity = ERROR_SEVERITY(),  
        @ErrorState = ERROR_STATE();  
		RAISERROR(@ErrorMessage,@ErrorSeverity,@ErrorState);
	END CATCH;
end;

go

CREATE OR ALTER PROCEDURE listarUsuario 
AS
BEGIN

select * from tUsers 

END
go


/*--------------------PELICULAS-----------------*/
CREATE OR ALTER PROCEDURE crearPelicula
@txt_desc varchar(500),  
@cant_disponibles_alquiler int,
@cant_disponibles_venta int,
@precio_alquiler numeric(18, 2),
@precio_venta numeric(18, 2)
AS
BEGIN
insert into tPelicula values(@txt_desc,@cant_disponibles_alquiler,@cant_disponibles_venta,@precio_alquiler,@precio_venta)
END

GO
/*------------*/
CREATE OR ALTER PROCEDURE modificarPelicula
@cod_pelicula int,
@txt_desc varchar(500),  
@cant_disponibles_alquiler int,
@cant_disponibles_venta int,
@precio_alquiler numeric(18, 2),
@precio_venta numeric(18, 2)
AS
BEGIN
UPDATE tPelicula SET 
txt_desc = @txt_desc,
cant_disponibles_alquiler=@cant_disponibles_alquiler,
cant_disponibles_venta = @cant_disponibles_venta,
precio_alquiler = @precio_alquiler,
precio_venta = @precio_venta
WHERE cod_pelicula = @cod_pelicula;
END

GO
/*----*/
CREATE OR ALTER PROCEDURE borrarPelicula
@cod_pelicula int
AS
BEGIN
UPDATE tPelicula SET 
cant_disponibles_alquiler=0,
cant_disponibles_venta = 0
WHERE cod_pelicula = @cod_pelicula;
END

GO

/*-------GENERO-----*/
CREATE OR ALTER PROCEDURE crearGenero
@txt_desc varchar(500)
AS
BEGIN
insert into tGenero values(@txt_desc);
END

GO
/*----------GENERO-PELICULA---------*/
CREATE OR ALTER PROCEDURE asignarGenero
@cod_pelicula int,
@cod_genero int
AS
 DECLARE @ErrorMessage NVARCHAR(4000);  
 DECLARE @ErrorSeverity INT;  
 DECLARE @ErrorState INT;  
DECLARE @genero varchar(500);
BEGIN
BEGIN TRY

IF (SELECT COUNT(*) FROM tGeneroPelicula  
          WHERE cod_pelicula = @cod_pelicula and cod_genero=@cod_genero) = 1 
BEGIN
		  set @genero = (select txt_desc from tGenero where cod_genero=@cod_genero);
		  RAISERROR ('Error, Esta película ya tiene asignado el género %s',16,1,@genero)      
END 


insert into tGeneroPelicula values(@cod_pelicula,@cod_genero);
END TRY
	BEGIN CATCH
	 SELECT   
        @ErrorMessage = ERROR_MESSAGE(),  
        @ErrorSeverity = ERROR_SEVERITY(),  
        @ErrorState = ERROR_STATE();  
		RAISERROR(@ErrorMessage,@ErrorSeverity,@ErrorState);
	END CATCH;
END

GO

/*------- stock para alquiler ------*/
CREATE OR ALTER PROCEDURE stockAlquiler
AS
BEGIN
select * from tPelicula where cant_disponibles_alquiler>0;
END

GO
/*------- stock para venta ------*/
CREATE OR ALTER PROCEDURE stockVenta
AS
BEGIN
select * from tPelicula where cant_disponibles_venta>0;
END

GO
/*-------  Devolver película ------*/
CREATE OR ALTER PROCEDURE devolverPelicula
@cod_alquiler int,
@cod_detalleAlquiler int,
@cod_pelicula int
AS
BEGIN
UPDATE tPelicula set cant_disponibles_alquiler= cant_disponibles_alquiler+1 
WHERE cod_pelicula = @cod_pelicula;
UPDATE TDetalleAlquiler set fechaDevolucion=GETDATE()
WHERE cod_alquiler = @cod_alquiler and cod_detalleAlquiler=@cod_detalleAlquiler;
END

GO

/*--------Peliculas sin devoluciones-------*/
CREATE OR ALTER PROCEDURE peliculasSinDevoluciones
AS
BEGIN
SELECT p.cod_pelicula,p.txt_desc FROM tPelicula p join TDetalleAlquiler a ON p.cod_pelicula = a.cod_pelicula
							WHERE a.fechaDevolucion is null
							GROUP BY p.cod_pelicula,p.txt_desc
END

GO

/*-------  Usuarios que deben devolucion por pelicula ------*/
CREATE OR ALTER PROCEDURE usuariosSinDevolucionPorPelicula
@cod_pelicula INT
AS
BEGIN
SELECT u.cod_usuario,u.txt_user,u.txt_nombre,u.txt_apellido,u.nro_doc from tUsers u join tAlquiler a on u.cod_usuario = a.cod_usuario
			JOIN TDetalleAlquiler da on da.cod_alquiler= a.cod_alquiler
			WHERE da.fechaDevolucion is null and da.cod_pelicula=@cod_pelicula
			GROUP BY u.cod_usuario,u.txt_user,u.txt_nombre,u.txt_apellido,u.nro_doc
END

GO

/*-------  Recaudo de peliculas alquiladas ------*/
CREATE OR ALTER PROCEDURE recaudoPeliculasAlquiladas
AS
BEGIN
 SELECT p.txt_desc,count(*) as CantidadAlquilado,sum(a.precio) as Recaudado FROM TDetalleAlquiler a join tPelicula p ON p.cod_pelicula= a.cod_pelicula
 GROUP BY p.txt_desc
END


/*transacciones*/

GO
/*----Alquilar Pelicula----*/
CREATE OR ALTER PROCEDURE alquilarPelicula
@cod_usuario int
AS
 DECLARE @ErrorMessage NVARCHAR(4000);  
 DECLARE @ErrorSeverity INT;  
 DECLARE @ErrorState INT;  
Declare @cod_alquiler  int; 
BEGIN
BEGIN TRY

insert into tAlquiler values(@cod_usuario,0,GETDATE());
set @cod_alquiler = (SELECT SCOPE_IDENTITY());
select @cod_alquiler as cod_alquiler
END TRY
	BEGIN CATCH
	 SELECT   
        @ErrorMessage = ERROR_MESSAGE(),  
        @ErrorSeverity = ERROR_SEVERITY(),  
        @ErrorState = ERROR_STATE();  
		RAISERROR(@ErrorMessage,@ErrorSeverity,@ErrorState);
	END CATCH;
END

GO
CREATE OR ALTER PROCEDURE detalleAlquilerPelicula
@cod_alquiler int,
@cod_detalleAlquiler int,
@cod_pelicula int
AS
 DECLARE @ErrorMessage NVARCHAR(4000);  
 DECLARE @ErrorSeverity INT;  
 DECLARE @ErrorState INT; 
 DECLARE @precio numeric(18, 2);
 BEGIN
BEGIN TRY
set @precio = (select precio_alquiler from tPelicula where cod_pelicula = @cod_pelicula);
insert into TDetalleAlquiler values(@cod_alquiler,@cod_detalleAlquiler,@cod_pelicula,@precio,null);
UPDATE tAlquiler SET total = total+@precio
WHERE cod_alquiler=@cod_alquiler;
UPDATE tPelicula SET cant_disponibles_alquiler = cant_disponibles_alquiler-1
WHERE cod_pelicula=@cod_pelicula;
END TRY
	BEGIN CATCH
	 SELECT   
        @ErrorMessage = ERROR_MESSAGE(),  
        @ErrorSeverity = ERROR_SEVERITY(),  
        @ErrorState = ERROR_STATE();  
		RAISERROR(@ErrorMessage,@ErrorSeverity,@ErrorState);
	END CATCH;
END


GO
/*----Vender Pelicula------*/
CREATE OR ALTER PROCEDURE venderPelicula
@cod_usuario int
AS
 DECLARE @ErrorMessage NVARCHAR(4000);  
 DECLARE @ErrorSeverity INT;  
 DECLARE @ErrorState INT;  
Declare @cod_venta  int; 
BEGIN
BEGIN TRY

insert into tVenta values(@cod_usuario,0,GETDATE());
set @cod_venta = (SELECT SCOPE_IDENTITY());
select @cod_venta as cod_venta
END TRY
	BEGIN CATCH
	 SELECT   
        @ErrorMessage = ERROR_MESSAGE(),  
        @ErrorSeverity = ERROR_SEVERITY(),  
        @ErrorState = ERROR_STATE();  
		RAISERROR(@ErrorMessage,@ErrorSeverity,@ErrorState);
	END CATCH;
END

GO
/*----Detalle Venta Pelicula------*/
CREATE OR ALTER PROCEDURE detalleVentaPelicula
@cod_venta int,
@cod_detalleVenta int,
@cod_pelicula int,
@cantidad int
AS
 DECLARE @ErrorMessage NVARCHAR(4000);  
 DECLARE @ErrorSeverity INT;  
 DECLARE @ErrorState INT; 
 DECLARE @precio numeric(18, 2);
 DECLARE @preciototal numeric(18, 2);
 BEGIN
BEGIN TRY
set @precio = (select precio_venta from tPelicula where cod_pelicula = @cod_pelicula);
insert into TDetalleVenta values(@cod_venta,@cod_detalleVenta,@cod_pelicula,@cantidad,@precio,(@precio*@cantidad));
UPDATE tVenta SET total = total+(@precio*@cantidad)
WHERE cod_venta=@cod_venta;
UPDATE tPelicula SET cant_disponibles_venta = cant_disponibles_venta-@cantidad
WHERE cod_pelicula=@cod_pelicula;
END TRY
	BEGIN CATCH
	 SELECT   
        @ErrorMessage = ERROR_MESSAGE(),  
        @ErrorSeverity = ERROR_SEVERITY(),  
        @ErrorState = ERROR_STATE();  
		RAISERROR(@ErrorMessage,@ErrorSeverity,@ErrorState);
	END CATCH;
END


GO
/*--Verificacion de stock alquiler-venta-*/

CREATE OR ALTER PROCEDURE verificarStock
/*tipo stock: 1- cantidad stock alquiler
			  2- cantidad stock venta*/
@tipo_stock int,
@cod_pelicula int,
@cantidad int
AS
 DECLARE @ErrorMessage NVARCHAR(4000);  
 DECLARE @ErrorSeverity INT;  
 DECLARE @ErrorState INT; 
 DECLARE @txtDesc varchar(500);
 DECLARE @stockRestanteVenta INT;
BEGIN
BEGIN TRY

IF(@tipo_stock=1)
BEGIN
	IF(select count(*) from tPelicula where cant_disponibles_alquiler=0 and cod_pelicula=@cod_pelicula)>0
	BEGIN
	 set @txtDesc = (select txt_desc from tPelicula where cod_pelicula=@cod_pelicula);
	 RAISERROR ('Error, ya no hay stock alquiler para la película %s',16,1,@txtDesc);    
	END

END

IF(@tipo_stock=2)
BEGIN
	IF(@cantidad<1)
	BEGIN
	RAISERROR ('Error,ingrese una cantidad válida...',16,1);  
	END

	IF(SELECT COUNT(*) FROM tPelicula WHERE cant_disponibles_venta<@cantidad and cod_pelicula=@cod_pelicula)>0
	BEGIN
	 set @txtDesc = (select txt_desc from tPelicula where cod_pelicula=@cod_pelicula);
	 set @stockRestanteVenta = (select cant_disponibles_venta from tPelicula where cod_pelicula=@cod_pelicula);
	 RAISERROR ('Error, ya no hay stock alquiler para la película %s. El stock restante es: %i',16,1,@txtDesc,@stockRestanteVenta);    
	END
END

END TRY
	BEGIN CATCH
	 SELECT   
        @ErrorMessage = ERROR_MESSAGE(),  
        @ErrorSeverity = ERROR_SEVERITY(),  
        @ErrorState = ERROR_STATE();  
		RAISERROR(@ErrorMessage,@ErrorSeverity,@ErrorState);
	END CATCH;
END

GO
/*----Lista de usuarios que ya alquilaron--*/
CREATE OR ALTER PROCEDURE usuariosConAlquiler
AS
BEGIN
SELECT u.cod_usuario,u.txt_user,u.txt_nombre,u.txt_apellido,u.nro_doc FROM tUsers u join tAlquiler a on a.cod_usuario=u.cod_usuario
			group by u.cod_usuario,u.txt_user,u.txt_nombre,u.txt_apellido,u.nro_doc
					
END
/*Peliculas alquiladas por usuario*/
GO
CREATE OR ALTER PROCEDURE peliculasAlquiladasPorUsuario
@cod_usuario INT
AS
BEGIN
SELECT da.cod_alquiler,da.cod_detalleAlquiler, p.txt_desc, da.precio,a.fecha,da.fechaDevolucion 
FROM TDetalleAlquiler da join tPelicula p on p.cod_pelicula=da.cod_pelicula
join tAlquiler a on a.cod_alquiler=da.cod_alquiler
WHERE a.cod_usuario=@cod_usuario				
END

