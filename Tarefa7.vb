'Tarefa: Produto Inmetro 2

''Data:08/05/2024
''Autor: Felipe Melo
''Breve Relato: Botão criado para importar dados via (CSV)
IfDirectCast(parametros(0), Ferramentas.RibbonButton).BotaoLateral_ID =16035ThenTry
        ValidoCSV =true
        ErrosValidacaoCSV =""
        Csv =new prjProgramador2.ClsImportarCSV(Controle)
        NomeArquivo =""
        DsCSV = Csv.LerCSV(NomeArquivo)
ifnot DsCSVisNothingThen
            ValidarColunasCSV()
if ValidoCSVthen
                ImportaCSV()
else
                controle.msg(ErrosValidacaoCSV,"Importar CSV")
endifelse
            controle.msg("Arquivo Invalido","Importar CSV")
endifDirectCast(frm,Object).Atualizar()
        Controle.EditadoManual()
Catch exas exception
        TerminalPrint(ex.message)
EndTry
EndIf

''Data:08/05/2024
''Autor: Felipe Melo
''Breve Relato: Validação das colunasdo arquivo csv
PrivateSub ValidarColunasCSV()
TryDim ColsNaoEncontradasAsString =""Dim ColsValidacaoAsNew Dictionary(OfString,String)()

        Col_Data_Emissao = Csv.ValidarNomeColuna(DsCSV.Tables(0),"DATA EMISSAO","data emissao","DATA_EMISSAO","data_emissao","DATAEMISSAO","dataemissao","EMISSAO","emissao")
        ColsValidacao.Add("DATA EMISSAO", Col_Data_Emissao)

        Col_Data_Validade = Csv.ValidarNomeColuna(DsCSV.Tables(0),"DATA VALIDADE","data validade","DATA_VALIDADE","data_validade","DATAVALIDADE","datavalidade","VALIDADE","validade")
        ColsValidacao.Add("DATA VALIDADE", Col_Data_Validade)

        Col_Numero_Certificado = Csv.ValidarNomeColuna(DsCSV.Tables(0),"NUMERO CERTIFICADO","numero certificado","NUMERO_CERTIFICADO","numero_certificado","NUMERO CERTIFICADO","numero certificado","CERTIFICADO","certificado")
        ColsValidacao.Add("NUMERO CERTIFICADO", Col_Numero_Certificado)

        Col_Numero_Registro = Csv.ValidarNomeColuna(DsCSV.Tables(0),"NUMERO REGISTRO","numero registro","NUMERO_REGISTRO","numero_registro","NUMEROREGISTRO","numeroregistro","REGISTRO","registro")
        ColsValidacao.Add("NUMERO REGISTRO", Col_Numero_Registro)

ForEach pairAs KeyValuePair(OfString,String)In ColsValidacao
IfString.IsNullOrEmpty(pair.Value)Then
                ColsNaoEncontradas +=" " & pair.Key &", "EndIfNextIf ColsNaoEncontradas <>""Then
            ErrosValidacaoCSV +="Arquivo Invalido. O arquivo deve conter a(s) coluna(s)" & ColsNaoEncontradas & vbCrLf
            ValidoCSV =FalseEndIfCatch exAs Exception
        TerminalPrint(ex.Message)
EndTry
EndSub


'Data:10/05/2024
'Autor: Felipe Melo
'Breve Relato: Método para importar o csv.
privatesub ImportaCSV()
TryDim AtualizadosasLong =0Dim queryasstring =""ForEach linhaas datarowIn DsCSV.tables(0).rows
tryif linha.item(Col_Data_Emissao) <>""then
                Data_Emissao = linha.item(Col_Data_Emissao)
                Data_Validade = linha.item(Col_Data_Validade)
                Numero_Certificado = linha.item(Col_Numero_Certificado)
                Numero_Registro = linha.item(Col_Numero_Registro)
Dim PRODUTO_ID =Clng(ParametroTela("PRODUTO_ID").Valor)
Dim RTNPROCDSASNEW DATASET
                RTNPROCDS = PROCEDUREDS("SP_PRODUTOINMETRO_IMPORT_CSV",
FALSE,
FALSE,
CDATE(data_emissao).TOSTRING("yyyy.MM.dd"),
CDATE(data_validade).TOSTRING("yyyy.MM.dd"),
                                        numero_certificado,
                                        numero_registro,
                                        PRODUTO_ID,
                                        Controle.Usuario_id)
                Atualizados +=1
                linha.item("Processado") =1
                linha.item("ProcObs") = RTNPROCDS.tables(0).rows(0).item(0)
else
                linha.item("Processado") =0
                linha.item("ProcObs") ="O Campo e obrigatorio"endifCatch exAs exception
              TerminalPrint(ex.Message)
              linha.item("Processado") =0
              linha.item("ProcObs") ="Nao foi possivel inserir/alterar"EndTryNextDim ArquivoProcessadoasstring
      ArquivoProcessado = csv.SavarCVS(DsCSV,NomeArquivo)
      Controle.Msg("Arquivo processado com sucesso na empresa atual." & vbcrlf & Atualizados.tostring &"/" & DsCSV.tables(0).rows.count.tostring &" registro(s) processado(s)." & vbcrlf &"Verifique o resultado de cada registro no arquivo " & ArquivoProcessado,"Importar CSV")

Catch exAs exception
        TerminalPrint("ERRO: " & ex.message)
EndTry
EndSub

''Autor: Felipe Melo
private csvas prjProgramador2.ClsImportarCSV
Private ValidoCSVasBoolean =true
Private ErrosValidacaoCSVasstring =""
Private NomeArquivoasstring
Private DsCSVasnew Dataset

''Colunasdo CSV
Private Col_Data_EmissaoAsString
Private Col_Data_ValidadeAsString
Private Col_Numero_CertificadoAsString
Private Col_Numero_RegistroAsString

''Valores das colunas
Private Data_EmissaoAs DateTime
Private Data_ValidadeAs DateTime
Private Numero_CertificadoAsString
Private Numero_RegistroAsString