
CREATE PROCEDURE [Seguridad].[Seg_MantUsuario]
	@PI_ParamXML xml
AS
SET ARITHABORT ON;
BEGIN TRY
	DECLARE @tipo char(2)
	DECLARE @email nvarchar(256)
	DECLARE @Id nvarchar(128)
	DECLARE @PasswordHash nvarchar(max)
	
	SELECT
		@tipo = nref.value('@tipo','char(2)'),
		@email = nref.value('@Email','nvarchar(256)'),
		@Id = nref.value('@Id','nvarchar(128)'),
		@PasswordHash = nref.value('@PasswordHash','nvarchar(max)')
	FROM @PI_ParamXML.nodes('/Root') AS R(nref)
	
	if(@tipo='RE')
	BEGIN
		INSERT INTO [Seguridad].[AspNetUsers]
           ([Id]
           ,[Ruc]
           ,[Email]
           ,[EmailConfirmed]
           ,[PasswordHash]
           ,[SecurityStamp]
           ,[PhoneNumber]
           ,[PhoneNumberConfirmed]
           ,[TwoFactorEnabled]
           --,[LockoutEndDateUtc]
           ,[LockoutEnabled]
           ,[AccessFailedCount]
           ,[UserName])
			SELECT nref.value('@Id','nvarchar(128)'),
			nref.value('@Ruc','nvarchar(13)'),
			nref.value('@Email','nvarchar(256)'),
			0,
			nref.value('@PasswordHash','nvarchar(max)'),
			nref.value('@SecurityStamp','nvarchar(max)'),
			nref.value('@PhoneNumber','nvarchar(max)'),
			0,
			0,
			--ref.value('@LockoutEndDateUtc','datetime'),
			nref.value('@LockoutEnabled','bit'),
			nref.value('@AccessFailedCount','int'),
			nref.value('@UserName','nvarchar(256)')
		FROM @PI_ParamXML.nodes('/Root') AS R(nref)
	END
	ELSE IF(@tipo='AC')----confirmacion de correo
	BEGIN
		update [Seguridad].[AspNetUsers] with(rowlock)
		set EmailConfirmed=1
		where Email=@email
	END
	ELSE IF(@tipo='PA')--recuperar password
	BEGIN
		update [Seguridad].[AspNetUsers] with(rowlock)
		set PasswordHash=@PasswordHash
		where Id=@Id
	END
	ELSE IF(@tipo='CO')--consulta
	BEGIN
		select [Email],[EmailConfirmed],[PasswordHash],UserName, Id, Ruc from [Seguridad].[AspNetUsers] with(nolock)
		where Email=@email
	END
	ELSE IF(@tipo='CP')--consulta con Password
	BEGIN
		select [Email],[EmailConfirmed],[PasswordHash],UserName, Id, Ruc from [Seguridad].[AspNetUsers] with(nolock)
		where Email=@email
		and PasswordHash=@PasswordHash
	END

END TRY
BEGIN CATCH
	exec SP_PROV_Error @sp='[Pro_P_ConsultaSolProvBancoId]'
END CATCH

