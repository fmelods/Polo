'Tarefa: ALTERAÇÃO DE CUSTO CONTABIL - ALTERAÇÃO DE CUSTO CONTABIL

'BotaoLateralClick

'Data:26/04/2024 -30/04/2024
'Autor: Felipe Melo - Robert Albert
'Breve Relato: Botão criado para confirmar registro de Alteração de Custo Contábil
IFDIRECTCAST(PARAMETROS(0), FERRAMENTAS.RIBBONBUTTON).BOTAOLATERAL_ID =15780THENTRYIFCBOOL(SUPERCAMPO("CONFIRMADO").OBJETO.CHECKED) =FALSETHENDIM QUERYASSTRING =""'Update na Capa'
            QUERY +=" UPDATE TB_ALTERACAOCUSTOCONTABILSET CONFIRMADO =1, DATA_CONFIRMADO = GETDATE(), STATUS =3, "
            QUERY +=" USUARIO_CONFIRMADO_ID = " & CONTROLE.USUARIO_ID &" "
            QUERY +=" WHERE STATUS <>0AND ALTERACAOCUSTOCONTABIL_ID = " & CONTROLE.ID &" "
            WSMOTOR.EXECUTARSIMPLES(QUERY)
'Update nos Itens'
            QUERY =""
            QUERY +=" UPDATE TB_ALTERACAOCUSTOCONTABILITEMSET STATUS =3 "
            QUERY +=" WHERE STATUS <>0AND ALTERACAOCUSTOCONTABIL_ID = " & CONTROLE.ID &" "
            WSMOTOR.EXECUTARSIMPLES(QUERY)
DIRECTCAST(FRM,OBJECT).ATUALIZAR()
ENDIFCATCHENDTRY
ENDIF

