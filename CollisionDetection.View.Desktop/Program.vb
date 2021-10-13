Imports CollisionDetection.Presentation

Module Program

    <STAThread()>
    Public Sub Main()
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)
        Application.Run(New Simulation(New DesktopDefaults()))
    End Sub

End Module
