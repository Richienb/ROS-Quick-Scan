Imports System.IO
Imports System.Security
Imports System.Security.Cryptography
Public Class Form1

    '###################################################################
    '#####################-ROS-QuickScan-Code-##########################
    '#########-When-You-Edit-This-You-Comply-With-The-Licence###########
    '###################################################################

    Dim DialogVar As MsgBoxResult
    Dim TempPath As String = Path.GetTempPath()
    Dim md5Dir As String = (TempPath & "\ROSQuickScanLibrary.txt")

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If My.Computer.FileSystem.FileExists(TextBox1.Text) And My.Computer.FileSystem.FileExists(TempPath & "\ROSQuickScanLibrary.txt") = True Then
            If Scan() = True Then
                Me.BackgroundImage = My.Resources.Danger
                Label3.Text = ("Status: Dangerous")
                TextBox1.BackColor = Color.Red
            ElseIf Scan() = False Then
                Me.BackgroundImage = My.Resources.Safe
                Label3.Text = ("Status: Safe")
                TextBox1.BackColor = Color.Green
            Else
                DialogVar = MsgBox("An Error Has Occured: Scan Response Neither True Nor False", 0 + 16, "Scan Response Error")
            End If
            Button5.Enabled = True
        ElseIf TextBox1.Text = Nothing Then
            DialogVar = MsgBox("You Haven't Chosen A File To Scan", 0 + 48, "File Not Specified")
        ElseIf My.Computer.FileSystem.FileExists(TextBox1.Text) = False Then
            DialogVar = MsgBox("The File You Chose Does Not Exist", 0 + 48, "File Not Found")
        Else
            DialogVar = MsgBox("The Library Has Not Been Downloaded", 0 + 48, "Library Not Downloaded")
        End If
    End Sub

    Private Function Scan()
        Dim path As String = OpenFileDialog1.FileName
        TextBox1.Text = path

        Dim sample As String
        sample = md5_hash(path)
        TextBox2.Text = md5_hash(path)

        Using f As System.IO.FileStream = System.IO.File.OpenRead(md5Dir)
            Using s As System.IO.StreamReader = New System.IO.StreamReader(f)
                While Not s.EndOfStream
                    Dim line As String = s.ReadLine

                    If (line = sample) Then
                        Return True
                    Else
                        Return False
                    End If
                End While
            End Using
        End Using
        Return False
    End Function
    'Start MD5 Generator
    Function md5_hash(ByVal file_name As String)
        Return hash_generator("md5", file_name)
    End Function

    Function hash_generator(ByRef hash_type As String, ByRef file_name As String)

        Dim hash
        hash = MD5.Create

        Dim hashValue() As Byte

        Dim filestream As FileStream = File.OpenRead(file_name)
        filestream.Position = 0
        hashValue = hash.ComputeHash(filestream)
        Dim hash_hex = PrintByteArray(hashValue)

        filestream.Close()

        Return hash_hex
    End Function

    Public Function PrintByteArray(ByRef array() As Byte)

        Dim hex_value As String = ""

        Dim i As Integer
        For i = 0 To array.Length - 1

            hex_value += array(i).ToString("x2")
        Next i

        Return hex_value.ToLower
    End Function
    'End MD5 Generator
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        OpenFileDialog1.ShowDialog()
        If Not OpenFileDialog1.FileName = Nothing Then
            Try
                TextBox1.Text = OpenFileDialog1.FileName
            Catch Exception As Exception
                Try
                    TextBox1.Text = (OpenFileDialog1.FileName).ToString
                Catch ex As Exception
                    DialogVar = MsgBox("An Error Has Occured: Unable To Set The Textbox's Text To The FileDialog's Selected Path", 0 + 16, "Textbox File Print Error")
                End Try
            End Try
        End If
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If My.Computer.FileSystem.FileExists(TempPath & "\ROSQuickScanLibrary.txt") Then
            Label6.Text = "Installation Status: Installed"
            Button4.Enabled = True
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        If My.Computer.Network.IsAvailable Then
            If My.Computer.FileSystem.FileExists(TempPath & "\ROSQuickScanLibrary.txt") Then
                Label6.Text = "Installation Status: Updating..."
                delay(1)
                My.Computer.FileSystem.DeleteFile(TempPath & "\ROSQuickScanLibrary.txt")
                My.Computer.Network.DownloadFile("https://raw.githubusercontent.com/Richienb/ROS-Quick-Scan/master/ROSQuickScanLibrary.txt",
    (TempPath & "\ROSQuickScanLibrary.txt"))
                Application.Restart()
            Else
                Label6.Text = "Installation Status: Installing..."
                delay(1)
                My.Computer.Network.DownloadFile("https://raw.githubusercontent.com/Richienb/ROS-Quick-Scan/master/ROSQuickScanLibrary.txt",
    (TempPath & "\ROSQuickScanLibrary.txt"))
                Application.Restart()
            End If
        Else
            DialogVar = MsgBox("This Computer Is Not Connected To The Internet", 0 + 48, "Not Connected")
        End If
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        If My.Computer.FileSystem.FileExists(TempPath & "\ROSQuickScanLibrary.txt") Then
            Label6.Text = "Installation Status: Removing..."
            delay(1)
            My.Computer.FileSystem.DeleteFile(TempPath & "\ROSQuickScanLibrary.txt")
            Application.Restart()
        End If
    End Sub

    Private Sub delay(ByVal interval As Integer)
        Dim sw As New Stopwatch
        sw.Start()
        Do While sw.ElapsedMilliseconds < interval
            Application.DoEvents()
        Loop
        sw.Stop()
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        If Not TextBox2.Text = Nothing Then
            Clipboard.SetText(TextBox2.Text.ToString)
        End If
    End Sub
End Class
