Namespace BVH
    Public MustInherit Class SplitMethod
        Private Sub New(name As String)
            Me.Name = name
        End Sub

        Public MustOverride Function Split(Of T As IFinite)(shapes As T()) As SplitResult(Of T)

        Public ReadOnly Property Name As String

        Public Shared Function Values() As IEnumerable(Of SplitMethod)
            Return New SplitMethod() { Midpoint, EqualCounts, SurfaceAreaHeuristic }
        End Function

        Public Shared Function Parse(name As String) As SplitMethod
            Return Values().Single(Function(value) value.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
        End Function

        Private Shared Function GetBounds(Of T As IFinite)(shapes As IEnumerable(Of T)) As Bounds
            Return Bounds.Union(shapes.Select(Function(shape) shape.Bounds()))
        End Function

        Private Shared Function GetCentroidBounds(Of T As IFinite)(shapes As IEnumerable(Of T)) As Bounds
            Return Bounds.Union(shapes.Select(Function(shape) shape.Center))
        End Function

        Public Shared ReadOnly Property Midpoint As SplitMethod = New MidpointSplit()

        Public Shared ReadOnly Property EqualCounts As SplitMethod = New EqualCountsSplit()

        Public Shared ReadOnly Property SurfaceAreaHeuristic As SplitMethod = New SurfaceAreaHeuristicSplit()

        Private NotInheritable Class MidpointSplit
            Inherits SplitMethod

            Public Sub New ()
                MyBase.New("Midpoint")
            End Sub

            Public Overrides Function Split(Of T As IFinite)(shapes As T()) As SplitResult(Of T)
                Dim centroidBounds As Bounds = GetCentroidBounds(shapes)

                If Math.Abs(centroidBounds.Area() - 0.0F) < 0.01 Then
                    Return SplitResult(Of T).Failure()
                End If

                Dim splitAxis As Axis = centroidBounds.LongestAxis()
                Dim middle As Single = (splitAxis.SelectAxis(centroidBounds.BottomLeft) + splitAxis.SelectAxis(centroidBounds.TopRight)) / 2.0F
                Dim sortedByAxis As T() = shapes.OrderBy(Function(shape) splitAxis.SelectAxis(shape.Center)).ToArray()

                Dim firstHalf As T() = sortedByAxis.TakeWhile(Function(shape) splitAxis.SelectAxis(shape.Center) < middle).ToArray()
                Dim secondHalf As T() = sortedByAxis.Skip(firstHalf.Length).ToArray()

                Return SplitResult(Of T).Success(firstHalf, secondHalf, splitAxis.Opposite())
            End Function
        End Class

        Private NotInheritable Class EqualCountsSplit
            Inherits SplitMethod

            Public Sub New ()
                MyBase.New("Equal Counts")
            End Sub

            Public Overrides Function Split(Of T As IFinite)(shapes As T()) As SplitResult(Of T)
                Dim centroidBounds As Bounds = GetCentroidBounds(shapes)

                If Math.Abs(centroidBounds.Area() - 0.0F) < 0.01 Then
                    Return SplitResult(Of T).Failure()
                End If

                Dim splitAxis As Axis = centroidBounds.LongestAxis()
                Dim sortedByAxis As T() = shapes.OrderBy(Function(shape) splitAxis.SelectAxis(shape.Center)).ToArray()

                Dim firstHalf As T() = sortedByAxis.Take(sortedByAxis.Length \ 2).ToArray()
                Dim secondHalf As T() = sortedByAxis.Skip(firstHalf.Length).ToArray()

                Return SplitResult(Of T).Success(firstHalf, secondHalf, splitAxis.Opposite())
            End Function

        End Class

        Private NotInheritable Class SurfaceAreaHeuristicSplit
            Inherits SplitMethod

            Public Sub New ()
                MyBase.New("Surface Area Heuristic")
            End Sub

            Public Overrides Function Split(Of T As IFinite)(shapes As T()) As SplitResult(Of T)
                If shapes.Length > 4 Then
                    Dim centroidBounds As Bounds = GetCentroidBounds(shapes)

                    If Math.Abs(centroidBounds.Area() - 0.0F) < 0.01 Then
                        Return SplitResult(Of T).Failure()
                    End If

                    Dim shapeBounds As Bounds = GetBounds(shapes)
                    Dim splitAxis As Axis = shapeBounds.LongestAxis()
                    Dim buckets(6) As SahBucket(Of T)

                    For index As Integer = LBound(buckets) To UBound(buckets)
                        buckets(index) = New SahBucket(Of T)(shapeBounds, CSng(index / UBound(buckets)), splitAxis, shapes) 
                    Next

                    Array.Sort(buckets, Function(l, r) l.SplitCost().CompareTo(r.SplitCost()))

                    If buckets(0).SplitCost() < shapes.Length Then
                        Return SplitResult(Of T).Success(buckets(0).FirstHalf, buckets(0).SecondHalf, splitAxis.Opposite())
                    Else
                        Return SplitResult(Of T).Failure()
                    End If
                Else
                    Return EqualCounts.Split(shapes)
                End If
            End Function

            Private NotInheritable Class SahBucket(Of T As IFinite)
                Public Sub New(original As Bounds, percentage As Single, splitAxis As Axis, shapes As IEnumerable(Of T))
                    Dim sortedShapes As T() = shapes.OrderBy(Function(shape) splitAxis.SelectAxis(shape.Center)).ToArray()
                    Dim partition As Single = splitAxis.SelectAxis(original.BottomLeft) + (splitAxis.SelectAxis(original) * percentage)
                    
                    FirstHalf = sortedShapes.TakeWhile(Function(shape) splitAxis.SelectAxis(shape.Center) <= partition).ToArray()
                    FirstHalfBounds = GetBounds(FirstHalf)
                    FirstHalfCount = FirstHalf.Length
                    SecondHalf = sortedShapes.Skip(FirstHalf.Length).ToArray()
                    SecondHalfBounds = GetBounds(SecondHalf)
                    SecondHalfCount = SecondHalf.Length
                    SplitCost = 0.125F + ((FirstHalfCount * FirstHalfBounds.Area()) + (SecondHalfCount * SecondHalfBounds.Area())) / original.Area()
                End Sub

                Public ReadOnly Property SplitCost As Double

                Public ReadOnly Property FirstHalf As T()

                Public ReadOnly Property FirstHalfBounds As Bounds

                Public ReadOnly Property FirstHalfCount As Integer

                Public ReadOnly Property SecondHalf As T()

                Public ReadOnly Property SecondHalfBounds As Bounds

                Public ReadOnly Property SecondHalfCount As Integer
            End Class
        End Class

    End Class
End NameSpace