'Tarefa: PEDIDO/ORÇAMENTO - OBS FINANCEIRO

IF CSTR(Controle.Servidor).ToUpper() = "MAXFER" Then
    IF CSTR(SC("OBS_FINANCEIRO").OBJETO.VALOR).LENGTH <= 0 Then
        RETORNO.RETORNO = FALSE      
        RETORNO.ERRO += "O campo 'OBS Financeiro' é obrigatório."
    END IF
END IF