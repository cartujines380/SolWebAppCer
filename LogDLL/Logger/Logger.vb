Imports System.IO
Imports System.Text

Public Class Logger
    Public FilePath As String
    Public Sub New()

    End Sub
    Public Sub WriteMensaje(msg As String)
        Try
            FilePath = FilePath & "_" & Date.Today.ToString("yyyyMMdd") & ".txt"
            Dim sw As New System.IO.StreamWriter(FilePath, True)
            sw.WriteLine("[" & Date.Now.ToString("yyyy/MM/dd hh:mm:ss:ms") & "]  -> " & msg & vbLf)
            sw.Close()
        Catch ex As Exception

        End Try

        
    End Sub
    Public Sub Linea()
        Try
            'FilePath = FilePath & "_" & Date.Today.ToString("yyyyMMdd") & ".txt"
            Dim sw As New System.IO.StreamWriter(FilePath, True)
            sw.WriteLine("=======================================================")
            sw.Close()
        Catch ex As Exception

        End Try
        

    End Sub
End Class