'''Data:30/04/2024
'''Autor: Robert Albert
'''Breve Relato: Botão criado para importar dados via (CSV)
IfDirectCast(parametros(0), Ferramentas.RibbonButton).BotaoLateral_ID =15978ThenTry
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
            controle.msg("Arquivo Inválido","Importar CSV")
endifDirectCast(frm,Object).Atualizar()
        Controle.EditadoManual()
Catch exas exception
        TerminalPrint(ex.message)
EndTry
EndIf

'Data:30/04/2024
'Autor: Robert Albert
'Breve Relato: Exporta os Produtos e o Custo Médio Contábil (também pode ser o Modelo de Import)
IfDirectCast(parametros(0), Ferramentas.RibbonButton).BotaoLateral_ID =15977ThenTry
        clsCSV =new PrjProgramador2.ClsExportarCSV()
        CampoCSVs =New List(Of PrjProgramador2.clsExportarCampoCSV)
Dim queryasstring =""Dim dsas dataset
        query +="SELECT TOP100 "
        query +="  P.CODIGOINTERNOAS CODIGOINTERNO, "
        query +="  I.CUSTO_MEDIO_CONTABILAS CUSTO_MEDIO_CONTABIL "
        query +=" FROM TB_ALTERACAOCUSTOCONTABILITEMAS I "
        query +=" LEFT JOIN TB_PRODUTOAS PON P.PRODUTO_ID = I.PRODUTO_IDAND P.STATUS <>0 "
        query +=" WHERE I.STATUS <>0 "
        query +="AND ISNULL(I.PRODUTO_ID,0) <>0 "
        query +="AND I.ALTERACAOCUSTOCONTABIL_ID = " & CONTROLE.ID
        TerminalPrint(query)'
        ds = wsmotor.consultar(query)
If CampoCSVs.Count =0ThenForEach colunaAs DataColumnIn ds.Tables(0).Columns
                 CampoCSV =new PrjProgramador2.clsExportarCampoCSV
                 CampoCSV.NomeTabela = coluna.ColumnName
                 CampoCSV.NomeCSV = coluna.ColumnName
                 CampoCSVs.Add(CampoCSV)
NextEndIf
        clsCSV.ExportarCSVDataSet(ds,CampoCSVs)
CatchEndtry
Endif

'FunçõesPersonalizadas

PrivateSub ValidarColunasCSV()
TryDim ColsNaoEncontradasAsString =""Dim ColsValidacaoAsNew Dictionary(OfString,String)()

        COL_CODIGOINTERNO = Csv.ValidarNomeColuna(DsCSV.Tables(0),"CODIGOINTERNO","CODIGO_INTERNO")
        ColsValidacao.Add("CODIGOINTERNO", COL_CODIGOINTERNO)

        COL_CUSTOMEDIOCONTABIL = Csv.ValidarNomeColuna(DsCSV.Tables(0),"CUSTOMEDIOCONTABIL","CUSTO_MEDIO_CONTABIL","CUSTO_MEDIO","CUSTOMEDIO")
        ColsValidacao.Add("CUSTOMEDIOCONTABIL", COL_CUSTOMEDIOCONTABIL)

ForEach pairAs KeyValuePair(OfString,String)In ColsValidacao
IfString.IsNullOrEmpty(pair.Value)Then
                ColsNaoEncontradas +=" " & pair.Key &", "EndIfNextIf ColsNaoEncontradas <>""Then
            ErrosValidacaoCSV +="Arquivo Inválido. O arquivo deve conter a(s) coluna(s)" & ColsNaoEncontradas & vbCrLf
            ValidoCSV =FalseEndIfCatch exAs Exception
        TerminalPrint(ex.message)
EndTry
EndSub


'Data:30/04/2024
'Autor: Robert Albert
'Breve Relato: Irá Inserir ou Atualizar
PrivateSub ImportaCSV()
TryDim AtualizadosasLong =0Dim queryasstring =""ForEach linhaas datarowIn DsCSV.tables(0).rows
tryIf linha.item(COL_CODIGOINTERNO) <>""andalso linha.item(COL_CUSTOMEDIOCONTABIL) <>""ThenDim RTNPROCDSASNEW DATASET
                    RTNPROCDS = PROCEDUREDS("SP_ALTERECAOCUSTOCONTABIL_ALTERARCUSTO_CSV",FALSE,FALSE,
                                            linha.item(COL_CODIGOINTERNO),
                                            linha.item(COL_CUSTOMEDIOCONTABIL),
                                            CONTROLE.ID,
                                            Controle.Usuario_id
                                            )
If RTNPROCDS.Tables(0).Columns.Count >0AndAlso RTNPROCDS.Tables(0).Rows.Count >0ThenIf RTNPROCDS.Tables(0).Rows(0).Item("RESULTADO").ToString <>""Then
                            linha.item("Processado") =1
                            linha.item("ProcObs") =CSTR(RTNPROCDS.tables(0).rows(0).item("RESULTADO"))
                            Atualizados +=1EndIfEndIfElse
                    linha.item("Processado") =0
                    linha.item("ProcObs") ="O Campo é obrigatório"EndifCatch exas exception
                linha.item("Processado") =0
                linha.item("ProcObs") ="Nao foi possível inserir/alterar"EndtryNext

Dim ArquivoProcessadoasstring = csv.SavarCVS(DsCSV,NomeArquivo)
        Controle.Msg("Arquivo processado com sucesso." & vbcrlf & Atualizados.tostring &"/" & DsCSV.tables(0).rows.count.tostring &" registro(s) atualizados" & vbcrlf &" verifique o resultado de cada registro no arquivo " & ArquivoProcessado,"Importar CSV")

Catch exas exception
        TerminalPrint("ERRO: " & ex.message)
Endtry
EndSub

'Variáveis

Private clsCSVAs PrjProgramador2.ClsExportarCSV
Private CampoCSVasnew PrjProgramador2.clsExportarCampoCSV
Private CampoCSVsAs List(Of PrjProgramador2.clsExportarCampoCSV)

private csvas prjProgramador2.ClsImportarCSV
Private ValidoCSVasBoolean =true
Private ErrosValidacaoCSVasstring =""
Private NomeArquivoasstring
Private DsCSVasnew Dataset

Private COL_CODIGOINTERNOAsString
Private COL_CUSTOMEDIOCONTABILAsString

