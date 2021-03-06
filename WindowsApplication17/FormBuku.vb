﻿Imports System.Data.Odbc
Public Class FormBuku
    Dim databaru As Boolean
    Dim Conn As OdbcConnection
    Dim da As OdbcDataAdapter
    Dim ds As DataSet
    Dim str As String
    Dim CMD As OdbcCommand
    Dim RD As OdbcDataReader
    Private Function caribuku(ByVal buku As String) As Integer
        Try
            Dim foundrow As DataGridViewRow = (
            From row As DataGridViewRow In DataGridView1.Rows
            Where row.Cells(1).Value = buku
            Select row
            ).First
            If foundrow IsNot Nothing Then
                Return (foundrow.Index)
            Else
                Return -1
            End If
        Catch ex As Exception
            Return -1
        End Try
    End Function
    Private Sub FormBuku_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        databaru = False
        isigrid()
        Me.ComboBox1.Enabled = False
        Me.TextBox1.Enabled = False
        Me.TextBox2.Enabled = False
        Me.TextBox3.Enabled = False
        Me.cari.Enabled = False
        Me.TextBox5.Enabled = False
        Me.TextBox6.Enabled = False
        Me.tambah.Enabled = True
        Me.batal.Enabled = False
    End Sub
    Private Sub tambah_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tambah.Click
        Dim simpan As String
        Dim pesan As Integer
        ComboBox1.Focus()
        ttp()
        databaru = True
        If ComboBox1.Text = "" Then Exit Sub
        If databaru Then
            pesan = MsgBox("Apakah Anda Yakin Data Akan ditambahkan ke Database ?", vbYesNo + vbInformation, "Perhatian")
            If pesan = vbYesNo Then
                Exit Sub
            End If
            simpan = "INSERT INTO buku(nomor_buku,judul,kategori,pengarang,penerbit,tahun,stok) VALUES ('" & ComboBox1.Text & "','" & TextBox1.Text & "','" & TextBox2.Text & "','" & TextBox3.Text & "','" & cari.Text & "','" & TextBox5.Text & "','" & TextBox6.Text & "') "
        End If
        jalankansql(simpan)
        Me.Cursor = Cursors.WaitCursor
        DataGridView1.Refresh()
        isigrid()
        Me.Cursor = Cursors.Default
        tambah.Text = "simpan"
        bersih()
    End Sub

    Private Sub DataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        isiTextBox(e.RowIndex)
        databaru = False
    End Sub
    Sub isigrid()
        bukaDB()
        da = New Odbc.OdbcDataAdapter("SELECT * FROM buku", konek)
        ds = New DataSet
        ds.Clear()
        da.Fill(ds, "buku")
        DataGridView1.DataSource = (ds.Tables("buku"))
        DataGridView1.Enabled = True
    End Sub

    Sub bersih()
        ComboBox1.Text = ""
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""
        cari.Text = ""
        TextBox5.Text = ""
        TextBox6.Text = ""
    End Sub
    Private Sub jalankansql(ByVal sQl As String)
        Dim objcmd As New System.Data.Odbc.OdbcCommand
        Call bukaDB()
        Try
            objcmd.Connection = konek
            objcmd.CommandType = CommandType.Text
            objcmd.CommandText = sQl
            objcmd.ExecuteNonQuery()
            objcmd.Dispose()
            MsgBox("Data Sudah Disimpan", vbInformation)
        Catch ex As Exception
            MsgBox("Tidak Bisa Menyimpan data ke Server" & ex.Message)
        End Try
    End Sub

    Sub ttp()
        Me.ComboBox1.Enabled = True
        Me.TextBox1.Enabled = True
        Me.TextBox2.Enabled = True
        Me.TextBox3.Enabled = True
        Me.cari.Enabled = True
        Me.TextBox5.Enabled = True
        Me.TextBox6.Enabled = True
        Me.tambah.Enabled = True
        Me.batal.Enabled = True
    End Sub

    Sub ttpcari()
        Me.ComboBox1.Enabled = True
        Me.tambah.Enabled = True
        Me.batal.Enabled = True
    End Sub

    Private Sub isiTextBox(ByVal x As Integer)
        Try
            ComboBox1.Text = DataGridView1.Rows(x).Cells(0).Value
            TextBox1.Text = DataGridView1.Rows(x).Cells(1).Value
            TextBox2.Text = DataGridView1.Rows(x).Cells(2).Value
            TextBox3.Text = DataGridView1.Rows(x).Cells(3).Value
            cari.Text = DataGridView1.Rows(x).Cells(4).Value
            TextBox5.Text = DataGridView1.Rows(x).Cells(5).Value
            TextBox6.Text = DataGridView1.Rows(x).Cells(6).Value
        Catch ex As Exception
        End Try
    End Sub
    Private Sub DataGridView1_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        isiTextBox(e.RowIndex)
        databaru = False
    End Sub

    Private Sub keluar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles keluar.Click
        Me.Close()
    End Sub

    Private Sub batal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles batal.Click
        Call bersih()
    End Sub

    Private Sub hapus_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles hapus.Click
        Dim hapussql As String
        Dim pesan As Integer
        pesan = MsgBox("Apakah anda yakin akan menghapus Data pada server .. " + ComboBox1.Text, vbExclamation + vbYesNo, "perhatian")
        If pesan = vbNo Then Exit Sub
        hapussql = "DELETE FROM buku WHERE nomor_buku='" & ComboBox1.Text & "'"
        jalankansql(hapussql)
        Me.Cursor = Cursors.WaitCursor
        DataGridView1.Refresh()
        isigrid()
        Me.Cursor = Cursors.Default
        Call bersih()

    End Sub

    Private Sub cari_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cari.TextChanged
        caribuku(cari.Text)
        Dim xloop1 As Integer
        Dim spola As String
        Dim sbuka As String
        Dim sbetul As Boolean
        spola = cari.Text + "*"
        databaru = False
        Me.Cursor = Cursors.WaitCursor
        tambah.Enabled = True
        For xloop1 = 1 To DataGridView1.RowCount - 1
            sbuka = DataGridView1.Rows(xloop1 - 1).Cells(1).Value
            sbetul = UCase(sbuka) Like UCase(spola)
            If sbetul = True Then
                DataGridView1.CurrentCell = DataGridView1.Item(1, xloop1 - 1)
                isiTextBox(xloop1 - 1)
            End If
        Next
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub ubah_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ubah.Click
        Call ttp()
        Dim ubah As String = "UPDATE buku SET " _
            + "judul ='" & TextBox1.Text & "'," _
            + "kategori ='" & TextBox2.Text & "'," _
            + "pengarang ='" & TextBox3.Text & "'," _
            + "penerbit ='" & TextBox3.Text & "'," _
            + "tahun ='" & cari.Text & "'," _
            + "stok ='" & TextBox5.Text & "' WHERE nomor_buku = '" & ComboBox1.Text & "' "
        jalankansql(ubah)
        Me.Cursor = Cursors.WaitCursor
        DataGridView1.Refresh()
        isigrid()
        Me.Cursor = Cursors.Default
        bersih()
        hapus.Enabled = True
    End Sub

    Private Sub OdbcConnection1_InfoMessage(sender As Object, e As OdbcInfoMessageEventArgs) Handles OdbcConnection1.InfoMessage

    End Sub
End Class