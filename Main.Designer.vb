<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FileAuditViewerBase
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FileAuditViewerBase))
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.FtoU = New System.Windows.Forms.TabPage()
        Me.FtoUTreeView = New System.Windows.Forms.TreeView()
        Me.UtoF = New System.Windows.Forms.TabPage()
        Me.UtoFTreeView = New System.Windows.Forms.TreeView()
        Me.GtoF = New System.Windows.Forms.TabPage()
        Me.GtoFTreeView = New System.Windows.Forms.TreeView()
        Me.DriveLetter = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.RunDate = New System.Windows.Forms.ComboBox()
        Me.RemoveInherited = New System.Windows.Forms.CheckBox()
        Me.BTNRun = New System.Windows.Forms.Button()
        Me.TreeViewHeading = New System.Windows.Forms.TreeView()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.ComputerName = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
        Me.TabControl1.SuspendLayout()
        Me.FtoU.SuspendLayout()
        Me.UtoF.SuspendLayout()
        Me.GtoF.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.FtoU)
        Me.TabControl1.Controls.Add(Me.UtoF)
        Me.TabControl1.Controls.Add(Me.GtoF)
        Me.TabControl1.Location = New System.Drawing.Point(3, 114)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(1005, 448)
        Me.TabControl1.TabIndex = 4
        '
        'FtoU
        '
        Me.FtoU.Controls.Add(Me.FtoUTreeView)
        Me.FtoU.Location = New System.Drawing.Point(4, 22)
        Me.FtoU.Name = "FtoU"
        Me.FtoU.Padding = New System.Windows.Forms.Padding(3)
        Me.FtoU.Size = New System.Drawing.Size(997, 422)
        Me.FtoU.TabIndex = 0
        Me.FtoU.Text = "Folders to Users"
        Me.FtoU.UseVisualStyleBackColor = True
        '
        'FtoUTreeView
        '
        Me.FtoUTreeView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FtoUTreeView.Location = New System.Drawing.Point(3, 3)
        Me.FtoUTreeView.Name = "FtoUTreeView"
        Me.FtoUTreeView.PathSeparator = ";"
        Me.FtoUTreeView.Size = New System.Drawing.Size(991, 416)
        Me.FtoUTreeView.TabIndex = 0
        '
        'UtoF
        '
        Me.UtoF.Controls.Add(Me.UtoFTreeView)
        Me.UtoF.Location = New System.Drawing.Point(4, 22)
        Me.UtoF.Name = "UtoF"
        Me.UtoF.Padding = New System.Windows.Forms.Padding(3)
        Me.UtoF.Size = New System.Drawing.Size(997, 422)
        Me.UtoF.TabIndex = 1
        Me.UtoF.Text = "Users to Folders"
        Me.UtoF.UseVisualStyleBackColor = True
        '
        'UtoFTreeView
        '
        Me.UtoFTreeView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UtoFTreeView.Location = New System.Drawing.Point(3, 3)
        Me.UtoFTreeView.Name = "UtoFTreeView"
        Me.UtoFTreeView.PathSeparator = ";"
        Me.UtoFTreeView.Size = New System.Drawing.Size(991, 416)
        Me.UtoFTreeView.TabIndex = 1
        '
        'GtoF
        '
        Me.GtoF.AutoScroll = True
        Me.GtoF.Controls.Add(Me.GtoFTreeView)
        Me.GtoF.Location = New System.Drawing.Point(4, 22)
        Me.GtoF.Name = "GtoF"
        Me.GtoF.Padding = New System.Windows.Forms.Padding(3)
        Me.GtoF.Size = New System.Drawing.Size(997, 422)
        Me.GtoF.TabIndex = 2
        Me.GtoF.Text = "Groups to Folders"
        Me.GtoF.UseVisualStyleBackColor = True
        '
        'GtoFTreeView
        '
        Me.GtoFTreeView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GtoFTreeView.Location = New System.Drawing.Point(3, 3)
        Me.GtoFTreeView.Name = "GtoFTreeView"
        Me.GtoFTreeView.PathSeparator = ";"
        Me.GtoFTreeView.Size = New System.Drawing.Size(991, 416)
        Me.GtoFTreeView.TabIndex = 1
        '
        'DriveLetter
        '
        Me.DriveLetter.Enabled = False
        Me.DriveLetter.FormattingEnabled = True
        Me.DriveLetter.Location = New System.Drawing.Point(293, 10)
        Me.DriveLetter.Name = "DriveLetter"
        Me.DriveLetter.Size = New System.Drawing.Size(121, 21)
        Me.DriveLetter.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(222, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(65, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Drive Letter:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(420, 14)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(56, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Run Date:"
        '
        'RunDate
        '
        Me.RunDate.Enabled = False
        Me.RunDate.FormattingEnabled = True
        Me.RunDate.Location = New System.Drawing.Point(482, 11)
        Me.RunDate.Name = "RunDate"
        Me.RunDate.Size = New System.Drawing.Size(121, 21)
        Me.RunDate.TabIndex = 2
        '
        'RemoveInherited
        '
        Me.RemoveInherited.AutoSize = True
        Me.RemoveInherited.Checked = True
        Me.RemoveInherited.CheckState = System.Windows.Forms.CheckState.Checked
        Me.RemoveInherited.Location = New System.Drawing.Point(798, 17)
        Me.RemoveInherited.Name = "RemoveInherited"
        Me.RemoveInherited.Size = New System.Drawing.Size(110, 17)
        Me.RemoveInherited.TabIndex = 3
        Me.RemoveInherited.Text = "Remove Inherited"
        Me.RemoveInherited.UseVisualStyleBackColor = True
        '
        'BTNRun
        '
        Me.BTNRun.Enabled = False
        Me.BTNRun.Location = New System.Drawing.Point(926, 13)
        Me.BTNRun.Name = "BTNRun"
        Me.BTNRun.Size = New System.Drawing.Size(75, 23)
        Me.BTNRun.TabIndex = 5
        Me.BTNRun.Text = "Run"
        Me.BTNRun.UseVisualStyleBackColor = True
        '
        'TreeViewHeading
        '
        Me.TreeViewHeading.Cursor = System.Windows.Forms.Cursors.Default
        Me.TreeViewHeading.Location = New System.Drawing.Point(7, 41)
        Me.TreeViewHeading.Name = "TreeViewHeading"
        Me.TreeViewHeading.Scrollable = False
        Me.TreeViewHeading.Size = New System.Drawing.Size(994, 67)
        Me.TreeViewHeading.TabIndex = 6
        Me.TreeViewHeading.TabStop = False
        '
        'Panel1
        '
        Me.Panel1.AutoSize = True
        Me.Panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.Panel1.Controls.Add(Me.ComputerName)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.BTNRun)
        Me.Panel1.Controls.Add(Me.TreeViewHeading)
        Me.Panel1.Controls.Add(Me.RemoveInherited)
        Me.Panel1.Controls.Add(Me.RunDate)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.DriveLetter)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1008, 562)
        Me.Panel1.TabIndex = 0
        '
        'ComputerName
        '
        Me.ComputerName.FormattingEnabled = True
        Me.ComputerName.Location = New System.Drawing.Point(95, 9)
        Me.ComputerName.Name = "ComputerName"
        Me.ComputerName.Size = New System.Drawing.Size(121, 21)
        Me.ComputerName.TabIndex = 8
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(3, 14)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(86, 13)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Computer Name:"
        '
        'BackgroundWorker1
        '
        Me.BackgroundWorker1.WorkerReportsProgress = Global.FileAuditViewer.My.MySettings.Default.EventProcess
        Me.BackgroundWorker1.WorkerSupportsCancellation = Global.FileAuditViewer.My.MySettings.Default.WorkerCancel
        '
        'FileAuditViewerBase
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.ClientSize = New System.Drawing.Size(1008, 562)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.Panel1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FileAuditViewerBase"
        Me.Text = "File Audit Viewer"
        Me.TabControl1.ResumeLayout(False)
        Me.FtoU.ResumeLayout(False)
        Me.UtoF.ResumeLayout(False)
        Me.GtoF.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents FtoU As System.Windows.Forms.TabPage
    Friend WithEvents UtoF As System.Windows.Forms.TabPage
    Friend WithEvents GtoF As System.Windows.Forms.TabPage
    Friend WithEvents DriveLetter As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents RunDate As System.Windows.Forms.ComboBox
    Friend WithEvents RemoveInherited As System.Windows.Forms.CheckBox
    Friend WithEvents FtoUTreeView As System.Windows.Forms.TreeView
    Friend WithEvents BTNRun As System.Windows.Forms.Button
    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
    Friend WithEvents TreeViewHeading As System.Windows.Forms.TreeView
    Friend WithEvents UtoFTreeView As System.Windows.Forms.TreeView
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents GtoFTreeView As System.Windows.Forms.TreeView
    Friend WithEvents ComputerName As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label

End Class
