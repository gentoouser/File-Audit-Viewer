'Imports System.Data.SqlClient
Imports MySql.Data.MySqlClient
Public Class FileAuditViewerBase
    'SQL Connection
    Public objSQLConnection As Object
    Dim objSQLDataAdapter As Object
    'Global variables for communication between threads
    Public strParentField As String
    Public c1Field As String
    Public c2Field As String
    Public c3Field As String
    Public c4Field As String
    Public c5Field As String
    Public c6Field As String
    Public intParentkey As Integer
    Public intc1key As Integer
    Public intc2key As Integer
    Public intc3key As Integer
    Public intc4key As Integer
    Public intc5key As Integer
    Public intc6key As Integer
    Public IsInherited As Boolean = False
    Public StrRunDate As String
    Public StrDriveLetter As String
    Public StrComputerName As String
    Public StrSelectedTreeView As String
    Public StrDatabase As String
    Public StrTableName As String = My.Settings.SQLTable

    Private Sub FileAuditViewerBase_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Dim objSQLCommand As Object
        Dim strSQL As String
        Dim objSQLReader As Object
 
        'Populates ComputerName Box
        Select Case UCase(My.Settings.SQLServerType)
            Case "MSSQL"
                strSQL = "SELECT DISTINCT [Computer] FROM [" & My.Settings.SQLTable & "] ORDER BY [Computer];"
                If objSQLConnection Is Nothing Then
                    objSQLConnection = New System.Data.SqlClient.SqlConnection(My.Settings.SQLConnection)
                    StrDatabase = objSQLConnection.Database
                    If Not objSQLConnection.state = ConnectionState.Open Then objSQLConnection.Open()
                End If
                objSQLCommand = New System.Data.SqlClient.SqlCommand(strSQL, objSQLConnection)
            Case "MYSQL"
                strSQL = "SELECT DISTINCT `Computer` FROM `" & My.Settings.SQLTable & "` ORDER BY `Computer`;"
                If objSQLConnection Is Nothing Then
                    objSQLConnection = New MySql.Data.MySqlClient.MySqlConnection(My.Settings.SQLConnection)
                    StrDatabase = objSQLConnection.Database
                    If Not objSQLConnection.state = ConnectionState.Open Then objSQLConnection.Open()
                End If
                objSQLCommand = New MySql.Data.MySqlClient.MySqlCommand
                objSQLCommand.Connection = objSQLConnection
                objSQLCommand.CommandText = strSQL
                objSQLCommand.CommandType = CommandType.Text
                objSQLCommand.ExecuteNonQuery()
            Case Else
                MsgBox("SQLServerType setting not understood: SQLServerType =" & My.Settings.SQLServerType, MsgBoxStyle.Critical, "SQL Server Type")
                Me.Close()
        End Select
        objSQLReader = objSQLCommand.ExecuteReader
        If objSQLReader.HasRows Then
            Do While objSQLReader.Read()
                If Not objSQLReader.Item(0) = "" Then
                    ComputerName.Items.Add(objSQLReader.Item(0))
                Else
                    Exit Do
                End If
            Loop
        End If

        objSQLReader.Close()
        objSQLCommand = Nothing
        objSQLReader = Nothing
        strSQL = Nothing
    End Sub

    Private Sub BTNRun_Click(sender As System.Object, e As System.EventArgs) Handles BTNRun.Click
        Dim pnode As TreeNode
        Dim cnode1 As TreeNode
        Dim cnode2 As TreeNode
        'Handles the Run/Cancel button
        Select Case BTNRun.Text
            Case "Run"
                'Start enumerating records for currently selected tab.
                'Change text in button 
                BTNRun.Text = "Cancel"
                'Finds out which tab is selected
                Select Case TabControl1.SelectedTab.Name
                    Case "FtoU"
                        'Setup layout for treeview
                        strParentField = "FolderPath"
                        c1Field = "GroupSAMAccountName"
                        c2Field = "AccountSAMAccountName"
                        c3Field = "Rights"
                        c4Field = "Owner"
                        c5Field = "Inheritance"
                        c6Field = "ManagedBy"
                        StrSelectedTreeView = "FtoUTreeView"
                    Case "UtoF"
                        'Setup layout for treeview
                        strParentField = "AccountSAMAccountName"
                        c1Field = "GroupSAMAccountName"
                        c2Field = "FolderPath"
                        c3Field = "Rights"
                        c4Field = "Owner"
                        c5Field = "Inheritance"
                        c6Field = "ManagedBy"
                        StrSelectedTreeView = "UtoFTreeView"
                    Case "GtoF"
                        'Setup layout for treeview
                        strParentField = "GroupSAMAccountName"
                        c1Field = "FolderPath"
                        c2Field = "AccountSAMAccountName"
                        c3Field = "Rights"
                        c4Field = "Owner"
                        c5Field = "Inheritance"
                        c6Field = "ManagedBy"
                        StrSelectedTreeView = "GtoFTreeView"
                    Case Else
                        Exit Sub
                End Select
                'Check for Inherited checkbox
                If RemoveInherited.Checked Then
                    IsInherited = False
                Else
                    IsInherited = True
                End If
                'Check for rundate
                If RunDate.SelectedItem.ToString = "" Then
                    MsgBox("Please select run date.", MsgBoxStyle.OkOnly)
                    Exit Sub
                Else
                    StrRunDate = RunDate.SelectedItem.ToString
                End If
                'Check for Drive letter
                If DriveLetter.SelectedItem.ToString = "" Then
                    MsgBox("Please select drive letter.", MsgBoxStyle.OkOnly)
                    Exit Sub
                Else
                    StrDriveLetter = DriveLetter.SelectedItem.ToString
                End If
                'Check for ComputerName
                If ComputerName.SelectedItem.ToString = "" Then
                    MsgBox("Please select drive letter.", MsgBoxStyle.OkOnly)
                    Exit Sub
                Else
                    StrComputerName = ComputerName.SelectedItem.ToString
                End If
                'Clear old data view
                FtoUTreeView.Nodes.Clear()
                'Fills out Key at the top of interface
                pnode = TreeViewHeading.Nodes.Add(strParentField)
                cnode1 = pnode.Nodes.Add(c1Field)
                cnode2 = cnode1.Nodes.Add(String.Format("{0,-50} {1,-50} {2,-50} {3,-50} {4,-50}", c2Field, c3Field, c4Field, c5Field, c6Field))
                TreeViewHeading.ExpandAll()
                TreeViewHeading.Enabled = False
                'Starts background worker to fill data
                If Not BackgroundWorker1.IsBusy Then BackgroundWorker1.RunWorkerAsync()
            Case "Cancel"
                'Stop Background worker
                If BackgroundWorker1.IsBusy Then
                    BackgroundWorker1.CancelAsync()
                    BTNRun.Text = "Run"
                End If
        End Select
    End Sub

    Private Sub BackgroundWorker1_DoWork(sender As System.Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        'Setup for private varibles
        Dim strSQL As String
        Dim strTemp As String
        Dim ArgDel(1) As Object
        Dim StartTime As DateTime, EndTime As DateTime, ElapsedTime As TimeSpan
        'SQL 

        Dim ObjDataSet As System.Data.DataSet
        Dim objSQLCommand As Object
        Dim parentrow As DataRow
        Dim childrow1 As DataRow
        Dim childrow2 As DataRow
        Dim ptable As DataTable
        Dim c1table As DataTable
        Dim c2table As DataTable
        'TreeView
        Dim RootTree As New TreeNode
        Dim pnode As TreeNode = New TreeNode
        Dim cnode1 As TreeNode = New TreeNode
        Dim cnode2 As TreeNode = New TreeNode
        Dim cnode3 As TreeNode = New TreeNode
        Dim cnode4 As TreeNode = New TreeNode
        Dim cnode5 As TreeNode = New TreeNode
        Dim cnode6 As TreeNode = New TreeNode
        'Delegates
        Dim UpdateTitleDel As UpdateTitleDelegate = New UpdateTitleDelegate(AddressOf UpdateTitle)
        Dim UpdateTreeViewDel As UpdateTreeViewDelegate = New UpdateTreeViewDelegate(AddressOf UpdateTreeView)
        Dim UpdateTreeViewDelC As UpdateTreeViewDelegateC = New UpdateTreeViewDelegateC(AddressOf UpdateTreeViewC)
        'Updates title in form
        Me.Invoke(UpdateTitleDel, "Working")
        StartTime = Now


        'Try to use parent data
        Try
            Select Case UCase(My.Settings.SQLServerType)
                Case "MSSQL"
                    'Select parent data using strParentField
                    If Not IsInherited Then
                        strSQL = "SELECT distinct [" & strParentField & "]" _
                         & " FROM [" & My.Settings.SQLTable & "]" _
                         & " Where [RunDate] = '" & StrRunDate & "' And [IsInherited] = '" & IsInherited.ToString & "' And SUBSTRING([FolderPath],1,3) = '" & StrDriveLetter & "' And [Computer] = '" & StrComputerName & "'" _
                         & " ORDER BY [" & strParentField & "];"
                    Else
                        strSQL = "SELECT distinct [" & strParentField & "]" _
                         & " FROM [" & My.Settings.SQLTable & "]" _
                         & " Where [RunDate] = '" & StrRunDate & "'  And SUBSTRING([FolderPath],1,3) = '" & StrDriveLetter & "' And [Computer] = '" & StrComputerName & "'" _
                         & " ORDER BY [" & strParentField & "];"
                    End If
                    If objSQLConnection Is Nothing Then
                        objSQLConnection = New System.Data.SqlClient.SqlConnection(My.Settings.SQLConnection)
                        StrDatabase = objSQLConnection.Database
                        If Not objSQLConnection.state = ConnectionState.Open Then objSQLConnection.Open()
                    End If
                    objSQLDataAdapter = New System.Data.SqlClient.SqlDataAdapter(strSQL, My.Settings.SQLConnection)
                    'Fill ptable
                    ObjDataSet = New System.Data.DataSet
                    objSQLDataAdapter.Fill(ObjDataSet, My.Settings.SQLTable)
                    ptable = ObjDataSet.Tables(My.Settings.SQLTable)
                    'Loops though all parent records
                    For Each parentrow In ptable.Rows
                        'checks for cancelation
                        If BackgroundWorker1.CancellationPending = True Then
                            Exit Try
                        End If
                        'Add parent node on to root node
                        pnode = New TreeNode(parentrow(strParentField))
                        Me.Invoke(UpdateTreeViewDel, pnode, StrSelectedTreeView)

                        'Start 1st child
                        'Select 1st child data using c1Field
                        If Not IsInherited Then
                            strSQL = "SELECT distinct [" & c1Field & "]" _
                             & " FROM [" & My.Settings.SQLTable & "]" _
                             & " Where [RunDate] = '" & StrRunDate & "' And [IsInherited] = '" & IsInherited.ToString & "' And [" & strParentField & "] = '" & parentrow(strParentField) & "' And SUBSTRING([FolderPath],1,3) = '" & StrDriveLetter & "'" _
                             & " ORDER BY [" & c1Field & "];"
                        Else
                            strSQL = "SELECT distinct [" & c1Field & "]" _
                             & " FROM [" & My.Settings.SQLTable & "]" _
                             & " Where [RunDate] = '" & StrRunDate & "' And [" & strParentField & "] = '" & parentrow(strParentField) & "' And SUBSTRING([FolderPath],1,3) = '" & StrDriveLetter & "'" _
                             & " ORDER BY [" & c1Field & "];"
                        End If
                        Select Case UCase(My.Settings.SQLServerType)
                            Case "MSSQL"
                                'objSQLConnection = New System.Data.SqlClient.SqlConnection(My.Settings.SQLConnection)
                                'StrDatabase = dicSQLInfo.Item("Initial Catalog")
                                'StrDatabase = objSQLConnection.Database
                                objSQLDataAdapter = New System.Data.SqlClient.SqlDataAdapter(strSQL, My.Settings.SQLConnection)
                            Case "MYSQL"
                                'objSQLConnection = New MySql.Data.MySqlClient.MySqlConnection(My.Settings.SQLConnection)
                                'StrDatabase = dicSQLInfo.Item("database")
                                'StrDatabase = objSQLConnection.Database
                                objSQLDataAdapter = New MySql.Data.MySqlClient.MySqlCommand(strSQL, objSQLConnection)
                            Case Else
                                MsgBox("SQLServerType setting not understood: SQLServerType =" & My.Settings.SQLServerType, MsgBoxStyle.Critical, "SQL Server Type")
                                Me.Close()
                        End Select
                        'Try to use 1st child data
                        Try
                            'Fill c1table
                            ObjDataSet = New System.Data.DataSet
                            objSQLDataAdapter.Fill(ObjDataSet, My.Settings.SQLTable)
                            c1table = ObjDataSet.Tables(My.Settings.SQLTable)
                            'Loops though all 1st child records
                            For Each childrow1 In c1table.Rows
                                'Adds 1st child node to parent node
                                cnode1 = New TreeNode(childrow1(c1Field))
                                cnode1 = Me.Invoke(UpdateTreeViewDelC, pnode, cnode1, childrow1(c1Field), StrSelectedTreeView)

                                'Start 2nd child
                                'Select 2nd child data using c2Field
                                If Not IsInherited Then
                                    strSQL = "SELECT distinct [" & c2Field & "],[" & c3Field & "],[" & c4Field & "],[" & c5Field & "],[" & c6Field & "]" _
                                     & " FROM [" & My.Settings.SQLTable & "]" _
                                     & " Where [RunDate] = '" & StrRunDate & "' And [IsInherited] = '" & IsInherited.ToString & "' And [" & strParentField & "] = '" & parentrow(strParentField) & "'" _
                                    & " And [" & c1Field & "] = '" & childrow1(c1Field) & "' And SUBSTRING([FolderPath],1,3) = '" & StrDriveLetter & "'" _
                                    & " ORDER BY [" & c2Field & "];"
                                Else
                                    strSQL = "SELECT distinct [" & c2Field & "],[" & c3Field & "],[" & c4Field & "],[" & c5Field & "],[" & c6Field & "]" _
                                     & " FROM [" & My.Settings.SQLTable & "]" _
                                     & " Where [RunDate] = '" & StrRunDate & "' And [" & strParentField & "] = '" & parentrow(strParentField) & "'" _
                                    & " And [" & c1Field & "] = '" & childrow1(c1Field) & "' And SUBSTRING([FolderPath],1,3) = '" & StrDriveLetter & "'" _
                                    & " ORDER BY [" & c2Field & "];"
                                End If

                                Select Case UCase(My.Settings.SQLServerType)
                                    Case "MSSQL"
                                        'objSQLConnection = New System.Data.SqlClient.SqlConnection(My.Settings.SQLConnection)
                                        'StrDatabase = dicSQLInfo.Item("Initial Catalog")
                                        'StrDatabase = objSQLConnection.Database
                                        objSQLDataAdapter = New System.Data.SqlClient.SqlDataAdapter(strSQL, My.Settings.SQLConnection)
                                    Case "MYSQL"
                                        'objSQLConnection = New MySql.Data.MySqlClient.MySqlConnection(My.Settings.SQLConnection)
                                        'StrDatabase = dicSQLInfo.Item("database")
                                        'StrDatabase = objSQLConnection.Database
                                        objSQLDataAdapter = New MySql.Data.MySqlClient.MySqlCommand(strSQL, objSQLConnection)
                                    Case Else
                                        MsgBox("SQLServerType setting not understood: SQLServerType =" & My.Settings.SQLServerType, MsgBoxStyle.Critical, "SQL Server Type")
                                        Me.Close()
                                End Select
                                'Try to use 2nd child data
                                Try
                                    'Fill c2table
                                    ObjDataSet = New System.Data.DataSet
                                    objSQLDataAdapter.Fill(ObjDataSet, My.Settings.SQLTable)
                                    c2table = ObjDataSet.Tables(My.Settings.SQLTable)
                                    'Loops though all 2nd child records
                                    For Each childrow2 In c2table.Rows
                                        'Since we are expending only to the 2nd level we fill out the rest of the data here
                                        strTemp = String.Format("{0,-50} {1,-50} {2,-50} {3,-50} {4,-50}", childrow2(c2Field), childrow2(c3Field), childrow2(c4Field), childrow2(c5Field), childrow2(c6Field))
                                        'Adds 2nd child node to 1st child node
                                        cnode2 = New TreeNode(strTemp)
                                        cnode2 = Me.Invoke(UpdateTreeViewDelC, cnode1, cnode2, strTemp, StrSelectedTreeView)
                                    Next childrow2
                                Catch ex As Exception
                                    MsgBox("2nd Child Node Error: " & Err.Description)
                                End Try
                            Next childrow1
                        Catch ex As Exception
                            MsgBox("1st Child Node Error: " & Err.Description)
                        End Try
                    Next parentrow
                Case "MYSQL"
                    'Select parent data using strParentField
                    If Not IsInherited Then
                        strSQL = "SELECT distinct `" & strParentField & "`" _
                         & " FROM `" & My.Settings.SQLTable & "`" _
                          & " Where `RunDate` = '" & StrRunDate & "' And `IsInherited` = '" & IsInherited.ToString & "' And SUBSTRING(`FolderPath`,1,3) = '" & Replace(Replace(StrDriveLetter, "\", "\\"), ":", "\:") & "' And `Computer` = '" & StrComputerName & "'" _
                          & " ORDER BY `" & strParentField & "`;"
                    Else
                        strSQL = "SELECT distinct `" & strParentField & "`" _
                         & " FROM `" & My.Settings.SQLTable & "`" _
                         & " Where `RunDate` = '" & StrRunDate & "'  And SUBSTRING(`FolderPath`,1,3) = '" & Replace(Replace(StrDriveLetter, "\", "\\"), ":", "\:") & "' And `Computer` = '" & StrComputerName & "'" _
                         & " ORDER BY `" & strParentField & "`;"
                    End If
                    If objSQLConnection Is Nothing Then
                        objSQLConnection = New MySql.Data.MySqlClient.MySqlConnection(My.Settings.SQLConnection)
                        StrDatabase = objSQLConnection.Database

                    End If
                    If Not objSQLConnection.state = ConnectionState.Open Then objSQLConnection.Open()
                    objSQLCommand = New MySql.Data.MySqlClient.MySqlCommand
                    objSQLCommand.Connection = objSQLConnection
                    objSQLCommand.CommandText = strSQL
                    objSQLCommand.CommandType = CommandType.Text
                    objSQLDataAdapter = New MySql.Data.MySqlClient.MySqlDataAdapter(objSQLCommand)


                    'Fill ptable
                    ObjDataSet = New System.Data.DataSet

                    objSQLDataAdapter.Fill(ObjDataSet, My.Settings.SQLTable)
                    ptable = ObjDataSet.Tables(My.Settings.SQLTable)
                    'Loops though all parent records
                    For Each parentrow In ptable.Rows
                        'checks for cancelation
                        If BackgroundWorker1.CancellationPending = True Then
                            Exit Try
                        End If
                        'Add parent node on to root node
                        pnode = New TreeNode(parentrow(strParentField))
                        Me.Invoke(UpdateTreeViewDel, pnode, StrSelectedTreeView)

                        'Start 1st child
                        'Select 1st child data using c1Field
                        If Not IsInherited Then
                            strSQL = "SELECT distinct `" & c1Field & "`" _
                             & " FROM `" & My.Settings.SQLTable & "`" _
                             & " Where `RunDate` = '" & StrRunDate & "' And `IsInherited` = '" & IsInherited.ToString & "' And `" & strParentField & "` = '" & Replace(Replace(parentrow(strParentField), "\", "\\"), ":", "\:") & "' And SUBSTRING(`FolderPath`,1,3) = '" & Replace(Replace(StrDriveLetter, "\", "\\"), ":", "\:") & "' And `Computer` = '" & StrComputerName & "'" _
                             & " ORDER BY `" & c1Field & "`;"
                        Else
                            strSQL = "SELECT distinct `" & c1Field & "`" _
                             & " FROM `" & My.Settings.SQLTable & "`" _
                             & " Where `RunDate` = '" & StrRunDate & "' And `" & strParentField & "` = '" & Replace(Replace(parentrow(strParentField), "\", "\\"), ":", "\:") & "' And SUBSTRING(`FolderPath`,1,3) = '" & Replace(Replace(StrDriveLetter, "\", "\\"), ":", "\:") & "' And `Computer` = '" & StrComputerName & "'" _
                             & " ORDER BY `" & c1Field & "`;"
                        End If
                        If objSQLConnection Is Nothing Then
                            objSQLConnection = New MySql.Data.MySqlClient.MySqlConnection(My.Settings.SQLConnection)
                            StrDatabase = objSQLConnection.Database

                        End If
                        If Not objSQLConnection.state = ConnectionState.Open Then objSQLConnection.Open()
                        objSQLCommand = New MySql.Data.MySqlClient.MySqlCommand
                        objSQLCommand.Connection = objSQLConnection
                        objSQLCommand.CommandText = strSQL
                        objSQLCommand.CommandType = CommandType.Text
                        objSQLDataAdapter = New MySql.Data.MySqlClient.MySqlDataAdapter(objSQLCommand)


    
            'Try to use 1st child data
            Try
                'Fill c1table
                ObjDataSet = New System.Data.DataSet
                objSQLDataAdapter.Fill(ObjDataSet, My.Settings.SQLTable)
                c1table = ObjDataSet.Tables(My.Settings.SQLTable)
                'Loops though all 1st child records
                For Each childrow1 In c1table.Rows
                    'Adds 1st child node to parent node
                    cnode1 = New TreeNode(childrow1(c1Field))
                    cnode1 = Me.Invoke(UpdateTreeViewDelC, pnode, cnode1, childrow1(c1Field), StrSelectedTreeView)

                    'Start 2nd child
                    'Select 2nd child data using c2Field
                    If Not IsInherited Then
                                    strSQL = "SELECT distinct `" & c2Field & "`,`" & c3Field & "`,`" & c4Field & "`,`" & c5Field & "`,`" & c6Field & "`" _
                         & " FROM `" & My.Settings.SQLTable & "`" _
                         & " Where `RunDate` = '" & StrRunDate & "' And `IsInherited` = '" & IsInherited.ToString & "' And `" & strParentField & "` = '" & Replace(Replace(parentrow(strParentField), "\", "\\"), ":", "\:") & "'" _
                        & " And `" & c1Field & "` = '" & childrow1(c1Field) & "' And SUBSTRING(`FolderPath`,1,3) = '" & Replace(Replace(StrDriveLetter, "\", "\\"), ":", "\:") & "' And `Computer` = '" & StrComputerName & "'" _
                        & " ORDER BY `" & c2Field & "`;"
                    Else
                                    strSQL = "SELECT distinct `" & c2Field & "`,`" & c3Field & "`,`" & c4Field & "`,`" & c5Field & "`,`" & c6Field & "`" _
                         & " FROM `" & My.Settings.SQLTable & "`" _
                         & " Where `RunDate` = '" & StrRunDate & "' And `" & strParentField & "` = '" & Replace(Replace(parentrow(strParentField), "\", "\\"), ":", "\:") & "'" _
                        & " And `" & c1Field & "` = '" & childrow1(c1Field) & "' And SUBSTRING(`FolderPath`,1,3) = '" & Replace(Replace(StrDriveLetter, "\", "\\"), ":", "\:") & "' And `Computer` = '" & StrComputerName & "'" _
                        & " ORDER BY `" & c2Field & "`;"
                    End If

                        If objSQLConnection Is Nothing Then
                                    objSQLConnection = New MySql.Data.MySqlClient.MySqlConnection(My.Settings.SQLConnection)
                                    StrDatabase = objSQLConnection.Database

                                End If
                                If Not objSQLConnection.state = ConnectionState.Open Then objSQLConnection.Open()
                                objSQLCommand = New MySql.Data.MySqlClient.MySqlCommand
                                objSQLCommand.Connection = objSQLConnection
                                objSQLCommand.CommandText = strSQL
                                objSQLCommand.CommandType = CommandType.Text
                                objSQLDataAdapter = New MySql.Data.MySqlClient.MySqlDataAdapter(objSQLCommand)
                    'Try to use 2nd child data
                    Try
                        'Fill c2table
                        ObjDataSet = New System.Data.DataSet
                        objSQLDataAdapter.Fill(ObjDataSet, My.Settings.SQLTable)
                        c2table = ObjDataSet.Tables(My.Settings.SQLTable)
                        'Loops though all 2nd child records
                        For Each childrow2 In c2table.Rows
                            'Since we are expending only to the 2nd level we fill out the rest of the data here
                            strTemp = String.Format("{0,-50} {1,-50} {2,-50} {3,-50} {4,-50}", childrow2(c2Field), childrow2(c3Field), childrow2(c4Field), childrow2(c5Field), childrow2(c6Field))
                            'Adds 2nd child node to 1st child node
                            cnode2 = New TreeNode(strTemp)
                            cnode2 = Me.Invoke(UpdateTreeViewDelC, cnode1, cnode2, strTemp, StrSelectedTreeView)
                        Next childrow2
                    Catch ex As Exception
                        MsgBox("2nd Child Node Error: " & Err.Description)
                    End Try
                Next childrow1
            Catch ex As Exception
                MsgBox("1st Child Node Error: " & Err.Description)
            End Try
                    Next parentrow
                Case Else
                    MsgBox("SQLServerType setting not understood: SQLServerType =" & My.Settings.SQLServerType, MsgBoxStyle.Critical, "SQL Server Type")
                    Me.Close()
            End Select
        Catch ex As Exception
            MsgBox("Parent Node Error: " & Err.Description)
        End Try

        'close the connection
        objSQLConnection.Close()
        'Updates title in form
        EndTime = Now
        ElapsedTime = EndTime.Subtract(StartTime)
        Me.Invoke(UpdateTitleDel, "Done; Total Time :" & vbTab & ElapsedTime.Hours & ":" & ElapsedTime.Minutes & ":" & ElapsedTime.Seconds & "." & ElapsedTime.Milliseconds)
        BackgroundWorker1.Dispose()
    End Sub
    Delegate Sub UpdateTitleDelegate(ByVal StrAppend As String)
    Delegate Sub UpdateTreeViewDelegate(ByVal ObjInput As TreeNode, ByVal StrTree As String)
    Delegate Function UpdateTreeViewDelegateC(ByVal ObjInputP As TreeNode, ByVal ObjInputC As TreeNode, ByVal StrInput As String, ByVal StrTree As String) As TreeNode
    Public Sub UpdateTitle(ByVal StrAppend As String)
        'Changes Form title 
        Dim ArrTemp(1) As String
        Dim strTemp As String = ""
        ArrTemp = Split(Me.Text, "(")
        If IsNothing(ArrTemp) Then
            strTemp = Me.Text
        Else
            If Not ArrTemp(0) = "" Then strTemp = ArrTemp(0)
        End If
        Me.Text = strTemp & " (" & StrAppend & ")"
        If InStr(StrAppend, "Done") > 0 Then BTNRun.Text = "Run"
    End Sub
    Public Sub UpdateTreeView(ByVal ObjInput As TreeNode, ByVal StrTree As String)
        'Adds new child node to root node.
        Select Case UCase(StrTree)
            Case UCase("FtoUTreeView")
                FtoUTreeView.Nodes.Add(ObjInput)
                FtoUTreeView.Update()
            Case UCase("UtoFTreeView")
                UtoFTreeView.Nodes.Add(ObjInput)
                UtoFTreeView.Update()
            Case UCase("GtoFTreeView")
                GtoFTreeView.Nodes.Add(ObjInput)
                GtoFTreeView.Update()
        End Select
    End Sub
    Public Function UpdateTreeViewC(ByVal ObjInputP As TreeNode, ByVal ObjInputC As TreeNode, ByVal StrInput As String, ByVal StrTree As String) As TreeNode
        'Needs to be a function otherwise the Treeview would not populate correctly
        'Adds new child node to parent
        Select Case UCase(StrTree)
            Case UCase("FtoUTreeView")
                ObjInputC = ObjInputP.Nodes.Add(StrInput)
                ObjInputC.Tag = StrInput
            Case UCase("UtoFTreeView")
                ObjInputC = ObjInputP.Nodes.Add(StrInput)
                ObjInputC.Tag = StrInput
            Case UCase("GtoFTreeView")
                ObjInputC = ObjInputP.Nodes.Add(StrInput)
                ObjInputC.Tag = StrInput
        End Select
        Return ObjInputC
    End Function

    Private Sub ComputerName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComputerName.SelectedIndexChanged
        Dim objSQLCommand As Object
        Dim strSQL As String
        Dim objSQLReader As Object
        'Populates Drive Letter Combobox
        StrComputerName = ComputerName.SelectedItem
        Select Case UCase(My.Settings.SQLServerType)
            Case "MSSQL"
                strSQL = "SELECT DISTINCT SUBSTRING([FolderPath],1,3) As Drive FROM [" & My.Settings.SQLTable & "] Where [Computer] = '" & StrComputerName & "' ORDER BY Drive;"
                If objSQLConnection Is Nothing Then
                    objSQLConnection = New System.Data.SqlClient.SqlConnection(My.Settings.SQLConnection)
                    StrDatabase = objSQLConnection.Database
                    If Not objSQLConnection.state = ConnectionState.Open Then objSQLConnection.Open()
                End If
                objSQLCommand = New System.Data.SqlClient.SqlCommand(strSQL, objSQLConnection)
            Case "MYSQL"
                strSQL = "SELECT DISTINCT SUBSTRING(`FolderPath`,1,3) As Drive FROM `" & My.Settings.SQLTable & "` Where `Computer` = '" & StrComputerName & "' ORDER BY Drive;"
                If objSQLConnection Is Nothing Then
                    objSQLConnection = New MySql.Data.MySqlClient.MySqlConnection(My.Settings.SQLConnection)
                    StrDatabase = objSQLConnection.Database
                    If Not objSQLConnection.state = ConnectionState.Open Then objSQLConnection.Open()
                End If
                objSQLCommand = New MySql.Data.MySqlClient.MySqlCommand
                objSQLCommand.Connection = objSQLConnection
                objSQLCommand.CommandText = strSQL
                objSQLCommand.CommandType = CommandType.Text
                objSQLCommand.ExecuteNonQuery()
            Case Else
                MsgBox("SQLServerType setting not understood: SQLServerType =" & My.Settings.SQLServerType, MsgBoxStyle.Critical, "SQL Server Type")
                Me.Close()
        End Select

        objSQLReader = objSQLCommand.ExecuteReader
        If objSQLReader.HasRows Then
            Do While objSQLReader.Read()
                If Not objSQLReader.Item(0) = "" Then
                    DriveLetter.Items.Add(objSQLReader.Item(0))
                Else
                    Exit Do
                End If
            Loop
        End If
        'DriveLetter.SelectedIndex = 0
        DriveLetter.Enabled = True
        objSQLReader.Close()
        objSQLCommand = Nothing
        objSQLReader = Nothing
        strSQL = Nothing

    End Sub

    Private Sub ComputerName_Click(sender As Object, e As EventArgs) Handles ComputerName.Click
        ComputerName.DroppedDown = True
    End Sub
    Private Sub DriveLetter_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DriveLetter.SelectedIndexChanged
        Dim objSQLCommand As Object
        Dim strSQL As String
        Dim objSQLReader As Object
        'Populates Run Date Box
        StrDriveLetter = DriveLetter.SelectedItem
        Select Case UCase(My.Settings.SQLServerType)
            Case "MSSQL"
                strSQL = "SELECT DISTINCT [RunDate] FROM [" & My.Settings.SQLTable & "]  Where [Computer] = '" & StrComputerName & "' And SUBSTRING([FolderPath],1,3) = '" & StrDriveLetter & "' ORDER BY [RunDate];"
                If objSQLConnection Is Nothing Then
                    objSQLConnection = New System.Data.SqlClient.SqlConnection(My.Settings.SQLConnection)
                    StrDatabase = objSQLConnection.Database
                    If Not objSQLConnection.state = ConnectionState.Open Then objSQLConnection.Open()
                End If
                objSQLCommand = New System.Data.SqlClient.SqlCommand(strSQL, objSQLConnection)
            Case "MYSQL"
                strSQL = "SELECT DISTINCT `RunDate` FROM `" & My.Settings.SQLTable & "` Where `Computer` = '" & StrComputerName & "' And SUBSTRING(`FolderPath`,1,3) = '" & Replace(Replace(StrDriveLetter, "\", "\\"), ":", "\:") & "' ORDER BY `RunDate`;"
                If objSQLConnection Is Nothing Then
                    objSQLConnection = New MySql.Data.MySqlClient.MySqlConnection(My.Settings.SQLConnection)
                    StrDatabase = objSQLConnection.Database
                    If Not objSQLConnection.state = ConnectionState.Open Then objSQLConnection.Open()
                End If
                objSQLCommand = New MySql.Data.MySqlClient.MySqlCommand
                objSQLCommand.Connection = objSQLConnection
                objSQLCommand.CommandText = strSQL
                objSQLCommand.CommandType = CommandType.Text
                objSQLCommand.ExecuteNonQuery()
            Case Else
                MsgBox("SQLServerType setting not understood: SQLServerType =" & My.Settings.SQLServerType, MsgBoxStyle.Critical, "SQL Server Type")
                Me.Close()
        End Select
        objSQLReader = objSQLCommand.ExecuteReader
        If objSQLReader.HasRows Then
            Do While objSQLReader.Read()
                If Not objSQLReader.Item(0) = 0 Then
                    RunDate.Items.Add(objSQLReader.Item(0))
                Else
                    Exit Do
                End If
            Loop
        End If
        'RunDate.SelectedIndex = 0
        RunDate.Enabled = True
        objSQLReader.Close()
        objSQLCommand = Nothing
        objSQLReader = Nothing
        strSQL = Nothing
    End Sub
    Private Sub DriveLetter_Click(sender As Object, e As EventArgs) Handles DriveLetter.Click
        DriveLetter.DroppedDown = True
    End Sub
    Private Sub RunDate_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RunDate.SelectedIndexChanged
        BTNRun.Enabled = True
    End Sub
    Private Sub RunDate_Click(sender As Object, e As EventArgs) Handles RunDate.Click
        RunDate.DroppedDown = True
    End Sub
End Class
