<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form2
    Inherits System.Windows.Forms.Form

    'Форма переопределяет dispose для очистки списка компонентов.
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

    'Является обязательной для конструктора форм Windows Forms
    Private components As System.ComponentModel.IContainer

    'Примечание: следующая процедура является обязательной для конструктора форм Windows Forms
    'Для ее изменения используйте конструктор форм Windows Form.  
    'Не изменяйте ее в редакторе исходного кода.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Chk1 = New System.Windows.Forms.CheckBox
        Me.txt1 = New System.Windows.Forms.TextBox
        Me.txt2 = New System.Windows.Forms.TextBox
        Me.txt3 = New System.Windows.Forms.TextBox
        Me.txt4 = New System.Windows.Forms.TextBox
        Me.txt5 = New System.Windows.Forms.TextBox
        Me.txt6 = New System.Windows.Forms.TextBox
        Me.txt7 = New System.Windows.Forms.TextBox
        Me.txt8 = New System.Windows.Forms.TextBox
        Me.txt9 = New System.Windows.Forms.TextBox
        Me.txt10 = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'Chk1
        '
        Me.Chk1.AutoSize = True
        Me.Chk1.Checked = True
        Me.Chk1.CheckState = System.Windows.Forms.CheckState.Checked
        Me.Chk1.Location = New System.Drawing.Point(0, 8)
        Me.Chk1.Name = "Chk1"
        Me.Chk1.Size = New System.Drawing.Size(15, 14)
        Me.Chk1.TabIndex = 0
        Me.Chk1.UseVisualStyleBackColor = True
        '
        'txt1
        '
        Me.txt1.Location = New System.Drawing.Point(14, 4)
        Me.txt1.Name = "txt1"
        Me.txt1.Size = New System.Drawing.Size(55, 22)
        Me.txt1.TabIndex = 1
        Me.txt1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txt2
        '
        Me.txt2.BackColor = System.Drawing.Color.LightBlue
        Me.txt2.Location = New System.Drawing.Point(69, 4)
        Me.txt2.Name = "txt2"
        Me.txt2.Size = New System.Drawing.Size(35, 22)
        Me.txt2.TabIndex = 2
        Me.txt2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txt3
        '
        Me.txt3.BackColor = System.Drawing.Color.Plum
        Me.txt3.Location = New System.Drawing.Point(104, 4)
        Me.txt3.Name = "txt3"
        Me.txt3.Size = New System.Drawing.Size(35, 22)
        Me.txt3.TabIndex = 3
        Me.txt3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txt4
        '
        Me.txt4.BackColor = System.Drawing.Color.LightBlue
        Me.txt4.Location = New System.Drawing.Point(139, 4)
        Me.txt4.Name = "txt4"
        Me.txt4.Size = New System.Drawing.Size(35, 22)
        Me.txt4.TabIndex = 4
        Me.txt4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txt5
        '
        Me.txt5.BackColor = System.Drawing.Color.Plum
        Me.txt5.Location = New System.Drawing.Point(174, 4)
        Me.txt5.Name = "txt5"
        Me.txt5.Size = New System.Drawing.Size(35, 22)
        Me.txt5.TabIndex = 5
        Me.txt5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txt6
        '
        Me.txt6.Location = New System.Drawing.Point(209, 4)
        Me.txt6.Name = "txt6"
        Me.txt6.Size = New System.Drawing.Size(45, 22)
        Me.txt6.TabIndex = 6
        Me.txt6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txt7
        '
        Me.txt7.Location = New System.Drawing.Point(254, 4)
        Me.txt7.Name = "txt7"
        Me.txt7.Size = New System.Drawing.Size(45, 22)
        Me.txt7.TabIndex = 7
        Me.txt7.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txt8
        '
        Me.txt8.Location = New System.Drawing.Point(299, 4)
        Me.txt8.Name = "txt8"
        Me.txt8.Size = New System.Drawing.Size(30, 22)
        Me.txt8.TabIndex = 8
        Me.txt8.Text = "0"
        Me.txt8.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txt9
        '
        Me.txt9.Location = New System.Drawing.Point(329, 4)
        Me.txt9.Name = "txt9"
        Me.txt9.Size = New System.Drawing.Size(30, 22)
        Me.txt9.TabIndex = 9
        Me.txt9.Text = "0"
        Me.txt9.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txt10
        '
        Me.txt10.Location = New System.Drawing.Point(359, 4)
        Me.txt10.Name = "txt10"
        Me.txt10.Size = New System.Drawing.Size(45, 22)
        Me.txt10.TabIndex = 10
        Me.txt10.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Form2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(407, 32)
        Me.Controls.Add(Me.txt10)
        Me.Controls.Add(Me.txt9)
        Me.Controls.Add(Me.txt8)
        Me.Controls.Add(Me.txt7)
        Me.Controls.Add(Me.txt6)
        Me.Controls.Add(Me.txt5)
        Me.Controls.Add(Me.txt4)
        Me.Controls.Add(Me.txt3)
        Me.Controls.Add(Me.txt2)
        Me.Controls.Add(Me.txt1)
        Me.Controls.Add(Me.Chk1)
        Me.Name = "Form2"
        Me.Text = "Form2"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Chk1 As System.Windows.Forms.CheckBox
    Friend WithEvents txt1 As System.Windows.Forms.TextBox
    Friend WithEvents txt2 As System.Windows.Forms.TextBox
    Friend WithEvents txt3 As System.Windows.Forms.TextBox
    Friend WithEvents txt4 As System.Windows.Forms.TextBox
    Friend WithEvents txt5 As System.Windows.Forms.TextBox
    Friend WithEvents txt6 As System.Windows.Forms.TextBox
    Friend WithEvents txt7 As System.Windows.Forms.TextBox
    Friend WithEvents txt8 As System.Windows.Forms.TextBox
    Friend WithEvents txt9 As System.Windows.Forms.TextBox
    Friend WithEvents txt10 As System.Windows.Forms.TextBox
End Class
