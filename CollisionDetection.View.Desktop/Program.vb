Imports CollisionDetection.Presentation

Module Program

    <STAThread()>
    Public Sub Main()
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)
        Application.Run(New Simulation(New FreshStart()))
    End Sub

End Module
