'Tarefa: Tipo de Nota Fiscal

'BotaoCadastrarA
'Data: 30/04/2024
'Autor: Felipe Melo
'Breve Relato: função que cadastra o registro como 100 automaticamente
'___________________
    
IF SC("CP_PERCENTUAL_QUANTIDADE").OBJETO.VALOR <> 100 THEN
    SC("CP_PERCENTUAL_QUANTIDADE").OBJETO.VALOR = 100
END IF


'Data: 30/04/2024
'Autor: Felipe Melo
'Breve Relato: validar BotaoCadastrarA
'___________________

if controle.cadastrando then
    IF SC("CP_PERCENTUAL_QUANTIDADE").OBJETO.VALOR <> 100 THEN
        SC("CP_PERCENTUAL_QUANTIDADE").OBJETO.VALOR = 100
    END IF
end if

'Data: 30/04/2024
'Autor: Felipe Melo
'Breve Relato: função que cadastra o registro como 100 automaticamente, permite apenas o usuário alterar o valor
'___________________

PRIVATE SUB FIXARPERCENTUAL()
    SC("CP_PERCENTUAL_QUANTIDADE").OBJETO.VALOR = 100 ' Valor FIXO para cadastro automatico'
    SC("CP_PERCENTUAL_QUANTIDADE").OBJETO.ENABLED = FALSE
END SUB