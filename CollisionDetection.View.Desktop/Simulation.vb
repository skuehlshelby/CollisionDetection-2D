Imports System.Reflection
Imports System.Runtime.InteropServices
Imports CollisionDetection.Model
Imports CollisionDetection.Presentation

Public Class Simulation
    Implements ISimulationView

    Private Const Second As Integer = 1000
    Private ReadOnly _presenter As ISimulationPresenter
    Private ReadOnly _timer As Timer = New Timer()
    Private paused As Boolean = False
    
    Public Sub New(defaults As IDefaultProvider)
        InitializeComponent()

        SetDoubleBuffered(particleArea)

        SetDefaults(defaults)

        _presenter = New Presenter(defaults, Me)

        ConfigureFPSButton()

        ConfigureEvents()

        ConfigureTimer()
    End Sub

    Public Event PropertyChanged As EventHandler(Of PropertyChangedEventArgs) Implements ISimulationView.PropertyChanged

    Public Function QueryProperty(monitoredProperty As MonitoredProperty) As Object Implements ISimulationView.QueryProperty
        Select Case monitoredProperty
            Case MonitoredProperty.WorldBounds
                return particleArea.DisplayRectangle.Size
            Case Else
                Return Nothing
        End Select
    End Function

    Public Function TryQueryProperty(monitoredProperty As MonitoredProperty, <Out> ByRef value As Object) As Boolean Implements ISimulationView.TryQueryProperty
        Select Case monitoredProperty
            Case MonitoredProperty.WorldBounds
                value = particleArea.DisplayRectangle.Size
                Return True
            Case Else
                value = Nothing
                Return False
        End Select
    End Function

#Region "Control Configuration"

    Private Sub ConfigureFPSButton()
        With btnFPS
            .Minimum = 1
            .Maximum = 60
        End With
    End Sub

    Private Sub ConfigureTimer()
        With _timer
            .Interval = Second \ CInt(btnFPS.Value)
            AddHandler .Tick, AddressOf OnTick
            .Start()
        End With
    End Sub

#End Region

#Region "Event Configuration"
    Private Sub ConfigureEvents()
        AddHandler btnParticleCount.ValueChanged, Sub() RaiseEvent PropertyChanged(Me, new PropertyChangedEventArgs(MonitoredProperty.ShapeCount, CInt(btnParticleCount.Value)))
        AddHandler cbxShowBoundingVolumes.CheckStateChanged, Sub() RaiseEvent PropertyChanged(Me, new PropertyChangedEventArgs(MonitoredProperty.BoundingVolumeVisibility, cbxShowBoundingVolumes.Checked))
        AddHandler cbxShowRenderTime.CheckStateChanged, Sub() RaiseEvent PropertyChanged(Me, new PropertyChangedEventArgs(MonitoredProperty.AverageRenderTimeVisibility, cbxShowRenderTime.Checked))
        AddHandler cbxSplitMethod.SelectedValueChanged, Sub() RaiseEvent PropertyChanged(Me, new PropertyChangedEventArgs(MonitoredProperty.SplitMethod, cbxSplitMethod.SelectedItem.ToString()))
        AddHandler cbxCollisionDetectionMethod.SelectedValueChanged, Sub() RaiseEvent PropertyChanged(Me, new PropertyChangedEventArgs(MonitoredProperty.CollisionHandler, cbxCollisionDetectionMethod.SelectedItem.ToString()))
        AddHandler particleArea.SizeChanged, Sub() RaiseEvent PropertyChanged(Me, new PropertyChangedEventArgs(MonitoredProperty.WorldBounds, particleArea.DisplayRectangle.Size))
        AddHandler btnFPS.ValueChanged, AddressOf OnFrameRateChanged
        AddHandler btnPause.Click, AddressOf OnPauseClick
        AddHandler colorPanel.Click, AddressOf OnColorPanelClick
        AddHandler particleArea.MouseHover, AddressOf OnMouseHover
    End Sub

    Private Sub OnPauseClick(sender As Object, e As EventArgs)
        paused = Not paused
        btnPause.Text = If(paused, "Continue", "Pause")
        RaiseEvent PropertyChanged(Me, new PropertyChangedEventArgs(MonitoredProperty.Paused, paused))
    End Sub
    
    Private Sub OnFrameRateChanged(sender As Object, e As EventArgs)
        _timer.Interval = Second \ CInt(btnFPS.Value)
        RaiseEvent PropertyChanged(Me, new PropertyChangedEventArgs(MonitoredProperty.FrameRate, CInt(btnFPS.Value)))
    End Sub

    Private Overloads Sub OnMouseHover(sender As Object, e As EventArgs)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(MonitoredProperty.MousePosition, MousePosition))
    End Sub

