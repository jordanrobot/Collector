'</iLogicCollectorHeader>

AddReference "Company.Library1.dll"
AddReference "microsoft.office.interop.excel.dll"
Imports Inventor
Imports System.IO
Imports Microsoft.Office.Interop
Imports System.Runtime.InteropServices
Imports Company.Library1
Imports System.Text.RegularExpressions


Module Main '</iLogicCollectorHide>

    Public Sub Main()

        Dim oSphere As New SphereClass.New(24, 0, 0, 0)
        Dim oExporter As New ExporterClass


        If oSphere.Volume < 360 Then

            oExporter.ToSat(oSphere, output_file)

        End If

    End Sub

    Public Enum ShapeTypeEnum
        Sphere = 0
        Box
        Torus
    End Enum

End Module '</iLogicCollectorHide>