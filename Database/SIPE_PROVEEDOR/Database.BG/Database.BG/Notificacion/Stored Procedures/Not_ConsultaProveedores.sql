



CREATE PROCEDURE [Notificacion].[Not_ConsultaProveedores]
@PI_ParamXML xml
	
AS
SET ARITHABORT ON;
BEGIN TRY
	
	Declare @CodNot int
	Declare @Estado varchar(1)
	Declare @TipoNot varchar(1)
	Declare @Accion varchar(1)
    select  @CodNot=nref.value('@CodNotificacion','int')
    from @PI_ParamXML.nodes('/Root/SecNotificacion') AS R(nref) 
	select  @Accion=nref.value('@Accion','varchar(1)')
    from @PI_ParamXML.nodes('/Root/SecNotificacion') AS R(nref) 
	

begin
  
  if (@Accion = 'G')
  begin
    
	select top 2 Ruta, Comunicado from Notificacion.Notificacion where TipoCorreo = 'G' and Estado = 'E' 
	order by Codigo desc
  end
  else
  begin

   Select  @Estado = [Estado],
           @TipoNot = TipoCorreo
	  FROM [Notificacion].[Notificacion]
	  where [Codigo] = @CodNot 

 
  if (@Estado = 'E' and @TipoNot = 'N')
  begin
  SELECT  a.Usuario Cod_proveedor,
          b.Ruc,
		  b.NomComercial,
          case a.Estado when'L' then 'Leído' when'X' then 'Leído' else 'Pendiente' end as Estado  ,
		  CONVERT(VARCHAR(50),a.FecAceptacion,120) as FecAceptacion
  FROM [Notificacion].[Notificacion_Proveedor] a, [Proveedor].[Pro_Proveedor] b
	  where a.Cod_Notificacion = @CodNot and a.Cod_proveedor = b.CodProveedor
	  and a.Estado in ('L', 'X')
  end
  else
  begin
  SELECT  a.Cod_proveedor,
          b.Ruc,
		  b.NomComercial,
          case a.Estado when'L' then 'Leído' when'X' then 'Leído' else 'Pendiente' end as Estado  ,
		  CONVERT(VARCHAR(50),a.FecAceptacion,120) as FecAceptacion
  FROM [Notificacion].[Notificacion_Proveedor] a, [Proveedor].[Pro_Proveedor] b
	  where a.Cod_Notificacion = @CodNot and a.Cod_proveedor = b.CodProveedor
	 
  end

  end
		
end



END TRY
BEGIN CATCH
	exec SP_PROV_Error @sp='[Not_ConsultaProveedores]'
END CATCH






