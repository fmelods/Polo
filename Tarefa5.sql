--Tarefa: NF-E - GERAR NFE II

--TREINANDO SELECT

SELECT TOP 10 NP.TIPO, * FROM TB_NATUREZAOPERACAO AS NP
WHERE
    ISNULL(NATUREZAOPERACAO_ID,0) <> 0
    AND NP.STATUS <> 0
    AND TIPO = 'PEDIDO'
    AND ARQUIVADO = 0

--INSERT

DECLARE @NaturezaOperacao NVARCHAR(100);

SELECT @NaturezaOperacao = NP.TIPO
FROM TB_NATUREZAOPERACAO AS NP
WHERE ISNULL(NP.NATUREZAOPERACAO_ID, 0) <> 0
AND NP.STATUS <> 0
AND NP.TIPO = 'PEDIDO'
AND ARQUIVADO = 0;

INSERT INTO TB_NFE (NATUREZAOPERACAO_TIPO)
VALUES (@NaturezaOperacao);

DECLARE @PedidoID BIGINT;
DECLARE @OrigemEstadoID BIGINT;
DECLARE @DestinoEstadoID BIGINT;
DECLARE @NaturezaOperacao NVARCHAR(100);

-- Obter o ID do estado da origem do pedido
SELECT 
FROM 
INNER JOIN 
INNER JOIN 
WHERE 

-- Obter o ID do estado do destino do pedido
SELECT 
FROM 
INNER JOIN 
INNER JOIN 
WHERE 

-- Determinar a natureza da operação com base nos estados
IF @OrigemEstadoID = @DestinoEstadoID
    SET @NaturezaOperacao = 'Dentro do Estado';
ELSE
    SET @NaturezaOperacao = 'Fora do Estado';

-- Atualizar o campo NATUREZA_OPERACAO na tabela TB_NFE
UPDATE TB_NFE
SET NATUREZA_OPERACAO = @NaturezaOperacao
WHERE PEDIDO_ID = @PedidoID;
