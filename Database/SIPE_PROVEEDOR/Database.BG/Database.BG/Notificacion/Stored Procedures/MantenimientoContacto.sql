
CREATE PROCEDURE [Notificacion].[MantenimientoContacto]
	@PI_ParamXML xml
AS
   Declare @Identificacion varchar(13);
	Declare @Nombre varchar(80);
SET ARITHABORT ON;
BEGIN TRY
   
   SELECT 
		 @Identificacion = CASE WHEN nref.value('@Identificacion','varchar(13)') = '' THEN NULL ELSE nref.value('@Identificacion','varchar(13)') END,
		 @Nombre = CASE WHEN nref.value('@Nombres','varchar(80)') = '' THEN NULL ELSE nref.value('@Nombres','varchar(80)') END
	FROM @PI_ParamXML.nodes('/Root') as item(nref) 
    
	BEGIN TRAN
	IF EXISTS (SELECT TOP 1 1 FROM @PI_ParamXML.nodes('/Root') AS item(nref) where nref.value('@accion','char(1)') = 'C')
	BEGIN
	  

	  if(@Nombre is null)
	  Begin
	  SELECT  Identificacion
           ,Nombres
           ,Apellidos
           ,FechaNac
           ,Direccion
           ,Telefono
           ,Celular
		   ,Estado, Email FROM   dbo.contacto
		   where Identificacion = isnull(@Identificacion,Identificacion)
		   and estado = 'A'
		   
	  End
	  else
	  Begin
	  SELECT  Identificacion
           ,Nombres
           ,Apellidos
           ,FechaNac
           ,Direccion
           ,Telefono
           ,Celular
		   ,Estado, Email FROM   dbo.contacto
		   where Identificacion = isnull(@Identificacion,Identificacion)
		   and (Nombres like '%'+ @Nombre +'%' or Apellidos like '%'+ @Nombre +'%')
		   and estado = 'A'
	  End
	  
	END


	IF EXISTS (SELECT TOP 1 1 FROM @PI_ParamXML.nodes('/Root') AS item(nref) where nref.value('@accion','char(1)') = 'I')
	BEGIN
		
		INSERT INTO dbo.contacto
           (Identificacion
           ,Nombres
           ,Apellidos
           ,FechaNac
           ,Direccion
           ,Telefono
           ,Celular
		   ,Estado, Email)
		SELECT 
				nref.value('@Identificacion','varchar(13)'),
				nref.value('@Nombres','varchar(50)'),
				nref.value('@Apellidos','varchar(50)'),
				Convert(date,nref.value('@FechaNacimiento','varchar(10)')),
				
				nref.value('@Direccion','varchar(50)'),
				nref.value('@Telefono','varchar(30)'),
				nref.value('@Celular','varchar(10)'),			
				'A',	
				nref.value('@Email','varchar(50)')
		FROM @PI_ParamXML.nodes('/Root') as item(nref)
		WHERE nref.value('@accion','char(1)') = 'I';


		UPDATE det
		SET 
		
			Nombres = 	nref.value('@Nombres','varchar(50)'),
			Apellidos = 	nref.value('@Apellidos','varchar(50)'),
			FechaNac = 	Convert(date,nref.value('@FechaNacimiento','varchar(10)')),
				
			Direccion= 	nref.value('@Direccion','varchar(50)'),
			Telefono =	nref.value('@Telefono','varchar(30)'),
			Celular = 	nref.value('@Celular','varchar(10)'),			
			Estado =	'A',	
			Email = 	nref.value('@Email','varchar(50)')
		
		FROM dbo.contacto det
		INNER JOIN @PI_ParamXML.nodes('/Root') item(nref) 
			ON (det.Identificacion= nref.value('@Identificacion','char(13)')  
				AND nref.value('@accion','char(1)') = 'U');
		
		 
		

		
	END

	IF EXISTS (SELECT TOP 1 1 FROM @PI_ParamXML.nodes('/Root') AS item(nref) where nref.value('@accion','char(1)') = 'U')
	BEGIN
		


		UPDATE det
		SET 
		
			Nombres = 	nref.value('@Nombres','varchar(50)'),
			Apellidos = 	nref.value('@Apellidos','varchar(50)'),
			FechaNac = 	Convert(date,nref.value('@FechaNacimiento','varchar(10)')),
				
			Direccion= 	nref.value('@Direccion','varchar(50)'),
			Telefono =	nref.value('@Telefono','varchar(30)'),
			Celular = 	nref.value('@Celular','varchar(10)'),			
			Estado =	'A',	
			Email = 	nref.value('@Email','varchar(50)')
		
		FROM dbo.contacto det
		INNER JOIN @PI_ParamXML.nodes('/Root') item(nref) 
			ON (det.Identificacion= nref.value('@Identificacion','char(13)')  
				AND nref.value('@accion','char(1)') = 'U');
		
		 
		

		
	END

	IF EXISTS (SELECT TOP 1 1 FROM @PI_ParamXML.nodes('/Root') AS item(nref) where nref.value('@accion','char(1)') = 'D')
	BEGIN
		

		--Eliminacion Logica
		--UPDATE det
		--SET 
		--	Estado =	'I'	
		--FROM dbo.contacto det
		--INNER JOIN @PI_ParamXML.nodes('/Root') item(nref) 
		--	ON (det.Identificacion= nref.value('@Identificacion','char(13)')  
		--		AND nref.value('@accion','char(1)') = 'D');
		
		
		DELETE det		
		FROM dbo.contacto det
		INNER JOIN @PI_ParamXML.nodes('/Root') item(nref) 
			ON (det.Identificacion= nref.value('@Identificacion','char(13)')  
				AND nref.value('@accion','char(1)') = 'D');

		
	END

	IF @@TRANCOUNT > 0
			COMMIT	TRAN
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRAN
		
		exec SP_PROV_Error @sp='[Ped_P_ActualizaEstadoPedido]'
END CATCH


