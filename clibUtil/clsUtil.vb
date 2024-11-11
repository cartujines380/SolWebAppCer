Imports System.IO
Imports System.Environment
Imports System.Diagnostics
Imports System.Xml
Imports System.Text
Imports System.Data
Imports System.Security.Cryptography

Namespace Util
    Public Class clsUtilitario


        Public Sub New()

        End Sub
#Region "Metodos de Convertidor a RecortSet"
        'Method Name : GetADORS
        'Description : Takes a DataSet and converts into a Recordset. The converted
        '              ADODB recordset is returned as a Recordset persisted XML string.
        'Output      : String containing ADODB formatted XML
        'Input parameters:
        '            1. DataSet object
        '            2. Database Name
        Public Function GetADORS(ByVal DS As DataSet, ByVal dbName As String) As String

            Try
                'Create a MemoryStream to contain the XML
                Dim mStream As New MemoryStream
                'Create an XmlWriter object, to write the formatted XML to the MemoryStream
                Dim xWriter As New XmlTextWriter(mStream, Nothing)

                'Additional formatting for XML
                xWriter.Indentation = 8
                xWriter.Formatting = Formatting.Indented
                'call this Sub to write the ADONamespaces
                WriteADONamespaces(xWriter)
                'call this Sub to write the ADO Recordset Schema
                WriteSchemaElement(DS, dbName, xWriter)
                'Call this sub to transform the data portion of the Dataset
                TransformData(DS, xWriter)
                'Flush all input to XmlWriter
                xWriter.Flush()

                'Prepare the return value
                mStream.Position = 0
                Dim Buffer As Array
                Buffer = Array.CreateInstance(GetType(Byte), mStream.Length)
                mStream.Read(Buffer, 0, mStream.Length)
                Dim TextConverter As New UTF8Encoding
                Return TextConverter.GetString(Buffer)

            Catch ex As Exception
                'Returns error message to the calling function.
                Err.Raise(100, ex.Source, ex.ToString)
            End Try

        End Function


        'Add ADO XML namespaces to the XML output
        Private Sub WriteADONamespaces(ByRef xWriter As XmlTextWriter)
            'Use the following line to change the encoding if special characters are required
            'writer.WriteProcessingInstruction("xml", "version='1.0' encoding='ISO-8859-1'")

            'Add XML start element
            xWriter.WriteStartElement("", "xml", "")

            'Append the ADO Recordset namespaces
            xWriter.WriteAttributeString("xmlns", "s", Nothing, "uuid:BDC6E3F0-6DA3-11d1-A2A3-00AA00C14882")
            xWriter.WriteAttributeString("xmlns", "dt", Nothing, "uuid:C2F41010-65B3-11d1-A29F-00AA00C14882")
            xWriter.WriteAttributeString("xmlns", "rs", Nothing, "urn:schemas-microsoft-com:rowset")
            xWriter.WriteAttributeString("xmlns", "z", Nothing, "#RowsetSchema")
            xWriter.Flush()
        End Sub

        'Add Schema element to the XML output
        Private Sub WriteSchemaElement(ByVal DS As DataSet, ByVal dbName As String, ByRef xWriter As XmlTextWriter)
            'write element Schema
            xWriter.WriteStartElement("s", "Schema", "uuid:BDC6E3F0-6DA3-11d1-A2A3-00AA00C14882")
            xWriter.WriteAttributeString("id", "RowsetSchema")

            'write element ElementType
            xWriter.WriteStartElement("s", "ElementType", "uuid:BDC6E3F0-6DA3-11d1-A2A3-00AA00C14882")

            'write the attributes for ElementType
            xWriter.WriteAttributeString("name", "", "row")
            xWriter.WriteAttributeString("content", "", "eltOnly")
            xWriter.WriteAttributeString("rs", "updatable", "urn:schemas-microsoft-com:rowset", "true")

            WriteSchema(DS, dbName, xWriter)
            'write the end element for ElementType
            xWriter.WriteFullEndElement()

            'write the end element for Schema
            xWriter.WriteFullEndElement()
            xWriter.Flush()
        End Sub

        'Add field definitions to the schema
        Private Sub WriteSchema(ByVal DS As DataSet, ByVal dbName As String, ByRef xWriter As XmlTextWriter)
            Dim i As Int32 = 1
            Dim DC As DataColumn

            For Each DC In DS.Tables(0).Columns

                DC.ColumnMapping = MappingType.Attribute

                xWriter.WriteStartElement("s", "AttributeType", "uuid:BDC6E3F0-6DA3-11d1-A2A3-00AA00C14882")
                'write all the attributes
                xWriter.WriteAttributeString("name", "", DC.ToString)
                xWriter.WriteAttributeString("rs", "number", "urn:schemas-microsoft-com:rowset", i.ToString)
                xWriter.WriteAttributeString("rs", "baseCatalog", "urn:schemas-microsoft-com:rowset", dbName)
                xWriter.WriteAttributeString("rs", "baseTable", "urn:schemas-microsoft-com:rowset", DC.Table.TableName.ToString)
                xWriter.WriteAttributeString("rs", "keycolumn", "urn:schemas-microsoft-com:rowset", DC.Unique.ToString)
                xWriter.WriteAttributeString("rs", "autoincrement", "urn:schemas-microsoft-com:rowset", DC.AutoIncrement.ToString)
                'write child element
                xWriter.WriteStartElement("s", "datatype", "uuid:BDC6E3F0-6DA3-11d1-A2A3-00AA00C14882")
                'write attributes
                xWriter.WriteAttributeString("dt", "type", "uuid:C2F41010-65B3-11d1-A29F-00AA00C14882", GetDatatype(DC.DataType.ToString))
                '   If DC.MaxLength < 0 And GetDatatype(DC.DataType.ToString) = "int" Then
                '       'xWriter.WriteAttributeString("dt", "maxlength", "uuid:C2F41010-65B3-11d1-A29F-00AA00C14882", "0")
                '   Else
                '       xWriter.WriteAttributeString("dt", "maxlength", "uuid:C2F41010-65B3-11d1-A29F-00AA00C14882", DC.MaxLength.ToString)
                '   End If
                xWriter.WriteAttributeString("rs", "maybenull", "urn:schemas-microsoft-com:rowset", DC.AllowDBNull.ToString)
                'write end element for datatype
                xWriter.WriteEndElement()
                'end element for AttributeType
                xWriter.WriteEndElement()
                xWriter.Flush()
                i = i + 1
            Next
            DC = Nothing
        End Sub

        'Function to get the ADO compatible datatype
        Private Function GetDatatype(ByVal DType As String) As String
            Select Case (DType)
                Case "System.Int32", "System.Int16", "System.Integer"
                    Return "int"
                Case "System.DateTime"
                    Return "dateTime.iso8601tz"
                Case "System.String"
                    Return "string"
                Case "System.Byte[]"
                    Return "bin.hex"
                Case "System.Boolean"
                    Return "boolean"
                Case "System.Guid"
                    Return "guid"
                Case Else
                    Return "string"
            End Select
        End Function


        'Transform the data format to ADO Recordset data format
        'This only transforms the data
        Private Sub TransformData(ByVal DS As DataSet, ByRef xWriter As XmlTextWriter)
            'Loop through DataSet and add data to XML
            xWriter.WriteStartElement("", "rs:data", "")
            Dim i As Long
            Dim j As Integer
            'For each row...
            For i = 0 To DS.Tables(0).Rows.Count - 1
                'Write the start element for the row
                xWriter.WriteStartElement("", "z:row", "")
                'For each field in the row...
                For j = 0 To DS.Tables(0).Columns.Count - 1
                    'Write the attribute that describes this field and it's value
                    If DS.Tables(0).Columns(j).DataType.ToString = "System.Byte[]" Then
                        'Binary data must be properly encoded (bin.hex)
                        If Not IsDBNull(DS.Tables(0).Rows(i).Item(DS.Tables(0).Columns(j).ColumnName)) Then
                            xWriter.WriteAttributeString(DS.Tables(0).Columns(j).ColumnName, DataToBinHex(DS.Tables(0).Rows(i).Item(DS.Tables(0).Columns(j).ColumnName)))
                        End If
                    Else
                        If Not IsDBNull(DS.Tables(0).Rows(i).Item(DS.Tables(0).Columns(j).ColumnName)) Then
                            xWriter.WriteAttributeString(DS.Tables(0).Columns(j).ColumnName, CType(DS.Tables(0).Rows(i).Item(DS.Tables(0).Columns(j).ColumnName), String))
                        End If
                    End If
                Next
                'End the row element
                xWriter.WriteEndElement()
            Next
            'Write the end element for rs:data
            xWriter.WriteEndElement()
            'Write the end element for xml
            xWriter.WriteEndElement()
            xWriter.Flush()
        End Sub

        'Helper function - encodes binary data to a bin.hex string
        Private Function DataToBinHex(ByVal thisData As Byte()) As String
            Dim sb As New StringBuilder
            Dim i As Integer = 0
            For i = 0 To thisData.Length - 1
                'First nibble of byte (4 most significant bits)
                sb.Append(Hex((thisData(i) And &HF0) / 2 ^ 4))
                'Second nibble of byte (4 least significant bits)
                sb.Append(Hex(thisData(i) And &HF))
            Next
            Return sb.ToString
        End Function