#End Region

#Region "Defaults"

    Private Sub SetDefaults(defaultProvider As IDefaultProvider)
        With defaultProvider
            btnPause.Text = If(.GetDefault(Of Boolean)(MonitoredProperty.Paused), "Continue", "Pause")
            btnFPS.Value = .GetDefault(Of Integer)(MonitoredProperty.FrameRate)
            btnParticleCount.Value = .GetDefault(Of Integer)(MonitoredProperty.ShapeCount)
            cbxSplitMethod.Items.AddRange(.GetDefault(Of String())(MonitoredProperty.AvailableSplitMethods))
            cbxSplitMethod.SelectedItem = .GetDefault(Of String)(MonitoredProperty.SplitMethod)
            cbxCollisionDetectionMethod.Items.AddRange(.GetDefault(Of String())(MonitoredProperty.AvailableCollisionHandlers))
            cbxCollisionDetectionMethod.SelectedItem = .GetDefault(Of String)(MonitoredProperty.CollisionHandler)
            cbxShowBoundingVolumes.Checked = .GetDefault(Of Boolean)(MonitoredProperty.BoundingVolumeVisibility)
            cbxShowRenderTime.Checked = .GetDefault(Of Boolean)(MonitoredProperty.AverageRenderTimeVisibility)
        End With

        For Each color As Color In defaultProvider.GetDefault(Of Color())(MonitoredProperty.AvailableColors)
            AddColorOption(color)
        Next
    End Sub

#End Region

#Region "Vanity"
    Private Sub SetDoubleBuffered(panel As Panel)
        panel.GetType().InvokeMember("DoubleBuffered",
                                        BindingFlags.NonPublic Or BindingFlags.Instance Or BindingFlags.SetProperty,
                                        Nothing, panel, New Object() {True})
    End Sub

    Private Sub AddColorOption(color As Color)
        Dim lblColor As Label = New Label With {
            .BackColor = color
        }

        AddHandler lblColor.Click, AddressOf RemoveColorOption

        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(MonitoredProperty.ColorAdded, color))

        colorPanel.Controls.Add(lblColor)

        ArrangeColorOptions()
    End Sub

    Private Sub RemoveColorOption(sender As Object, e As EventArgs)
        If colorPanel.Controls.Count > 1 AndAlso sender IsNot Nothing AndAlso TypeOf sender Is Control Then
            Dim c As Control = DirectCast(sender, Control)

            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(MonitoredProperty.ColorRemoved, c.BackColor))

            colorPanel.Controls.Remove(c)

            ArrangeColorOptions()
        End If
    End Sub

    Private Sub ArrangeColorOptions()
        Const maxColors As Integer = 10
        Const margin As Integer = 5
        Dim size As Integer = (colorPanel.Width - (margin * maxColors + 1)) \ maxColors
        Dim offsetNeededToCenter As Integer = (colorPanel.Height - size) \ 2

        If colorPanel.Controls.Count > 0 Then
            For index As Integer = 0 To colorPanel.Controls.Count - 1
                With colorPanel.Controls.Item(index)
                    .Left = margin * (index + 1) + size * index
                    .Top = offsetNeededToCenter
                    .Height = size
                    .Width = size
                End With
            Next
        End If
    End Sub

    Private Sub OnColorPanelClick(sender As Object, e As EventArgs)
        With New ColorDialog
            If .ShowDialog(Me) = DialogResult.OK Then
                AddColorOption(.Color)
            End If
        End With
    End Sub
#End Region

    Private Sub OnTick(sender As Object, e As EventArgs)
        particleArea.Refresh()
    End Sub

    Private Sub OnParticleAreaPaint(sender As Object, e As PaintEventArgs) Handles particleArea.Paint
        _presenter.DrawScene(New GraphicsWrapper(e.Graphics))
    End Sub

End Class
