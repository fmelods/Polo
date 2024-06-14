'Tarefa: CADASTRO>FISCAL>NATUREZA DA OPERAÇÃO	NATUREZA AUTOMÁTICACADASTRO>FISCAL>NATUREZA DA OPERAÇÃO - NATUREZA AUTOMÁTICA

'CampoAlterado'
' Felipe Melo 17/04/2024'
TRY 
Select Case campo.nome
    case "TIPO"
        VerificarBrinde()
End Select   
CATCH
END TRY 

'FormCarregado'
' Felipe Melo 17/04/2024'
VerificarBrinde()

'FunçõesPersonalizadas'
'Data: 16/04/2024
'Autor: Felipe Melo
'Breve Relato: Torna o checkbox de brinde visível apenas para a opção de bonificação no campo tipo.
'___________________'

Private Sub VerificarBrinde()
    Try        
        IF SC("TIPO").OBJETO.VALOR = "BONIFICAÇÃO" THEN
            SC("BRINDE").OBJETO.ESCONDER = FALSE ' FICA VISIVEL'
        ELSE
            SC("BRINDE").OBJETO.ESCONDER = TRUE ' FICA OCULTO'
        END IF
    Catch
    End Try       
End Sub

'Data: 17/04/2024
'Autor: Felipe Melo
'Breve Relato: Retorna um booleano da Select que busca se existe um registro já cadastrado na tabela.
'___________________'

Private Function VerificarRegistroDuplicado() As Boolean
    Dim blnRegistrosDuplicados As Boolean = False
    
    Try 
        Dim strQuery As String
        Dim ds As DataSet
        
        ' Construa a consulta SQL para verificar registros duplicados
        strQuery = "SELECT COUNT(*) AS REGISTROS_DUPLICADOS " & _
                   "FROM TB_NATUREZAOPERACAO " & _
                   "WHERE STATUS <> 0 " & _
                   "AND TIPO = 'BONIFICAÇÃO' " & _
                   "AND ISNULL(BRINDE, 0) = 1"
                   
        ' Execute a consulta SQL
        ds = wsMotor.Consultar(strQuery)            
        
        ' Verifique se há registros duplicados
        If ds.Tables(0).Rows.Count > 0 Then
            Dim registrosDuplicados As Integer = Convert.ToInt32(ds.Tables(0).Rows(0)("REGISTROS_DUPLICADOS"))
            If registrosDuplicados > 1 Then
                blnRegistrosDuplicados = True
            End If
        End If
    Catch ex As Exception    
        Throw
    End Try    
    
    Return blnRegistrosDuplicados
End Function

Private Function VerificarRegistroDuplicado() As Boolean
    Dim blnRegistrosDuplicados As Boolean = False    
    Try 
        Dim ds As DataSet
        Dim strQuery As String = ""                
        strQuery += " SELECT COUNT(*) AS REGISTROS_DUPLICADOS "
        strQuery += " FROM TB_NATUREZAOPERACAO "
        strQuery += " WHERE STATUS <> 0 "
        strQuery += " AND TIPO = 'BONIFICAÇÃO' "
        strQuery += " AND ISNULL(BRINDE, 0) = 1 "
        ds = wsMotor.Consultar(strQuery)        
        If ds.Tables(0).Rows.Count > 0 Then            
            If Cint(ds.Tables(0).Rows(0)("REGISTROS_DUPLICADOS")) > 1 Then
                blnRegistrosDuplicados = True
            End If
        End If
    Catch ex As Exception            
    End Try    
    Return blnRegistrosDuplicados
End Function


'Validar'
'Data: 17/04/2024
'Autor: Felipe Melo
'Breve Relato: Pede ao usuário que preencha ao menos um dos dois campos disponíveis.
'___________________'

If String.IsNullOrEmpty(SC("fora_do_estado").OBJETO.VALOR) AndAlso String.IsNullOrEmpty(SC("dentro_do_estado").OBJETO.VALOR) Then
    RETORNO.RETORNO = False
    RETORNO.ERRO += "Preencha pelo menos um dos campos 'Fora do Estado' ou 'Dentro do Estado'."
End If

'Data: 17/04/2024
'Autor: Felipe Melo
'Breve Relato: Pega o Retorno e valida se irá inserir novos dados na tabela, caso não for permitido exibe a mensagem de erro.
'___________________'

If VerificarRegistroDuplicado() Then
    ' Não permita a inserção do novo registro
    retorno.retorno = False
    retorno.erro = retorno.erro & vbCrLf & "Não é permitido inserir registros duplicados para Tipo 'BONIFICAÇÃO' e Brinde."
Else
    retorno.retorno = True
End If

If VerificarRegistroDuplicado() Then    
    RETORNO.RETORNO = False
    RETORNO.ERRO += vbCrLf & " - Não é permitido inserir registros duplicados para Tipo 'BONIFICAÇÃO' e Brinde."    
End If

