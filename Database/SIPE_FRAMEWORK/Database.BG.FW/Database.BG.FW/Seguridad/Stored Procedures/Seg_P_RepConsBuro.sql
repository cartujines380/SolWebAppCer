CREATE PROCEDURE [Seguridad].[Seg_P_RepConsBuro]
@PI_Fecha varchar(10) --YYYYMM
AS
   DECLARE @VL_FechaR1 datetime ,@VL_FechaR2 datetime
  SET  @VL_FechaR1 = convert(datetime,substring(@PI_Fecha,5,2) + '-01-' + 
		     substring(@PI_Fecha,1,4) + ' 00:00:01')
  SET  @VL_FechaR2 = dateadd(mm,1,@VL_FechaR1)

  SELECT IdUsuario Usuario, case IdTransaccion
		    WHEN 1 THEN 'Local'
		    WHEN 2 THEN 'Buro'
		    END Consulta, count(*) Cantidad
  FROM Seguridad.Seg_Auditoria
  WHERE IdOrganizacion=101 and IdTransaccion in (1,2)
       AND FechaMovi >= @VL_FechaR1 AND FechaMovi < @VL_FechaR2 
  GROUP BY IdUsuario,IdTransaccion





