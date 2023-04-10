Imports System.IO
Imports System.Math
Public Class Form1
    Dim startCoordinate As coordinate
    Dim endCoordinate As coordinate
    Dim startCoordSelected As Boolean
    Dim currentMap(1, 1) As gridSquare
    Dim size As Integer
    Public Structure coordinate
        Public _x As Integer
        Public _y As Integer
    End Structure
    Public Structure gridSquare
        Public _up As Boolean
        Public _down As Boolean
        Public _left As Boolean
        Public _right As Boolean
    End Structure

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.MouseDown
        Dim screen As New Bitmap(16 * size, 16 * size)
        Dim gfx As Graphics = Graphics.FromImage(screen)
        If startCoordSelected Then
            endCoordinate._x = (MousePosition.X - Me.Location.X - 8) \ 16
            endCoordinate._y = (MousePosition.Y - Me.Location.Y - 32) \ 16
            startCoordSelected = False
            Dim xDiff As Integer
            Dim yDiff As Integer
            xDiff = (startCoordinate._x - endCoordinate._x)
            yDiff = (startCoordinate._y - endCoordinate._y)
            If (Abs(xDiff) = 0 And Abs(yDiff) = 1) Or (Abs(xDiff) = 1 And Abs(yDiff) = 0) Then
                If xDiff > 0 Then
                    currentMap(startCoordinate._x, startCoordinate._y)._left = Not currentMap(startCoordinate._x, startCoordinate._y)._left
                ElseIf xDiff < 0 Then
                    currentMap(startCoordinate._x, startCoordinate._y)._right = Not currentMap(startCoordinate._x, startCoordinate._y)._right
                End If
                If yDiff > 0 Then
                    currentMap(startCoordinate._x, startCoordinate._y)._up = Not currentMap(startCoordinate._x, startCoordinate._y)._up
                ElseIf yDiff < 0 Then
                    currentMap(startCoordinate._x, startCoordinate._y)._down = Not currentMap(startCoordinate._x, startCoordinate._y)._down
                End If
                If xDiff > 0 Then
                    currentMap(endCoordinate._x, endCoordinate._y)._right = Not currentMap(endCoordinate._x, endCoordinate._y)._right
                ElseIf xDiff < 0 Then
                    currentMap(endCoordinate._x, endCoordinate._y)._left = Not currentMap(endCoordinate._x, endCoordinate._y)._left
                End If
                If yDiff > 0 Then
                    currentMap(endCoordinate._x, endCoordinate._y)._down = Not currentMap(endCoordinate._x, endCoordinate._y)._down
                ElseIf yDiff < 0 Then
                    currentMap(endCoordinate._x, endCoordinate._y)._up = Not currentMap(endCoordinate._x, endCoordinate._y)._up
                End If
                RenderScreen()
            Else
                MsgBox("Invalid placement")
            End If
        Else
            startCoordinate._x = (MousePosition.X - Me.Location.X - 8) \ 16
            startCoordinate._y = (MousePosition.Y - Me.Location.Y - 32) \ 16
            startCoordSelected = True
        End If
    End Sub

    Public Sub RenderScreen()
        Dim bmp As New Bitmap(size * 16, size * 16)
        Dim gfx As Graphics = Graphics.FromImage(bmp)
        For x = 0 To size - 1
            For y = 0 To size - 1
                If currentMap(x, y)._up Then
                    Dim triange() As Point = {New Point(x * 16 + 5, y * 16), New Point(x * 16 + 5, y * 16 + 10), New Point(x * 16 + 10, y * 16 + 10), New Point(x * 16 + 10, y * 16)}
                    gfx.FillPolygon(Brushes.Gray, triange)
                End If
                If currentMap(x, y)._down Then
                    Dim triange() As Point = {New Point(x * 16 + 5, y * 16 + 16), New Point(x * 16 + 5, y * 16 + 5), New Point(x * 16 + 10, y * 16 + 5), New Point(x * 16 + 10, y * 16 + 16)}
                    gfx.FillPolygon(Brushes.Gray, triange)
                End If
                If currentMap(x, y)._left Then
                    Dim triange() As Point = {New Point(x * 16, y * 16 + 5), New Point(x * 16 + 10, y * 16 + 5), New Point(x * 16 + 10, y * 16 + 10), New Point(x * 16, y * 16 + 10)}
                    gfx.FillPolygon(Brushes.Gray, triange)
                End If
                If currentMap(x, y)._right Then
                    Dim triange() As Point = {New Point(x * 16 + 5, y * 16 + 5), New Point(x * 16 + 16, y * 16 + 5), New Point(x * 16 + 16, y * 16 + 10), New Point(x * 16 + 5, y * 16 + 10)}
                    gfx.FillPolygon(Brushes.Gray, triange)
                End If
            Next
        Next
        For x = 0 To size - 1
            For y = 0 To size - 1
                gfx.FillRectangle(Brushes.Gray, x * 16 + 5, y * 16 + 5, 5, 5)
            Next
        Next

        PictureBox1.Image = bmp
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim newMap(19, 19) As gridSquare
        currentMap = newMap
        RenderScreen()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim continueLoop As Boolean = False
        Do
            Try
                continueLoop = False
                size = InputBox("Please select width and height" & vbCrLf & "Standard: 10" & vbCrLf & "Large: 20", "SCP Mapper", "10")
                ReDim currentMap(size - 1, size - 1)
                Me.Width = 16 * size + 16
                Me.Height = 16 * size + 63
                PictureBox1.Size = New Size(16 * size, 16 * size)
                Button1.Width = size * 5
                Button2.Width = size * 6
                Button3.Width = size * 5
                RenderScreen()
            Catch ex As Exception
                continueLoop = True
            End Try
        Loop While continueLoop

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        With SaveFileDialog1
            .CreatePrompt = False
            .DefaultExt = "png"
            .Filter = "File Images (*.jpg;*.jpeg;) | *.jpg;*.jpeg; |PNG Images | *.png |GIF Images | *.GIF"
            .InitialDirectory = "C:\Users\terra\OneDrive\Documents\SCP MAPS\"
            If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
                PictureBox1.Image.Save(.FileName)
            End If
        End With
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim continueLoop As Boolean = False
        Do
            Try
                continueLoop = False
                size = InputBox("Please select width and height" & vbCrLf & "Standard: 10" & vbCrLf & "Large: 20", "SCP Mapper", "10")
                ReDim currentMap(size - 1, size - 1)
                Me.Width = 16 * size + 16
                Me.Height = 16 * size + 63
                PictureBox1.Size = New Size(16 * size, 16 * size)
                Button1.Width = size * 5
                Button2.Width = size * 6
                Button3.Width = size * 5
                RenderScreen()
            Catch ex As Exception
                continueLoop = True
            End Try
        Loop While continueLoop
    End Sub
End Class
