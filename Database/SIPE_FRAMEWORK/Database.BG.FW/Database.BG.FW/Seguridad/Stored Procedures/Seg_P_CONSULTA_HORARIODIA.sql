



create procedure [Seguridad].[Seg_P_CONSULTA_HORARIODIA]
@PV_idHorario              int
AS
     SELECT idhorariodia,
		substring(convert(char,horainicio,109),13,5) + ' '
		+ substring(convert(char,horainicio,109),25,2)
--convert(char,horainicio,108) --'HH:MI AM'
		 as HoraInicio,
		substring(convert(char,horafin,109),13,5) + ' '
		+ substring(convert(char,horafin,109),25,2)
             --convert(char,horafin,108) --'HH:MI AM')
		 as HoraFin,dias
      FROM Seguridad.Seg_HORARIODIA
      WHERE idhorario=@PV_idHorario
      order by idhorariodia
    






