


CREATE procedure [Seguridad].[Seg_P_CONSULTA_APLICACIONID]
@PV_Aplicacion int
AS

       SELECT IdAplicacion,IdEmpresa,Nombre,Descripcion,TipoServidor,
				' ' as Datagrama, Link
       FROM Seguridad.Seg_APLICACION 
       Where idaplicacion = @PV_Aplicacion
       
       SELECT Parametro, 
			  Valor = CASE Encriptado
						WHEN 0 THEN Valor
						ELSE '' 
					   END, 
						Encriptado 
		FROM Seguridad.Seg_ParamAplicacion 
			WHERE idaplicacion = @PV_Aplicacion

	  SELECT sa.IdServidor, a.Nombre, sa.TipoServidor
	  FROM Seguridad.Seg_ServAplicacion  sa
				INNER JOIN Seguridad.Seg_Aplicacion a
				ON a.IdAplicacion = sa.IdServidor
	  		WHERE sa.IdAplicacion = @PV_Aplicacion

	  SELECT Puerto, IdRol, QueueIN, QueueOUT, MaxHilos, BackLog,
	         Separador, MaxReadIdleMs, MaxSendIdleMs, TipoTrama, PosTransaccion
	  FROM Seguridad.Seg_PuertoAplicacion
	  WHERE IdAplicacion = @PV_Aplicacion
