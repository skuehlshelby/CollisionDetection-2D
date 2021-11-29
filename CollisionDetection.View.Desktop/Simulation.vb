Imports System.Reflection
Imports CollisionDetection.Model
Imports CollisionDetection.Presentation

Public Class Simulation

    Private Const Second As Integer = 1000
    Private ReadOnly _presenter As ISimulationPresenter
    Private ReadOnly _timer As Timer = New Timer()
    Private paused As Boolean = False
    
    Public Sub New(defaults As IDefaultProvider)
        InitializeComponent()

        SetDoubleBuffered(particleArea)

        SetDefaults(defaults)

        _presenter = New Presenter(defaults)

        OnBoundsChanged(particleArea, EventArgs.Empty)

        ConfigureFPSButton()

        ConfigureEvents()

        ConfigureTimer()
    End Sub

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
        AddHandler btnParticleCount.ValueChanged, AddressOf OnShapeCountChanged
        AddHandler cbxShowBoundingVolumes.CheckStateChanged, AddressOf OnShowBoundingVolumesChanged
        'AddHandler cbxSplitMethod.SelectedValueChanged, Sub() RaiseEvent PropertyChanged(Me, new PropertyChangedEventArgs(MonitoredProperty.SplitMethod, cbxSplitMethod.SelectedItem.ToString()))
        'AddHandler cbxCollisionDetectionMethod.SelectedValueChanged, Sub() RaiseEvent PropertyChanged(Me, new PropertyChangedEventArgs(MonitoredProperty.CollisionHandler, cbxCollisionDetectionMethod.SelectedItem.ToString()))
        AddHandler particleArea.SizeChanged, AddressOf OnBoundsChanged
        AddHandler btnFPS.ValueChanged, AddressOf OnFrameRateChanged
        AddHandler btnPause.Click, AddressOf OnPauseClick
        AddHandler colorPanel.Click, AddressOf OnColorPanelClick
    End Sub

    Private Sub OnBoundsChanged(sender As Object, e As EventArgs)
        _presenter.Notify(Sub(app) app.WorldBounds = DirectCast(sender, Panel).DisplayRectangle)
    End Sub

    Private Sub OnShowBoundingVolumesChanged(sender As Object, e As EventArgs)
        _presenter.Notify(Sub(app) app.VisualizeAccelerationStructure = DirectCast(sender, CheckBox).Checked)
    End Sub

    Private Sub OnShapeCountChanged(sender As Object, e As EventArgs)
        _presenter.Notify(Sub(app) app.NumberOfShapes = DirectCast(sender, NumericUpDown).Value)
    End Sub

    Private Sub OnPauseClick(sender As Object, e As EventArgs)
        paused = Not paused
        btnPause.Text = If(paused, "Continue", "Pause")
        _presenter.Notify(Function(app) app.IsPaused = paused)
    End Sub
    
    Private Sub OnFrameRateChanged(sender As Object, e As EventArgs)
        _timer.Interval = Second \ CInt(btnFPS.Value)
    End Sub

#End Region

#Region "Defaults"

    Private Sub SetDefaults(defaultProvider As IDefaultProvider)
        With defaultProvider
            btnPause.Text = If(.GetDefault(Of Boolean)(MonitoredProperty.Paused), "Continue", "Pause")
            btnFPS.Value = .GetDefault(Of Integer)(MonitoredProperty.FrameRate)
            btnFPS.Maximum = .GetDefault(Of Integer)(MonitoredProperty.MaximumFrameRate)
            btnParticleCount.Value = .GetDefault(Of Integer)(MonitoredProperty.ShapeCount)
            btnParticleCount.Maximum = .GetDefault(Of Integer)(MonitoredProperty.MaximumShapeCount)
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

        _presenter.Notify(Sub(app) app.ShapeColors.Add(color))

        colorPanel.Controls.Add(lblColor)

        ArrangeColorOptions()
    End Sub

    Private Sub RemoveColorOption(sender As Object, e As EventArgs)
        If colorPanel.Controls.Count > 1 AndAlso sender IsNot Nothing AndAlso TypeOf sender Is Control Then
            Dim c As Control = DirectCast(sender, Control)

            _presenter.Notify(Sub(app) app.ShapeColors.Remove(c.BackColor))

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
        _presenter.Update(TimeSpan.FromMilliseconds(_timer.Interval))
        _presenter.Render(New GraphicsWrapper(e.Graphics))
    End Sub

End Class
