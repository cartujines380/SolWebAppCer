

CREATE procedure [dbo].[Pro_P_FrameBACKUP]

	@ruta nvarchar(100), --ejemplo: 'C:\Sipecom\SQL_BACKUP\'

	@tipo nvarchar(1), -- F=Full, D=Diferencial

	@dia nvarchar(100) = null

as

begin



declare @lenguaje nvarchar(256), @archivo nvarchar(100), @archivoPuro nvarchar(100)



if (@dia is null)

	begin

		select @lenguaje = @@LANGUAGE



		set language 'us_english'



		select @dia =

			CASE lower(DATENAME(weekday, getdate()))

				WHEN 'monday' THEN 'lunes'

				WHEN 'tuesday' THEN 'martes'

				WHEN 'wednesday' THEN 'miercoles'

				WHEN 'thursday' THEN 'jueves'

				WHEN 'friday' THEN 'viernes'

				WHEN 'saturday' THEN 'sabado'

				WHEN 'sunday' THEN 'domingo'

				ELSE 'nd'

			END



		set language @lenguaje

	end



if  (@tipo = 'F')

	begin

		select  @archivo    =  @ruta + 'SIPE_FRAMEWORK_backup_' + @dia + '.bak'

		select  @archivoPuro = @ruta + 'SIPE_FRAMEWORK_backup_' + @dia

		BACKUP DATABASE [SIPE_FRAMEWORK] TO  DISK = @archivo

			WITH NOFORMAT, INIT,  NAME = @archivoPuro, SKIP, REWIND, NOUNLOAD,  STATS = 10

	end

	else

	begin

		select  @archivo=@ruta + 'SIPE_FRAMEWORK_backup_' + @dia + '.bak'

		select  @archivoPuro = @ruta + 'SIPE_FRAMEWORK_backup_' + @dia

		BACKUP DATABASE [SIPE_FRAMEWORK] TO  DISK = @archivo

			WITH  DIFFERENTIAL , NOFORMAT, INIT,  NAME = @archivoPuro, SKIP, REWIND, NOUNLOAD,  STATS = 10

	end



end



