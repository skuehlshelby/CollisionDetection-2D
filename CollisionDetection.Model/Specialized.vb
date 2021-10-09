Public MustInherit Class Specialized
    Implements ISpecialized

    Private ReadOnly _requirements As ICollection(Of Predicate(Of Object())) = New List(Of Predicate(Of Object()))()
    Private ReadOnly _requirementsByArgument As IDictionary(Of Integer, ICollection(Of Predicate(Of Object))) = New Dictionary(Of Integer, ICollection(Of Predicate(Of Object)))()
    
    Private Readonly _options As ICollection(Of Predicate(Of Object())) = New List(Of Predicate(Of Object()))()
    Private ReadOnly _optionsByArgument As IDictionary(Of Integer, ICollection(Of Predicate(Of Object))) = New Dictionary(Of Integer, ICollection(Of Predicate(Of Object)))()

    Protected Sub New ()
        SetRequirements()

        _requirements.Add(Function(args) args.Length > HighestIndex())
    End Sub

    Private Function HighestIndex() As Integer
        Dim maxRequirementIndex As Integer = If(_requirementsByArgument.Any(), _requirementsByArgument.Max(Function(k) k.Key), 0)
        Dim maxOptionsIndex As Integer = If(_optionsByArgument.Any(), _optionsByArgument.Max(Function(k) k.Key), 0)

        Return Math.Max(maxRequirementIndex, maxOptionsIndex)
    End Function

    Protected Overridable Sub SetRequirements()
        Require(Function(args) args IsNot Nothing)
        RequireAllArguments(Function(arg) arg IsNot Nothing)
    End Sub

    Protected Sub Require(requirement As Predicate(Of Object()))
        _requirements.Add(requirement)
    End Sub

    Protected Sub Require(index As Integer, requirement As Predicate(Of Object))
        If Not _requirementsByArgument.ContainsKey(index) Then
            _requirementsByArgument.Add(index, New List(Of Predicate(Of Object)))
        End If

        _requirementsByArgument(index).Add(requirement)
    End Sub

    Protected Sub RequireType(Of T)()
        _requirements.Add(Function(args) args.All(Function(arg) TypeOf arg Is T))
    End Sub

    Protected Sub RequireType(Of T)(ParamArray indices() As Integer)
        For Each index As Integer In indices
            If Not _requirementsByArgument.ContainsKey(index) Then
                _requirementsByArgument.Add(index, New List(Of Predicate(Of Object)))
            End If

            _requirementsByArgument(index).Add(Function(arg) TypeOf arg Is T)
        Next

    End Sub

    Protected Sub CouldBeType(Of T)()
        _options.Add(Function(args) args.All(Function(arg) TypeOf arg Is T))
    End Sub

    Protected Sub CouldBeType(Of T)(ParamArray indices() As Integer)
        For Each index As Integer In indices
            If Not _optionsByArgument.ContainsKey(index) Then
                _optionsByArgument.Add(index, New List(Of Predicate(Of Object)))
            End If

            _optionsByArgument(index).Add(Function(arg) TypeOf arg Is T)
        Next
    End Sub

    Protected Sub RequireAllArguments(requirement As Func(Of Object, Boolean))
        _requirements.Add(Function(args) args.All(requirement))
    End Sub

    Protected Overridable Function IsApplicable(ParamArray args() As Object) As Boolean Implements ISpecialized.IsApplicable
        Return MeetsRequirements(args) AndAlso MeetsOptions(args)
    End Function

    Private Function MeetsRequirements(args() As Object) As Boolean
        Dim passed As Boolean

        If _requirements.Count > 0 Then
            passed = _requirements.All(Function(opt) opt(args))
        Else
            passed = True
        End If

        If Not passed Then Return False

        If Not passed Then Return False

        If _requirementsByArgument.Count > 0 Then
            passed = _requirementsByArgument.All(Function(opt) opt.Value.All(Function(o) o(args(opt.Key))))
        Else
            passed = True
        End If

        Return passed
    End Function

    Private Function MeetsOptions(args() As Object) As Boolean
        Dim passed As Boolean

        If _options.Count > 0 Then
            passed = _options.Any(Function(opt) opt(args))
        Else
            passed = True
        End If

        If Not passed Then Return False

        If _optionsByArgument.Count > 0 Then
            passed = _optionsByArgument.All(Function(opt) opt.Value.Any(Function(o) o(args(opt.Key))))
        Else
            passed = True
        End If

        Return passed
    End Function
End Class