Public Class Form1
    Dim InputFileList As New List(Of String)
    Dim OutputFileList As New List(Of String)
    Dim ExtractAudio As New List(Of Boolean)
    Dim CurrInputFile As String
    Dim CurrOutputFile As String
    Dim CurrRotation As Integer
    Public Sub ProcessStart(ByRef Executable, ByRef CommandLine)
        Dim P As New Process
        With P
            .StartInfo.FileName = Executable
            .StartInfo.Arguments = CommandLine
            .StartInfo.RedirectStandardOutput = False
            .StartInfo.RedirectStandardError = False
            .StartInfo.UseShellExecute = True
            .StartInfo.CreateNoWindow = False
            .Start()
            .WaitForExit()
        End With
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If TextBox1.Text = "" Then
            MsgBox("An Input video is needed")
        ElseIf TextBox2.Text = "" Then
            MsgBox("An Output location is needed")
        Else
            CurrInputFile = TextBox1.Text
            CurrOutputFile = TextBox2.Text
            ProcessStart("ffmpeg.exe", "-i """ & TextBox1.Text & """ """ & TextBox2.Text & """")
            If CheckBox1.Checked Then
                ProcessStart("ffmpeg.exe", "-i """ & TextBox1.Text & """ -vn """ & My.Computer.FileSystem.GetParentPath(TextBox2.Text) & "\" & IO.Path.GetFileNameWithoutExtension(TextBox2.Text) & ".wav""")
            End If
            MsgBox("Video processed!")
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If TextBox1.Text = "" = False Then OpenFileDialog1.FileName = My.Computer.FileSystem.GetName(TextBox1.Text) Else OpenFileDialog1.FileName = ""
        OpenFileDialog1.Filter = "All file types|*.*"
        OpenFileDialog1.Title = "Select a source video"
        Dim DialogOk As DialogResult = OpenFileDialog1.ShowDialog
        If DialogOk = Windows.Forms.DialogResult.OK Then If OpenFileDialog1.FileName = "" = False Then TextBox1.Text = OpenFileDialog1.FileName
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If TextBox2.Text = "" = False Then SaveFileDialog2.FileName = My.Computer.FileSystem.GetName(TextBox2.Text) Else SaveFileDialog2.FileName = ""
        SaveFileDialog2.Filter = "y4m|*.y4m"
        SaveFileDialog2.Title = "Browse for output"
        Dim DialogOk As DialogResult = SaveFileDialog2.ShowDialog
        If DialogOk = Windows.Forms.DialogResult.OK Then If SaveFileDialog2.FileName = "" = False Then TextBox2.Text = SaveFileDialog2.FileName
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If TextBox1.Text = "" Then
            MsgBox("An Input video is needed")
        ElseIf TextBox2.Text = "" Then
            MsgBox("An Output location is needed")
        Else
            ListBox1.Items.Add(My.Computer.FileSystem.GetName(TextBox1.Text))
            InputFileList.Add(TextBox1.Text)
            OutputFileList.Add(TextBox2.Text)
            ExtractAudio.Add(CheckBox1.Checked)
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim Index As Integer = ListBox1.SelectedIndex
        ListBox1.Items.RemoveAt(Index)
        InputFileList.RemoveAt(Index)
        OutputFileList.RemoveAt(Index)
        ExtractAudio.RemoveAt(Index)
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        If ListBox1.SelectedItem = "" = False Then Button4.Enabled = True Else Button4.Enabled = False
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Dim processeditems As Integer = 0
        For i As Integer = 0 To ListBox1.Items.Count - 1
            CurrInputFile = InputFileList(i)
            CurrOutputFile = OutputFileList(i)
            CurrRotation = ExtractAudio(i)
            ProcessStart("ffmpeg.exe", "-i """ & CurrInputFile & """ """ & CurrOutputFile & """")
            If ExtractAudio(i) Then
                ProcessStart("ffmpeg.exe", "-i """ & CurrInputFile & """ -vn """ & My.Computer.FileSystem.GetParentPath(CurrOutputFile) & "\" & IO.Path.GetFileNameWithoutExtension(CurrOutputFile) & ".wav""")
            End If
            processeditems = processeditems + 1
        Next
        MsgBox(processeditems & " Videos processed!")
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        ListBox1.Items.Clear()
        InputFileList.Clear()
        OutputFileList.Clear()
        ExtractAudio.Clear()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not My.Computer.FileSystem.FileExists("ffmpeg.exe") Then
            MsgBox("ffmpeg.exe not found. You'll be redirected to the ffmpeg download website")
        End If
    End Sub
End Class
