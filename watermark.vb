Imports System.Net

Public Class WatermarkForm
    Inherits Form
    Private watermarkTimer As Timer
    Public Sub New()
        Me.FormBorderStyle = FormBorderStyle.None
        Me.WindowState = FormWindowState.Maximized
        Me.TopMost = True
        Me.BackColor = Color.Black
        Me.TransparencyKey = Color.Black
        Me.Opacity = 0.2
        Me.ShowInTaskbar = False
        AddHandler Me.Paint, AddressOf Me.OnPaint

        ' Initialize and start the timer
        watermarkTimer = New Timer()
        AddHandler watermarkTimer.Tick, AddressOf OnTimedEvent
        watermarkTimer.Interval = 30000 ' 30 seconds
        watermarkTimer.Start()

    End Sub

    Private Sub OnPaint(sender As Object, e As PaintEventArgs)
        Dim g As Graphics = e.Graphics
        Dim font As New Font("Segoe UI", 12, FontStyle.Regular)
        Dim brush As New SolidBrush(Color.Red)
        Dim text As String = GetWatermarkText()

        Dim textSize As SizeF = g.MeasureString(text, font)
        Dim angle As Single = -30.0F

        ' Step sizes for repeating the text
        Dim stepX As Integer = 600
        Dim stepY As Integer = 400

        ' Draw the watermark text repeatedly
        For x As Integer = 0 To Me.ClientSize.Width Step stepX
            For y As Integer = 0 To Me.ClientSize.Height Step stepY
                DrawRotatedText(g, text, font, brush, angle, x, y)
            Next
        Next
    End Sub

    Private Sub DrawRotatedText(g As Graphics, text As String, font As Font, brush As Brush, angle As Single, x As Integer, y As Integer)
        g.TranslateTransform(x, y)
        g.RotateTransform(angle)
        g.DrawString(text, font, brush, 0, 0)
        g.RotateTransform(-angle)
        g.TranslateTransform(-x, -y)
    End Sub

    Private Function GetWatermarkText() As String
        Dim ipAddress As String = GetIPAddresses()
        Dim DateTime1 As String = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        Dim loginName As String = Environment.UserName
        Return $"IP: {ipAddress} | {DateTime1} | {loginName}"
    End Function

    ' Old IP address function
    'Private Function GetIPAddress() As String
    'Dim host As String = Dns.GetHostName()
    'Dim ip As IPAddress = Dns.GetHostAddresses(host).FirstOrDefault(Function(a) a.AddressFamily = Sockets.AddressFamily.InterNetwork)
    'Return If(ip IsNot Nothing, ip.ToString(), "N/A")
    'End Function

    Private Function GetIPAddresses() As String
        Dim host As String = Dns.GetHostName()
        Dim ipAddresses As IPAddress() = Dns.GetHostAddresses(host).Where(Function(a) a.AddressFamily = Sockets.AddressFamily.InterNetwork).ToArray()

        If ipAddresses.Length > 0 Then
            Return String.Join(", ", ipAddresses.Select(Function(ip) ip.ToString()))
        Else
            Return "N/A"
        End If
    End Function

    Private Sub OnTimedEvent(sender As Object, e As EventArgs)
        Me.Invalidate() ' Force the form to repaint
    End Sub

    <STAThread>
    Public Shared Sub Main()
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)
        Application.Run(New WatermarkForm())
    End Sub

    Private Sub InitializeComponent()
        SuspendLayout()
        ' 
        ' WatermarkForm
        ' 
        ClientSize = New Size(1718, 958)
        Name = "WatermarkForm"
        ResumeLayout(False)

    End Sub
End Class