#End Region

        '#Region "Recuperación de Tablas"
        '        Shared Function getDCTableDescription(ByVal PI_TableCod As String, ByVal PI_ItemCod As String)
        '            Dim Path As String = System.AppDomain.CurrentDomain.BaseDirectory()
        '            Dim Retorno As String
        '            Dim xmlDoc As XmlDocument = New XmlDocument()
        '            Dim elemForm As XmlElement
        '            Path = Path + "\xml\DCTablas.xml"
        '            Try
        '                xmlDoc.Load(Path)
        '                elemForm = CType(xmlDoc.DocumentElement.SelectSingleNode( _
        '                                    "//Tabla[@Id='" + PI_TableCod + _
        '                                    "']/Row[@Codigo='" + PI_ItemCod + "']"), XmlElement)
        '                Retorno = elemForm.GetAttribute("Descripcion")
        '            Catch ex As Exception
        '                Retorno = ""
        '            End Try
        '            Return Retorno
        '        End Function
        '#End Region

        Public Function Encrypt(clearText As String) As String
            Dim EncryptionKey As String = "S1pec0m"
            Dim clearBytes As Byte() = Encoding.Unicode.GetBytes(clearText)
            Using encryptor As Aes = Aes.Create()
                Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D, _
                 &H65, &H64, &H76, &H65, &H64, &H65, _
                 &H76})
                encryptor.Key = pdb.GetBytes(32)
                encryptor.IV = pdb.GetBytes(16)
                Using ms As New MemoryStream()
                    Using cs As New CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write)
                        cs.Write(clearBytes, 0, clearBytes.Length)
                        cs.Close()
                    End Using
                    clearText = Convert.ToBase64String(ms.ToArray())
                End Using
            End Using
            Return clearText
        End Function

        Public Function Decrypt(cipherText As String) As String
            Dim EncryptionKey As String = "S1pec0m"
            cipherText = cipherText.Replace(" ", "+")
            Dim cipherBytes As Byte() = Convert.FromBase64String(cipherText)
            Using encryptor As Aes = Aes.Create()
                Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D, _
                 &H65, &H64, &H76, &H65, &H64, &H65, _
                 &H76})
                encryptor.Key = pdb.GetBytes(32)
                encryptor.IV = pdb.GetBytes(16)
                Using ms As New MemoryStream()
                    Using cs As New CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write)
                        cs.Write(cipherBytes, 0, cipherBytes.Length)
                        cs.Close()
                    End Using
                    cipherText = Encoding.Unicode.GetString(ms.ToArray())
                End Using
            End Using
            Return cipherText
        End Function

    End Class
End Namespace
