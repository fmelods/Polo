--Tarefa: ALTERAÇÃO DE CUSTO CONTÁBIL

'SP_ALTERECAOCUSTOCONTABIL_ALTERARCUSTO_CSV (procedure)
    
    @CODIGOINTERNO AS NVARCHAR(500),
    @CUSTOMEDIOCONTABIL AS NVARCHAR(500),
    @ALTERACAOCUSTOCONTABIL_ID AS BIGINT,
    @USUARIO_ID AS BIGINT    
AS
BEGIN
    --> SP_ALTERECAOCUSTOCONTABIL_ALTERARCUSTO_CSV
    DECLARE @RESULTADO AS NVARCHAR(1000) = 'ERRO AO PROCESSAR O REGISTRO!'
    DECLARE @PRODUTO_ID AS BIGINT
        
    SELECT TOP 1 @PRODUTO_ID=ISNULL(P.PRODUTO_ID, 0)
    FROM TB_PRODUTO AS P WHERE STATUS <> 0
    AND (P.CODIGOINTERNO = @CODIGOINTERNO OR P.CODIGOINTERNO_PGOOGLE = @CODIGOINTERNO)
           
    IF ISNULL(@PRODUTO_ID, 0) <> 0
    BEGIN
        IF NOT EXISTS(SELECT TOP 1 I.PRODUTO_ID 
                      FROM TB_ALTERACAOCUSTOCONTABILITEM AS I 
                      INNER JOIN TB_ALTERACAOCUSTOCONTABIL AS C ON C.ALTERACAOCUSTOCONTABIL_ID = I.ALTERACAOCUSTOCONTABIL_ID AND C.STATUS <> 0
                      WHERE I.STATUS <> 0 
                      AND C.ALTERACAOCUSTOCONTABIL_ID = @ALTERACAOCUSTOCONTABIL_ID
                      AND I.PRODUTO_ID = @PRODUTO_ID)
        BEGIN
        INSERT INTO [dbo].[TB_ALTERACAOCUSTOCONTABILITEM]
                    ([ALTERACAOCUSTOCONTABIL_ID]
                    ,[PRODUTO_ID]
                    ,[CUSTO_MEDIO_CONTABIL]                    
                    ,[ACAO_ID]
                    ,[ULT_ALT]
                    ,[ULT_USER_ID]
                    ,[STATUS]
                    ,[USER_ID]
                    )
            VALUES
                (@ALTERACAOCUSTOCONTABIL_ID                                      --<ALTERACAOCUSTOCONTABIL_ID, bigint,>
                ,@PRODUTO_ID                                                     --<PRODUTO_ID, bigint,>
                ,CAST(REPLACE(@CUSTOMEDIOCONTABIL, ',', '.') AS DECIMAL(24,6))   --<CUSTO_MEDIO_CONTABIL, decimal(24,6),>
                ,1                                                               --<ACAO_ID, int,>
                ,GETDATE()                                                       --<ULT_ALT, smalldatetime,>
                ,@USUARIO_ID                                                     --<ULT_USER_ID, int,>
                ,1                                                               --<STATUS, int,>
                ,@USUARIO_ID                                                     --<USER_ID, int,>
                )
    
            SET @RESULTADO = 'NOVO REGISTRO CADASTRADO COM SUCESSO'         
        END
        ELSE
        BEGIN            
            UPDATE TB_ALTERACAOCUSTOCONTABILITEM
                SET 
                 CUSTO_MEDIO_CONTABIL = CAST(REPLACE(@CUSTOMEDIOCONTABIL, ',', '.') AS DECIMAL(24,6))
                ,STATUS = 1
                ,ALTERACAO_ID = 0
                ,ULT_ALT = GETDATE()
                ,ULT_USER_ID = @USUARIO_ID
                ,USER_ID = 0
                ,DT_BLOQUEIO = NULL
            WHERE STATUS <> 0 
            AND ALTERACAOCUSTOCONTABIL_ID = @ALTERACAOCUSTOCONTABIL_ID
            AND PRODUTO_ID = @PRODUTO_ID
    
            SET @RESULTADO = 'CUSTO DO PRODUTO ATUALZIADO COM SUCESSO'
        END
    END
    ELSE
    BEGIN
        SET @RESULTADO = 'CODIGO INTERNO NAO LOCALIZADO'    
    END
    
    SELECT @RESULTADO AS RESULTADO
END