'Tarefa: Orçamento Container 12


'Data: 21/05/2024
'Autor: Felipe Melo
'Breve Relato: Altera ou insere o tipo de NFE
CONTROLE.MSG("TESTE1","TESTE1")
Try
    if supercampo("cp_tiponotafiscal").Objeto.Valor <> 0 then
    controle.msg("TESTE2","TESTE2")
    dim query As String
    
    query = " update tb_orcamentocontainer "
    controle.msg(query)
    query += " set cp_tiponotafiscal_id = 1, status = 1, alteracao_id = 0, acao_id = 1, ult_alt = getdate(),"
    controle.msg(query)
    query += " ult_user_id = " & controle.usuario_Id & ", user_id = 0, dt_bloqueio = null "
    controle.msg(query)
    query += " where orcamento_id = " & controle.id
    TerminalPrint(query)
    wSMotor.executarSimples(query)
    controle.msg("Tipo de nota fiscal alterado ou inserido com sucesso.", "Sucesso")
    Resultado = True
    ELSE
        Resultado = False
    END IF
Catch ex As Exception
    controle.msg(ex.message, "")
    controle.msg("Falha ao alterar ou inserir um tipo de nota fiscal.", "Aviso")
End try
CONTROLE.MSG("TESTE3","TESTE3")

'FormCarregado
VerificarAlterarTipoNotaFiscal()

'FunçõesPersonalizadasB
'Data: 21/05/2024
'Autor: Felipe Melo
'Breve Relato: Exibir o botão 'Alterar Tipo Nota Fiscal' quando o orçamento estiver confirmado pelo comercial.
Private Sub VerificarAlterarTipoNotaFiscal()
    Try
        IF SC("CP_CONFIRMADO_COMERCIAL").Objeto.Checked = False THEN
            BotaoLateral(-50032).enabled = false 'Botão para abrir tela auxiliar de executar função'
        ELSE
            BotaoLateral(-50032).enabled = true
        END IF
    
    Catch Ex As Exception
        TerminalPrint(ex.Message)
    End Try
End Sub
        
'PosMovGridA
VerificarAlterarTipoNotaFiscal()

'ExecutarFuncaoB
'Data:21/05/2024
'Autor: Felipe Melo
'Breve Relato: Altera ou insere o tipo de Nota Fiscal.
Tryif supercampo("cp_tiponotafiscal").Objeto.id <>0thendim queryAsString
    query =" update tb_orcamentocontainer "
    query +="set cp_tiponotafiscal_id = " & supercampo("cp_tiponotafiscal").Objeto.id &", status =1, alteracao_id =0, acao_id =1, ult_alt = getdate(),"
    query +=" ult_user_id = " & controle.usuario_Id &", user_id =0, dt_bloqueio = null "
    query +=" where status <>0and orcamento_id = " & controle.id
    wSMotor.executarSimples(query)
    controle.msg("Tipo de nota fiscal alterado com sucesso.","Sucesso")
    Resultado =TrueELSE
        Resultado =FalseENDIF
Catch exAs Exception
    controle.msg("Falha ao alterar um tipo de nota fiscal.","Aviso")
Endtry

'FormCarregadoB
Controle.PrepararFuncao()