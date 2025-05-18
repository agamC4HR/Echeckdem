<%@ Language=VBScript %>
<% Response.Buffer = True %>

<!--#include file="dbdata.asp" -->
<!--#include file="xfunctions.asp" -->
<!--#include file="dbfunctions.asp" -->
<!--#include file="adovbs.inc" -->

<%
    gp = getreq("gp")
    uid = getreq("uid")
    pass = getreq("pass")

    If ((uid = "") Or (pass = "")) And uid <> "demo" Then
        Response.Redirect("default.asp?nocode=1&gp=" & gp)
    Else
        ' Create database connection
        Set conn = Server.CreateObject("ADODB.Connection")
        Set rst = Server.CreateObject("ADODB.Recordset")
        conn.Open strcon & ";UID=" & webuser & ";pwd=" & webpassword

        ' Prepare SQL query with parameterized inputs to avoid SQL injection
        If pass = "anonymous" Then
            sql = "SELECT * FROM lluser WHERE RTRIM(userid) = ?"
        Else
            sql = "SELECT * FROM lluser WHERE RTRIM(userid) = ? AND RTRIM(password) = ? AND uactive = 1"
        End If

        ' Using parameterized query
        Set cmd = Server.CreateObject("ADODB.Command")
        cmd.ActiveConnection = conn
        cmd.CommandText = sql
        cmd.Parameters.Append cmd.CreateParameter("@uid", 8, 1, 255, uid) ' 8 = adVarChar
        If pass <> "anonymous" Then
            cmd.Parameters.Append cmd.CreateParameter("@pass", 8, 1, 255, pass)
        End If

        ' Execute the query
        Set rst = cmd.Execute

        If rst.EOF Then
            rst.Close
            Response.Redirect("default.asp?nocode=2")
        Else
            ' Session management
            Session("username") = rst("username")
            Session("userid") = rst("userid")
            Session("ulevel") = rst("ulevel")
            uoid = rst("uoid")
            Session("uoid") = uoid
            Session("uno") = rst("uno")
            ulev = rst("ulevel")
            rst.Close

            ' Output user level for debugging (optional)
            response.write("<br/>" & ulev)

            ' Redirect based on user level
            If uid = "demo" Then
                Response.Redirect("demo.asp")
    ElseIf ulev = 0 Then
   ' Response.write("spadmin.asp")
'response.end
    Response.Redirect("spadmin.asp")
            ElseIf ulev = 1 Then
                Response.Redirect("admina.asp")
            ElseIf ulev >= 2 Then
                Response.Redirect("main.asp")
            End If
        End If
    End If
%>
