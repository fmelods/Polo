--Tarefa: Pedido container 3

SELECT *FROM TB_PEDIDOWHERE STATUS <>0
AND PEDIDO_IDIN (201125,201123,201122,201119)
UPDATE TB_PEDIDOQQ
SET ATENDIDO =0
WHERE STATUS <>0AND PEDIDO_IDIN (201125,201123)