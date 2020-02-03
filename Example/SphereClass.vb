'</Collector>

Public Class SphereClass

    Public Property Diameter As Double
    Public Property Center As New Blah.Blah.Point3D
    Public Property Diameter As Double


    Public Sub New(diam As Double, x As Double, y As Double, z As Double)
        Diameter = diam
        Center.Add = (x, y, z)
    End Sub

    Public Function Volume()
        Returns(Math.Pi * (4 / 3) * (Diameter / 2) ^ 3)
    End Function

    Public Function SurfaceArea()
        Returns(Math.Pi * 2 * Diameter ^ 2)
    End Function

End Class