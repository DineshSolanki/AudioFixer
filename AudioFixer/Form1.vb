'Audio Fixer Project By Dinesh Solanki
'DineshSolanki@gmx.us
'*Please Do Not Change This Comment
Imports System.IO

Public Class Form1

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If OpenFileDialog1.ShowDialog = DialogResult.OK Then
            DamFiletxt.Text = OpenFileDialog1.FileName
        End If
    End Sub

    Private Sub DamFiletxt_TextChanged(sender As Object, e As EventArgs) Handles DamFiletxt.TextChanged
        FixFiletxt.Text = ChangeName(DamFiletxt.Text)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If SaveFileDialog1.ShowDialog = DialogResult.OK Then
            FixFiletxt.Text = SaveFileDialog1.FileName
        End If
    End Sub

    Private Sub Form1_DragEnter(sender As Object, e As DragEventArgs) Handles MyBase.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If

    End Sub

    Private Sub Form1_DragDrop(sender As Object, e As DragEventArgs) Handles MyBase.DragDrop
        Dim files() As String = CType(e.Data.GetData(DataFormats.FileDrop), String())
        If files.Length = 1 Then
            For Each file In files
                Dim ext = Path.GetExtension(file)
                If ext.Equals(".mp3", StringComparison.CurrentCultureIgnoreCase) OrElse ext.Equals(".m4a", StringComparison.CurrentCultureIgnoreCase) OrElse ext.Equals(".aac", StringComparison.CurrentCultureIgnoreCase) Then
                    DamFiletxt.Text = Path.GetFullPath(file)
                End If
            Next
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles SetFFPathbtn.Click
        SetPathFunction()
    End Sub
    Public Sub SetPathFunction()

        If (FolderBrowserDialog1.ShowDialog() = DialogResult.OK) Then
            If CheckFile("SetPath") Then
                My.Settings.FFmpegPath = FolderBrowserDialog1.SelectedPath
                My.Settings.Save()
            Else
                MessageBox.Show("File Not Found", "Please select Correct Folder")
            End If
        End If



    End Sub
    Public Function CheckFile(ByVal sender As String) As Boolean
        If (sender = "SetPath") Then
            If File.Exists((FolderBrowserDialog1.SelectedPath & "\ffmpeg.exe")) Then
                Return True
            End If
        ElseIf ((sender = "DirectRun") AndAlso ((Not String.IsNullOrEmpty(My.Settings.FFmpegPath) AndAlso File.Exists((My.Settings.FFmpegPath & "\ffmpeg.exe"))) AndAlso (Not String.IsNullOrEmpty(DamFiletxt.Text) AndAlso Not String.IsNullOrEmpty(FixFiletxt.Text)))) Then
            Return True
        End If
        Return False
    End Function

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If File.Exists(FixFiletxt.Text) Then
            If (MessageBox.Show("Overwrite ?", "File Already Exist", MessageBoxButtons.YesNo) = DialogResult.Yes) Then
                File.Delete(FixFiletxt.Text)
            Else
                Exit Sub
            End If
        End If
        If CheckFile("DirectRun") Then
            'Process.Start("cmd", "/k cd " & My.Settings.FFmpegPath & " & ffmpeg -i " & """" & DamFiletxt.Text & """" & " -c copy " & """" & FixFiletxt.Text & """").WaitForExit()

            Dim oProcess As New Process()
            Dim oStartInfo As New ProcessStartInfo("CMD.EXE")
            oStartInfo.Arguments = "/c cd " & My.Settings.FFmpegPath & " & ffmpeg -i " & """" & DamFiletxt.Text & """" & " -c copy " & """" & FixFiletxt.Text & """"
            oStartInfo.UseShellExecute = False
            oStartInfo.RedirectStandardOutput = True
            oStartInfo.RedirectStandardError = True
            oProcess.StartInfo = oStartInfo
            oProcess.Start()
            Dim sOutput As String
            Using oStreamReader As System.IO.StreamReader = oProcess.StandardError
                sOutput = oStreamReader.ReadToEnd()
            End Using
            MessageBox.Show(sOutput)
            Process.Start("explorer.exe", ("/select," & FixFiletxt.Text))
        Else
            MessageBox.Show("FFMPEG library not found" & vbCrLf & "or Incorrect File Path")
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        AboutBox.Show()
    End Sub
End Class
