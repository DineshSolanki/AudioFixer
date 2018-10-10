Imports System.IO

Module Functions
    Public Function ChangeName(ByVal fileName As String) As String
        Dim extension As String = Path.GetExtension(fileName)
        Dim fileNameWithoutExtension As String = Path.GetFileNameWithoutExtension(fileName)
        Dim expression As String = fileName
        Return Replace(Replace(expression, extension, ".m4a", 1, -1, CompareMethod.Binary), fileNameWithoutExtension, ("(Fixed)" & fileNameWithoutExtension), 1, -1, CompareMethod.Binary)
    End Function
    
    
End Module
